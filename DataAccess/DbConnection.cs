using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        public static string GetConnectionString()
        {
            var settings = ConfigurationManager.ConnectionStrings["SchoolDBConnection"];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    "لم يتم إعداد اتصال قاعدة البيانات. يرجى ضبط SchoolDBConnection في ملف إعداد التطبيق.");
            }

            return settings.ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
