using System.Configuration;
using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    public static class DbConnection
    {
        private static readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["SchoolDBConnection"]?.ConnectionString ?? 
            @"Data Source=.;Initial Catalog=SchoolDB;Integrated Security=True;MultipleActiveResultSets=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
