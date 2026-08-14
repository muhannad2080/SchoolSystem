using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SchoolSystem.Services
{
    [Serializable]
    public class ApplicationSettingsData
    {
        public string ServerInstance { get; set; }
        public string DatabaseName { get; set; }
        public string BackupDirectory { get; set; }
    }

    public static class ApplicationSettingsService
    {
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolSystem");
        private static readonly string SettingsFile = Path.Combine(SettingsDirectory, "application-settings.xml");

        public static ApplicationSettingsData Load()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    using (FileStream stream = File.OpenRead(SettingsFile))
                    {
                        ApplicationSettingsData value = (ApplicationSettingsData)new XmlSerializer(typeof(ApplicationSettingsData)).Deserialize(stream);
                        if (value != null)
                            return ApplyDefaults(value);
                    }
                }
            }
            catch
            {
                // Fall back to safe defaults. A corrupted local settings file must not prevent login.
            }

            return ApplyDefaults(new ApplicationSettingsData());
        }

        public static void Save(ApplicationSettingsData value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value = ApplyDefaults(value);
            Validate(value);
            Directory.CreateDirectory(SettingsDirectory);
            string temporaryFile = SettingsFile + ".tmp";
            using (FileStream stream = File.Create(temporaryFile))
            {
                new XmlSerializer(typeof(ApplicationSettingsData)).Serialize(stream, value);
            }
            if (File.Exists(SettingsFile))
                File.Delete(SettingsFile);
            File.Move(temporaryFile, SettingsFile);
        }

        private static void Validate(ApplicationSettingsData value)
        {
            value.ServerInstance = (value.ServerInstance ?? string.Empty).Trim();
            value.DatabaseName = (value.DatabaseName ?? string.Empty).Trim();
            value.BackupDirectory = (value.BackupDirectory ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(value.ServerInstance) || value.ServerInstance.Length > 128 ||
                value.ServerInstance.Any(char.IsControl) || value.ServerInstance.IndexOf(';') >= 0)
                throw new ArgumentException("اسم خادم SQL Server غير صالح.", "value");

            if (string.IsNullOrWhiteSpace(value.DatabaseName) || value.DatabaseName.Length > 128 ||
                value.DatabaseName.Any(char.IsControl) || value.DatabaseName.IndexOf(';') >= 0)
                throw new ArgumentException("اسم قاعدة البيانات غير صالح.", "value");

            if (string.IsNullOrWhiteSpace(value.BackupDirectory) ||
                value.BackupDirectory.Any(char.IsControl) || !Path.IsPathRooted(value.BackupDirectory))
                throw new ArgumentException("مسار النسخ الاحتياطية يجب أن يكون مساراً كاملاً صالحاً.", "value");
        }

        private static ApplicationSettingsData ApplyDefaults(ApplicationSettingsData value)
        {
            if (string.IsNullOrWhiteSpace(value.ServerInstance))
                value.ServerInstance = ReadConfiguredServer() ?? ".";
            if (string.IsNullOrWhiteSpace(value.DatabaseName))
                value.DatabaseName = "SchoolDB";
            if (string.IsNullOrWhiteSpace(value.BackupDirectory))
                value.BackupDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolSystem", "Backups");
            return value;
        }

        private static string ReadConfiguredServer()
        {
            try
            {
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["SchoolDBConnection"];
                if (setting == null)
                    return null;
                return new System.Data.SqlClient.SqlConnectionStringBuilder(setting.ConnectionString).DataSource;
            }
            catch
            {
                return null;
            }
        }
    }
}
