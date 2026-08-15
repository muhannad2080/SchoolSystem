using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class GradeRepository
    {
        public DataTable GetAllSubjects()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT SubjectID, SubjectName
                    FROM Subjects
                    WHERE IsActive = 1
                    ORDER BY SubjectName";

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
                    SELECT
                        SubjectID,
                        SubjectName
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

        public DataTable GetGradeEntryStudents(int classId, string section, string academicYear, int subjectId, string termName)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        s.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        ISNULL(g.GradeID, 0) AS GradeID,
                        ISNULL(g.Quiz1, 0) AS Quiz1,
                        ISNULL(g.Quiz2, 0) AS Quiz2,
                        ISNULL(g.CourseWork, 0) AS CourseWork,
                        ISNULL(g.FinalExam, 0) AS FinalExam,
                        ISNULL(g.Total, 0) AS Total,
                        ISNULL(g.GradeLetter, N'') AS GradeLetter,
                        ISNULL(g.ResultStatus, N'') AS ResultStatus,
                        ISNULL(g.Notes, N'') AS Notes
                    FROM Students s
                    LEFT JOIN StudentGrades g
                        ON g.StudentID = s.StudentID
                        AND g.SubjectID = @SubjectID
                        AND g.AcademicYear = @AcademicYear
                        AND g.TermName = @TermName
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
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@TermName", termName);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public bool SaveGrade(StudentGrade grade)
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
                        THROW 50002, N'لا يمكن حفظ درجة لطالب غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Subjects
                        WHERE SubjectID = @SubjectID
                          AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 50005, N'لا يمكن حفظ درجة لمادة غير نشطة.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM StudentGrades
                        WHERE StudentID = @StudentID
                          AND SubjectID = @SubjectID
                          AND AcademicYear = @AcademicYear
                          AND TermName = @TermName
                    )
                    BEGIN
                        UPDATE StudentGrades
                        SET
                            ClassID = @ClassID,
                            Section = @Section,
                            Quiz1 = @Quiz1,
                            Quiz2 = @Quiz2,
                            CourseWork = @CourseWork,
                            FinalExam = @FinalExam,
                            Total = @Total,
                            GradeLetter = @GradeLetter,
                            ResultStatus = @ResultStatus,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID
                          AND SubjectID = @SubjectID
                          AND AcademicYear = @AcademicYear
                          AND TermName = @TermName
                    END
                    ELSE
                    BEGIN
                        INSERT INTO StudentGrades
                        (
                            StudentID,
                            SubjectID,
                            ClassID,
                            Section,
                            AcademicYear,
                            TermName,
                            Quiz1,
                            Quiz2,
                            CourseWork,
                            FinalExam,
                            Total,
                            GradeLetter,
                            ResultStatus,
                            Notes,
                            CreatedAt
                        )
                        VALUES
                        (
                            @StudentID,
                            @SubjectID,
                            @ClassID,
                            @Section,
                            @AcademicYear,
                            @TermName,
                            @Quiz1,
                            @Quiz2,
                            @CourseWork,
                            @FinalExam,
                            @Total,
                            @GradeLetter,
                            @ResultStatus,
                            @Notes,
                            GETDATE()
                        )
                    END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, grade);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteGrade(int gradeId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM StudentGrades WHERE GradeID = @GradeID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GradeID", gradeId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, StudentGrade grade)
        {
            cmd.Parameters.AddWithValue("@StudentID", grade.StudentID);
            cmd.Parameters.AddWithValue("@SubjectID", grade.SubjectID);
            cmd.Parameters.AddWithValue("@ClassID", grade.ClassID);
            cmd.Parameters.AddWithValue("@Section", grade.Section);
            cmd.Parameters.AddWithValue("@AcademicYear", grade.AcademicYear);
            cmd.Parameters.AddWithValue("@TermName", grade.TermName);
            cmd.Parameters.AddWithValue("@Quiz1", grade.Quiz1);
            cmd.Parameters.AddWithValue("@Quiz2", grade.Quiz2);
            cmd.Parameters.AddWithValue("@CourseWork", grade.CourseWork);
            cmd.Parameters.AddWithValue("@FinalExam", grade.FinalExam);
            cmd.Parameters.AddWithValue("@Total", grade.Total);
            cmd.Parameters.AddWithValue("@GradeLetter", grade.GradeLetter);
            cmd.Parameters.AddWithValue("@ResultStatus", grade.ResultStatus);

            if (string.IsNullOrWhiteSpace(grade.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", grade.Notes.Trim());
        }
    }
}
