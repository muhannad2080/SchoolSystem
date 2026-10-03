using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using SchoolSystem.Helpers;

namespace SchoolSystem.Services
{
    /// <summary>نتيجة اختبار اتصال تحليلي بمثيل SQL Server وقاعدة بيانات.</summary>
    public class ProbeResult
    {
        public bool Success { get; set; }
        public bool ServerReachable { get; set; }
        public bool AuthenticationSucceeded { get; set; }
        public bool DatabaseExists { get; set; }
        public bool SchemaValid { get; set; }
        public bool VersionCompatible { get; set; }
        public int? DatabaseVersion { get; set; }
        public string ErrorMessage { get; set; }

        public bool ConnectionSucceeded
        {
            get { return Success && ServerReachable && AuthenticationSucceeded && DatabaseExists; }
        }

        public IList<string> MissingObjects { get; } = new List<string>();
    }

    /// <summary>
    /// أداة فحص اتصال متدرجة بـ SQL Server: يتحقق من إمكانية الوصول للخادم، ثم صحة
    /// المصادقة، ثم وجود قاعدة البيانات، ثم سلامة المخطط، ثم توافق الإصدار. يستخدم
    /// مصادقة Windows أو مصادقة SQL حسب الإعدادات المحفوظة أو المعطاة.
    /// </summary>
    public static class SqlServerProbe
    {
        /// <summary>الحد الأقصى للانتظار بالثواني لكل محاولة اتصال.</summary>
        public static int ConnectTimeoutSeconds = 15;

        // الجداول الأساسية التي يجب توفرها لتُعدّ قاعدة البيانات "صالحة".
        private static readonly string[] RequiredTables =
        {
            "Students", "StudentClasses", "Classes", "SchoolSections", "Users",
            "Roles", "RolePermissions", "UserRoles", "Permissions",
            "Fees", "FeePlans", "Vouchers", "StudentAttendance", "TeacherAttendance",
            "Payroll", "TeacherContracts", "Expenses"
        };

        public static ProbeResult Probe(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword)
        {
            ProbeResult result = new ProbeResult();

            string serverInstance = (server ?? string.Empty).Trim();
            string databaseName = (database ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(serverInstance))
            {
                result.ErrorMessage = "اسم خادم SQL Server غير محدد.";
                return result;
            }
            result.ServerReachable = IsServerReachable(serverInstance);
            if (!result.ServerReachable)
            {
                result.ErrorMessage = "الخادم غير قابل للوصول. تحقق من الاسم/العنوان والمنفذ وجدار الحماية.";
                result.DatabaseExists = false;
                result.AuthenticationSucceeded = false;
                return result;
            }

            string connection = BuildConnectionString(serverInstance, databaseName, useSqlAuth, sqlUser, sqlPassword);
            result.AuthenticationSucceeded = CanAuthenticate(connection);
            if (!result.AuthenticationSucceeded)
            {
                result.ErrorMessage = "فشلت مصادقة الاتصال (Windows أو SQL). تحقق من بيانات الدخول والصلاحيات.";
                result.DatabaseExists = false;
                return result;
            }

            result.DatabaseExists = DatabaseExists(connection, databaseName);
            if (!result.DatabaseExists)
            {
                result.ErrorMessage = "قاعدة البيانات غير موجودة على الخادم.";
                return result;
            }

            ProbeSchema(connection, databaseName, result);

            result.Success = result.SchemaValid;
            if (!result.Success && string.IsNullOrWhiteSpace(result.ErrorMessage))
                result.ErrorMessage = "مخطط قاعدة البيانات غير صالح أو غير مكتمل.";

            return result;
        }

