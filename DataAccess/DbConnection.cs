using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        private static readonly string connectionString =
            @"Server=MUHANNADALJRADI;Database=SchoolDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}