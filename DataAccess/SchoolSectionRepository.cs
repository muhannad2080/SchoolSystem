using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class SchoolSectionRepository
    {
        public DataTable GetAllSections(string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT
                        ss.SectionID,
                        ss.ClassID,
                        c.ClassName,
                        REPLACE(LTRIM(RTRIM(ISNULL(ss.AcademicYear, N''))), N'-', N'/') AS AcademicYear,
                        LTRIM(RTRIM(ss.SectionName)) AS SectionName,
                        ss.Capacity,
                        ISNULL(ss.AllowedGender, N'مختلط') AS AllowedGender,
                        ISNULL(ss.IsActive, 1) AS IsActive,
                        ss.CreatedAt,
                        COUNT(DISTINCT sc.StudentClassID) AS AssignedCount
                    FROM dbo.SchoolSections ss
                    INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
                    LEFT JOIN dbo.StudentClasses sc
                        ON sc.ClassID = ss.ClassID
                        AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(ISNULL(ss.SectionName, N'')))
                        AND REPLACE(LTRIM(RTRIM(ISNULL(sc.AcademicYear, N''))), N'-', N'/')
                            = REPLACE(LTRIM(RTRIM(ISNULL(ss.AcademicYear, N''))), N'-', N'/')
                    WHERE REPLACE(LTRIM(RTRIM(ISNULL(ss.AcademicYear, N''))), N'-', N'/')
                        = REPLACE(@AcademicYear, N'-', N'/')
                    GROUP BY
                        ss.SectionID, ss.ClassID, c.ClassName, ss.AcademicYear, ss.SectionName,
                        ss.Capacity, ss.AllowedGender, ss.IsActive, ss.CreatedAt
                    ORDER BY c.ClassName, ss.SectionName;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value =
                        (academicYear ?? string.Empty).Trim().Replace('-', '/');

                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public SchoolSection GetSectionById(int sectionId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT TOP (1)
                        ss.SectionID,
                        ss.ClassID,
                        c.ClassName,
                        LTRIM(RTRIM(ss.SectionName)) AS SectionName,
                        REPLACE(LTRIM(RTRIM(ISNULL(ss.AcademicYear, N''))), N'-', N'/') AS AcademicYear,
                        ISNULL(ss.IsActive, 1) AS IsActive,
                        ss.Capacity,
                        ISNULL(ss.AllowedGender, N'مختلط') AS AllowedGender,
                        ss.CreatedAt,
                        (
                            SELECT COUNT(1)
                            FROM dbo.StudentClasses sc
                            WHERE sc.ClassID = ss.ClassID
                              AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(ISNULL(ss.SectionName, N'')))
                              AND REPLACE(LTRIM(RTRIM(ISNULL(sc.AcademicYear, N''))), N'-', N'/')
                                  = REPLACE(LTRIM(RTRIM(ISNULL(ss.AcademicYear, N''))), N'-', N'/')
                        ) AS AssignedCount
                    FROM dbo.SchoolSections ss
                    INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
                    WHERE ss.SectionID = @SectionID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@SectionID", SqlDbType.Int).Value = sectionId;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new SchoolSection
                        {
                            SectionID = Convert.ToInt32(reader["SectionID"]),
                            ClassID = Convert.ToInt32(reader["ClassID"]),
                            ClassName = Convert.ToString(reader["ClassName"]),
                            SectionName = Convert.ToString(reader["SectionName"]),
                            AcademicYear = Convert.ToString(reader["AcademicYear"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            Capacity = reader["Capacity"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Capacity"]),
                            AllowedGender = Convert.ToString(reader["AllowedGender"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            AssignedCount = reader["AssignedCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AssignedCount"])
                        };
                    }
                }
            }
        }

        public int GetReferenceCount(int sectionId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    DECLARE @ClassID INT, @SectionName NVARCHAR(100), @AcademicYear NVARCHAR(20);
                    SELECT
                        @ClassID = ClassID,
                        @SectionName = SectionName,
                        @AcademicYear = AcademicYear
                    FROM dbo.SchoolSections
                    WHERE SectionID = @SectionID;

                    IF @ClassID IS NULL
                    BEGIN
                        SELECT 0;
                        RETURN;
                    END;

                    SELECT
                        (
                            SELECT COUNT(1) FROM dbo.StudentClasses
                            WHERE ClassID = @ClassID
                              AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@SectionName, N'')))
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                        )
                        +
                        (
                            SELECT COUNT(1) FROM dbo.SchoolTimetable
                            WHERE ClassID = @ClassID
                              AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@SectionName, N'')))
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                        )
                        +
                        (
                            SELECT COUNT(1) FROM dbo.StudentAttendance
                            WHERE ClassID = @ClassID
                              AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@SectionName, N'')))
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N'-'), N'-', N'/')
                        )
                        +
                        (
                            SELECT COUNT(1) FROM dbo.Grades
                            WHERE ClassID = @ClassID
                              AND LTRIM(RTRIM(ISNULL(Section, N''))) = LTRIM(RTRIM(ISNULL(@SectionName, N'')))
                              AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                        ) AS ReferenceCount;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@SectionID", SqlDbType.Int).Value = sectionId;
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int AddSection(SchoolSection item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateClassAndDuplicate(conn, transaction, item, 0);

                    const string query = @"
                        INSERT INTO dbo.SchoolSections
                        (
                            ClassID, SectionName, AcademicYear, IsActive, Capacity, AllowedGender, CreatedAt
                        )
                        OUTPUT INSERTED.SectionID
                        VALUES
                        (
                            @ClassID, @SectionName, @AcademicYear, @IsActive, @Capacity, @AllowedGender, GETDATE()
                        );";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, item, false);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        transaction.Commit();
                        return id;
                    }
                }
            }
        }

        public bool UpdateSection(SchoolSection item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string currentQuery = @"
                        SELECT TOP (1) ClassID, SectionName, AcademicYear, IsActive
                        FROM dbo.SchoolSections WITH (UPDLOCK, HOLDLOCK)
                        WHERE SectionID = @SectionID;";

                    SchoolSection current = null;
                    using (SqlCommand currentCommand = new SqlCommand(currentQuery, conn, transaction))
                    {
                        currentCommand.Parameters.Add("@SectionID", SqlDbType.Int).Value = item.SectionID;
                        using (SqlDataReader reader = currentCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                current = new SchoolSection
                                {
                                    ClassID = Convert.ToInt32(reader["ClassID"]),
                                    SectionName = Convert.ToString(reader["SectionName"]),
                                    AcademicYear = Convert.ToString(reader["AcademicYear"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };
                            }
                        }
                    }

                    if (current == null)
                        throw new InvalidOperationException("الشعبة غير موجودة.");

                    ValidateClassAndDuplicate(conn, transaction, item, item.SectionID);

                    int references = GetReferenceCountInTransaction(conn, transaction, item.SectionID);

                    bool identityChanged =
                        current.ClassID != item.ClassID
                        || !Normalize(current.SectionName).Equals(Normalize(item.SectionName), StringComparison.OrdinalIgnoreCase)
                        || !NormalizeYear(current.AcademicYear).Equals(NormalizeYear(item.AcademicYear), StringComparison.OrdinalIgnoreCase);

                    if (references > 0 && identityChanged)
                        throw new InvalidOperationException(
                            "لا يمكن تغيير الصف أو اسم الشعبة أو العام الدراسي لأن للشعبة سجلات طلاب أو حضور أو درجات أو جدول مرتبطة بها.");

                    if (references > 0 && current.IsActive && !item.IsActive)
                        throw new InvalidOperationException(
                            "لا يمكن تعطيل شعبة تحتوي على طلاب أو سجلات تشغيلية. انقل الطلاب أولاً إلى شعبة أخرى.");

                    const string query = @"
                        UPDATE dbo.SchoolSections
                        SET
                            ClassID = @ClassID,
                            SectionName = @SectionName,
                            AcademicYear = @AcademicYear,
                            IsActive = @IsActive,
                            Capacity = @Capacity,
                            AllowedGender = @AllowedGender
                        WHERE SectionID = @SectionID;";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, item, true);
                        if (cmd.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("لم يتم العثور على الشعبة أو لم يتم تعديلها.");

                        transaction.Commit();
                        return true;
                    }
                }
            }
        }

        public bool DeleteSection(int sectionId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    GetReferenceCountInTransaction(conn, transaction, sectionId, true);

                    const string query = "DELETE FROM dbo.SchoolSections WHERE SectionID = @SectionID;";
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@SectionID", SqlDbType.Int).Value = sectionId;
                        if (cmd.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("الشعبة غير موجودة.");

                        transaction.Commit();
                        return true;
                    }
                }
            }
        }

        private void ValidateClassAndDuplicate(SqlConnection conn, SqlTransaction transaction, SchoolSection item, int excludedId)
        {
            const string classQuery = @"
                SELECT COUNT(1)
                FROM dbo.Classes
                WHERE ClassID = @ClassID
                  AND ISNULL(IsActive, 1) = 1;";

            using (SqlCommand classCommand = new SqlCommand(classQuery, conn, transaction))
            {
                classCommand.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
                if (Convert.ToInt32(classCommand.ExecuteScalar()) == 0)
                    throw new InvalidOperationException("الصف المحدد غير موجود أو غير نشط.");
            }

            const string duplicateQuery = @"
                SELECT COUNT(1)
                FROM dbo.SchoolSections
                WHERE SectionID <> @SectionID
                  AND ClassID = @ClassID
                  AND LTRIM(RTRIM(ISNULL(SectionName, N''))) = LTRIM(RTRIM(ISNULL(@SectionName, N'')))
                  AND REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
                      = REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/');";

            using (SqlCommand duplicateCommand = new SqlCommand(duplicateQuery, conn, transaction))
            {
                duplicateCommand.Parameters.Add("@SectionID", SqlDbType.Int).Value = excludedId;
                duplicateCommand.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
                duplicateCommand.Parameters.Add("@SectionName", SqlDbType.NVarChar, 50).Value = item.SectionName.Trim();
                duplicateCommand.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = NormalizeYear(item.AcademicYear);

                if (Convert.ToInt32(duplicateCommand.ExecuteScalar()) > 0)
                    throw new InvalidOperationException("هذه الشعبة موجودة مسبقًا لهذا الصف والعام الدراسي.");
            }
        }

        private int GetReferenceCountInTransaction(SqlConnection conn, SqlTransaction transaction, int sectionId, bool throwIfReferenced = false)
        {
            const string query = @"
                DECLARE @ClassID INT, @SectionName NVARCHAR(100), @AcademicYear NVARCHAR(20);
                SELECT @ClassID = ClassID, @SectionName = SectionName, @AcademicYear = AcademicYear
                FROM dbo.SchoolSections WITH (UPDLOCK, HOLDLOCK)
                WHERE SectionID = @SectionID;

                IF @ClassID IS NULL
                    THROW 51400, N'الشعبة غير موجودة.', 1;

                DECLARE @Count INT =
                    (SELECT COUNT(1) FROM dbo.StudentClasses
                     WHERE ClassID=@ClassID AND LTRIM(RTRIM(ISNULL(Section,N'')))=LTRIM(RTRIM(ISNULL(@SectionName,N'')))
                       AND REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(@AcademicYear,N''),N'-',N'/'))
                    +
                    (SELECT COUNT(1) FROM dbo.SchoolTimetable
                     WHERE ClassID=@ClassID AND LTRIM(RTRIM(ISNULL(Section,N'')))=LTRIM(RTRIM(ISNULL(@SectionName,N'')))
                       AND REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(@AcademicYear,N''),N'-',N'/'))
                    +
                    (SELECT COUNT(1) FROM dbo.StudentAttendance
                     WHERE ClassID=@ClassID AND LTRIM(RTRIM(ISNULL(Section,N'')))=LTRIM(RTRIM(ISNULL(@SectionName,N'')))
                       AND REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(@AcademicYear,N''),N'-',N'/'))
                    +
                    (SELECT COUNT(1) FROM dbo.Grades
                     WHERE ClassID=@ClassID AND LTRIM(RTRIM(ISNULL(Section,N'')))=LTRIM(RTRIM(ISNULL(@SectionName,N'')))
                       AND REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(@AcademicYear,N''),N'-',N'/'));

                SELECT @Count;";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.Add("@SectionID", SqlDbType.Int).Value = sectionId;
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (throwIfReferenced && count > 0)
                    throw new InvalidOperationException(
                        "لا يمكن حذف الشعبة لأنها مرتبطة بطلاب أو حضور أو درجات أو جدول دراسي. استخدم التعطيل أو انقل الطلاب أولاً.");

                return count;
            }
        }

        private static string Normalize(SchoolSection value) { return value == null ? string.Empty : Normalize(value.SectionName); }
        private static string Normalize(string value) { return (value ?? string.Empty).Trim(); }
        private static string NormalizeYear(string value) { return (value ?? string.Empty).Trim().Replace('-', '/'); }

        private static void AddParameters(SqlCommand cmd, SchoolSection item, bool includeId)
        {
            if (includeId)
                cmd.Parameters.Add("@SectionID", SqlDbType.Int).Value = item.SectionID;

            cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
            cmd.Parameters.Add("@SectionName", SqlDbType.NVarChar, 50).Value = Normalize(item.SectionName);
            cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = NormalizeYear(item.AcademicYear);
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = item.IsActive;
            cmd.Parameters.Add("@Capacity", SqlDbType.Int).Value = item.Capacity.HasValue ? (object)item.Capacity.Value : DBNull.Value;
            cmd.Parameters.Add("@AllowedGender", SqlDbType.NVarChar, 20).Value =
                string.IsNullOrWhiteSpace(item.AllowedGender) ? (object)"مختلط" : item.AllowedGender.Trim();
        }
    }
}