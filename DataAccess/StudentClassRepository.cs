using System;
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
                            AND sc.AcademicYear = @AcademicYear
                      )
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

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
                    SELECT SectionName AS Section
                    FROM SchoolSections
                    WHERE ClassID = @ClassID
                      AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                      AND IsActive = 1
                    ORDER BY SectionName";

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
                      AND sc.Section = @Section
                      AND sc.AcademicYear = @AcademicYear
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

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

                    IF EXISTS
                    (
                        SELECT 1
                        FROM StudentClasses
                        WHERE StudentID = @StudentID
                          AND AcademicYear = @AcademicYear
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
                              AND (AcademicYear = @AcademicYear OR AcademicYear IS NULL);
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

        public bool IsStudentAssignedInYear(int studentId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM StudentClasses
                    WHERE StudentID = @StudentID
                      AND AcademicYear = @AcademicYear";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

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
                "SELECT COUNT(1) FROM Students WHERE StudentID = @StudentID AND ISNULL(Status, N'منتظم') <> N'محذوف'", conn))
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
            cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (assignment.AcademicYear ?? string.Empty).Trim();
            cmd.Parameters.Add("@AssignedBy", SqlDbType.Int).Value = assignment.AssignedBy.HasValue ? (object)assignment.AssignedBy.Value : DBNull.Value;
        }
    }
}
