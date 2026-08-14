using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;

namespace SchoolSystem.Services
{
    public class DashboardService
    {
        public int GetStudentCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Students", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetTeacherCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetSubjectCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetClassCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Classes", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public DataTable GetStudentsPerClass()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                @"SELECT c.ClassName, COUNT(sc.StudentID) AS StudentCount
                  FROM Classes c
                  LEFT JOIN StudentClasses sc ON c.ClassID = sc.ClassID
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
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                @"SELECT COALESCE(SUM(CASE WHEN RemainingAmount > 0 THEN RemainingAmount ELSE 0 END), 0)
                  FROM Fees", conn))
            {
                conn.Open();
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value);
            }
        }

        public int GetPendingFeesCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Fees WHERE Status = N'غير مدفوع'", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetTodayAbsenceCount()
        {
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM StudentAttendance WHERE AttendanceDate = @Date AND Status = N'غائب'", conn))
            {
                cmd.Parameters.AddWithValue("@Date", DateTime.Today);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}