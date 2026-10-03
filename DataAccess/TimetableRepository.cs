using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class TimetableRepository
    {
        // تعريف الدالة GetTeachers لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetSections لتنفيذ العملية المرتبطة بها.
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
                {

                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;

                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim().Replace('-', '/');


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        return dt;
                    }
                }
            }
        }

        // تعريف الدالة GetSubjectsByClass لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetAllTimetable لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddTimetable لتنفيذ العملية المرتبطة بها.
        public bool AddTimetable(TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateAndLockSlot(conn, transaction, item, 0);

                    const string query = @"
                        INSERT INTO SchoolTimetable
                        (
                            ClassID, Section, SubjectID, TeacherID, AcademicYear, TermName,
                            DayName, PeriodNo, StartTime, EndTime, RoomName, Notes, IsActive, CreatedAt
                        )
                        VALUES
                        (
                            @ClassID, @Section, @SubjectID, @TeacherID, @AcademicYear, @TermName,
                            @DayName, @PeriodNo, @StartTime, @EndTime, @RoomName, @Notes, @IsActive, GETDATE()
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, item, false);
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        item.TimetableID = Convert.ToInt32(result);
                    }

                    transaction.Commit();
                    return item.TimetableID > 0;
                }
            }
        }

        // تعريف الدالة UpdateTimetable لتنفيذ العملية المرتبطة بها.
        public bool UpdateTimetable(TimetableEntry item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateAndLockSlot(conn, transaction, item, item.TimetableID);

                    const string query = @"
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

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, item, true);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }

        // تعريف الدالة ValidateAndLockSlot لتنفيذ العملية المرتبطة بها.
        private void ValidateAndLockSlot(SqlConnection conn, SqlTransaction transaction, TimetableEntry item, int excludedTimetableId)
        {
            const string validation = @"
                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Classes WITH (UPDLOCK, HOLDLOCK)
                    WHERE ClassID = @ClassID AND ISNULL(IsActive, 1) = 1
                )
                    THROW 50008, N'لا يمكن حفظ حصة لصف غير نشط.', 1;

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Teachers WITH (UPDLOCK, HOLDLOCK)
                    WHERE TeacherID = @TeacherID AND ISNULL(Status, N'نشط') <> N'غير نشط'
                )
                    THROW 50007, N'لا يمكن حفظ حصة لمعلم غير نشط.', 1;

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Subjects WITH (UPDLOCK, HOLDLOCK)
                    WHERE SubjectID = @SubjectID
                      AND ISNULL(IsActive, 1) = 1
                      AND (ClassID = @ClassID OR ClassID IS NULL)
                )
                    THROW 50006, N'المادة غير نشطة أو غير مرتبطة بالصف المحدد.', 1;

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM SchoolSections WITH (UPDLOCK, HOLDLOCK)
                    WHERE ClassID = @ClassID
                      AND LTRIM(RTRIM(SectionName)) = LTRIM(RTRIM(@Section))
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
                          = REPLACE(@AcademicYear, N'-', N'/')
                      AND ISNULL(IsActive, 1) = 1
                )
                    THROW 50009, N'الشعبة غير موجودة أو غير فعالة لهذا الصف والعام الدراسي.', 1;

                IF EXISTS
                (
                    SELECT 1
                    FROM SchoolTimetable WITH (UPDLOCK, HOLDLOCK)
                    WHERE TimetableID <> @ExcludedTimetableID
                      AND IsActive = 1
                      AND ClassID = @ClassID
                      AND LTRIM(RTRIM(Section)) = LTRIM(RTRIM(@Section))
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
                          = REPLACE(@AcademicYear, N'-', N'/')
                      AND TermName = @TermName
                      AND DayName = @DayName
                      AND StartTime < @EndTime
                      AND EndTime > @StartTime
                )
                    THROW 50020, N'يوجد تعارض زمني مع حصة أخرى للصف والشعبة.', 1;

                IF EXISTS
                (
                    SELECT 1
                    FROM SchoolTimetable WITH (UPDLOCK, HOLDLOCK)
                    WHERE TimetableID <> @ExcludedTimetableID
                      AND IsActive = 1
                      AND TeacherID = @TeacherID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
                          = REPLACE(@AcademicYear, N'-', N'/')
                      AND TermName = @TermName
                      AND DayName = @DayName
                      AND StartTime < @EndTime
                      AND EndTime > @StartTime
                )
                    THROW 50021, N'المعلم لديه حصة أخرى في نفس الوقت.', 1;

                IF NULLIF(LTRIM(RTRIM(@RoomName)), N'') IS NOT NULL
                   AND EXISTS
                (
                    SELECT 1
                    FROM SchoolTimetable WITH (UPDLOCK, HOLDLOCK)
                    WHERE TimetableID <> @ExcludedTimetableID
                      AND IsActive = 1
                      AND LTRIM(RTRIM(ISNULL(RoomName, N''))) = LTRIM(RTRIM(@RoomName))
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
                          = REPLACE(@AcademicYear, N'-', N'/')
                      AND TermName = @TermName
                      AND DayName = @DayName
                      AND StartTime < @EndTime
                      AND EndTime > @StartTime
                )
                    THROW 50022, N'القاعة مستخدمة في حصة أخرى في نفس الوقت.', 1;";

            using (SqlCommand cmd = new SqlCommand(validation, conn, transaction))
            {
                AddParameters(cmd, item, true);
                cmd.Parameters.Add("@ExcludedTimetableID", SqlDbType.Int).Value = excludedTimetableId;
                cmd.ExecuteNonQuery();
            }
        }

        // تعريف الدالة DeleteTimetable لتنفيذ العملية المرتبطة بها.
        public bool DeleteTimetable(int timetableId)
        {
            if (timetableId <= 0)
                throw new ArgumentException("رقم الحصة غير صحيح.");

            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string query = @"
                        DELETE FROM SchoolTimetable
                        WHERE TimetableID = @TimetableID";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@TimetableID", SqlDbType.Int).Value = timetableId;
                        bool deleted = cmd.ExecuteNonQuery() > 0;
                        if (!deleted)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }

        // تعريف الدالة HasClassConflict لتنفيذ العملية المرتبطة بها.
        public bool HasClassConflict(TimetableEntry item)
        {
            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND ClassID = @ClassID
                  AND Section = @Section
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";


            return CountConflict(query, item) > 0;
        }

        // تعريف الدالة HasTeacherConflict لتنفيذ العملية المرتبطة بها.
        public bool HasTeacherConflict(TimetableEntry item)
        {
            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND TeacherID = @TeacherID
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";


            return CountConflict(query, item) > 0;
        }

        // تعريف الدالة HasRoomConflict لتنفيذ العملية المرتبطة بها.
        public bool HasRoomConflict(TimetableEntry item)
        {
            if (string.IsNullOrWhiteSpace(item.RoomName))
                return false;

            string query = @"
                SELECT COUNT(*)
                FROM SchoolTimetable
                WHERE TimetableID <> @TimetableID
                  AND RoomName = @RoomName
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND TermName = @TermName
                  AND DayName = @DayName
                  AND IsActive = 1
                  AND StartTime < @EndTime
                  AND EndTime > @StartTime";


            return CountConflict(query, item) > 0;
        }

        // تعريف الدالة CountConflict لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة NormalizeDay لتنفيذ العملية المرتبطة بها.
        private static string NormalizeDay(string dayName)
        {
            if (string.IsNullOrWhiteSpace(dayName))
                return string.Empty;


            return dayName.Trim().Replace("الإثنين", "الاثنين");
        }
    }
}
