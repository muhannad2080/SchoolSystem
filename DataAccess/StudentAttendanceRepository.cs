using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class StudentAttendanceRepository
    {
        public DataTable GetSections(int classId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT DISTINCT LTRIM(RTRIM(sc.Section)) AS Section
                    FROM StudentClasses sc
                    INNER JOIN Students s ON s.StudentID = sc.StudentID
                    WHERE sc.ClassID = @ClassID
                      AND sc.AcademicYear = @AcademicYear
                      AND ISNULL(s.Status, N'نشط') = N'نشط'
                      AND NULLIF(LTRIM(RTRIM(sc.Section)), N'') IS NOT NULL
                    ORDER BY LTRIM(RTRIM(sc.Section))";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear.Trim());
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAttendanceSheet(int classId, string section, string academicYear, DateTime date)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        s.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        ISNULL(a.AttendanceID, 0) AS AttendanceID,
                        ISNULL(a.Status, N'حاضر') AS Status,
                        CONVERT(VARCHAR(5), a.ArrivalTime, 108) AS ArrivalTime,
                        ISNULL(a.ExcuseStatus, N'بدون عذر') AS ExcuseStatus,
                        ISNULL(a.Notes, N'') AS Notes
                    FROM Students s
                    LEFT JOIN StudentAttendance a
                        ON a.StudentID = s.StudentID
                        AND a.AttendanceDate = @AttendanceDate
                    WHERE s.ClassID = @ClassID
                      AND ISNULL(s.Section, N'') = @Section
                      AND ISNULL(s.AcademicYear, N'') = @AcademicYear
                      AND ISNULL(s.Status, N'نشط') = N'نشط'
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);
                    cmd.Parameters.AddWithValue("@AttendanceDate", date.Date);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public bool SaveAttendance(StudentAttendance item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Students
                        WHERE StudentID = @StudentID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 50001, N'لا يمكن تسجيل حضور لطالب غير نشط.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM StudentAttendance
                        WHERE StudentID = @StudentID
                          AND AttendanceDate = @AttendanceDate
                    )
                    BEGIN
                        UPDATE StudentAttendance
                        SET
                            ClassID = @ClassID,
                            Section = @Section,
                            AcademicYear = @AcademicYear,
                            Status = @Status,
                            ArrivalTime = @ArrivalTime,
                            ExcuseStatus = @ExcuseStatus,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID
                          AND AttendanceDate = @AttendanceDate
                    END
                    ELSE
                    BEGIN
                        INSERT INTO StudentAttendance
                        (
                            StudentID,
                            ClassID,
                            Section,
                            AcademicYear,
                            AttendanceDate,
                            Status,
                            ArrivalTime,
                            ExcuseStatus,
                            Notes,
                            CreatedAt
                        )
                        VALUES
                        (
                            @StudentID,
                            @ClassID,
                            @Section,
                            @AcademicYear,
                            @AttendanceDate,
                            @Status,
                            @ArrivalTime,
                            @ExcuseStatus,
                            @Notes,
                            GETDATE()
                        )
                    END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, item);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, StudentAttendance item)
        {
            cmd.Parameters.AddWithValue("@StudentID", item.StudentID);
            cmd.Parameters.AddWithValue("@ClassID", item.ClassID);
            cmd.Parameters.AddWithValue("@Section", item.Section);
            cmd.Parameters.AddWithValue("@AcademicYear", item.AcademicYear);
            cmd.Parameters.AddWithValue("@AttendanceDate", item.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@Status", item.Status);

            if (item.ArrivalTime.HasValue)
                cmd.Parameters.AddWithValue("@ArrivalTime", item.ArrivalTime.Value);
            else
                cmd.Parameters.AddWithValue("@ArrivalTime", DBNull.Value);

            if (string.IsNullOrWhiteSpace(item.ExcuseStatus))
                cmd.Parameters.AddWithValue("@ExcuseStatus", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ExcuseStatus", item.ExcuseStatus.Trim());

            if (string.IsNullOrWhiteSpace(item.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", item.Notes.Trim());
        }
    }
}
