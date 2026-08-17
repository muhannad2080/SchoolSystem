using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess.Repositories
{
    public class StudentRepository
    {
        public List<Student> GetAll()
        {
            var students = new List<Student>();

            string query = @"
SELECT 
    s.StudentId,
    ISNULL(s.StudentNumber, N'') AS StudentNumber,
    ISNULL(s.FullName, N'') AS FullName,
    ISNULL(s.Gender, N'') AS Gender,
    s.BirthDate,
    ISNULL(s.BirthPlace, N'') AS BirthPlace,
    ISNULL(s.Nationality, N'') AS Nationality,
    ISNULL(s.NationalId, N'') AS NationalId,
    ISNULL(s.StudentPhone, N'') AS StudentPhone,
    ISNULL(s.Status, N'') AS Status,
    ISNULL(s.GuardianName, N'') AS GuardianName,
    ISNULL(s.GuardianRelation, N'') AS GuardianRelation,
    ISNULL(s.GuardianPhone, N'') AS GuardianPhone,
    ISNULL(s.GuardianEmail, N'') AS GuardianEmail,
    ISNULL(s.GuardianJob, N'') AS GuardianJob,
    ISNULL(s.Governorate, N'') AS Governorate,
    ISNULL(s.District, N'') AS District,
    ISNULL(s.Address, N'') AS Address,
    s.Photo,
    s.CreatedAt,
    s.UpdatedAt,
    ISNULL(c.ClassName, N'') AS CurrentClassName,
    ISNULL(s.Section, N'') AS CurrentSection,
    ISNULL(s.AcademicYear, N'') AS AcademicYear
FROM Students s
LEFT JOIN Classes c ON c.ClassID = s.ClassID
ORDER BY s.StudentId DESC;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(MapStudent(reader));
                    }
                }
            }

            return students;
        }

        public List<Student> GetActive()
        {
            return GetAll().FindAll(student =>
                string.Equals(student.Status, "نشط", System.StringComparison.OrdinalIgnoreCase));
        }

        public Student GetById(int studentId)
        {
            string query = @"
SELECT 
    s.StudentId,
    ISNULL(s.StudentNumber, N'') AS StudentNumber,
    ISNULL(s.FullName, N'') AS FullName,
    ISNULL(s.Gender, N'') AS Gender,
    s.BirthDate,
    ISNULL(s.BirthPlace, N'') AS BirthPlace,
    ISNULL(s.Nationality, N'') AS Nationality,
    ISNULL(s.NationalId, N'') AS NationalId,
    ISNULL(s.StudentPhone, N'') AS StudentPhone,
    ISNULL(s.Status, N'') AS Status,
    ISNULL(s.GuardianName, N'') AS GuardianName,
    ISNULL(s.GuardianRelation, N'') AS GuardianRelation,
    ISNULL(s.GuardianPhone, N'') AS GuardianPhone,
    ISNULL(s.GuardianEmail, N'') AS GuardianEmail,
    ISNULL(s.GuardianJob, N'') AS GuardianJob,
    ISNULL(s.Governorate, N'') AS Governorate,
    ISNULL(s.District, N'') AS District,
    ISNULL(s.Address, N'') AS Address,
    s.Photo,
    s.CreatedAt,
    s.UpdatedAt,
    ISNULL(c.ClassName, N'') AS CurrentClassName,
    ISNULL(s.Section, N'') AS CurrentSection,
    ISNULL(s.AcademicYear, N'') AS AcademicYear
FROM Students s
LEFT JOIN Classes c ON c.ClassID = s.ClassID
WHERE s.StudentId = @StudentId;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapStudent(reader);
                }
            }

            return null;
        }

        public List<Student> Search(string keyword)
        {
            var students = new List<Student>();

            string query = @"
SELECT 
    s.StudentId,
    ISNULL(s.StudentNumber, N'') AS StudentNumber,
    ISNULL(s.FullName, N'') AS FullName,
    ISNULL(s.Gender, N'') AS Gender,
    s.BirthDate,
    ISNULL(s.BirthPlace, N'') AS BirthPlace,
    ISNULL(s.Nationality, N'') AS Nationality,
    ISNULL(s.NationalId, N'') AS NationalId,
    ISNULL(s.StudentPhone, N'') AS StudentPhone,
    ISNULL(s.Status, N'') AS Status,
    ISNULL(s.GuardianName, N'') AS GuardianName,
    ISNULL(s.GuardianRelation, N'') AS GuardianRelation,
    ISNULL(s.GuardianPhone, N'') AS GuardianPhone,
    ISNULL(s.GuardianEmail, N'') AS GuardianEmail,
    ISNULL(s.GuardianJob, N'') AS GuardianJob,
    ISNULL(s.Governorate, N'') AS Governorate,
    ISNULL(s.District, N'') AS District,
    ISNULL(s.Address, N'') AS Address,
    s.Photo,
    s.CreatedAt,
    s.UpdatedAt,
    ISNULL(c.ClassName, N'') AS CurrentClassName,
    ISNULL(s.Section, N'') AS CurrentSection,
    ISNULL(s.AcademicYear, N'') AS AcademicYear
FROM Students s
LEFT JOIN Classes c ON c.ClassID = s.ClassID
WHERE 
    ISNULL(s.FullName, N'') LIKE @Keyword
    OR ISNULL(s.StudentNumber, N'') LIKE @Keyword
    OR ISNULL(s.NationalId, N'') LIKE @Keyword
    OR ISNULL(s.StudentPhone, N'') LIKE @Keyword
    OR ISNULL(s.GuardianPhone, N'') LIKE @Keyword
ORDER BY s.StudentId DESC;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 250).Value = "%" + keyword.Trim() + "%";

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(MapStudent(reader));
                    }
                }
            }

            return students;
        }

        public int Add(Student student)
        {
            const string nextNumberQuery = @"
SELECT ISNULL(MAX(TRY_CONVERT(INT, StudentNumber)), 0) + 1
FROM Students WITH (UPDLOCK, HOLDLOCK)
WHERE ISNUMERIC(StudentNumber) = 1;";

            const string insertQuery = @"
INSERT INTO Students
(
    StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality,
    NationalId, StudentPhone, Status, GuardianName, GuardianRelation,
    GuardianPhone, GuardianEmail, GuardianJob, Governorate, District,
    Address, Photo, CreatedAt
)
OUTPUT INSERTED.StudentId
VALUES
(
    @StudentNumber, @FullName, @Gender, @BirthDate, @BirthPlace, @Nationality,
    @NationalId, @StudentPhone, @Status, @GuardianName, @GuardianRelation,
    @GuardianPhone, @GuardianEmail, @GuardianJob, @Governorate, @District,
    @Address, @Photo, GETDATE()
);";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (SqlCommand nextNumberCommand = new SqlCommand(nextNumberQuery, conn, transaction))
                        {
                            int nextNumber = Convert.ToInt32(nextNumberCommand.ExecuteScalar());
                            student.StudentNumber = nextNumber.ToString("000000");
                        }

                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn, transaction))
                        {
                            AddParameters(insertCommand, student);
                            int studentId = Convert.ToInt32(insertCommand.ExecuteScalar());
                            transaction.Commit();
                            return studentId;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(Student student)
        {
            string query = @"
UPDATE Students SET
    FullName = @FullName,
    Gender = @Gender,
    BirthDate = @BirthDate,
    BirthPlace = @BirthPlace,
    Nationality = @Nationality,
    NationalId = @NationalId,
    StudentPhone = @StudentPhone,
    Status = @Status,
    GuardianName = @GuardianName,
    GuardianRelation = @GuardianRelation,
    GuardianPhone = @GuardianPhone,
    GuardianEmail = @GuardianEmail,
    GuardianJob = @GuardianJob,
    Governorate = @Governorate,
    District = @District,
    Address = @Address,
    Photo = @Photo,
    UpdatedAt = GETDATE()
WHERE StudentId = @StudentId;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                AddParameters(cmd, student);
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = student.StudentId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int studentId)
        {
            const string query = @"
UPDATE Students
SET Status = N'محذوف',
    UpdatedAt = GETDATE()
WHERE StudentId = @StudentId
  AND ISNULL(Status, N'') <> N'محذوف';";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;

                conn.Open();
                if (cmd.ExecuteNonQuery() == 0)
                    throw new System.InvalidOperationException("الطالب غير موجود أو تم تعطيله مسبقاً.");
            }
        }

        public bool IsNationalIdExists(string nationalId, int exceptStudentId = 0)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return false;

            string query = @"
