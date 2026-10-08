using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SchoolSystem.Helpers;

namespace SchoolSystem.Services
{
        /// <summary>
        /// محرك ترحيل قاعدة البيانات بإصدارات مُرقّمة.
        /// - عند توفّر نسخة احتياطية (.bak) في مجلد Database يستعيدها كاملة (الجداول
        ///   والبيانات والكائنات) بدلاً من إنشاء قاعدة فارغة من المخطط.
        /// - وإلا ينشئ قاعدة البيانات من الصفر عند غيابها (تثبيت أول لجهاز Standalone/Server).
        /// - يعتمد جدول SchemaMigrations لتسجيل تاريخ الترحيل ويمنع تكرار التنفيذ.
        /// - يطبّق كل ترحيل داخل معاملة مستقلّة، فيفشل بأمان دون ترك القاعدة نصف محدَّثة.
        /// - يستخدم sp_getapplock لمنع تشغيل أكثر من ترحيل في نفس اللحظة على نفس الخادم.
        /// - يعتمد "التبني كأساس" (Baseline Adoption): قاعدة موجودة مكتملة بلا تاريخ تُعتبر
        ///   عند الإصدار 1 دون إعادة تنفيذ المخطط.
        /// </summary>
    public static class DatabaseMigrationService
    {
        public const string HistoryTable = "dbo.SchemaMigrations";

        public const int InitialBaselineVersion = 1;

        /// <summary>
        /// يحدد ما إذا كانت آخر عملية Apply أنشأت قاعدة البيانات فعليًا.
        /// لا يُستخدم لتعديل قاعدة موجودة.
        /// </summary>
        public static bool DatabaseCreatedInLastRun { get; private set; }

        /// <summary>اسم ملف النسخة الاحتياطية التي تُستخدم للاستعادة عند التثبيت أول مرة.</summary>
        public const string BackupFileName = "SchoolDB_14480309_230530.bak";

