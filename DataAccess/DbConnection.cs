using System.Configuration;
using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        private static readonly string connectionString = LoadConnectionString();

        private static string LoadConnectionString()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["SchoolDBConnection"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "لم يتم العثور على إعداد SchoolDBConnection في ملف إعدادات التطبيق.");
            }

            return settings.ConnectionString.Trim();
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
