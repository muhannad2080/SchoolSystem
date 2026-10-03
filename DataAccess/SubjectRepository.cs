using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class SubjectRepository
    {
        // تعريف الدالة GetAllSubjects لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllSubjects()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        s.SubjectID,
                        s.SubjectCode,
                        s.SubjectName,
                        s.ClassID,
                        c.ClassName,
                        s.MaxDegree,
                        s.PassDegree,
                        s.IsActive,
                        s.Notes,
                        s.CreatedAt,
                        s.UpdatedAt
                    FROM Subjects s
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                    ORDER BY c.ClassID, s.SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة GetActiveSubjects لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveSubjects()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        SubjectID,
                        SubjectName
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

        // تعريف الدالة GetActiveSubjectsByClass لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveSubjectsByClass(int classId)
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

        // تعريف الدالة AddSubject لتنفيذ العملية المرتبطة بها.
        public int AddSubject(Subject subject)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    if (string.IsNullOrWhiteSpace(subject.SubjectCode))
                        subject.SubjectCode = GenerateNextSubjectCode(conn, transaction);

                    ValidateSubjectReferences(conn, transaction, subject, 0);
                    EnsureNoDuplicateSubject(conn, transaction, subject, 0);

                    const string query = @"
                        INSERT INTO Subjects
                        (
                            SubjectCode, SubjectName, ClassID, MaxDegree, PassDegree,
                            IsActive, Notes, CreatedAt
                        )
                        OUTPUT INSERTED.SubjectID
                        VALUES
                        (
                            @SubjectCode, @SubjectName, @ClassID, @MaxDegree, @PassDegree,
                            @IsActive, @Notes, GETDATE()
                        );";

                    int subjectId;
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, subject, false);
                        subjectId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    transaction.Commit();
                    subject.SubjectID = subjectId;
                    return subjectId;
                }
            }
        }

        // تعريف الدالة UpdateSubject لتنفيذ العملية المرتبطة بها.
        public bool UpdateSubject(Subject subject)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateSubjectReferences(conn, transaction, subject, subject.SubjectID);
                    EnsureNoDuplicateSubject(conn, transaction, subject, subject.SubjectID);

                    const string query = @"
                        UPDATE Subjects
                        SET
                            SubjectCode = @SubjectCode,
                            SubjectName = @SubjectName,
                            ClassID = @ClassID,
                            MaxDegree = @MaxDegree,
                            PassDegree = @PassDegree,
                            IsActive = @IsActive,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE SubjectID = @SubjectID";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, subject, true);
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

        // تعريف الدالة DeleteSubject لتنفيذ العملية المرتبطة بها.
        public bool DeleteSubject(int subjectId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string referenceQuery = @"
                        IF EXISTS (SELECT 1 FROM dbo.ClassSubjects WITH (UPDLOCK, HOLDLOCK) WHERE SubjectID = @SubjectID)
                            THROW 51201, N'لا يمكن حذف المادة لأنها مرتبطة بصفوف أو مقررات دراسية. عطّل المادة بدلاً من حذفها.', 1;

                        IF EXISTS (SELECT 1 FROM dbo.Marks WITH (UPDLOCK, HOLDLOCK) WHERE SubjectID = @SubjectID)
                            THROW 51202, N'لا يمكن حذف المادة لأنها مرتبطة بدرجات اختبارات. عطّل المادة بدلاً من حذفها.', 1;

                        IF EXISTS (SELECT 1 FROM dbo.Grades WITH (UPDLOCK, HOLDLOCK) WHERE SubjectID = @SubjectID)
                            THROW 51203, N'لا يمكن حذف المادة لأنها مرتبطة بنتائج دراسية. عطّل المادة بدلاً من حذفها.', 1;

                        IF EXISTS (SELECT 1 FROM dbo.SchoolTimetable WITH (UPDLOCK, HOLDLOCK) WHERE SubjectID = @SubjectID)
                            THROW 51204, N'لا يمكن حذف المادة لأنها مستخدمة في الجدول الدراسي. عطّل المادة بدلاً من حذفها.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.ClassSubjectTerms cst WITH (UPDLOCK, HOLDLOCK)
                            INNER JOIN dbo.ClassSubjects cs WITH (UPDLOCK, HOLDLOCK)
                                ON cs.ClassSubjectID = cst.ClassSubjectID
                            WHERE cs.SubjectID = @SubjectID
                        )
                            THROW 51205, N'لا يمكن حذف المادة لأنها مرتبطة بفصول دراسية. عطّل المادة بدلاً من حذفها.', 1;";

                    using (SqlCommand referenceCommand = new SqlCommand(referenceQuery, conn, transaction))
                    {
                        referenceCommand.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectId;
                        referenceCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand deleteCommand = new SqlCommand(
                        "DELETE FROM dbo.Subjects WHERE SubjectID = @SubjectID", conn, transaction))
                    {
                        deleteCommand.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectId;
                        bool deleted = deleteCommand.ExecuteNonQuery() > 0;
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

        /// <summary>يولّد كود مادة متسلسلاً وفريداً داخل نفس معاملة الإضافة.</summary>
        private string GenerateNextSubjectCode(SqlConnection conn, SqlTransaction transaction)
        {
            const string query = @"
                SELECT ISNULL(MAX(TRY_CONVERT(INT, SUBSTRING(SubjectCode, 5, 10))), 0) + 1
                FROM dbo.Subjects WITH (TABLOCKX, HOLDLOCK)
                WHERE SubjectCode LIKE N'MAT-%'
                  AND TRY_CONVERT(INT, SUBSTRING(SubjectCode, 5, 10)) IS NOT NULL;";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                object result = cmd.ExecuteScalar();
                int next = result == null || result == DBNull.Value ? 1 : Convert.ToInt32(result);
                if (next > 999999)
                    throw new InvalidOperationException("تم الوصول إلى الحد الأقصى لأكواد المواد التلقائية.");

                return "MAT-" + next.ToString("D6");
            }
        }

        private void ValidateSubjectReferences(SqlConnection conn, SqlTransaction transaction, Subject subject, int excludedSubjectId)
        {
            const string query = @"
                IF @ClassID IS NOT NULL
                   AND NOT EXISTS
                   (
                       SELECT 1 FROM dbo.Classes WITH (UPDLOCK, HOLDLOCK)
                       WHERE ClassID = @ClassID AND ISNULL(IsActive, 1) = 1
                   )
                    THROW 51206, N'الصف المحدد للمادة غير موجود أو غير نشط.', 1;

                IF NOT EXISTS (SELECT 1 FROM dbo.Subjects WITH (UPDLOCK, HOLDLOCK) WHERE SubjectID = @SubjectID)
                    AND @SubjectID > 0
                    THROW 51207, N'المادة المحددة غير موجودة.', 1;";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subject.SubjectID;
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = subject.ClassID.HasValue ? (object)subject.ClassID.Value : DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }

        private void EnsureNoDuplicateSubject(SqlConnection conn, SqlTransaction transaction, Subject subject, int excludedSubjectId)
        {
            const string query = @"
                IF EXISTS
                (
                    SELECT 1
                    FROM dbo.Subjects WITH (UPDLOCK, HOLDLOCK)
                    WHERE SubjectID <> @SubjectID
                      AND LTRIM(RTRIM(ISNULL(SubjectCode, N''))) = LTRIM(RTRIM(@SubjectCode))
                      AND NULLIF(LTRIM(RTRIM(ISNULL(@SubjectCode, N''))), N'') IS NOT NULL
                )
                    THROW 51208, N'كود المادة مستخدم مسبقاً.', 1;

                IF EXISTS
                (
                    SELECT 1
                    FROM dbo.Subjects WITH (UPDLOCK, HOLDLOCK)
                    WHERE SubjectID <> @SubjectID
                      AND LTRIM(RTRIM(SubjectName)) = LTRIM(RTRIM(@SubjectName))
                      AND ISNULL(ClassID, 0) = ISNULL(@ClassID, 0)
                )
                    THROW 51209, N'اسم المادة موجود مسبقاً للصف المحدد.', 1;";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = excludedSubjectId;
                cmd.Parameters.Add("@SubjectCode", SqlDbType.NVarChar, 30).Value = subject.SubjectCode.Trim();
                cmd.Parameters.Add("@SubjectName", SqlDbType.NVarChar, 100).Value = subject.SubjectName.Trim();
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = subject.ClassID.HasValue ? (object)subject.ClassID.Value : DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }

        // تعريف الدالة GetSubjectCount لتنفيذ العملية المرتبطة بها.
        public int GetSubjectCount()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Subjects WHERE ISNULL(IsActive, 1) = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
