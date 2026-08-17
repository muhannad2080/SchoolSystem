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
                (UserID, UserName, Module, MachineName, IpAddress, ActionName, EntityName, EntityID, Details, CreatedAt)
                VALUES
                (@UserID, @UserName, @Module, @MachineName, @IpAddress, @ActionName, @EntityName, @EntityID, @Details, GETDATE());", connection))
            {
                AddParameters(command, item);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search)
        {
            return GetRecent(fromDate, toDate, search, string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search,
            string userName, string actionName, string entityName)
        {
            EnsureTable();
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT TOP 1000
                    AuditLogID,
                    CreatedAt,
                    ISNULL(UserName, N'نظام') AS UserName,
                    Module,
                    MachineName,
                    IpAddress,
                    ActionName,
                    EntityName,
                    EntityID,
                    Details
                FROM AuditLogs
                WHERE CreatedAt >= @FromDate
                  AND CreatedAt < @ToDate
                  AND (@Search = N'' OR UserName LIKE @LikeSearch OR ActionName LIKE @LikeSearch OR EntityName LIKE @LikeSearch OR Details LIKE @LikeSearch)
                  AND (@UserName = N'' OR UserName = @UserName)
                  AND (@ActionName = N'' OR ActionName = @ActionName)
                  AND (@EntityName = N'' OR EntityName = @EntityName)
                ORDER BY CreatedAt DESC, AuditLogID DESC;", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                string searchValue = (search ?? string.Empty).Trim();
                string userValue = (userName ?? string.Empty).Trim();
                string actionValue = (actionName ?? string.Empty).Trim();
                string entityValue = (entityName ?? string.Empty).Trim();

                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Date;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Date.AddDays(1);
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = searchValue;
                command.Parameters.Add("@LikeSearch", SqlDbType.NVarChar, 210).Value = "%" + searchValue + "%";
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = userValue;
                command.Parameters.Add("@ActionName", SqlDbType.NVarChar, 100).Value = actionValue;
                command.Parameters.Add("@EntityName", SqlDbType.NVarChar, 100).Value = entityValue;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public DataTable GetFilterValues(string filterName)
        {
            EnsureTable();
            string columnName;
            switch (filterName)
            {
                case "UserName":
                    columnName = "UserName";
                    break;
                case "ActionName":
                    columnName = "ActionName";
                    break;
                case "EntityName":
                    columnName = "EntityName";
                    break;
                default:
                    throw new ArgumentException("اسم فلتر سجل التدقيق غير مدعوم.", "filterName");
            }

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT DISTINCT TOP 500 " + columnName + " AS Value " +
                "FROM AuditLogs WHERE NULLIF(LTRIM(RTRIM(" + columnName + ")), N'') IS NOT NULL " +
                "ORDER BY Value;", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                connection.Open();
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
                command.Parameters.Add("@Module", SqlDbType.NVarChar, 100).Value = (object)(item.Module ?? string.Empty);
                command.Parameters.Add("@MachineName", SqlDbType.NVarChar, 150).Value = (object)(item.MachineName ?? string.Empty);
                command.Parameters.Add("@IpAddress", SqlDbType.NVarChar, 64).Value = (object)(item.IpAddress ?? string.Empty);
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
                        Module NVARCHAR(100) NULL,
                        MachineName NVARCHAR(150) NULL,
                        IpAddress NVARCHAR(64) NULL,
                        ActionName NVARCHAR(100) NOT NULL,
                        EntityName NVARCHAR(100) NULL,
                        EntityID NVARCHAR(100) NULL,
                        Details NVARCHAR(MAX) NULL,
                        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())
                    );
                    CREATE INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs(CreatedAt DESC);
                    CREATE INDEX IX_AuditLogs_Entity ON dbo.AuditLogs(EntityName, EntityID);
                END;
                IF COL_LENGTH(N'dbo.AuditLogs', N'Module') IS NULL ALTER TABLE dbo.AuditLogs ADD Module NVARCHAR(100) NULL;
                IF COL_LENGTH(N'dbo.AuditLogs', N'MachineName') IS NULL ALTER TABLE dbo.AuditLogs ADD MachineName NVARCHAR(150) NULL;
                IF COL_LENGTH(N'dbo.AuditLogs', N'IpAddress') IS NULL ALTER TABLE dbo.AuditLogs ADD IpAddress NVARCHAR(64) NULL;
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_Module' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
                    CREATE INDEX IX_AuditLogs_Module ON dbo.AuditLogs(Module, CreatedAt DESC);
                ", connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    tableEnsured = true;
                }
            }
        }
    }
}
