using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace SchoolSystem.Services
{
    public class DatabaseBackupService
    {
        private static readonly Regex SafeIdentifier = new Regex("^[A-Za-z0-9_\\-]+$", RegexOptions.Compiled);

        public void TestConnection(string serverInstance, string databaseName)
        {
            using (SqlConnection connection = CreateConnection(serverInstance, databaseName))
            {
                connection.Open();
            }
        }

        public string Backup(string serverInstance, string databaseName, string backupDirectory)
        {
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

            return backupFile;
        }

        public void Restore(string serverInstance, string backupFile, string targetDatabase, bool replaceExisting)
        {
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

        private static SqlConnection CreateConnection(string serverInstance, string databaseName)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverInstance.Trim(),
                InitialCatalog = databaseName.Trim(),
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                ConnectTimeout = 15
            };
            return new SqlConnection(builder.ConnectionString);
        }

        private static void ValidateInputs(string serverInstance, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(serverInstance))
                throw new ArgumentException("يجب تحديد اسم خادم SQL Server.");
            if (string.IsNullOrWhiteSpace(databaseName) || !SafeIdentifier.IsMatch(databaseName))
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.");
        }

        private static string EnsureBackupDirectory(string backupDirectory)
        {
            if (string.IsNullOrWhiteSpace(backupDirectory))
                throw new ArgumentException("يجب تحديد مجلد النسخ الاحتياطي.");

            string directory;
            try
            {
                directory = Path.GetFullPath(backupDirectory.Trim());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("مسار مجلد النسخ الاحتياطي غير صالح.", ex);
            }

            string applicationDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            if (directory.StartsWith(applicationDirectory, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("يجب حفظ النسخ الاحتياطية خارج مجلد البرنامج.");
            if (File.Exists(directory))
                throw new IOException("المسار المحدد لنسخ قاعدة البيانات هو ملف وليس مجلداً.");

            try
            {
                Directory.CreateDirectory(directory);
                if (Directory.Exists(directory))
                    return directory;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
            {
                string fallback = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem", "Backups");
                try
                {
                    Directory.CreateDirectory(fallback);
                    if (Directory.Exists(fallback))
                        return fallback;
                }
                catch (Exception fallbackException) when (fallbackException is IOException || fallbackException is UnauthorizedAccessException || fallbackException is NotSupportedException)
                {
                    throw new IOException("تعذر إنشاء مجلد النسخ الاحتياطي. اختر مجلداً محلياً صالحاً مثل D:\\SchoolSystemBackups ثم أعد المحاولة.", ex);
                }
            }

            throw new IOException("تعذر إنشاء مجلد النسخ الاحتياطي. اختر مجلداً محلياً صالحاً ثم أعد المحاولة.");
        }
    }
}
