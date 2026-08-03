using System.Configuration;
using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        private static readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["SchoolDBConnection"]?.ConnectionString ?? 
            @"Server=MUHANNADALJRADI;Database=SchoolDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
