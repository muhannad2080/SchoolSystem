using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    internal static class LibrarySchemaInitializer
    {
        private static readonly object SyncRoot = new object();
        private static bool booksSchemaChecked;

        public static void EnsureBooksSchema(SqlConnection connection)
        {
            if (booksSchemaChecked)
                return;

            lock (SyncRoot)
            {
                if (booksSchemaChecked)
                    return;

                bool openedHere = connection.State != System.Data.ConnectionState.Open;
                if (openedHere)
                    connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Books', N'IsActive') IS NULL
BEGIN
    ALTER TABLE dbo.Books
        ADD IsActive BIT NOT NULL
            CONSTRAINT DF_Books_IsActive DEFAULT (1) WITH VALUES;
END";
                    command.ExecuteNonQuery();
                }

                booksSchemaChecked = true;
            }
        }
    }
}