        private static readonly Regex MigrationFilePattern =
            new Regex(@"^(\d{3,6})[_.]\s*(.+?)\.sql$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// عدد الترحيلات المطبَّقة في هذه الجولة (0 = لا تغيير، القاعدة محدَّثة بالفعل).
        /// </summary>
        public static int AppliedInLastRun { get; private set; }

        /// <summary>
        /// تنفيذ كل الترحيلات المعلّقة. يعيد وصفاً موجزاً للنتيجة.
        /// عند توفُّر نسخة احتياطية (.bak) في مجلد Database يُستعاد منها الكائن والبيانات
        /// بدلاً من إنشاء قاعدة فارغة من المخطط. إذا كانت replaceFromBackup=true يُستبدَل
        /// أي قاعدة هدف قائمة بالنسخة الاحتياطية دائماً (وضع الإعداد/الاستبدال الأولي).
        /// </summary>
        public static string Apply(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword, string databaseFolder,
            bool replaceFromBackup = false)
        {
            AppliedInLastRun = 0;
            DatabaseCreatedInLastRun = false;

            string databaseFolderFull =
                (databaseFolder ?? string.Empty).Trim();

            string initialFolder = Path.Combine(databaseFolderFull, "initial");
            string migrationsFolder = Path.Combine(databaseFolderFull, "migrations");

            // 1) تجهيز القاعدة: إنشاء/استعادة/استبدال.
            string masterConnection = BuildConnection(server, "master", useSqlAuth, sqlUser, sqlPassword, 60);
            string backupPath = ResolveBackupFile(databaseFolderFull);
            bool replaceRequested = replaceFromBackup && !string.IsNullOrWhiteSpace(backupPath);

            bool existed = DatabaseExists(masterConnection, database);
            if (!existed)
            {
                // قاعدة غير موجودة: نستعيد من النسخة الاحتياطية إن وُجدت، وإلا ننشئ قاعدة فارغة.
                if (!string.IsNullOrWhiteSpace(backupPath))
                {
                    ApplicationLogger.Log("DatabaseMigration",
                        "استعادة قاعدة البيانات من النسخة الاحتياطية: " + backupPath);
                    RestoreDatabase(masterConnection, database, backupPath);
                }
                else
                {
                    CreateDatabase(masterConnection, database);
                }
            }
            else if (replaceRequested)
            {
                // قاعدة قائمة مع طلب استبدال صريح (واجهة الإعداد): نحذفها ونستعيدها كاملة من النسخة.
                ApplicationLogger.Log("DatabaseMigration",
                    "استبدال قاعدة البيانات الحالية بالنسخة الاحتياطية: " + backupPath);
                RestoreDatabase(masterConnection, database, backupPath);
            }

            string dbConnection = BuildConnection(server, database, useSqlAuth, sqlUser, sqlPassword, 60);

            int currentVersion = 0;

            // 2) قفل لمنع الترحيل المتزامن.
            using (SqlConnection con = new SqlConnection(dbConnection))
            {
                con.Open();
                SetSessionOptions(con);
                AcquireMigrationLock(con);
                try
                {
                    EnsureHistoryTable(con);

                    currentVersion = ReadCurrentVersion(con);

                    if (currentVersion == 0 && IsDatabasePopulated(con))
                    {
                        // قاعدة موجودة مكتملة بلا سجل ترحيل → اعتمدها كأساس إصدار 1.
                        RecordMigration(con, InitialBaselineVersion, "baseline-adopted-existing-schema");
                        currentVersion = InitialBaselineVersion;
                        AppliedInLastRun = 1;
                    }

                    // 3) تطبيق المخطط الأولي على قاعدة فارغة جديدة.
                    if (currentVersion == 0)
                    {
                        ApplyInitialSchema(con, initialFolder);

                        /*
                         * بعض نسخ databaseschool.sql هي Snapshot حالي للمخطط وليست
                         * مخطط الإصدار 1 فقط؛ وقد تحتوي بالفعل على SchemaMigrations
                         * مثل 1 و3. بعد التنفيذ نقرأ التاريخ مرة أخرى ونبدأ من آخر
                         * نسخة مسجلة بدل محاولة تسجيل 1 مكررًا.
                         */
                        currentVersion = ReadCurrentVersion(con);

                        if (currentVersion == 0)
                        {
                            RecordMigration(con, InitialBaselineVersion, "initial-schema");
                            currentVersion = InitialBaselineVersion;
                            AppliedInLastRun++;
                        }
                    }

                    // 4) تطبيق الترحيلات المرقّمة الأحدث.
                    if (Directory.Exists(migrationsFolder))
                    {
                        var pending = LoadPendingMigrations(migrationsFolder, currentVersion);
                        foreach (MigrationScript script in pending)
                        {
                            ApplySingleMigration(con, script);
                            RecordMigration(con, script.Version, script.Name);
                            currentVersion = script.Version;
                            AppliedInLastRun++;
                        }
                    }
                }
                finally
                {
                    ReleaseMigrationLock(con);
                }
            }

            if (AppliedInLastRun == 0)
                return "قاعدة البيانات محدَّثة بالفعل (الإصدار " + currentVersion + ").";
            return "تم تطبيق " + AppliedInLastRun + " ترحيل بنجاح، الإصدار الحالي: " + currentVersion + ".";
        }

        /// <summary>اسم ملف السكربت الكامل الذي يُنشئ قاعدة البيانات من الصفر (v3 dump).</summary>
        public const string FullSchemaScriptFileName = "databaseschool.sql";

        /// <summary>
        /// إنشاء قاعدة البيانات من سكربت SQL كامل (databaseschool.sql) بدلاً من استعادة نسخة
        /// احتياطية. يضمن:
        ///  - إنشاء القاعدة الفارغة (أو حذف القائمة إن طُلب ذلك ثم إعادة إنشائها).
        ///  - تنفيذ سكربت المخطط والبيانات كاملاً (يُنشئ الجداول والإجراءات والبيانات).
        ///  - تطبيق الترحيلات المرقّمة المعلّقة بعد ذلك (مثل 003_EnsureAcademicReference).
        /// يعيد وصفاً موجزاً للنتيجة.
        /// </summary>
        public static string CreateFromSqlScript(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword, string databaseFolder,
            bool replaceExisting = false)
        {
            AppliedInLastRun = 0;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");

            string databaseFolderFull = (databaseFolder ?? string.Empty).Trim();
            string scriptPath = Path.Combine(databaseFolderFull, FullSchemaScriptFileName);
            if (!File.Exists(scriptPath))
                throw new FileNotFoundException(
                    "لم يتم العثور على سكربت إنشاء قاعدة البيانات: " + scriptPath);

            string migrationsFolder = Path.Combine(databaseFolderFull, "migrations");
            string masterConnection = BuildConnection(server, "master", useSqlAuth, sqlUser, sqlPassword, 60);

            bool existing = DatabaseExists(masterConnection, database);
            if (existing && replaceExisting)
            {
                ApplicationLogger.Log("DatabaseMigration",
                    "حذف قاعدة البيانات الحالية لإنشائها من جديد من السكربت: " + database);
                DropDatabase(masterConnection, database);
                existing = false;
            }

            if (!existing)
            {
                ApplicationLogger.Log("DatabaseMigration",
                    "إنشاء قاعدة البيانات من السكربت الكامل: " + scriptPath);
                CreateDatabase(masterConnection, database);
            }

            string dbConnection = BuildConnection(server, database, useSqlAuth, sqlUser, sqlPassword, 60);

            // تنفيذ سكربت المخطط والبيانات كاملاً.
            using (SqlConnection con = new SqlConnection(dbConnection))
            {
                con.Open();
                SetSessionOptions(con);
                string scriptText = ReadSqlScript(scriptPath);
                // السكربت الكامل يُنفّذ داخل قاعدة البيانات الهدف بعد حذف مقدمته
                // التي تتضمن CREATE DATABASE / USE.
                string scriptBody = StripPreamble(scriptText);
                ExecuteScript(con, scriptBody);

                // تثبيت baseline بعد نجاح إنشاء المخطط الكامل حتى يعرف محرك الترحيل
                // أن القاعدة بدأت من الإصدار 1 ولا يعيد تنفيذ المخطط عليها.
                EnsureHistoryTable(con);
                if (ReadCurrentVersion(con) == 0)
                    RecordMigration(con, InitialBaselineVersion, "initial-schema-script");
            }

            // أي migrations مرقّمة موجودة في الحزمة تُطبّق بالطريقة القياسية نفسها.
            // الدالة Apply لا تستبدل قاعدة موجودة ولا تحذف البيانات.
            string migrationResult = Apply(
                server,
                database,
                useSqlAuth,
                sqlUser,
                sqlPassword,
                databaseFolderFull);

            if (AppliedInLastRun == 0)
                return "تم إنشاء قاعدة البيانات من السكربت وتسجيل الـ baseline بنجاح.";
            return "تم إنشاء قاعدة البيانات وتحديثها بنجاح. " + migrationResult;
        }

        /// <summary>
        /// يقرأ نص سكربت SQL كاملاً ويتعامل مع ترميز UTF-16 LE (المنتج من تصدير SQL Server)
        /// وترميز UTF-8 على حد سواء.
        /// </summary>
        private static string ReadSqlScript(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            // كشف بوم (BOM) UTF-16 LE/ BE أو BE.
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return System.Text.Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return System.Text.Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);

            // ترميز Unicode بلا بوم شائع في تصديرات SQL Server (كل حرف = 2 بايت).
            if (LooksLikeUtf16(bytes))
                return System.Text.Encoding.Unicode.GetString(bytes);

            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        /// <summary>تقدير تقريبي: إذا تتابع بايتات بفارق صفر بين الأحرف فغالباً النص UTF-16.</summary>
        private static bool LooksLikeUtf16(byte[] bytes)
        {
            if (bytes.Length < 4)
                return false;
            int zeros = 0;
            int total = Math.Min(bytes.Length, 2048) / 2;
            for (int i = 1; i < bytes.Length && i < 2048; i += 2)
            {
                if (bytes[i] == 0)
                    zeros++;
            }
            // أكثر من نصف البايتات الثانية صفر؟ يشير إلى ترميز UTF-16 LE.
            return total > 0 && (double)zeros / total > 0.4;
        }

        /// <summary>حذف قاعدة بيانات قائمة (يضعها في وضع مستخدم واحد ثم يحذفها).</summary>
        private static void DropDatabase(string masterConnection, string database)
        {
            string safe = new Regex("^[A-Za-z0-9_]+$").Match(database).Value;
            if (string.IsNullOrWhiteSpace(safe))
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");

            using (SqlConnection con = new SqlConnection(masterConnection))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(String.Format(@"
                    IF DB_ID(N'{0}') IS NOT NULL
                    BEGIN
                        ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{0}];
                    END", safe), con))
                {
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// يقتطع "المقدمة" الرأسية من سكربت تصدير SQL Server — أي كل ما يسبق أول كائن
        /// مولَّد/معرَّف بالتعليق النموذجي "/****** Object:" (بداية جسم التصدير الفعلي).
        /// تشمل المقدمة عادة عبارة USE [SchoolDB] وكذلك المقدمة التي تضيف إنشاء القاعدة
        /// تلقائياً (IF DB_ID... CREATE DATABASE). نحذفها نهائياً لأن التطبيق متصل مباشرة
        /// بقاعدة البيانات الهدف وقد أنشأها بنفسه بالاسم الصحيح، فنمنع إنشاء قاعدة خاطئة
        /// أو تنفيذ السكربت خطأً على قاعدة master.
        /// </summary>
        private static string StripPreamble(string scriptText)
        {
            if (string.IsNullOrWhiteSpace(scriptText))
                return scriptText;
            int idx = scriptText.IndexOf("/****** Object:", StringComparison.Ordinal);
            if (idx <= 0)
                return scriptText;
            return scriptText.Substring(idx);
        }

        // ------------------------------------------------------------------ helpers

        /// <summary>
        /// يضبط خيارات الجلسة اللازمة للعديد من عبارات DDL (الفهارس الفريدة/المفلترة،
        /// الأعمدة المحسوبة، الدوال وما شابه) قبل بدء أي ترحيل.
        /// </summary>
        private static void SetSessionOptions(SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET ANSI_PADDING ON;" +
                "SET ARITHABORT ON; SET ANSI_WARNINGS ON;", con))
            {
                cmd.CommandTimeout = 30;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// <summary>
        /// يبحث عن ملف نسخة احتياطية (.bak) في مجلد قاعدة البيانات المزوَّد، مفضِّلاً
        /// الاسم الثابت المهارتَج إن وُجد (BackupFileName)، ثم أي ملف .bak أول.
        /// يعيد مسار الملف أو سلسلة فارغة إن لم يوجد.
        /// </summary>
        private static string ResolveBackupFile(string databaseFolder)
        {
            if (string.IsNullOrWhiteSpace(databaseFolder) || !Directory.Exists(databaseFolder))
                return string.Empty;

            string fixedName = Path.Combine(databaseFolder, BackupFileName);
            if (File.Exists(fixedName))
                return fixedName;

            string direct = Directory.GetFiles(databaseFolder, "*.bak", SearchOption.TopDirectoryOnly)
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            return direct ?? string.Empty;
        }

        /// <summary>
        /// يستعيد مسار مجلد بيانات SQL Server الافتراضي من إعدادات الخادم إن أمكن،
        /// وإلا يعيد سلسلة فارغة ليعود المتصل إلى قيمة بديلة.
        /// </summary>
        private static string ResolveServerDataPath(SqlConnection masterCon, string database)
        {
            try
            {
                masterCon.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT SERVERPROPERTY('InstanceDefaultDataPath') AS DataPath;", masterCon))
                {
                    cmd.CommandTimeout = 30;
                    object result = cmd.ExecuteScalar();
                    masterCon.Close();
                    if (result == null || result == DBNull.Value)
                        return string.Empty;
                    string path = Convert.ToString(result).Trim();
                    return path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? path : path + "\\";
                }
            }
            catch
            {
                try { masterCon.Close(); } catch { }
                return string.Empty;
            }
        }

        /// <summary>
        /// يستعيد قاعدة البيانات كاملة من ملف نسخة احتياطية .bak. يحلّل محتويات
        /// الملف تلقائياً (RESTORE FILELISTONLY) ليعيد تسمية ملفات MDF/LDF بأسماء
        /// القاعدة الهدف قبل الاستعادة، فيعمل حتى لو اختلف اسم القاعدة في النسخة.
        /// </summary>
        private static void RestoreDatabase(string masterConnection, string database, string backupPath)
        {
            string safe = new Regex("^[A-Za-z0-9_]+$").Match(database).Value;
            if (string.IsNullOrWhiteSpace(safe))
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");

            if (!File.Exists(backupPath))
                throw new FileNotFoundException("ملف النسخ الاحتياطية غير موجود: " + backupPath);

            string workingCopy = backupPath;
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "SchoolSystemRestore");
                Directory.CreateDirectory(tempDir);
                string tempCopy = Path.Combine(tempDir, Path.GetFileName(backupPath));
                File.Copy(backupPath, tempCopy, true);
                workingCopy = tempCopy;
            }
            catch { workingCopy = backupPath; }

            try
            {
            using (SqlConnection con = new SqlConnection(masterConnection))
            using (SqlCommand fileCmd = new SqlCommand(
                "RESTORE FILELISTONLY FROM DISK = @bak;", con))
            {
                fileCmd.CommandTimeout = 120;
                fileCmd.Parameters.Add("@bak", SqlDbType.NVarChar, 500).Value = workingCopy;

                List<RestoreFileInfo> files = new List<RestoreFileInfo>();
                con.Open();
                using (SqlDataReader reader = fileCmd.ExecuteReader())
                {
                    int logicalNameOrd = reader.GetOrdinal("LogicalName");
                    int typeOrd = reader.GetOrdinal("Type");
                    while (reader.Read())
                    {
                        files.Add(new RestoreFileInfo
                        {
                            LogicalName = reader.GetString(logicalNameOrd),
                            Type = reader.GetString(typeOrd)
                        });
                    }
                }
                con.Close();

                if (files.Count == 0)
                    throw new InvalidOperationException(
                        "تعذر تحليل محتويات ملف النسخة الاحتياطية: " + backupPath);

                var dataFile = files.FirstOrDefault(f => f.Type == "D");
                var logFile = files.FirstOrDefault(f => f.Type == "L");

                // بيانات/mفتره: نزيل أي قاعدة هدف موجودة ونستعيدها مع إعادة تسمية الملفات.
                using (SqlCommand dropCmd = new SqlCommand(String.Format(@"
                    IF DB_ID(N'{0}') IS NOT NULL
                    BEGIN
                        ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{0}];
                    END", safe), con))
                {
                    con.Open();
                    dropCmd.CommandTimeout = 300;
                    dropCmd.ExecuteNonQuery();
                    con.Close();
                }

                // مجلد بيانات SQL Server الافتراضي كي توضع الملفات ضمن بيانات الخادم.
                string dataRoot = ResolveServerDataPath(con, safe);
                if (string.IsNullOrWhiteSpace(dataRoot))
                    dataRoot = "C:\\Program Files\\Microsoft SQL Server\\MSSQL16.MSSQLSERVER\\MSSQL\\DATA";

                string physicalData = Path.Combine(dataRoot, safe + ".mdf");
                string physicalLog = Path.Combine(dataRoot, safe + "_log.ldf");

                // بناء عبارة RESTORE مع عبارات MOVE لكل ملف بيانات/سجل.
                string moveArgs = string.Empty;
                if (dataFile != null)
                    moveArgs += " , MOVE @LogicalData TO @PhysicalData";
                if (logFile != null)
                    moveArgs += " , MOVE @LogicalLog TO @PhysicalLog";

                string restoreSql = string.Format(
                    "RESTORE DATABASE [{0}] FROM DISK = @bak WITH REPLACE, RECOVERY{1};",
                    safe, moveArgs);

                using (SqlCommand restoreCmd = new SqlCommand(restoreSql, con))
                {
                    restoreCmd.CommandTimeout = 900;
                    restoreCmd.Parameters.Add("@bak", SqlDbType.NVarChar, 500).Value = workingCopy;

                    if (dataFile != null)
                    {
                        restoreCmd.Parameters.Add("@LogicalData", SqlDbType.NVarChar, 128).Value = dataFile.LogicalName;
                        restoreCmd.Parameters.Add("@PhysicalData", SqlDbType.NVarChar, 500).Value = physicalData;
                    }
                    if (logFile != null)
                    {
                        restoreCmd.Parameters.Add("@LogicalLog", SqlDbType.NVarChar, 128).Value = logFile.LogicalName;
                        restoreCmd.Parameters.Add("@PhysicalLog", SqlDbType.NVarChar, 500).Value = physicalLog;
                    }

                    con.Open();
                    restoreCmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            }
            finally
            {
                try
                {
                    if (workingCopy != backupPath && File.Exists(workingCopy))
                        File.Delete(workingCopy);
                }
                catch { }
            }
        }

        private static string BuildConnection(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword, int timeout)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                TrustServerCertificate = true,
                ConnectTimeout = timeout
            };
            if (useSqlAuth)
            {
                b.IntegratedSecurity = false;
                b.UserID = sqlUser ?? string.Empty;
                b.Password = sqlPassword ?? string.Empty;
            }
            else
            {
                b.IntegratedSecurity = true;
            }
            return b.ConnectionString;
        }

        private static bool DatabaseExists(string masterConnection, string database)
        {
            using (SqlConnection con = new SqlConnection(masterConnection))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT CASE WHEN DB_ID(@db) IS NOT NULL THEN 1 ELSE 0 END", con))
                {
                    cmd.CommandTimeout = 30;
                    cmd.Parameters.Add("@db", SqlDbType.NVarChar, 128).Value = database;
                    return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                }
            }
        }

        private static void CreateDatabase(string masterConnection, string database)
        {
            string safe = new Regex("^[A-Za-z0-9_]+$").Match(database).Value;
            if (string.IsNullOrWhiteSpace(safe))
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");

            using (SqlConnection con = new SqlConnection(masterConnection))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = "CREATE DATABASE [" + safe + "];";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void AcquireMigrationLock(SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand(
                "DECLARE @res INT; " +
                "EXEC @res = sp_getapplock @Resource = N'SchoolSystem_Migration', " +
                "@LockMode = 'Exclusive', @LockOwner = 'Session', @LockTimeout = 600000; " +
                "IF @res < 0 THROW 51000, N'تعذر الحصول على قفل الترحيل؛ قد يكون تحديث آخر قيد التشغيل.', 1;", con))
            {
                cmd.CommandTimeout = 700;
                cmd.ExecuteNonQuery();
            }
        }

        private static void ReleaseMigrationLock(SqlConnection con)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "EXEC sp_releaseapplock @Resource = N'SchoolSystem_Migration', @LockOwner = 'Session';", con))
                {
                    cmd.CommandTimeout = 30;
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // يُطلق القفل تلقائياً عند نهاية الجلسة.
            }
        }

