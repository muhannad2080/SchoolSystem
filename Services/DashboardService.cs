using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class DashboardService
    {
        private readonly AnnualClosingService annualClosingService = new AnnualClosingService();

        private string GetActiveYear()
        {
            string year = annualClosingService.GetActiveAcademicYear();
            return (year ?? string.Empty).Trim().Replace('-', '/');
        }

        private static void AddYear(SqlCommand command, string academicYear)
        {
            command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = academicYear ?? string.Empty;
        }

        public int GetStudentCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(DISTINCT sc.StudentID)
                FROM StudentClasses sc
                INNER JOIN Students s ON s.StudentID = sc.StudentID
                WHERE REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND ISNULL(s.Status, N'نشط') = N'نشط'", conn))
            {
                AddYear(cmd, GetActiveYear());
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int GetTeacherCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers WHERE ISNULL(Status, N'نشط') <> N'غير نشط'", conn))
            {
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int GetSubjectCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE ISNULL(IsActive, 1) = 1", conn))
            {
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int GetClassCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ISNULL(IsActive, 1) = 1", conn))
            {
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public DataTable GetStudentsPerClass()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT c.ClassName, COUNT(DISTINCT sc.StudentID) AS StudentCount
                FROM Classes c
                INNER JOIN StudentClasses sc ON c.ClassID = sc.ClassID
                INNER JOIN Students s ON s.StudentID = sc.StudentID
                WHERE ISNULL(c.IsActive, 1) = 1
                  AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND ISNULL(s.Status, N'نشط') = N'نشط'
                GROUP BY c.ClassName
                ORDER BY c.ClassName", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                AddYear(cmd, GetActiveYear());
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public decimal GetPendingFeesTotal()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT COALESCE(SUM(CASE WHEN f.RemainingAmount > 0 THEN f.RemainingAmount ELSE 0 END), 0)
                FROM Fees f
                INNER JOIN StudentClasses sc ON sc.StudentID = f.StudentID
                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')
                INNER JOIN Students s ON s.StudentID = f.StudentID
                WHERE REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND ISNULL(s.Status, N'نشط') = N'نشط'", conn))
            {
                AddYear(cmd, GetActiveYear());
                conn.Open();
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value);
            }
        }

        public int GetPendingFeesCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(*)
                FROM Fees f
                INNER JOIN StudentClasses sc ON sc.StudentID = f.StudentID
                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')
                INNER JOIN Students s ON s.StudentID = f.StudentID
                WHERE REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND f.Status = N'غير مدفوع'
                  AND ISNULL(s.Status, N'نشط') = N'نشط'", conn))
            {
                AddYear(cmd, GetActiveYear());
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public DataTable GetOperationalStatus()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("dbo.GetDashboardOperationalStatus", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var da = new SqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    da.Fill(table);
                    return table;
                }
            }
        }

        public int GetTodayAbsenceCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(*)
                FROM StudentAttendance a
                INNER JOIN StudentClasses sc ON sc.StudentID = a.StudentID
                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                INNER JOIN Students s ON s.StudentID = a.StudentID
                WHERE a.AttendanceDate = @Date
                  AND a.Status = N'غائب'
                  AND ISNULL(s.Status, N'نشط') = N'نشط'", conn))
            {
                AddYear(cmd, GetActiveYear());
                cmd.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.Today;
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
