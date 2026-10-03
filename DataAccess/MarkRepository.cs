using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class MarkRepository
    {
        // تعريف الدالة GetAllMarks لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllMarks()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT m.MarkID, m.StudentID, s.FullName AS StudentName,
                  m.SubjectID, sub.SubjectName, m.TeacherID, t.FullName AS TeacherName,
                  m.Mark, m.ExamType, m.CreatedAt
                  FROM Marks m
                  INNER JOIN Students s ON m.StudentID = s.StudentID
                  INNER JOIN Subjects sub ON m.SubjectID = sub.SubjectID
                  LEFT JOIN Teachers t ON m.TeacherID = t.TeacherID
                  ORDER BY m.CreatedAt DESC", conn))
            {

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();

                    dt.Load(reader);

                    return dt;
                }
            }
        }

        // تعريف الدالة MarkExists لتنفيذ العملية المرتبطة بها.
        public bool MarkExists(int studentId, int subjectId, string examType, int excludeId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT COUNT(1) FROM Marks
                  WHERE StudentID = @SID AND SubjectID = @SubID AND ExamType = @Exam
                  AND MarkID <> @MID", conn))
            {

                cmd.Parameters.Add("@SID", SqlDbType.Int).Value = studentId;

                cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = subjectId;

                cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = examType ?? (object)DBNull.Value;

                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = excludeId;

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // تعريف الدالة AddMark لتنفيذ العملية المرتبطة بها.
        public void AddMark(Mark mark)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Students
                        WHERE StudentID = @SID AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 53001, N'لا يمكن إضافة درجة لطالب غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Subjects
                        WHERE SubjectID = @SubID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 53002, N'لا يمكن إضافة درجة لمادة غير نشطة.', 1;

                    IF @TID IS NOT NULL AND NOT EXISTS
                    (
                        SELECT 1 FROM Teachers
                        WHERE TeacherID = @TID AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 53003, N'المعلم المحدد غير موجود أو غير نشط.', 1;

                    IF @Mark < 0 OR @Mark > 100
                        THROW 53004, N'الدرجة يجب أن تكون بين 0 و100.', 1;

                    IF NULLIF(LTRIM(RTRIM(ISNULL(@Exam, N''))), N'') IS NULL
                        THROW 53005, N'نوع الاختبار مطلوب.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM Marks WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @SID
                          AND SubjectID = @SubID
                          AND LTRIM(RTRIM(ISNULL(ExamType, N''))) = LTRIM(RTRIM(@Exam))
                    )
                        THROW 53006, N'توجد درجة مسجلة مسبقاً لنفس الطالب والمادة ونوع الاختبار.', 1;

                    INSERT INTO Marks (StudentID, SubjectID, TeacherID, Mark, ExamType)
                    VALUES (@SID, @SubID, @TID, @Mark, LTRIM(RTRIM(@Exam)));

                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    COMMIT TRANSACTION;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@SID", SqlDbType.Int).Value = mark.StudentID;
                    cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = mark.SubjectID;
                    cmd.Parameters.Add("@TID", SqlDbType.Int).Value = mark.TeacherID.HasValue ? (object)mark.TeacherID.Value : DBNull.Value;
                    cmd.Parameters.Add("@Mark", SqlDbType.Decimal).Value = mark.MarkValue;
                    cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = mark.ExamType.Trim();

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        throw new InvalidOperationException("تعذر إنشاء سجل الدرجة.");

                    mark.MarkID = Convert.ToInt32(result);
                }
            }
        }

        // تعريف الدالة UpdateMark لتنفيذ العملية المرتبطة بها.
        public bool UpdateMark(Mark mark)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Marks WHERE MarkID = @MID
                    )
                        THROW 53007, N'سجل الدرجة المحدد غير موجود.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Students
                        WHERE StudentID = @SID AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 53001, N'لا يمكن تعديل درجة لطالب غير نشط.', 1;

                    IF NOT EXISTS
                    (
                        SELECT 1 FROM Subjects
                        WHERE SubjectID = @SubID AND ISNULL(IsActive, 1) = 1
                    )
                        THROW 53002, N'لا يمكن تعديل درجة لمادة غير نشطة.', 1;

                    IF @TID IS NOT NULL AND NOT EXISTS
                    (
                        SELECT 1 FROM Teachers
                        WHERE TeacherID = @TID AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 53003, N'المعلم المحدد غير موجود أو غير نشط.', 1;

                    IF @Mark < 0 OR @Mark > 100
                        THROW 53004, N'الدرجة يجب أن تكون بين 0 و100.', 1;

                    IF NULLIF(LTRIM(RTRIM(ISNULL(@Exam, N''))), N'') IS NULL
                        THROW 53005, N'نوع الاختبار مطلوب.', 1;

                    IF EXISTS
                    (
                        SELECT 1 FROM Marks
                        WHERE StudentID = @SID
                          AND SubjectID = @SubID
                          AND LTRIM(RTRIM(ISNULL(ExamType, N''))) = LTRIM(RTRIM(@Exam))
                          AND MarkID <> @MID
                    )
                        THROW 53006, N'توجد درجة أخرى لنفس الطالب والمادة ونوع الاختبار.', 1;

                    UPDATE Marks
                    SET StudentID = @SID,
                        SubjectID = @SubID,
                        TeacherID = @TID,
                        Mark = @Mark,
                        ExamType = LTRIM(RTRIM(@Exam))
                    WHERE MarkID = @MID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MID", SqlDbType.Int).Value = mark.MarkID;
                    cmd.Parameters.Add("@SID", SqlDbType.Int).Value = mark.StudentID;
                    cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = mark.SubjectID;
                    cmd.Parameters.Add("@TID", SqlDbType.Int).Value = mark.TeacherID.HasValue ? (object)mark.TeacherID.Value : DBNull.Value;
                    cmd.Parameters.Add("@Mark", SqlDbType.Decimal).Value = mark.MarkValue;
                    cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = mark.ExamType.Trim();

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // تعريف الدالة DeleteMark لتنفيذ العملية المرتبطة بها.
        public bool DeleteMark(int markId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Marks WHERE MarkID=@MID", conn))
            {

                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = markId;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
