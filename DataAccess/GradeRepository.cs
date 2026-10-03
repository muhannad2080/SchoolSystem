using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class GradeRepository
    {
        // تعريف الدالة GetAllSubjects لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetSubjectsByClass لتنفيذ العملية المرتبطة بها.
        public DataTable GetSubjectsByClass(int classId, string termName)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT DISTINCT s.SubjectID, s.SubjectName
                    FROM dbo.Subjects s
                    INNER JOIN dbo.ClassSubjects cs ON cs.SubjectID = s.SubjectID AND cs.ClassID = @ClassID AND ISNULL(cs.IsActive, 1) = 1
                    INNER JOIN dbo.ClassSubjectTerms cst ON cst.ClassSubjectID = cs.ClassSubjectID AND ISNULL(cst.IsActive, 1) = 1
                    INNER JOIN dbo.AcademicTerms t ON t.TermID = cst.TermID AND t.TermName = @TermName AND ISNULL(t.IsActive, 1) = 1
                    WHERE ISNULL(s.IsActive, 1) = 1
                    ORDER BY s.SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    cmd.Parameters.AddWithValue("@TermName", termName == null ? string.Empty : termName.Trim());


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        return dt;
                    }
                }
            }
        }

        // تعريف الدالة GetGradeEntryStudents لتنفيذ العملية المرتبطة بها.
        public DataTable GetGradeEntryStudents(int classId, string section, string academicYear, int subjectId, string termName)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    WITH AllValidSubjects AS
                    (
                        SELECT DISTINCT cs.SubjectID, s.SubjectName
                        FROM dbo.ClassSubjects cs
                        INNER JOIN dbo.Subjects s ON s.SubjectID = cs.SubjectID AND ISNULL(s.IsActive, 1) = 1
                        INNER JOIN dbo.ClassSubjectTerms cst ON cst.ClassSubjectID = cs.ClassSubjectID AND ISNULL(cst.IsActive, 1) = 1
                        INNER JOIN dbo.AcademicTerms t ON t.TermID = cst.TermID AND t.TermName = @TermName AND ISNULL(t.IsActive, 1) = 1
                        WHERE cs.ClassID = @ClassID AND ISNULL(cs.IsActive, 1) = 1
                    ),
                    DisplaySubjects AS
                    (
                        SELECT SubjectID, SubjectName FROM AllValidSubjects
                        WHERE @SubjectID = 0 OR SubjectID = @SubjectID
                    ),
                    StudentList AS
                    (
                        SELECT DISTINCT s.StudentID, s.StudentNumber, s.FullName AS StudentName, s.Gender
                        FROM Students s
                        INNER JOIN StudentClasses sc ON sc.StudentID = s.StudentID
                            AND sc.ClassID = @ClassID
                            AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(@Section))
                            AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                        WHERE ISNULL(s.Status, N'نشط') = N'نشط'
                    ),
                    AllStudentSubjects AS
                    (
                        SELECT sl.StudentID, avs.SubjectID,
                               ISNULL(g.GradeID, 0) AS GradeID,
                               ISNULL(g.GradeValue, 0) AS Total,
                               CASE WHEN g.GradeID IS NULL THEN 0 ELSE 1 END AS HasGrade
                        FROM StudentList sl
                        CROSS JOIN AllValidSubjects avs
                        LEFT JOIN Grades g ON g.StudentID = sl.StudentID AND g.SubjectID = avs.SubjectID
                            AND g.ClassID = @ClassID
                            AND LTRIM(RTRIM(ISNULL(g.Section, N''))) = LTRIM(RTRIM(@Section))
                            AND REPLACE(ISNULL(g.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                            AND g.TermName = @TermName
                    ),
                    StudentSummary AS
                    (
                        SELECT StudentID,
                               SUM(Total) AS StudentTotal,
                               COUNT(*) * 100 AS StudentMaxTotal,
                               CAST(SUM(Total) * 100.0 / NULLIF(COUNT(*) * 100, 0) AS DECIMAL(18,2)) AS StudentAverage,
                               CASE WHEN SUM(CASE WHEN HasGrade = 0 THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل'
                                    WHEN MIN(CASE WHEN HasGrade = 1 THEN Total ELSE 100 END) < 50 THEN N'راسب' ELSE N'ناجح' END AS StudentResultStatus,
                               CASE WHEN SUM(CASE WHEN HasGrade = 0 THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل'
                                    WHEN SUM(Total) * 100.0 / NULLIF(COUNT(*) * 100, 0) >= 90 THEN N'ممتاز'
                                    WHEN SUM(Total) * 100.0 / NULLIF(COUNT(*) * 100, 0) >= 80 THEN N'جيد جدًا'
                                    WHEN SUM(Total) * 100.0 / NULLIF(COUNT(*) * 100, 0) >= 70 THEN N'جيد'
                                    WHEN SUM(Total) * 100.0 / NULLIF(COUNT(*) * 100, 0) >= 60 THEN N'مقبول'
                                    ELSE N'ضعيف' END AS StudentGradeLetter
                        FROM AllStudentSubjects
                        GROUP BY StudentID
                    )
                    SELECT sl.StudentID, sl.StudentNumber, sl.StudentName, sl.Gender,
                           @TermName AS TermName,
                           ds.SubjectID, ds.SubjectName, ISNULL(g.GradeID, 0) AS GradeID,
                           ISNULL(g.Quiz1, 0) AS Quiz1, ISNULL(g.Quiz2, 0) AS Quiz2,
                           ISNULL(g.CourseWork, 0) AS CourseWork, ISNULL(g.FinalExam, 0) AS FinalExam,
                           ISNULL(g.GradeValue, 0) AS Total,
                           CAST(ISNULL(g.GradeValue, 0) AS DECIMAL(18,2)) AS SubjectAverage,
                           ISNULL(g.GradeLetter, N'') AS GradeLetter,
                           ISNULL(g.ResultStatus, CASE WHEN g.GradeID IS NULL THEN N'غير مكتمل' WHEN ISNULL(g.GradeValue, 0) >= 50 THEN N'ناجح' ELSE N'راسب' END) AS ResultStatus,
                           ISNULL(g.Notes, N'') AS Notes,
                           ss.StudentTotal, ss.StudentMaxTotal, ss.StudentAverage, ss.StudentGradeLetter, ss.StudentResultStatus
                    FROM StudentList sl
                    CROSS JOIN DisplaySubjects ds
                    LEFT JOIN Grades g ON g.StudentID = sl.StudentID AND g.SubjectID = ds.SubjectID
                        AND g.ClassID = @ClassID
                        AND LTRIM(RTRIM(ISNULL(g.Section, N''))) = LTRIM(RTRIM(@Section))
                        AND REPLACE(ISNULL(g.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                        AND g.TermName = @TermName
                    INNER JOIN StudentSummary ss ON ss.StudentID = sl.StudentID
                    ORDER BY sl.StudentName, ds.SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    cmd.Parameters.AddWithValue("@Section", section);

                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    cmd.Parameters.AddWithValue("@SubjectID", subjectId <= 0 ? 0 : subjectId);

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

        // تعريف الدالة GradeExists لتنفيذ العملية المرتبطة بها.
        public bool GradeExists(int studentId, int subjectId, int classId, string section, string academicYear, string termName)
        {
            if (studentId <= 0 || subjectId <= 0 || classId <= 0)
                return false;

            const string existsQuery = @"
                SELECT COUNT(*)
                FROM Grades
                WHERE StudentID = @StudentID
                  AND SubjectID = @SubjectID
                  AND ClassID = @ClassID
                  AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND LTRIM(RTRIM(ISNULL(TermName, N''))) = LTRIM(RTRIM(ISNULL(@TermName, N'')));";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(existsQuery, conn))
            {
                cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectId;
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;
                cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 100).Value = (section ?? string.Empty).Trim();
                cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim().Replace('-', '/');
                cmd.Parameters.Add("@TermName", SqlDbType.NVarChar, 100).Value = (termName ?? string.Empty).Trim();

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // تعريف الدالة SaveGrade لتنفيذ العملية المرتبطة بها.
        public bool SaveGrade(StudentGrade grade)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

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

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM ClassSubjects cs
                        INNER JOIN ClassSubjectTerms cst ON cst.ClassSubjectID = cs.ClassSubjectID AND ISNULL(cst.IsActive, 1) = 1
                        INNER JOIN AcademicTerms t ON t.TermID = cst.TermID AND t.TermName = @TermName AND ISNULL(t.IsActive, 1) = 1
                        WHERE cs.ClassID = @ClassID AND cs.SubjectID = @SubjectID AND ISNULL(cs.IsActive, 1) = 1
                    )
                        THROW 50007, N'لا يمكن حفظ الدرجة؛ المادة غير مربوطة بهذا الصف والفصل الدراسي.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM StudentClasses sc
                        WHERE sc.StudentID = @StudentID
                          AND sc.ClassID = @ClassID
                          AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                          AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                    )
                        THROW 50006, N'لا يمكن حفظ الدرجة؛ الطالب غير موزع في الصف والشعبة والعام المحدد.', 1;

                    DECLARE @GradeID INT;

                    SELECT TOP 1 @GradeID = GradeID
                    FROM Grades WITH (UPDLOCK, HOLDLOCK)
                    WHERE StudentID = @StudentID
                      AND SubjectID = @SubjectID
                      AND ClassID = @ClassID
                      AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(TermName, N''))) = LTRIM(RTRIM(ISNULL(@TermName, N'')))
                    ORDER BY GradeID;

                    IF @GradeID IS NOT NULL
                    BEGIN
                        UPDATE Grades
                        SET
                            Quiz1 = @Quiz1,
                            Quiz2 = @Quiz2,
                            CourseWork = @CourseWork,
                            FinalExam = @FinalExam,
                            GradeValue = @Total,
                            GradeLetter = @GradeLetter,
                            ResultStatus = @ResultStatus,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE GradeID = @GradeID;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Grades
                        (
                            StudentID, SubjectID, ClassID, Section, AcademicYear, TermName,
                            Quiz1, Quiz2, CourseWork, FinalExam, GradeValue, GradeLetter,
                            ResultStatus, Notes, CreatedAt, UpdatedAt
                        )
                        VALUES
                        (
                            @StudentID, @SubjectID, @ClassID, @Section, @AcademicYear, @TermName,
                            @Quiz1, @Quiz2, @CourseWork, @FinalExam, @Total, @GradeLetter,
                            @ResultStatus, @Notes, GETDATE(), GETDATE()
                        );

                        SET @GradeID = CONVERT(INT, SCOPE_IDENTITY());
                    END;

                    SELECT @GradeID;
                    COMMIT TRANSACTION;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, grade);
                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    grade.GradeID = Convert.ToInt32(result);
                    return grade.GradeID > 0;
                }
            }
        }

        // تعريف الدالة DeleteGrade لتنفيذ العملية المرتبطة بها.
        public bool DeleteGrade(int gradeId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM Grades WHERE GradeID = @GradeID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@GradeID", gradeId);


                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
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
