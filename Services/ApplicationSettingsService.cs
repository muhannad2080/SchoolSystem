using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SchoolSystem.Helpers;


namespace SchoolSystem.Services
{

    [Serializable]

    public class ApplicationSettingsData
    {

        public string ServerInstance { get; set; }

        public string DatabaseName { get; set; }

        /// <summary>نوع المصادقة على SQL Server: false = مصادقة Windows (Integrated)، true = مصادقة SQL.</summary>
        public bool UseSqlAuthentication { get; set; }

        /// <summary>اسم مستخدم SQL عند تفعيل مصادقة SQL (يُخزَّن نصًا صريحاً؛ اسم المستخدم ليس سراً).</summary>
        public string SqlUser { get; set; }

        /// <summary>كلمة مرور SQL مشفّرة عبر DPAPI (لا تُخزَّن نصًا صريحاً).</summary>
        public string SqlPasswordProtected { get; set; }

        public string BackupDirectory { get; set; }

        public string Language { get; set; }

        public string CurrencySymbol { get; set; }

        public string DateFormat { get; set; }

        public string ReceiptPrefix { get; set; }

        public string PaymentPrefix { get; set; }

        public bool EnableEmailNotifications { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool EnableSsl { get; set; }
    }


    public static class ApplicationSettingsService
    {
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolSystem");
        // تعريف الدالة Combine لتنفيذ العملية المرتبطة بها.
        private static readonly string SettingsFile = Path.Combine(SettingsDirectory, "application-settings.xml");

        // تعريف الدالة Load لتنفيذ العملية المرتبطة بها.
        public static ApplicationSettingsData Load()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(ApplicationSettingsService) + ": تحميل إعدادات التطبيق", ex);
                // Fall back to safe defaults. A corrupted local settings file must not prevent login.
            }


            return ApplyDefaults(new ApplicationSettingsData());
        }

        // تعريف الدالة Save لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة Validate لتنفيذ العملية المرتبطة بها.
        private static void Validate(ApplicationSettingsData value)
        {
            value.ServerInstance = (value.ServerInstance ?? string.Empty).Trim();
            value.DatabaseName = (value.DatabaseName ?? string.Empty).Trim();
            value.SqlUser = (value.SqlUser ?? string.Empty).Trim();
            value.BackupDirectory = (value.BackupDirectory ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(value.ServerInstance) || value.ServerInstance.Length > 128 ||
                value.ServerInstance.Any(char.IsControl) || value.ServerInstance.IndexOf(';') >= 0)

                throw new ArgumentException("اسم خادم SQL Server غير صالح.", "value");


            if (string.IsNullOrWhiteSpace(value.DatabaseName) || value.DatabaseName.Length > 128 ||
                value.DatabaseName.Any(char.IsControl) || value.DatabaseName.IndexOf(';') >= 0)

                throw new ArgumentException("اسم قاعدة البيانات غير صالح.", "value");

            if (value.UseSqlAuthentication)
            {
                if (string.IsNullOrWhiteSpace(value.SqlUser) || value.SqlUser.Length > 128 ||
                    value.SqlUser.Any(char.IsControl) || value.SqlUser.IndexOf(';') >= 0)

                    throw new ArgumentException("اسم مستخدم SQL غير صالح.", "value");

                if (string.IsNullOrWhiteSpace(UnprotectPassword(value.SqlPasswordProtected)))

                    throw new ArgumentException("كلمة مرور مستخدم SQL مطلوبة.", "value");
            }


            if (string.IsNullOrWhiteSpace(value.BackupDirectory) ||
                value.BackupDirectory.Any(char.IsControl) || !Path.IsPathRooted(value.BackupDirectory))

                throw new ArgumentException("مسار النسخ الاحتياطية يجب أن يكون مساراً كاملاً صالحاً.", "value");
        }

        // تعريف الدالة ApplyDefaults لتنفيذ العملية المرتبطة بها.
        private static ApplicationSettingsData ApplyDefaults(ApplicationSettingsData value)
        {
            if (string.IsNullOrWhiteSpace(value.ServerInstance))
                value.ServerInstance = ReadConfiguredServer() ?? ".";
            if (string.IsNullOrWhiteSpace(value.DatabaseName))
                value.DatabaseName = "SchoolDB";
            if (string.IsNullOrWhiteSpace(value.BackupDirectory))
                value.BackupDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolSystem", "Backups");
            if (string.IsNullOrWhiteSpace(value.Language)) value.Language = "ar";
            if (string.IsNullOrWhiteSpace(value.CurrencySymbol)) value.CurrencySymbol = "ر.ي";
            if (string.IsNullOrWhiteSpace(value.DateFormat)) value.DateFormat = "yyyy/MM/dd";
            if (string.IsNullOrWhiteSpace(value.ReceiptPrefix)) value.ReceiptPrefix = "REC";
            if (string.IsNullOrWhiteSpace(value.PaymentPrefix)) value.PaymentPrefix = "PAY";
            if (value.SmtpPort <= 0) value.SmtpPort = 587;
            return value;
        }

        /// <summary>إعداد كلمة مرور مستخدم SQL وتشفيرها قبل الحفظ.</summary>
        public static void SetSqlPassword(ApplicationSettingsData value, string plainPassword)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            value.SqlPasswordProtected = SchoolSystem.Helpers.SecureStorage.Protect(plainPassword ?? string.Empty);
        }

        /// <summary>استرجاع كلمة مرور مستخدم SQL مفكوكة التشفير.</summary>
        public static string GetSqlPassword(ApplicationSettingsData value)
        {
            if (value == null)
                return string.Empty;
            return UnprotectPassword(value.SqlPasswordProtected);
        }

        private static string UnprotectPassword(string protectedValue)
        {
            return SchoolSystem.Helpers.SecureStorage.Unprotect(protectedValue);
        }

        // تعريف الدالة ReadConfiguredServer لتنفيذ العملية المرتبطة بها.
        private static string ReadConfiguredServer()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["SchoolDBConnection"];
                if (setting == null)
                    return null;

                return new System.Data.SqlClient.SqlConnectionStringBuilder(setting.ConnectionString).DataSource;
            }

            catch (Exception ex)
            {

                ApplicationLogger.LogException("خدمة " + nameof(ApplicationSettingsService) + ": قراءة خادم SQL المُعد في الإعدادات", ex);

                return null;
            }
        }
    }
}
