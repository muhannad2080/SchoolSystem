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
                        COALESCE(
                            STUFF(
                                (
                                    SELECT N'، ' + c2.ClassName
                                    FROM dbo.ClassSubjects cs2
                                    INNER JOIN dbo.Classes c2 ON c2.ClassID = cs2.ClassID
                                    WHERE cs2.SubjectID = s.SubjectID
                                      AND ISNULL(cs2.IsActive, 1) = 1
                                      AND ISNULL(c2.IsActive, 1) = 1
                                    ORDER BY c2.GradeOrder, c2.ClassID
                                    FOR XML PATH(N'')
                                ),
                                1,
                                2,
                                N''
                            ),
                            CASE WHEN c.ClassID IS NULL THEN N'كل الصفوف' ELSE c.ClassName END
                        ) AS ClassName,
                        COALESCE(
                            s.ClassID,
                            (
                                SELECT TOP 1 cs3.ClassID
                                FROM dbo.ClassSubjects cs3
                                INNER JOIN dbo.Classes c3 ON c3.ClassID = cs3.ClassID
                                WHERE cs3.SubjectID = s.SubjectID
                                  AND ISNULL(cs3.IsActive, 1) = 1
                                  AND ISNULL(c3.IsActive, 1) = 1
                                ORDER BY c3.GradeOrder, c3.ClassID
                            )
                        ) AS DefaultClassID,
                        N'|' +
                        ISNULL(
                            STUFF(
                                (
                                    SELECT N'|' + CONVERT(nvarchar(20), cs4.ClassID)
                                    FROM dbo.ClassSubjects cs4
                                    INNER JOIN dbo.Classes c4 ON c4.ClassID = cs4.ClassID
                                    WHERE cs4.SubjectID = s.SubjectID
                                      AND ISNULL(cs4.IsActive, 1) = 1
                                      AND ISNULL(c4.IsActive, 1) = 1
                                    ORDER BY c4.GradeOrder, c4.ClassID
                                    FOR XML PATH(N'')
                                ),
                                1,
                                1,
                                N''
                            ),
                            N''
                        ) + N'|' AS MappedClassIDs,
                        s.MaxDegree,
                        s.PassDegree,
                        s.IsActive,
                        s.Notes,
                        s.CreatedAt,
                        s.UpdatedAt
                    FROM dbo.Subjects s
                    LEFT JOIN dbo.Classes c ON s.ClassID = c.ClassID
                    ORDER BY s.SubjectName";

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
                    SELECT DISTINCT
                        s.SubjectID,
                        s.SubjectName
                    FROM dbo.Subjects s
                    INNER JOIN dbo.ClassSubjects cs
                        ON cs.SubjectID = s.SubjectID
                       AND cs.ClassID = @ClassID
                       AND ISNULL(cs.IsActive, 1) = 1
                    WHERE ISNULL(s.IsActive, 1) = 1
                    ORDER BY s.SubjectName";

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
                    // كود المادة يولّده النظام دائمًا داخل المعاملة ولا يعتمد على إدخال الواجهة.
                    subject.SubjectCode = GenerateNextSubjectCode(conn, transaction);

                    ValidateSubjectReferences(conn, transaction, subject);
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

                    /* الربط الأكاديمي الفعلي للصف يتم عبر ClassSubjects/Terms. */
                    EnsureClassSubjectMapping(
                        conn,
                        transaction,
                        subjectId,
                        subject.ClassID);

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
                    ValidateSubjectReferences(conn, transaction, subject);
                    EnsureNoDuplicateSubject(conn, transaction, subject, subject.SubjectID);

                    const string query = @"
                        UPDATE Subjects
                        SET
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

                    /* عند تخصيص الصف من شاشة المواد، أنشئ/أكمل الربط الأكاديمي. */
                    EnsureClassSubjectMapping(
                        conn,
                        transaction,
                        subject.SubjectID,
                        subject.ClassID);

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

        private void ValidateSubjectReferences(SqlConnection conn, SqlTransaction transaction, Subject subject)
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

        /// <summary>يضيف معاملات المادة إلى أمر SQL، مع اختيار تضمين رقم المادة عند التعديل.</summary>
        private void AddParameters(SqlCommand cmd, Subject subject, bool includeId)
        {
            if (includeId)
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subject.SubjectID;

            cmd.Parameters.Add("@SubjectCode", SqlDbType.NVarChar, 30).Value = subject.SubjectCode.Trim();
            cmd.Parameters.Add("@SubjectName", SqlDbType.NVarChar, 100).Value = subject.SubjectName.Trim();
            cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = subject.ClassID.HasValue
                ? (object)subject.ClassID.Value
                : DBNull.Value;

            SqlParameter maxDegree = cmd.Parameters.Add("@MaxDegree", SqlDbType.Decimal);
            maxDegree.Precision = 18;
            maxDegree.Scale = 2;
            maxDegree.Value = subject.MaxDegree;

            SqlParameter passDegree = cmd.Parameters.Add("@PassDegree", SqlDbType.Decimal);
            passDegree.Precision = 18;
            passDegree.Scale = 2;
            passDegree.Value = subject.PassDegree;

            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = subject.IsActive;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value =
                string.IsNullOrWhiteSpace(subject.Notes) ? (object)DBNull.Value : subject.Notes.Trim();
        }

        /// <summary>
        /// يضمن وجود الربط الفعلي بين المادة والصف ثم يربطها بالفصول
        /// الدراسية النشطة عبر ClassSubjectTerms.
        /// </summary>
        private void EnsureClassSubjectMapping(
            SqlConnection conn,
            SqlTransaction transaction,
            int subjectId,
            int? classId)
        {
            if (!classId.HasValue || classId.Value <= 0)
                return;

            const string ensureLinkSql = @"
                IF NOT EXISTS
                (
                    SELECT 1
                    FROM dbo.ClassSubjects WITH (UPDLOCK, HOLDLOCK)
                    WHERE ClassID = @ClassID
                      AND SubjectID = @SubjectID
                )
                BEGIN
                    INSERT dbo.ClassSubjects
                    (
                        ClassID,
                        SubjectID,
                        IsActive,
                        CreatedAt
                    )
                    VALUES
                    (
                        @ClassID,
                        @SubjectID,
                        1,
                        SYSUTCDATETIME()
                    );
                END
                ELSE
                BEGIN
                    UPDATE dbo.ClassSubjects
                    SET IsActive = 1
                    WHERE ClassID = @ClassID
                      AND SubjectID = @SubjectID;
                END;";

            using (SqlCommand cmd = new SqlCommand(ensureLinkSql, conn, transaction))
            {
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId.Value;
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectId;
                cmd.ExecuteNonQuery();
            }

            const string ensureTermsSql = @"
                INSERT dbo.ClassSubjectTerms
                (
                    ClassSubjectID,
                    TermID,
                    IsActive,
                    CreatedAt
                )
                SELECT
                    cs.ClassSubjectID,
                    t.TermID,
                    1,
                    SYSUTCDATETIME()
                FROM dbo.ClassSubjects cs
                CROSS JOIN dbo.AcademicTerms t
                WHERE cs.ClassID = @ClassID
                  AND cs.SubjectID = @SubjectID
                  AND ISNULL(cs.IsActive, 1) = 1
                  AND ISNULL(t.IsActive, 1) = 1
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.ClassSubjectTerms cst
                      WHERE cst.ClassSubjectID = cs.ClassSubjectID
                        AND cst.TermID = t.TermID
                  );";

            using (SqlCommand cmd = new SqlCommand(ensureTermsSql, conn, transaction))
            {
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId.Value;
                cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectId;
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
