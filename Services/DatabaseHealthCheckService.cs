using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Helpers;

namespace SchoolSystem.Services
{
    public class HealthCheckResult
    {
        public bool Healthy { get; set; }
        public int? DatabaseVersion { get; set; }
        public bool VersionCompatible { get; set; }

        /// <summary>سليم بنيوياً: الجداول والأعمدة والإجراءات المخزّنة والاستعلام الأساسي متوفرة.
        /// لا يشمل تحذير "بيانات أولية مفقودة" حتى لا يعيق التثبيت الأول (يُنشئ المسؤول تلقائياً عند أول تشغيل).</summary>
        public bool StructuralHealthy { get; set; }
        public IList<string> Issues { get; } = new List<string>();
        public string Server { get; set; }
        public string Database { get; set; }

        public string Summarize()
        {
            if (Healthy)
                return "قاعدة البيانات سليمة وجاهزة.";
            return "توجد مشكلات في قاعدة البيانات (" + Issues.Count + "): " + string.Join(" | ", Issues);
        }
    }

    /// <summary>
    /// فحص صحة شامل لقاعدة البيانات: وجود الجداول والأعمدة والإجراءات الأساسية،
    /// الإصدار، البيانات المرجعية، المصادقة، واستعلام أساسي.
    /// </summary>
    public static class DatabaseHealthCheckService
    {
        private static readonly string[] RequiredTables =
        {
            "Students", "StudentClasses", "Classes", "SchoolSections", "Users", "Roles",
            "RolePermissions", "UserRoles", "Permissions", "Fees", "StudentFees",
            "FeePlans", "Vouchers", "Receipts", "AcademicYears", "Teachers", "Marks"
        };

        private static readonly Dictionary<string, string[]> RequiredColumns =
            new Dictionary<string, string[]>
            {
                { "Users", new[] { "UserID", "UserName", "PasswordHash", "PasswordSalt", "IsActive", "RoleName" } },
                { "Students", new[] { "StudentID", "FullName", "NationalId", "Gender", "BirthDate" } },
                { "Classes", new[] { "ClassID", "ClassName", "StageID" } },
                { "Fees", new[] { "FeeID", "StudentID", "TotalAmount", "PaidAmount", "DueDate" } },
                { "SchoolSections", new[] { "SectionID", "ClassID", "SectionName", "AcademicYear" } }
            };

        public static HealthCheckResult Check(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword)
        {
            HealthCheckResult result = new HealthCheckResult
            {
                Server = server,
                Database = database
            };

            string connection = BuildConnection(server, database, useSqlAuth, sqlUser, sqlPassword);
            CheckWithConnectionString(connection, result);
            return result;
        }

        private static string BuildConnection(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder
            {
                DataSource = server ?? string.Empty,
                InitialCatalog = database ?? string.Empty,
                TrustServerCertificate = true,
                ConnectTimeout = 15
            };
            if (useSqlAuth)
            {
                b.IntegratedSecurity = false;
                b.UserID = sqlUser ?? string.Empty;
                b.Password = sqlPassword ?? string.Empty;
            }
            else
            {
                b.IntegratedSecurity = true;
            }
            return b.ConnectionString;
        }

        private static void CheckWithConnectionString(string connection, HealthCheckResult result)
        {
            bool structural = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    structural = true;

                    result.DatabaseVersion = ReadVersion(con);
                    result.VersionCompatible = result.DatabaseVersion == null || result.DatabaseVersion >= 1;

                    structural &= CheckTables(con, result);
                    structural &= CheckColumns(con, result);
                    structural &= CheckProcedures(con, result);
                    structural &= RunBasicQuery(con, result);
                    CheckSeed(con, result);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": فحص صحة قاعدة البيانات", ex);
                result.Issues.Add("تعذر الاتصال أو فحص قاعدة البيانات: " + ex.Message);
                structural = false;
            }