        private static void EnsureHistoryTable(SqlConnection con)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.SchemaMigrations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchemaMigrations
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchemaMigrations PRIMARY KEY,
        Version INT NOT NULL,
        Name NVARCHAR(255) NOT NULL,
        AppliedAtUtc DATETIME NOT NULL CONSTRAINT DF_SchemaMigrations_AppliedAtUtc DEFAULT GETUTCDATE(),
        Success BIT NOT NULL CONSTRAINT DF_SchemaMigrations_Success DEFAULT (1),
        CONSTRAINT UQ_SchemaMigrations_Version UNIQUE (Version)
    );
END;";
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandTimeout = 60;
                cmd.ExecuteNonQuery();
            }
        }

        private static int ReadCurrentVersion(SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT ISNULL(MAX(Version), 0) FROM dbo.SchemaMigrations;", con))
            {
                cmd.CommandTimeout = 30;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static bool IsDatabasePopulated(SqlConnection con)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT CASE WHEN OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL THEN 1 ELSE 0 END;", con))
                {
                    cmd.CommandTimeout = 30;
                    return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void ApplyInitialSchema(SqlConnection con, string initialFolder)
        {
            string[] files = Directory.Exists(initialFolder)
                ? Directory.GetFiles(initialFolder, "*.sql")
                    .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                    .ToArray()
                : new string[0];

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    string script = ReadSqlScript(file);
                    ExecuteScript(con, script);
                }

                return;
            }

            /*
               بعض الحزم تحتوي فقط على databaseschool.sql ولا تحتوي مجلد initial.
               قاعدة جديدة يجب ألا تفشل بسبب ذلك؛ نستخدم السكربت الكامل كمصدر المخطط
               ونزيل مقدمته التي تحتوي CREATE DATABASE/USE قبل التنفيذ على القاعدة الهدف.
            */
            string fullSchemaPath = Path.Combine(
                Path.GetDirectoryName(initialFolder) ?? string.Empty,
                FullSchemaScriptFileName);

            if (File.Exists(fullSchemaPath))
            {
                string fullScript = ReadSqlScript(fullSchemaPath);
                ExecuteScript(con, StripPreamble(fullScript));
                return;
            }

            throw new FileNotFoundException(
                "لم يتم العثور على مخطط قاعدة البيانات الأولي. ابحث عن مجلد initial أو الملف " +
                FullSchemaScriptFileName + " داخل مجلد Database.");
        }

        private static List<MigrationScript> LoadPendingMigrations(string migrationsFolder, int currentVersion)
        {
            List<MigrationScript> result = new List<MigrationScript>();

            foreach (string file in Directory.GetFiles(migrationsFolder, "*.sql"))
            {
                string name = Path.GetFileName(file);
                Match match = MigrationFilePattern.Match(name);
                if (!match.Success)
                    continue;

                int version = int.Parse(match.Groups[1].Value);
                if (version <= currentVersion)
                    continue;

                result.Add(new MigrationScript
                {
                    Version = version,
                    Name = match.Groups[2].Value.Trim(),
                    Path = file
                });
            }

            return result
                .OrderBy(m => m.Version)
                .ToList();
        }

        private static void ApplySingleMigration(SqlConnection con, MigrationScript script)
        {
            string scriptText = File.ReadAllText(script.Path);
            ExecuteScript(con, scriptText);
        }

        private static void ExecuteScript(SqlConnection con, string scriptText)
        {
            // تقسيم النص إلى دفعات مع إدراك لنصوص SQL (سلاسل وأقواس وتعليقات) حتى لا
            // يُقصّ Go الموجود داخل السلسل النصية، ثم تنفيذ كل دفعة داخل معاملة واحدة
            // بحيث يفشل الترحيل بأمان دون نصف تحديث.
            string[] batches = SplitSqlBatches(scriptText);

            // عبارات USE (تبديل سياق قاعدة البيانات) و CREATE DATABASE لا يمكن تنفيذها
            // داخل معاملة في SQL Server ("Cannot use the USE statement within a
            // transaction" / "CREATE DATABASE statement not allowed within multi-
            // statement transaction")، لذلك ننفّذها مباشرة على الاتصال قبل فتح المعاملة.
            // السكربت الكامل (databaseschool.sql) يبدأ بمقدمة تنشئ القاعدة إن لم تكن موجودة
            // ثم USE [SchoolDB]، فلا بد من معالجتها خارج المعاملة.
            foreach (string batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch))
                    continue;
                if (RequiresDirectExecution(batch))
                {
                    using (SqlCommand cmd = new SqlCommand(batch, con))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            using (SqlTransaction tx = con.BeginTransaction())
            {
                try
                {
                    foreach (string batch in batches)
                    {
                        if (string.IsNullOrWhiteSpace(batch))
                            continue;
                        if (RequiresDirectExecution(batch))
                            continue;
                        using (SqlCommand cmd = new SqlCommand(batch, con, tx))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// يحدّد ما إذا كانت الدفعة يجب أن تُنفَّذ مباشرة على الاتصال خارج المعاملة، لأن
        /// SQL Server يحظرها داخل معاملة نشطة. تغطي هذه عبارات تبديل السياق (USE [اسم])
        /// وعبارات إنشاء قاعدة بيانات (CREATE DATABASE) الظاهرة في مقدمة السكربت الكامل.
        /// </summary>
        private static bool RequiresDirectExecution(string batch)
        {
            string trimmed = batch.Trim();
            if (Regex.IsMatch(trimmed, @"(?i)^USE\s+\[[^\]]*\]\s*;?\s*$"))
                return true;
            if (Regex.IsMatch(trimmed, @"(?i)^CREATE\s+DATABASE\s+"))
                return true;
            if (Regex.IsMatch(trimmed, @"(?i)^IF\s+DB_ID\s*\(.*\bIS\s+NULL\b"))
                return true;
            return false;
        }

        /// <summary>
        /// تقسيم نص SQL إلى دفعات عند فواصل GO المستقلة، متجاهلاً GO داخل السلاسل
        /// النصية أو الأقواس المربعة أو التعليقات.
        /// </summary>
        private static string[] SplitSqlBatches(string scriptText)
        {
            List<string> batches = new List<string>();
            System.Text.StringBuilder current = new System.Text.StringBuilder();

            int i = 0;
            int n = scriptText.Length;
            bool atBatchStart = true;

            // الحالة: في وضع عادي، نرصد بداية السطر لالتقاط GO المستقل.
            while (i < n)
            {
                char c = scriptText[i];
                char next = (i + 1 < n) ? scriptText[i + 1] : '\0';

                // بداية تعليق سطر.
                if (c == '-' && next == '-')
                {
                    current.Append(scriptText, i, 2);
                    i += 2;
                    while (i < n && scriptText[i] != '\n')
                    {
                        current.Append(scriptText[i]);
                        i++;
                    }
                    atBatchStart = false;
                    continue;
                }

                // بداية تعليق كتلة.
                if (c == '/' && next == '*')
                {
                    current.Append(scriptText, i, 2);
                    i += 2;
                    while (i < n)
                    {
                        current.Append(scriptText[i]);
                        if (scriptText[i] == '*' && i + 1 < n && scriptText[i + 1] == '/')
                        {
                            current.Append('/');
                            i += 2;
                            break;
                        }
                        i++;
                    }
                    if (i == n)
                        break;
                    atBatchStart = false;
                    continue;
                }

                // سلسلة نصية مفردة أو مزدوجة.
                if (c == '\'' || c == '"' || c == '[')
                {
                    char closing = (c == '\'') ? '\'' : (c == '"' ? '"' : ']');
                    current.Append(c);
                    i++;
                    while (i < n)
                    {
                        if (scriptText[i] == closing)
                        {
                            current.Append(closing);
                            i++;
                            break;
                        }
                        current.Append(scriptText[i]);
                        i++;
                    }
                    atBatchStart = false;
                    continue;
                }

                // عند بداية السطر: رصد GO المستقل.
                if (atBatchStart && (c == 'g' || c == 'G') && (next == 'o' || next == 'O'))
                {
                    // تحقق من أن هذا ليس اسم عمود/متغير ويليه مسافة/نهاية سطر.
                    int j = i + 2;
                    while (j < n && (scriptText[j] == ' ' || scriptText[j] == '\t'))
                        j++;
                    if (j >= n || scriptText[j] == '\n' || scriptText[j] == '\r')
                    {
                        // فاصل GO حقيقي.
                        batches.Add(current.ToString());
                        current.Clear();
                        i = j;
                        // تخطي نهاية السطر.
                        while (i < n && (scriptText[i] == '\r' || scriptText[i] == '\n'))
                            i++;
                        atBatchStart = true;
                        continue;
                    }
                }

                if (c == '\n')
                    atBatchStart = true;
                else if (!char.IsWhiteSpace(c))
                    atBatchStart = false;

                current.Append(c);
                i++;
            }

            if (current.Length > 0)
                batches.Add(current.ToString());

            return batches.ToArray();
        }

        private static void RecordMigration(SqlConnection con, int version, string name)
        {
            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.SchemaMigrations (Version, Name, Success)
VALUES (@version, @name, 1);", con))
            {
                cmd.CommandTimeout = 30;
                cmd.Parameters.Add("@version", SqlDbType.Int).Value = version;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 255).Value = name;
                cmd.ExecuteNonQuery();
            }
        }

        private sealed class MigrationScript
        {
            public int Version { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
        }

        /// <summary>معلومات ملف منطقي داخل نسخة احتياطية (نتيجة RESTORE FILELISTONLY).</summary>
        private sealed class RestoreFileInfo
        {
            public string LogicalName { get; set; }
            public string Type { get; set; }
        }
    }
}