using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SchoolSystem.Security;
using SchoolSystem.Helpers;


namespace SchoolSystem.Services
{
    // تعريف الصنف DatabaseBackupService المسؤول عن وظائف طبقة Services.
    public class DatabaseBackupService
    {
        // تعريف الدالة Regex لتنفيذ العملية المرتبطة بها.
        private static readonly Regex SafeIdentifier = new Regex("^[A-Za-z0-9_\\-]+$", RegexOptions.Compiled);

        // تعريف الدالة TestConnection لتنفيذ العملية المرتبطة بها.
        public void TestConnection(string serverInstance, string databaseName)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny("ليس لديك صلاحية اختبار اتصال قاعدة البيانات.", PermissionKeys.SettingsView, PermissionKeys.SettingsManage);
            using (SqlConnection connection = CreateConnection(serverInstance, databaseName))
            {

                connection.Open();
            }
        }

        // تعريف الدالة Backup لتنفيذ العملية المرتبطة بها.
        public string Backup(string serverInstance, string databaseName, string backupDirectory)
        {
            return Backup(serverInstance, databaseName, backupDirectory, false);
        }

        /// <summary>
        /// إنشاء نسخة احتياطية من قاعدة البيانات.
        /// عند استدعائه كجزء من الإغلاق السنوي، تكفي صلاحية الإغلاق السنوي بالإضافة إلى
        /// صلاحيات النسخ الاحتياطي الاعتيادية، مع بقاء التحقق من الصلاحيات إلزاميًا.
        /// </summary>
        public string Backup(string serverInstance, string databaseName, string backupDirectory, bool forAnnualClosing)
        {
            if (forAnnualClosing)
                CurrentUser.DemandAny(
                    "ليس لديك صلاحية إنشاء نسخة احتياطية ضمن الإغلاق السنوي.",
                    PermissionKeys.AnnualClosingManage,
                    PermissionKeys.SettingsEdit,
                    PermissionKeys.SettingsManage);
            else
                CurrentUser.DemandAny(
                    "ليس لديك صلاحية إنشاء نسخة احتياطية لقاعدة البيانات.",
                    PermissionKeys.SettingsEdit,
                    PermissionKeys.SettingsManage);

            ValidateInputs(serverInstance, databaseName);
            string directory = EnsureBackupDirectory(backupDirectory);

            string backupFile = Path.Combine(
                directory,
                databaseName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak");

            using (SqlConnection connection = CreateConnection(serverInstance, "master"))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;

                command.CommandText = "BACKUP DATABASE [" + databaseName + "] TO DISK = @backupFile WITH CHECKSUM, INIT, STATS = 10;";

                command.Parameters.Add("@backupFile", SqlDbType.NVarChar, 4000).Value = backupFile;

                connection.Open();

                command.ExecuteNonQuery();
            }

            RetainBackups(directory, databaseName);

            return backupFile;
        }

        /// <summary>عدد النسخ الاحتياطية التي يُحتفظ بها لكل قاعدة بيانات (سياسة الاحتفاظ).</summary>
        public const int BackupRetentionCount = 10;

        // تعريف الدالة RetainBackups لتنفيذ العملية المرتبطة بها.
        // يحذف أقدم النسخ الاحتياطية عند تجاوز عدد النسخ المحفوظة للقاعدة.
        private static void RetainBackups(string backupDirectory, string databaseName)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (backupDirectory == null || !Directory.Exists(backupDirectory))
                    return;

                string prefix = databaseName + "_";

                var backups = new DirectoryInfo(backupDirectory)
                    .GetFiles(prefix + "*.bak")
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();

                if (backups.Count <= BackupRetentionCount)
                    return;

