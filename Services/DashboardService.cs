using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class DashboardService
    {
        public int GetStudentCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE ISNULL(Status, N'نشط') = N'نشط'", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetTeacherCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers WHERE ISNULL(Status, N'نشط') <> N'غير نشط'", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetSubjectCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE ISNULL(IsActive, 1) = 1", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetClassCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ISNULL(IsActive, 1) = 1", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public DataTable GetStudentsPerClass()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                @"SELECT c.ClassName, COUNT(DISTINCT sc.StudentID) AS StudentCount
                  FROM Classes c
                  LEFT JOIN StudentClasses sc ON c.ClassID = sc.ClassID
                  LEFT JOIN Students s ON s.StudentID = sc.StudentID
                      AND ISNULL(s.Status, N'نشط') = N'نشط'
                  WHERE ISNULL(c.IsActive, 1) = 1
                  GROUP BY c.ClassName", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public decimal GetPendingFeesTotal()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                @"SELECT COALESCE(SUM(CASE WHEN RemainingAmount > 0 THEN RemainingAmount ELSE 0 END), 0)
                  FROM Fees f
                  INNER JOIN Students s ON s.StudentID = f.StudentID
                      AND ISNULL(s.Status, N'نشط') = N'نشط'", conn))
            {
                conn.Open();
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value);
            }
        }

        public int GetPendingFeesCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Fees f INNER JOIN Students s ON s.StudentID = f.StudentID AND ISNULL(s.Status, N'نشط') = N'نشط' WHERE f.Status = N'غير مدفوع'", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetTodayAbsenceCount()
        {
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM StudentAttendance a INNER JOIN Students s ON s.StudentID = a.StudentID AND ISNULL(s.Status, N'نشط') = N'نشط' WHERE a.AttendanceDate = @Date AND a.Status = N'غائب'", conn))
            {
                cmd.Parameters.AddWithValue("@Date", DateTime.Today);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}