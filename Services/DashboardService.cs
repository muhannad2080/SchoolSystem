using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class DashboardService
    {
        // تعريف الدالة AnnualClosingService لتنفيذ العملية المرتبطة بها.
        private readonly AnnualClosingService annualClosingService = new AnnualClosingService();

        // تعريف الدالة GetActiveYear لتنفيذ العملية المرتبطة بها.
        private string GetActiveYear()
        {
            string year = annualClosingService.GetActiveAcademicYear();

            return (year ?? string.Empty).Trim().Replace('-', '/');
        }

        // تعريف الدالة AddYear لتنفيذ العملية المرتبطة بها.
        private static void AddYear(SqlCommand command, string academicYear)
        {

            command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = academicYear ?? string.Empty;
        }

        // تعريف الدالة GetStudentCount لتنفيذ العملية المرتبطة بها.
        public int GetStudentCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة GetTeacherCount لتنفيذ العملية المرتبطة بها.
        public int GetTeacherCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers WHERE ISNULL(Status, N'نشط') <> N'غير نشط'", conn))
            {

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // تعريف الدالة GetSubjectCount لتنفيذ العملية المرتبطة بها.
        public int GetSubjectCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE ISNULL(IsActive, 1) = 1", conn))
            {

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // تعريف الدالة GetClassCount لتنفيذ العملية المرتبطة بها.
        public int GetClassCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.DashboardView, "ليس لديك صلاحية عرض لوحة التحكم.");
            using (var conn = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ISNULL(IsActive, 1) = 1", conn))
            {

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // تعريف الدالة GetStudentsPerClass لتنفيذ العملية المرتبطة بها.
        public DataTable GetStudentsPerClass()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة GetPendingFeesTotal لتنفيذ العملية المرتبطة بها.
        public decimal GetPendingFeesTotal()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة GetPendingFeesCount لتنفيذ العملية المرتبطة بها.
        public int GetPendingFeesCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة GetOperationalStatus لتنفيذ العملية المرتبطة بها.
        public DataTable GetOperationalStatus()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة GetTodayAbsenceCount لتنفيذ العملية المرتبطة بها.
        public int GetTodayAbsenceCount()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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
