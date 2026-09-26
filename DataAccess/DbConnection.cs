using System;
using System.Configuration;
using System.Data.SqlClient;
using SchoolSystem.Services;


namespace SchoolSystem.DataAccess
{

    public static class DbConnection
    {
        // تعريف الدالة LoadConnectionString لتنفيذ العملية المرتبطة بها.
        private static string connectionString = LoadConnectionString();

        // تعريف الدالة LoadConnectionString لتنفيذ العملية المرتبطة بها.
        private static string LoadConnectionString()
        {
            ApplicationSettingsData saved = null;

            try
            {
                saved = ApplicationSettingsService.Load();
            }
            catch (Exception ex)
            {
                SchoolSystem.Helpers.ApplicationLogger.LogException("قراءة إعدادات الخادم المحفوظة", ex);
            }

            return BuildEffectiveConnectionString(saved, 15);
        }

        /// <summary>
        /// يبني سلسلة اتصال فعّالة انطلاقاً من إعدادات التطبيق المحفوظة، ويعود إلى قيم
        /// App.config عند عدم وجود إعدادات محفوظة. يدعم مصادقة Windows أو مصادقة SQL.
        /// </summary>
        public static string BuildEffectiveConnectionString(ApplicationSettingsData saved, int connectTimeout)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["SchoolDBConnection"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                // لا يوجد إعداد في App.config: نبني افتراضياً على مضيف محلي وقاعدة SchoolDB.
                SqlConnectionStringBuilder fallback = new SqlConnectionStringBuilder
                {
                    DataSource = ".",
                    InitialCatalog = "SchoolDB",
                    IntegratedSecurity = true,
                    TrustServerCertificate = true,
                    ConnectTimeout = connectTimeout > 0 ? connectTimeout : 15
                };

                return fallback.ConnectionString;
            }

            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(settings.ConnectionString.Trim());

            if (connectTimeout > 0)
                builder.ConnectTimeout = connectTimeout;

            if (saved != null)
            {
                if (!string.IsNullOrWhiteSpace(saved.ServerInstance))
                    builder.DataSource = saved.ServerInstance.Trim();
                if (!string.IsNullOrWhiteSpace(saved.DatabaseName))
                    builder.InitialCatalog = saved.DatabaseName.Trim();

                if (saved.UseSqlAuthentication)
                {
                    builder.IntegratedSecurity = false;
                    builder.UserID = (saved.SqlUser ?? string.Empty).Trim();
                    builder.Password = ApplicationSettingsService.GetSqlPassword(saved);
                }
                else
                {
                    builder.IntegratedSecurity = true;
                    builder.Remove("User ID");
                    builder.Remove("Password");
                }
            }

            builder.TrustServerCertificate = true;

            return builder.ConnectionString;
        }

        // تعريف الدالة Reload لتنفيذ العملية المرتبطة بها.
        public static void Reload()
        {
            connectionString = LoadConnectionString();
        }

        // تعريف الدالة GetConnectionString لتنفيذ العملية المرتبطة بها.
        public static string GetConnectionString()
        {

            return connectionString;
        }

        // تعريف الدالة GetConnection لتنفيذ العملية المرتبطة بها.
        public static SqlConnection GetConnection()
        {

            return new SqlConnection(connectionString);
        }
    }
}