                IEnumerable<FileInfo> toDelete = backups.Skip(BackupRetentionCount);
                foreach (FileInfo file in toDelete)
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        file.Delete();
                    }
                    catch (IOException ex)
                    {
                        ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": حذف نسخة احتياطية قديمة", ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": حذف نسخة احتياطية قديمة (صلاحيات)", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": تنظيف النسخ الاحتياطية القديمة", ex);
                // فشل التنظيف لا يمنع نجاح النسخ الاحتياطي الحالي.
            }
        }

        // تعريف الدالة Restore لتنفيذ العملية المرتبطة بها.
        public void Restore(string serverInstance, string backupFile, string targetDatabase, bool replaceExisting)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny("ليس لديك صلاحية استعادة قاعدة البيانات.", PermissionKeys.SettingsEdit, PermissionKeys.SettingsManage);
            ValidateInputs(serverInstance, targetDatabase);
            if (string.IsNullOrWhiteSpace(backupFile) || !File.Exists(backupFile))

                throw new FileNotFoundException("ملف النسخة الاحتياطية غير موجود.", backupFile);

            string fullBackupPath = Path.GetFullPath(backupFile);
            string restoreSql;
            if (replaceExisting)
            {
                restoreSql = @"
BEGIN TRY
    IF DB_ID(@targetDatabase) IS NOT NULL
        ALTER DATABASE [" + targetDatabase + @"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    RESTORE DATABASE [" + targetDatabase + @"] FROM DISK = @backupFile WITH REPLACE, RECOVERY, CHECKSUM, STATS = 10;
    ALTER DATABASE [" + targetDatabase + @"] SET MULTI_USER;
END TRY
BEGIN CATCH
    IF DB_ID(@targetDatabase) IS NOT NULL
    BEGIN
        BEGIN TRY
            ALTER DATABASE [" + targetDatabase + @"] SET MULTI_USER;
        END TRY
        BEGIN CATCH
            -- Preserve the original RESTORE error while attempting recovery.
        END CATCH
    END
    THROW;
END CATCH;";
            }

            else
            {
                restoreSql = @"
IF DB_ID(@targetDatabase) IS NOT NULL
    THROW 50001, 'قاعدة البيانات الهدف موجودة. اختر اسماً جديداً أو فعّل الاستبدال.', 1;
RESTORE DATABASE [" + targetDatabase + @"] FROM DISK = @backupFile WITH RECOVERY, CHECKSUM, STATS = 10;";
            }

            using (SqlConnection connection = CreateConnection(serverInstance, "master"))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;

                command.CommandText = restoreSql;

                command.Parameters.Add("@targetDatabase", SqlDbType.NVarChar, 128).Value = targetDatabase;

                command.Parameters.Add("@backupFile", SqlDbType.NVarChar, 4000).Value = fullBackupPath;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        // تعريف الدالة CreateConnection لتنفيذ العملية المرتبطة بها.
        private static SqlConnection CreateConnection(string serverInstance, string databaseName)
        {
            ApplicationSettingsData saved =
                ApplicationSettingsService.Load();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverInstance.Trim(),
                InitialCatalog = databaseName.Trim(),
                TrustServerCertificate = true,
                ConnectTimeout = 15
            };

            if (saved != null && saved.UseSqlAuthentication && !string.IsNullOrWhiteSpace(saved.SqlUser))
            {
                builder.IntegratedSecurity = false;
                builder.UserID = saved.SqlUser.Trim();
                builder.Password = ApplicationSettingsService.GetSqlPassword(saved);
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            return new SqlConnection(builder.ConnectionString);
        }

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
        private static void ValidateInputs(string serverInstance, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(serverInstance))

                throw new ArgumentException("يجب تحديد اسم خادم SQL Server.");

            string normalizedServer = serverInstance.Trim();
            if (normalizedServer.Length > 128 || normalizedServer.Any(char.IsControl) || normalizedServer.IndexOf(';') >= 0)

                throw new ArgumentException("اسم خادم SQL Server غير صالح.");

            string normalizedDatabase = databaseName == null ? string.Empty : databaseName.Trim();
            if (normalizedDatabase.Length > 128 || string.IsNullOrWhiteSpace(normalizedDatabase) ||
                !SafeIdentifier.IsMatch(normalizedDatabase))

                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");
        }

        // تعريف الدالة EnsureBackupDirectory لتنفيذ العملية المرتبطة بها.
        private static string EnsureBackupDirectory(string backupDirectory)
        {
            if (string.IsNullOrWhiteSpace(backupDirectory))

                throw new ArgumentException("يجب تحديد مجلد النسخ الاحتياطي.");

            string directory;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                directory = Path.GetFullPath(backupDirectory.Trim());
            }

            catch (Exception ex)
            {

                ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": التحقق من مسار مجلد النسخ الاحتياطي", ex);

                throw new ArgumentException("مسار مجلد النسخ الاحتياطي غير صالح.", ex);
            }

            string applicationDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string normalizedDirectory = directory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (string.Equals(normalizedDirectory, applicationDirectory, StringComparison.OrdinalIgnoreCase) ||
                normalizedDirectory.StartsWith(applicationDirectory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                normalizedDirectory.StartsWith(applicationDirectory + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))

                throw new InvalidOperationException("يجب حفظ النسخ الاحتياطية خارج مجلد البرنامج.");

            if (File.Exists(directory))

                throw new IOException("المسار المحدد لنسخ قاعدة البيانات هو ملف وليس مجلداً.");

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Directory.CreateDirectory(directory);
                if (Directory.Exists(directory))
                    return directory;
            }

            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": إنشاء مجلد النسخ الاحتياطي", ex);
                string fallback = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem", "Backups");
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    Directory.CreateDirectory(fallback);
                    if (Directory.Exists(fallback))
                        return fallback;
                }

                catch (Exception fallbackException) when (fallbackException is IOException || fallbackException is UnauthorizedAccessException || fallbackException is NotSupportedException)
                {

                    ApplicationLogger.LogException("خدمة " + nameof(DatabaseBackupService) + ": إنشاء مجلد النسخ الاحتياطي (المسار البديل)", fallbackException);

                    throw new IOException("تعذر إنشاء مجلد النسخ الاحتياطي. اختر مجلداً محلياً صالحاً مثل D:\\SchoolSystemBackups ثم أعد المحاولة.", ex);
                }
            }


            throw new IOException("تعذر إنشاء مجلد النسخ الاحتياطي. اختر مجلداً محلياً صالحاً ثم أعد المحاولة.");
        }
    }
}
