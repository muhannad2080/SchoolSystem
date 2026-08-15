using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class AuditLogRepository
    {
        private bool tableEnsured;
        private readonly object tableEnsureLock = new object();

        public void Write(AuditLog item)
        {
            EnsureTable();
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                INSERT INTO AuditLogs
                (UserID, UserName, ActionName, EntityName, EntityID, Details, CreatedAt)
                VALUES
                (@UserID, @UserName, @ActionName, @EntityName, @EntityID, @Details, GETDATE());", connection))
            {
                AddParameters(command, item);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search)
        {
            EnsureTable();
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT TOP 1000
                    AuditLogID,
                    CreatedAt,
                    ISNULL(UserName, N'نظام') AS UserName,
                    ActionName,
                    EntityName,
                    EntityID,
                    Details
                FROM AuditLogs
                WHERE CreatedAt >= @FromDate
                  AND CreatedAt < @ToDate
                  AND (@Search = N'' OR UserName LIKE @LikeSearch OR ActionName LIKE @LikeSearch OR EntityName LIKE @LikeSearch OR Details LIKE @LikeSearch)
                ORDER BY CreatedAt DESC, AuditLogID DESC;", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Date;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Date.AddDays(1);
                string value = (search ?? string.Empty).Trim();
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = value;
                command.Parameters.Add("@LikeSearch", SqlDbType.NVarChar, 210).Value = "%" + value + "%";
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private void AddParameters(SqlCommand command, AuditLog item)
        {
            if (item.UserId.HasValue)
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = item.UserId.Value;
            else
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = DBNull.Value;
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = (object)(item.UserName ?? string.Empty);
            command.Parameters.Add("@ActionName", SqlDbType.NVarChar, 100).Value = (object)(item.ActionName ?? string.Empty);
            command.Parameters.Add("@EntityName", SqlDbType.NVarChar, 100).Value = (object)(item.EntityName ?? string.Empty);
            command.Parameters.Add("@EntityID", SqlDbType.NVarChar, 100).Value = (object)(item.EntityId ?? string.Empty);
            command.Parameters.Add("@Details", SqlDbType.NVarChar, -1).Value = (object)(item.Details ?? string.Empty);
        }

        private void EnsureTable()
        {
            if (tableEnsured)
                return;

            lock (tableEnsureLock)
            {
                if (tableEnsured)
                    return;

                using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.AuditLogs
                    (
                        AuditLogID BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY,
                        UserID INT NULL,
                        UserName NVARCHAR(150) NULL,
                        ActionName NVARCHAR(100) NOT NULL,
                        EntityName NVARCHAR(100) NULL,
                        EntityID NVARCHAR(100) NULL,
                        Details NVARCHAR(MAX) NULL,
                        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())
                    );
                    CREATE INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs(CreatedAt DESC);
                    CREATE INDEX IX_AuditLogs_Entity ON dbo.AuditLogs(EntityName, EntityID);
                END;", connection))
            {
                    connection.Open();
                    command.ExecuteNonQuery();
                    tableEnsured = true;
                }
            }
        }
    }
}