        private static string BuildConnectionString(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                TrustServerCertificate = true,
                ConnectTimeout = ConnectTimeoutSeconds
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

        public static bool DatabaseExists(string connectionString, string database)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BuildMasterConnection(connectionString)))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT CASE WHEN DB_ID(@db) IS NOT NULL THEN 1 ELSE 0 END", con))
                    {
                        cmd.CommandTimeout = 30;
                        cmd.Parameters.Add("@db", SqlDbType.NVarChar, 128).Value = database;
                        return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": التحقق من وجود قاعدة البيانات", ex);
                return false;
            }
        }

        private static string BuildMasterConnection(string databaseConnection)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(databaseConnection);
            b.InitialCatalog = "master";
            return b.ConnectionString;
        }

        private static bool CanAuthenticate(string connection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BuildMasterConnection(connection)))
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": التحقق من المصادقة على SQL Server", ex);
                return false;
            }
        }

        private static void ProbeSchema(string connection, string database, ProbeResult result)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    // الإصدار من جدول تاريخ الترحيل إذا وُجد.
                    result.DatabaseVersion = ReadDatabaseVersion(con);
                    result.VersionCompatible = result.DatabaseVersion == null || result.DatabaseVersion >= 1;

                    // الجداول الأساسية.
                    const string tableQuery = @"
SELECT t.name FROM sys.tables t
WHERE t.schema_id = SCHEMA_ID(N'dbo')
  AND t.name IN ('Students','StudentClasses','Classes','SchoolSections','Users',
                 'Roles','RolePermissions','UserRoles','Permissions',
                 'Fees','StudentFees','FeePlans','Vouchers','Receipts');";
                    HashSet<string> present = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    using (SqlCommand cmd = new SqlCommand(tableQuery, con))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            present.Add(r.GetString(0));
                    }

                    foreach (string required in RequiredTables)
                    {
                        if (!present.Contains(required))
                            result.MissingObjects.Add("جدول: " + required);
                    }

                    result.SchemaValid = result.MissingObjects.Count == 0;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": فحص مخطط قاعدة البيانات", ex);
                result.MissingObjects.Add("تعذر فحص المخطط: " + ex.Message);
                result.SchemaValid = false;
            }
        }

        private static int? ReadDatabaseVersion(SqlConnection con)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT MAX(Version) FROM dbo.SchemaMigrations;", con))
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
                ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": قراءة إصدار قاعدة البيانات", ex);
                // جدول الترحيل غير موجود بعد = لا يوجد إصدار معرف.
                return null;
            }
        }

        /// <summary>يقاوم اكتشاف الخادم عبر TCP. يدعم أسماء الحواسيب والمنافذ بصيغة name,1433.</summary>
        public static bool IsServerReachable(string serverInstance)
        {
            string host;
            int port = 1433;

            int commaIndex = serverInstance.IndexOf(',');
            if (commaIndex >= 0)
            {
                host = serverInstance.Substring(0, commaIndex).Trim();
                if (int.TryParse(serverInstance.Substring(commaIndex + 1).Trim(), out int parsedPort))
                    port = parsedPort;
            }
            else
            {
                host = serverInstance;
            }

            if (string.IsNullOrWhiteSpace(host))
                return false;

            // خادم محلي أو مثيل محلي.
            if (host.Equals(".", StringComparison.Ordinal) ||
                host.Equals("(local)", StringComparison.OrdinalIgnoreCase) ||
                host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                return true;

            if (serverInstance.EndsWith("\\SQLEXPRESS", StringComparison.OrdinalIgnoreCase))
                return true;

            IPAddress[] addresses;
            try
            {
                addresses = Dns.GetHostAddresses(host);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": تحليل اسم خادم SQL Server", ex);
                return false;
            }

            foreach (IPAddress address in addresses)
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        IAsyncResult asyncResult = client.BeginConnect(address, port, null, null);
                        if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(ConnectTimeoutSeconds)))
                        {
                            client.Close();
                            continue;
                        }
                        client.EndConnect(asyncResult);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationLogger.LogException("خدمة " + nameof(SqlServerProbe) + ": محاولة الاتصال بالخادم عبر TCP", ex);
                    // جرّب العنوان التالي.
                }
            }

            return false;
        }
    }
}
