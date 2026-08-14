using System;
using System.Configuration;
using System.Data.SqlClient;
using SchoolSystem.Services;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        private static string connectionString = LoadConnectionString();

        private static string LoadConnectionString()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["SchoolDBConnection"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "لم يتم العثور على إعداد SchoolDBConnection في ملف إعدادات التطبيق.");
            }

            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(settings.ConnectionString.Trim());

            try
            {
                ApplicationSettingsData saved = ApplicationSettingsService.Load();
                if (saved != null && !string.IsNullOrWhiteSpace(saved.ServerInstance))
                    builder.DataSource = saved.ServerInstance.Trim();
                if (saved != null && !string.IsNullOrWhiteSpace(saved.DatabaseName))
                    builder.InitialCatalog = saved.DatabaseName.Trim();
            }
            catch
            {
                // إذا تعذر تحميل الإعدادات المحلية، نستخدم إعداد App.config دون تعطيل التشغيل.
            }

            return builder.ConnectionString;
        }

        public static void Reload()
        {
            connectionString = LoadConnectionString();
        }

        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