SELECT COUNT(1)
FROM Students
WHERE ISNULL(NationalId, N'') = @NationalId
  AND StudentId <> @StudentId;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@NationalId", SqlDbType.NVarChar, 50).Value = nationalId.Trim();
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = exceptStudentId;

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool IsPhoneExists(string phone, int exceptStudentId = 0)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string query = @"
SELECT COUNT(1)
FROM Students
WHERE 
    (
        ISNULL(StudentPhone, N'') = @Phone
        OR ISNULL(GuardianPhone, N'') = @Phone
    )
    AND StudentId <> @StudentId;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 30).Value = phone.Trim();
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = exceptStudentId;

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public string GenerateNextStudentNumber()
        {
            string query = @"
SELECT ISNULL(MAX(CAST(StudentNumber AS INT)), 0) + 1
FROM Students
WHERE ISNUMERIC(StudentNumber) = 1;";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                int nextNumber = 1;

                if (result != null && result != DBNull.Value)
                    nextNumber = Convert.ToInt32(result);

                return nextNumber.ToString("000000");
            }
        }

        private static void AddParameters(SqlCommand cmd, Student student)
        {
            cmd.Parameters.Add("@StudentNumber", SqlDbType.NVarChar, 30).Value = ToDb(student.StudentNumber);
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 200).Value = ToDb(student.FullName);
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = ToDb(student.Gender);

            if (student.BirthDate.HasValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = student.BirthDate.Value.Date;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = DBNull.Value;

            cmd.Parameters.Add("@BirthPlace", SqlDbType.NVarChar, 100).Value = ToDb(student.BirthPlace);
            cmd.Parameters.Add("@Nationality", SqlDbType.NVarChar, 100).Value = ToDb(student.Nationality);
            cmd.Parameters.Add("@NationalId", SqlDbType.NVarChar, 50).Value = ToDb(student.NationalId);
            cmd.Parameters.Add("@StudentPhone", SqlDbType.NVarChar, 30).Value = ToDb(student.StudentPhone);
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 30).Value = ToDb(student.Status);

            cmd.Parameters.Add("@GuardianName", SqlDbType.NVarChar, 200).Value = ToDb(student.GuardianName);
            cmd.Parameters.Add("@GuardianRelation", SqlDbType.NVarChar, 50).Value = ToDb(student.GuardianRelation);
            cmd.Parameters.Add("@GuardianPhone", SqlDbType.NVarChar, 30).Value = ToDb(student.GuardianPhone);
            cmd.Parameters.Add("@GuardianEmail", SqlDbType.NVarChar, 150).Value = ToDb(student.GuardianEmail);
            cmd.Parameters.Add("@GuardianJob", SqlDbType.NVarChar, 100).Value = ToDb(student.GuardianJob);

            cmd.Parameters.Add("@Governorate", SqlDbType.NVarChar, 100).Value = ToDb(student.Governorate);
            cmd.Parameters.Add("@District", SqlDbType.NVarChar, 100).Value = ToDb(student.District);
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 300).Value = ToDb(student.Address);

            SqlParameter photoParam = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1);

            if (student.Photo != null && student.Photo.Length > 0)
                photoParam.Value = student.Photo;
            else
                photoParam.Value = DBNull.Value;
        }

        private static object ToDb(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;

            return value.Trim();
        }

        private static Student MapStudent(SqlDataReader reader)
        {
            Student student = new Student();

            student.StudentId = GetInt(reader, "StudentId");
            student.StudentNumber = GetString(reader, "StudentNumber");
            student.FullName = GetString(reader, "FullName");
            student.Gender = GetString(reader, "Gender");
            student.BirthDate = GetNullableDate(reader, "BirthDate");
            student.BirthPlace = GetString(reader, "BirthPlace");
            student.Nationality = GetString(reader, "Nationality");
            student.NationalId = GetString(reader, "NationalId");
            student.StudentPhone = GetString(reader, "StudentPhone");
            student.Status = GetString(reader, "Status");

            student.GuardianName = GetString(reader, "GuardianName");
            student.GuardianRelation = GetString(reader, "GuardianRelation");
            student.GuardianPhone = GetString(reader, "GuardianPhone");
            student.GuardianEmail = GetString(reader, "GuardianEmail");
            student.GuardianJob = GetString(reader, "GuardianJob");

            student.Governorate = GetString(reader, "Governorate");
            student.District = GetString(reader, "District");
            student.Address = GetString(reader, "Address");

            student.Photo = GetBytes(reader, "Photo");
            student.CurrentClassName = GetString(reader, "CurrentClassName");
            student.CurrentSection = GetString(reader, "CurrentSection");
            student.AcademicYear = GetString(reader, "AcademicYear");
            student.CreatedAt = GetDate(reader, "CreatedAt");
            student.UpdatedAt = GetNullableDate(reader, "UpdatedAt");

            return student;
        }

        private static string GetString(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);

            if (reader.IsDBNull(ordinal))
                return string.Empty;

            return Convert.ToString(reader.GetValue(ordinal));
        }

        private static int GetInt(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);

            if (reader.IsDBNull(ordinal))
                return 0;

            return Convert.ToInt32(reader.GetValue(ordinal));
        }

        private static DateTime GetDate(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);

            if (reader.IsDBNull(ordinal))
                return DateTime.MinValue;

            return Convert.ToDateTime(reader.GetValue(ordinal));
        }

        private static DateTime? GetNullableDate(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);

            if (reader.IsDBNull(ordinal))
                return null;

            return Convert.ToDateTime(reader.GetValue(ordinal));
        }

        private static byte[] GetBytes(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);

            if (reader.IsDBNull(ordinal))
                return null;

            return (byte[])reader.GetValue(ordinal);
        }

        private static SqlConnection GetConnection()
        {
            return SchoolSystem.DataAccess.DbConnection.GetConnection();
        }
    }
}
