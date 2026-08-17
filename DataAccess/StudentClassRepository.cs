using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class StudentClassRepository
    {
        public DataTable GetUnassignedStudents(string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        s.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        s.StudentPhone AS Phone
                    FROM Students s
                    WHERE ISNULL(s.Status, N'نشط') = N'نشط'
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM StudentClasses sc
                          WHERE sc.StudentID = s.StudentID
                            AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                      )
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace('-', '/'));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetSections(int classId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT LTRIM(RTRIM(SectionName)) AS Section
                    FROM dbo.SchoolSections
                    WHERE ClassID = @ClassID
                      AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                      AND ISNULL(IsActive, 1) = 1
                      AND NULLIF(LTRIM(RTRIM(SectionName)), N'') IS NOT NULL
                    GROUP BY LTRIM(RTRIM(SectionName))
                    ORDER BY LTRIM(RTRIM(SectionName))";

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

        public DataTable GetAssignedStudents(int classId, string section, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        sc.StudentClassID,
                        sc.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        s.StudentPhone AS Phone,
                        sc.ClassID,
                        c.ClassName,
                        sc.Section,
                        sc.AcademicYear,
                        sc.AssignedDate
                    FROM StudentClasses sc
                    INNER JOIN Students s ON sc.StudentID = s.StudentID
                    INNER JOIN Classes c ON sc.ClassID = c.ClassID
                    WHERE sc.ClassID = @ClassID
                      AND LTRIM(RTRIM(sc.Section)) = LTRIM(RTRIM(@Section))
                      AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace('-', '/'));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public bool AssignStudent(StudentClass assignment)
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
                        THROW 50006, N'لا يمكن تعيين طالب غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Classes
                        WHERE ClassID = @ClassID
                          AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50007, N'لا يمكن التعيين إلى فصل غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.SchoolSections ss
                        INNER JOIN Students s ON s.StudentID = @StudentID
                        WHERE ss.ClassID = @ClassID
                          AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(@Section))
                          AND REPLACE(ss.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                          AND ISNULL(ss.IsActive, 1) = 1
                          AND (ss.AllowedGender IS NULL OR LTRIM(RTRIM(ss.AllowedGender)) = N''
                               OR LTRIM(RTRIM(ss.AllowedGender)) IN (N'Any', N'All', N'الكل', N'مختلط')
                               OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Male', N'ذكر', N'ذكور')
                                   AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Male', N'ذكر', N'ذكور'))
                               OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Female', N'أنثى', N'إناث')
                                   AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Female', N'أنثى', N'إناث')))
                    )
                        THROW 50008, N'الشعبة المحددة غير موجودة أو لا تسمح بجنس الطالب.', 1;

                    IF EXISTS
                    (
                        SELECT 1 FROM dbo.SchoolSections ss
                        WHERE ss.ClassID = @ClassID
                          AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(@Section))
                          AND REPLACE(ss.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                          AND ISNULL(ss.IsActive, 1) = 1 AND ss.Capacity IS NOT NULL AND ss.Capacity > 0
                          AND (SELECT COUNT(1) FROM StudentClasses sc
                               WHERE sc.ClassID = @ClassID AND LTRIM(RTRIM(sc.Section)) = LTRIM(RTRIM(@Section))
                                 AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')) >= ss.Capacity
                    )
                        THROW 50013, N'سعة الشعبة المحددة مكتملة.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM StudentClasses
                        WHERE StudentID = @StudentID
                          AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                    )
                        THROW 50009, N'هذا الطالب موزع مسبقاً في نفس العام الدراسي.', 1;

                    INSERT INTO StudentClasses
                    (
                        StudentID,
                        ClassID,
                        Section,
                        AcademicYear,
                        AssignedDate,
                        AssignedBy
                    )
                    VALUES
                    (
                        @StudentID,
                        @ClassID,
                        @Section,
                        @AcademicYear,
                        GETDATE(),
                        @AssignedBy
                    );

                    UPDATE Students
                    SET 
                        ClassID = @ClassID,
                        Section = @Section,
                        AcademicYear = @AcademicYear,
                        UpdatedAt = GETDATE()
                    WHERE StudentID = @StudentID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, assignment);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool RemoveAssignment(int studentClassId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

                    DECLARE @StudentID INT;
                    DECLARE @AcademicYear NVARCHAR(20);
                    DECLARE @Deleted INT;

                    SELECT
                        @StudentID = StudentID,
                        @AcademicYear = AcademicYear
                    FROM StudentClasses
                    WHERE StudentClassID = @StudentClassID;

                    DELETE FROM StudentClasses
                    WHERE StudentClassID = @StudentClassID;
                    SET @Deleted = @@ROWCOUNT;

                    IF @Deleted > 0 AND @StudentID IS NOT NULL
                    BEGIN
                        IF EXISTS (SELECT 1 FROM StudentClasses WHERE StudentID = @StudentID)
                        BEGIN
                            UPDATE s
                            SET
                                s.ClassID = latest.ClassID,
                                s.Section = latest.Section,
                                s.AcademicYear = latest.AcademicYear,
                                s.UpdatedAt = GETDATE()
                            FROM Students s
                            CROSS APPLY
                            (
                                SELECT TOP (1)
                                    sc.ClassID,
                                    sc.Section,
                                    sc.AcademicYear
                                FROM StudentClasses sc
                                WHERE sc.StudentID = @StudentID
                                ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC
                            ) latest
                            WHERE s.StudentID = @StudentID;
                        END
                        ELSE
                        BEGIN
                            UPDATE Students
                            SET
                                ClassID = NULL,
                                Section = NULL,
                                AcademicYear = NULL,
                                UpdatedAt = GETDATE()
                            WHERE StudentID = @StudentID
                              AND (REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-') OR AcademicYear IS NULL);
                        END
                    END;

                    COMMIT TRANSACTION;
                    SELECT @Deleted;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentClassID", studentClassId);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public int RemoveAssignments(IList<int> studentClassIds)
        {
            if (studentClassIds == null || studentClassIds.Count == 0)
                return 0;

            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;
                    DECLARE @Removed TABLE (StudentID INT, AcademicYear NVARCHAR(20));
                    DELETE FROM StudentClasses
                    OUTPUT deleted.StudentID, deleted.AcademicYear INTO @Removed(StudentID, AcademicYear)
                    WHERE StudentClassID IN (SELECT TRY_CONVERT(INT, value) FROM STRING_SPLIT(@Ids, N',') WHERE TRY_CONVERT(INT, value) IS NOT NULL);
                    DECLARE @StudentID INT;
                    DECLARE student_cursor CURSOR LOCAL FAST_FORWARD FOR
                        SELECT DISTINCT StudentID FROM @Removed;
                    OPEN student_cursor;
                    FETCH NEXT FROM student_cursor INTO @StudentID;
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        IF EXISTS (SELECT 1 FROM StudentClasses WHERE StudentID = @StudentID)
                        BEGIN
                            UPDATE s SET s.ClassID = latest.ClassID, s.Section = latest.Section,
                                s.AcademicYear = latest.AcademicYear, s.UpdatedAt = GETDATE()
                            FROM Students s
                            CROSS APPLY (SELECT TOP (1) sc.ClassID, sc.Section, sc.AcademicYear
                                FROM StudentClasses sc WHERE sc.StudentID = @StudentID
                                ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC) latest
                            WHERE s.StudentID = @StudentID;
                        END
                        ELSE
                        BEGIN
                            UPDATE Students SET ClassID = NULL, Section = NULL, AcademicYear = NULL,
                                UpdatedAt = GETDATE() WHERE StudentID = @StudentID;
                        END;
                        FETCH NEXT FROM student_cursor INTO @StudentID;
                    END;
                    CLOSE student_cursor;
                    DEALLOCATE student_cursor;
                    COMMIT TRANSACTION;
                    SELECT COUNT(1) FROM @Removed;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Ids", SqlDbType.NVarChar, -1).Value = string.Join(",", studentClassIds);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool TransferAssignment(int studentClassId, int targetClassId, string targetSection, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;
                    DECLARE @StudentID INT;
                    SELECT @StudentID = StudentID FROM StudentClasses WITH (UPDLOCK, HOLDLOCK)
                    WHERE StudentClassID = @StudentClassID;
                    IF @StudentID IS NULL THROW 50010, N'سجل التوزيع غير موجود.', 1;
                    IF NOT EXISTS (SELECT 1 FROM SchoolSections ss INNER JOIN Students s ON s.StudentID = @StudentID
                        WHERE ss.ClassID = @ClassID
                        AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(@Section))
                        AND REPLACE(ss.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                        AND ISNULL(ss.IsActive, 1) = 1
                        AND (ss.AllowedGender IS NULL OR LTRIM(RTRIM(ss.AllowedGender)) = N''
                             OR LTRIM(RTRIM(ss.AllowedGender)) IN (N'Any', N'All', N'الكل', N'مختلط')
                             OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Male', N'ذكر', N'ذكور') AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Male', N'ذكر', N'ذكور'))
                             OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Female', N'أنثى', N'إناث') AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Female', N'أنثى', N'إناث')))
                    )
                        THROW 50011, N'الشعبة الهدف غير موجودة أو لا تسمح بجنس الطالب.', 1;
                    IF EXISTS (SELECT 1 FROM SchoolSections ss WHERE ss.ClassID = @ClassID
                        AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(@Section))
                        AND REPLACE(ss.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                        AND ISNULL(ss.IsActive, 1) = 1 AND ss.Capacity IS NOT NULL AND ss.Capacity > 0
                        AND (SELECT COUNT(1) FROM StudentClasses sc WHERE sc.ClassID = @ClassID
                             AND LTRIM(RTRIM(sc.Section)) = LTRIM(RTRIM(@Section))
                             AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                             AND sc.StudentClassID <> @StudentClassID) >= ss.Capacity)
                        THROW 50014, N'سعة الشعبة الهدف مكتملة.', 1;
                    IF EXISTS (SELECT 1 FROM StudentClasses WHERE StudentID = @StudentID
                        AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                        AND StudentClassID <> @StudentClassID)
                        THROW 50012, N'يوجد توزيع آخر للطالب في نفس العام الدراسي.', 1;
                    UPDATE StudentClasses SET ClassID = @ClassID, Section = LTRIM(RTRIM(@Section)),
                        AcademicYear = REPLACE(@AcademicYear, N'-', N'/'), AssignedDate = GETDATE()
                    WHERE StudentClassID = @StudentClassID;
                    UPDATE Students SET ClassID = @ClassID, Section = LTRIM(RTRIM(@Section)),
                        AcademicYear = REPLACE(@AcademicYear, N'-', N'/'), UpdatedAt = GETDATE()
                    WHERE StudentID = @StudentID;
                    COMMIT TRANSACTION;
                    SELECT 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@StudentClassID", SqlDbType.Int).Value = studentClassId;
                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = targetClassId;
                    cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 50).Value = (targetSection ?? string.Empty).Trim();
                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim().Replace('-', '/');
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public DataTable GetSectionStatistics(int classId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT LTRIM(RTRIM(ss.SectionName)) AS Section,
                        COUNT(sc.StudentClassID) AS AssignedCount,
                        ss.Capacity
                    FROM SchoolSections ss
                    LEFT JOIN StudentClasses sc ON sc.ClassID = ss.ClassID
                        AND LTRIM(RTRIM(sc.Section)) = LTRIM(RTRIM(ss.SectionName))
                        AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(ss.AcademicYear, N'/', N'-')
                    WHERE ss.ClassID = @ClassID
                      AND REPLACE(ss.AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                      AND ISNULL(ss.IsActive, 1) = 1
                      AND NULLIF(LTRIM(RTRIM(ss.SectionName)), N'') IS NOT NULL
                    GROUP BY LTRIM(RTRIM(ss.SectionName)), ss.Capacity
                    ORDER BY LTRIM(RTRIM(ss.SectionName));";
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

        public bool IsStudentAssignedInYear(int studentId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM StudentClasses
                    WHERE StudentID = @StudentID
                      AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace('-', '/'));

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool StudentExists(int studentId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Students WHERE StudentID = @StudentID AND ISNULL(Status, N'نشط') = N'نشط'", conn))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool ClassExists(int classId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Classes WHERE ClassID = @ClassID AND ISNULL(IsActive, 1) = 1", conn))
            {
                cmd.Parameters.AddWithValue("@ClassID", classId);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void AddParameters(SqlCommand cmd, StudentClass assignment)
        {
            cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = assignment.StudentID;
            cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = assignment.ClassID;
            cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 50).Value = (assignment.Section ?? string.Empty).Trim();
            cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (assignment.AcademicYear ?? string.Empty).Trim().Replace('-', '/');
            cmd.Parameters.Add("@AssignedBy", SqlDbType.Int).Value = assignment.AssignedBy.HasValue ? (object)assignment.AssignedBy.Value : DBNull.Value;
        }
    }
}
