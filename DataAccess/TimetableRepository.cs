using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class TimetableRepository
    {
        public DataTable GetTeachers()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT TeacherID, FullName
                    FROM Teachers
                    WHERE ISNULL(Status, N'نشط') <> N'غير نشط'
                    ORDER BY FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSubjectsByClass(int classId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT SubjectID, SubjectName
                    FROM Subjects
                    WHERE IsActive = 1
                      AND (ClassID = @ClassID OR ClassID IS NULL)
                    ORDER BY SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetAllTimetable()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        t.TimetableID,
                        t.AcademicYear,
                        t.TermName,
                        c.ClassName,
                        t.Section,
                        t.DayName,
                        t.PeriodNo,
                        CONVERT(VARCHAR(5), t.StartTime, 108) AS StartTime,
                        CONVERT(VARCHAR(5), t.EndTime, 108) AS EndTime,
                        s.SubjectName,
                        te.FullName AS TeacherName,
                        t.RoomName,
                        t.Notes,
                        t.IsActive,
                        t.ClassID,
                        t.SubjectID,
                        t.TeacherID
                    FROM SchoolTimetable t
                    INNER JOIN Classes c ON t.ClassID = c.ClassID
                    INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                    INNER JOIN Teachers te ON t.TeacherID = te.TeacherID
                    ORDER BY c.ClassName, t.Section, 
                        CASE t.DayName
                            WHEN N'السبت' THEN 1
                            WHEN N'الأحد' THEN 2
                            WHEN N'الاثنين' THEN 3
                            WHEN N'الثلاثاء' THEN 4
                            WHEN N'الأربعاء' THEN 5
                            WHEN N'الخميس' THEN 6
                            ELSE 7
                        END,
                        t.PeriodNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddTimetable(TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO SchoolTimetable
                    (
                        ClassID,
                        Section,
                        SubjectID,
                        TeacherID,
                        AcademicYear,
                        TermName,
                        DayName,
                        PeriodNo,
                        StartTime,
                        EndTime,
                        RoomName,
                        Notes,
                        IsActive,
                        CreatedAt
                    )
                    VALUES
                    (
                        @ClassID,
                        @Section,
                        @SubjectID,
                        @TeacherID,
                        @AcademicYear,
                        @TermName,
                        @DayName,
                        @PeriodNo,
                        @StartTime,
                        @EndTime,
                        @RoomName,
                        @Notes,
                        @IsActive,
                        GETDATE()
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, item, false);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateTimetable(TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE SchoolTimetable
                    SET
                        ClassID = @ClassID,
                        Section = @Section,
                        SubjectID = @SubjectID,
                        TeacherID = @TeacherID,
                        AcademicYear = @AcademicYear,
                        TermName = @TermName,
                        DayName = @DayName,
                        PeriodNo = @PeriodNo,
                        StartTime = @StartTime,
                        EndTime = @EndTime,
                        RoomName = @RoomName,
                        Notes = @Notes,
                        IsActive = @IsActive,
                        UpdatedAt = GETDATE()
                    WHERE TimetableID = @TimetableID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, item, true);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteTimetable(int timetableId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM SchoolTimetable WHERE TimetableID = @TimetableID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TimetableID", timetableId);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasClassConflict(TimetableEntry item)
        {
            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND ClassID = @ClassID
                  AND Section = @Section
                  AND AcademicYear = @AcademicYear
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";

            return CountConflict(query, item) > 0;
        }

        public bool HasTeacherConflict(TimetableEntry item)
        {
            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND TeacherID = @TeacherID
                  AND AcademicYear = @AcademicYear
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";

            return CountConflict(query, item) > 0;
        }

        public bool HasRoomConflict(TimetableEntry item)
        {
            if (string.IsNullOrWhiteSpace(item.RoomName))
                return false;

            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND RoomName = @RoomName
                  AND AcademicYear = @AcademicYear
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";

            return CountConflict(query, item) > 0;
        }

        private int CountConflict(string query, TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TimetableID", item.TimetableID);
                cmd.Parameters.AddWithValue("@ClassID", item.ClassID);
                cmd.Parameters.AddWithValue("@Section", item.Section);
                cmd.Parameters.AddWithValue("@TeacherID", item.TeacherID);
                cmd.Parameters.AddWithValue("@AcademicYear", item.AcademicYear);
                cmd.Parameters.AddWithValue("@TermName", item.TermName);
                cmd.Parameters.AddWithValue("@DayName", item.DayName);
                cmd.Parameters.AddWithValue("@StartTime", item.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", item.EndTime);
                cmd.Parameters.AddWithValue("@RoomName", string.IsNullOrWhiteSpace(item.RoomName) ? "" : item.RoomName.Trim());

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void AddParameters(SqlCommand cmd, TimetableEntry item, bool includeId)
        {
            if (includeId)
                cmd.Parameters.AddWithValue("@TimetableID", item.TimetableID);
            else
                cmd.Parameters.AddWithValue("@TimetableID", 0);

            cmd.Parameters.AddWithValue("@ClassID", item.ClassID);
            cmd.Parameters.AddWithValue("@Section", item.Section);
            cmd.Parameters.AddWithValue("@SubjectID", item.SubjectID);
            cmd.Parameters.AddWithValue("@TeacherID", item.TeacherID);
            cmd.Parameters.AddWithValue("@AcademicYear", item.AcademicYear);
            cmd.Parameters.AddWithValue("@TermName", item.TermName);
            cmd.Parameters.AddWithValue("@DayName", item.DayName);
            cmd.Parameters.AddWithValue("@PeriodNo", item.PeriodNo);
            cmd.Parameters.AddWithValue("@StartTime", item.StartTime);
            cmd.Parameters.AddWithValue("@EndTime", item.EndTime);
            cmd.Parameters.AddWithValue("@IsActive", item.IsActive);

            if (string.IsNullOrWhiteSpace(item.RoomName))
                cmd.Parameters.AddWithValue("@RoomName", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@RoomName", item.RoomName.Trim());

            if (string.IsNullOrWhiteSpace(item.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", item.Notes.Trim());
        }
    }
}
