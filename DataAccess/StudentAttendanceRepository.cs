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
                    SELECT SectionName AS Section
                    FROM SchoolSections
                    WHERE ClassID = @ClassID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND IsActive = 1
                    ORDER BY SectionName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;
                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim();
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
                        CONVERT(VARCHAR(5), a.DepartureTime, 108) AS DepartureTime,
                        ISNULL(a.ExcuseStatus, N'بدون عذر') AS ExcuseStatus,
                        ISNULL(a.AbsenceReason, N'') AS AbsenceReason,
                        ISNULL(a.Notes, N'') AS Notes
                    FROM Students s
                    LEFT JOIN StudentAttendance a
                        ON a.StudentID = s.StudentID
                        AND a.AttendanceDate = @AttendanceDate
                    WHERE s.ClassID = @ClassID
                      AND ISNULL(s.Section, N'') = @Section
                      AND REPLACE(ISNULL(s.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND ISNULL(s.Status, N'نشط') = N'نشط'
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace('-', '/'));
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
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

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
                        FROM StudentAttendance WITH (UPDLOCK, HOLDLOCK)
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
                            DepartureTime = @DepartureTime,
                            ExcuseStatus = @ExcuseStatus,
                            AbsenceReason = @AbsenceReason,
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
                            DepartureTime,
                            ExcuseStatus,
                            AbsenceReason,
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
                            @DepartureTime,
                            @ExcuseStatus,
                            @AbsenceReason,
                            @Notes,
                            GETDATE()
                        )
                    END;

                    COMMIT TRANSACTION;";

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

            if (item.DepartureTime.HasValue)
                cmd.Parameters.AddWithValue("@DepartureTime", item.DepartureTime.Value);
            else
                cmd.Parameters.AddWithValue("@DepartureTime", DBNull.Value);

            if (string.IsNullOrWhiteSpace(item.ExcuseStatus))
                cmd.Parameters.AddWithValue("@ExcuseStatus", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ExcuseStatus", item.ExcuseStatus.Trim());

            if (string.IsNullOrWhiteSpace(item.AbsenceReason))
                cmd.Parameters.AddWithValue("@AbsenceReason", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@AbsenceReason", item.AbsenceReason.Trim());

            if (string.IsNullOrWhiteSpace(item.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", item.Notes.Trim());
        }
    }
}