            result.StructuralHealthy = structural && result.VersionCompatible;
            result.Healthy = result.StructuralHealthy && result.VersionCompatible && result.Issues.Count == 0;
        }

        private static int? ReadVersion(SqlConnection con)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(MAX(Version), 0) FROM dbo.SchemaMigrations;", con))
                {
                    cmd.CommandTimeout = 30;
                    object value = cmd.ExecuteScalar();
                    if (value == null || value == DBNull.Value)
                        return null;
                    return Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": قراءة إصدار قاعدة البيانات", ex);
                return null;
            }
        }

        private static bool CheckTables(SqlConnection con, HealthCheckResult result)
        {
            bool ok = true;
            HashSet<string> present = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT t.name FROM sys.tables t WHERE t.schema_id = SCHEMA_ID(N'dbo');", con))
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        present.Add(r.GetString(0));
            }
            catch (Exception ex) { ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": فحص الجداول الأساسية", ex); ok = false; }

            foreach (string table in RequiredTables)
                if (!present.Contains(table))
                {
                    result.Issues.Add("جدول مفقود: " + table);
                    ok = false;
                }
            return ok;
        }

        private static bool CheckColumns(SqlConnection con, HealthCheckResult result)
        {
            bool ok = true;
            foreach (KeyValuePair<string, string[]> pair in RequiredColumns)
            {
                foreach (string column in pair.Value)
                {
                    if (ColumnMissing(con, pair.Key, column))
                    {
                        result.Issues.Add("عمود مفقود: " + pair.Key + "." + column);
                        ok = false;
                    }
                }
            }
            return ok;
        }

        private static bool ColumnMissing(SqlConnection con, string table, string column)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COL_LENGTH(N'dbo." + table + "', N'" + column + "');", con))
                {
                    cmd.CommandTimeout = 30;
                    return cmd.ExecuteScalar() == DBNull.Value;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": التحقق من الأعمدة الأساسية", ex);
                return true;
            }
        }

        private static readonly string[] RequiredProcedures =
        {
            "SetActiveAcademicYear", "RegisterDatabaseBackup", "GetDashboardOperationalStatus"
        };

        private static bool CheckProcedures(SqlConnection con, HealthCheckResult result)
        {
            bool ok = true;
            HashSet<string> present = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT name FROM sys.procedures WHERE schema_id = SCHEMA_ID(N'dbo');", con))
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        present.Add(r.GetString(0));
            }
            catch (Exception ex) { ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": فحص الإجراءات المخزنة الأساسية", ex); ok = false; }

            foreach (string proc in RequiredProcedures)
                if (!present.Contains(proc))
                {
                    result.Issues.Add("إجراء مخزّن مفقود: " + proc);
                    ok = false;
                }
            return ok;
        }

        private static void CheckSeed(SqlConnection con, HealthCheckResult result)
        {
            // تحقق من وجود بيانات أساسية افتراضية (مستخدم مسؤول ونظام دراسة أساسي).
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"
SELECT (SELECT COUNT(*) FROM dbo.Users) +
       (SELECT COUNT(*) FROM dbo.Roles) +
       (SELECT COUNT(*) FROM dbo.AcademicYears);", con))
                {
                    cmd.CommandTimeout = 30;
                    int total = Convert.ToInt32(cmd.ExecuteScalar());
                    if (total <= 0)
                        result.Issues.Add("بيانات أولية مفقودة: لا يوجد مستخدمون/أدوار/سنوات دراسية.");
                }
            }
            catch (Exception ex) { ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": فحص البيانات الأولية", ex); }
        }

        private static bool RunBasicQuery(SqlConnection con, HealthCheckResult result)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT 1;", con))
                {
                    cmd.CommandTimeout = 15;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        result.Issues.Add("فشل الاستعلام الأساسي.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(DatabaseHealthCheckService) + ": الاستعلام الأساسي لفحص الصحة", ex);
                result.Issues.Add("فشل الاستعلام الأساسي: " + ex.Message);
                return false;
            }
        }
    }
}
