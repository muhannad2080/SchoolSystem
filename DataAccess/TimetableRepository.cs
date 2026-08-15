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

        public DataTable GetSections(int classId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT DISTINCT LTRIM(RTRIM(Section)) AS Section
                    FROM StudentClasses
                    WHERE ClassID = @ClassID
                      AND LTRIM(RTRIM(AcademicYear)) = @AcademicYear
                      AND NULLIF(LTRIM(RTRIM(Section)), N'') IS NOT NULL
                    ORDER BY Section";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;
                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
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
                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Subjects
                        WHERE SubjectID = @SubjectID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50006, N'لا يمكن إضافة حصة لمادة غير نشطة.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Teachers
                        WHERE TeacherID = @TeacherID AND ISNULL(Status, N'نشط') <> N'غير نشط'
                    )
                        THROW 50007, N'لا يمكن إضافة حصة لمعلم غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Classes
                        WHERE ClassID = @ClassID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50008, N'لا يمكن إضافة حصة لصف غير نشط.', 1;

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
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, item, false);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    item.TimetableID = Convert.ToInt32(result);
                    return item.TimetableID > 0;
                }
            }
        }

        public bool UpdateTimetable(TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Subjects
                        WHERE SubjectID = @SubjectID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50006, N'لا يمكن تعديل الحصة إلى مادة غير نشطة.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Teachers
                        WHERE TeacherID = @TeacherID AND ISNULL(Status, N'نشط') <> N'غير نشط'
                    )
                        THROW 50007, N'لا يمكن تعديل الحصة إلى معلم غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Classes
                        WHERE ClassID = @ClassID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50008, N'لا يمكن تعديل الحصة إلى صف غير نشط.', 1;

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
                cmd.Parameters.Add("@TimetableID", SqlDbType.Int).Value = item.TimetableID;
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
                cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 100).Value = item.Section.Trim();
                cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = item.TeacherID;
                cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = item.AcademicYear.Trim();
                cmd.Parameters.Add("@TermName", SqlDbType.NVarChar, 50).Value = item.TermName.Trim();
                cmd.Parameters.Add("@DayName", SqlDbType.NVarChar, 30).Value = NormalizeDay(item.DayName);
                cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = item.StartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.Time).Value = item.EndTime;
                cmd.Parameters.Add("@RoomName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(item.RoomName) ? (object)DBNull.Value : item.RoomName.Trim();

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void AddParameters(SqlCommand cmd, TimetableEntry item, bool includeId)
        {
            cmd.Parameters.Add("@TimetableID", SqlDbType.Int).Value = includeId ? item.TimetableID : 0;
            cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
            cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 100).Value = item.Section.Trim();
            cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = item.SubjectID;
            cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = item.TeacherID;
            cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = item.AcademicYear.Trim();
            cmd.Parameters.Add("@TermName", SqlDbType.NVarChar, 50).Value = item.TermName.Trim();
            cmd.Parameters.Add("@DayName", SqlDbType.NVarChar, 30).Value = NormalizeDay(item.DayName);
            cmd.Parameters.Add("@PeriodNo", SqlDbType.Int).Value = item.PeriodNo;
            cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = item.StartTime;
            cmd.Parameters.Add("@EndTime", SqlDbType.Time).Value = item.EndTime;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = item.IsActive;
            cmd.Parameters.Add("@RoomName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(item.RoomName) ? (object)DBNull.Value : item.RoomName.Trim();
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(item.Notes) ? (object)DBNull.Value : item.Notes.Trim();
        }

        private static string NormalizeDay(string dayName)
        {
            if (string.IsNullOrWhiteSpace(dayName))
                return string.Empty;

            return dayName.Trim().Replace("الإثنين", "الاثنين");
        }
    }
}
