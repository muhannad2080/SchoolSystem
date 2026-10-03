using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Helpers;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class TeacherRepository
    {

        private readonly string _connectionString;

        // تعريف الدالة TeacherRepository لتنفيذ العملية المرتبطة بها.
        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // تعريف الدالة GetAllTeachers لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllTeachers()
        {

            return GetTeachers(null);
        }

        // تعريف الدالة GetActiveTeachers لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveTeachers()
        {

            return GetTeachers("نشط");
        }

        // تعريف الدالة GetTeachers لتنفيذ العملية المرتبطة بها.
        private DataTable GetTeachers(string requiredStatus)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT TeacherID, EmployeeNumber, FullName, Gender, BirthDate, BirthPlace,
                                  Nationality, NationalID, Phone, Email, Address, Qualification,
                                  Specialization, HireDate, BasicSalary, TransportAllowance,
                                  HousingAllowance, Status, Notes, CreatedAt
                                  FROM Teachers";

            if (!string.IsNullOrWhiteSpace(requiredStatus))
                query += " WHERE ISNULL(Status, N'نشط') = @Status AND ISNULL(Status, N'') <> N'محذوف'";
            else
                query += " WHERE ISNULL(Status, N'') <> N'محذوف'";

            query += " ORDER BY TeacherID DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                if (!string.IsNullOrWhiteSpace(requiredStatus))
                    cmd.Parameters.AddWithValue("@Status", requiredStatus);

                adapter.Fill(dt);
            }

            return dt;
        }

        // تعريف الدالة AddTeacher لتنفيذ العملية المرتبطة بها.
        public int AddTeacher(Teacher teacher)
        {
            const string nextNumberQuery = @"
SELECT ISNULL(MAX(TRY_CONVERT(INT, RIGHT(EmployeeNumber, 4))), 0) + 1
FROM Teachers WITH (UPDLOCK, HOLDLOCK)
WHERE EmployeeNumber LIKE @Prefix + N'%';";

            const string insertQuery = @"INSERT INTO Teachers (EmployeeNumber, FullName, Gender, BirthDate, BirthPlace, Nationality,
                                    NationalID, Phone, Email, Address, Qualification, Specialization, HireDate,
                                    BasicSalary, TransportAllowance, HousingAllowance, Status, Notes, CreatedAt)
                                  VALUES (@EmployeeNumber, @FullName, @Gender, @BirthDate, @BirthPlace, @Nationality,
                                    @NationalID, @Phone, @Email, @Address, @Qualification, @Specialization, @HireDate,
                                    @BasicSalary, @TransportAllowance, @HousingAllowance, @Status, @Notes, GETDATE());
                                  SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conn = new SqlConnection(_connectionString))
            {

                conn.Open();

                using (var transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        if (string.IsNullOrWhiteSpace(teacher.EmployeeNumber))
                        {
                            int year = teacher.HireDate.HasValue ? teacher.HireDate.Value.Year : DateTime.Now.Year;
                            string prefix = string.Format("TCH-{0}-", year);
                            using (var numberCommand = new SqlCommand(nextNumberQuery, conn, transaction))
                            {

                                numberCommand.Parameters.AddWithValue("@Prefix", prefix);

                                int nextSuffix = Convert.ToInt32(numberCommand.ExecuteScalar());

                                teacher.EmployeeNumber = string.Format("{0}{1:0000}", prefix, nextSuffix);
                            }
                        }

                        using (var insertCommand = new SqlCommand(insertQuery, conn, transaction))
                        {
                            AddTeacherParameters(insertCommand, teacher);

                            object result = insertCommand.ExecuteScalar();
                            if (result == null || result == DBNull.Value)
                                throw new InvalidOperationException("تعذر إنشاء رقم المعلم بعد إضافة السجل.");

                            teacher.TeacherID = Convert.ToInt32(result);
                        }

                        transaction.Commit();
                        return teacher.TeacherID;
                    }

                    catch (Exception ex)
                    {
                        ApplicationLogger.LogException("مستودع " + nameof(TeacherRepository) + ": إضافة معلم", ex);
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        // تعريف الدالة UpdateTeacher لتنفيذ العملية المرتبطة بها.
        public void UpdateTeacher(Teacher teacher)
        {
            const string query = @"UPDATE Teachers SET
                                    FullName = @FullName, Gender = @Gender,
                                    BirthDate = @BirthDate, BirthPlace = @BirthPlace, Nationality = @Nationality,
                                    NationalID = @NationalID, Phone = @Phone, Email = @Email, Address = @Address,
                                    Qualification = @Qualification, Specialization = @Specialization, HireDate = @HireDate,
                                    BasicSalary = @BasicSalary, TransportAllowance = @TransportAllowance,
                                    HousingAllowance = @HousingAllowance, Status = @Status, Notes = @Notes
                                  WHERE TeacherID = @TeacherID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);

                AddTeacherParameters(cmd, teacher);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // تعريف الدالة DeleteTeacher لتنفيذ العملية المرتبطة بها.
        public void DeleteTeacher(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("رقم المعلم غير صحيح.");

            const string referenceQuery = @"
                SELECT
                    (SELECT COUNT(1) FROM TeacherContracts WHERE TeacherID = @TeacherID) +
                    (SELECT COUNT(1) FROM TeacherAttendance WHERE TeacherID = @TeacherID) +
                    (SELECT COUNT(1) FROM Payroll WHERE TeacherID = @TeacherID) +
                    (SELECT COUNT(1) FROM SchoolTimetable WHERE TeacherID = @TeacherID);";

            const string deleteQuery = @"
UPDATE Teachers
SET Status = N'محذوف'
WHERE TeacherID = @TeacherID
  AND ISNULL(Status, N'') <> N'محذوف';";

            using (var conn = new SqlConnection(_connectionString))
            using (var transaction = BeginSerializableTransaction(conn))
            {
                using (var referenceCommand = new SqlCommand(referenceQuery, conn, transaction))
                {
                    referenceCommand.Parameters.Add("@TeacherID", SqlDbType.Int).Value = teacherId;
                    int references = Convert.ToInt32(referenceCommand.ExecuteScalar());
                    if (references > 0)
                        throw new InvalidOperationException(
                            "لا يمكن حذف المعلم لأنه مرتبط بعقود أو حضور أو رواتب أو جدول دراسي. قم بتغيير حالته بدلاً من الحذف.");
                }

                using (var deleteCommand = new SqlCommand(deleteQuery, conn, transaction))
                {
                    deleteCommand.Parameters.Add("@TeacherID", SqlDbType.Int).Value = teacherId;

                    if (deleteCommand.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("المعلم غير موجود أو تم تعطيله مسبقاً.");
                }

                transaction.Commit();
            }
        }

        private static SqlTransaction BeginSerializableTransaction(SqlConnection conn)
        {
            conn.Open();
            return conn.BeginTransaction(IsolationLevel.Serializable);
        }

        // تعريف الدالة GetMaxEmployeeNumberSuffix لتنفيذ العملية المرتبطة بها.
        public int GetMaxEmployeeNumberSuffix(int year)
        {
            const string query = @"SELECT ISNULL(MAX(CAST(SUBSTRING(EmployeeNumber, LEN('TCH-') + 6, 4) AS INT)), 0)
                                  FROM Teachers WHERE EmployeeNumber LIKE @Prefix + '%'";
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@Prefix", $"TCH-{year}-");

                conn.Open();

                var result = cmd.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }

        // تعريف الدالة IsNationalIDUnique لتنفيذ العملية المرتبطة بها.
        public bool IsNationalIDUnique(string nationalID, int? excludeTeacherId = null)
        {
            string query = "SELECT COUNT(*) FROM Teachers WHERE NationalID = @NationalID";
            if (excludeTeacherId.HasValue)
                query += " AND TeacherID <> @TeacherID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@NationalID", nationalID);

                if (excludeTeacherId.HasValue)
                    cmd.Parameters.AddWithValue("@TeacherID", excludeTeacherId.Value);

                conn.Open();

                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        // تعريف الدالة IsEmailUnique لتنفيذ العملية المرتبطة بها.
        public bool IsEmailUnique(string email, int? excludeTeacherId = null)
        {
            string query = "SELECT COUNT(*) FROM Teachers WHERE Email = @Email";
            if (excludeTeacherId.HasValue)
                query += " AND TeacherID <> @TeacherID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@Email", email);

                if (excludeTeacherId.HasValue)
                    cmd.Parameters.AddWithValue("@TeacherID", excludeTeacherId.Value);

                conn.Open();

                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        // تعريف الدالة AddTeacherParameters لتنفيذ العملية المرتبطة بها.
        private void AddTeacherParameters(SqlCommand cmd, Teacher teacher)
        {

            cmd.Parameters.AddWithValue("@EmployeeNumber", teacher.EmployeeNumber);

            cmd.Parameters.AddWithValue("@FullName", teacher.FullName);

            cmd.Parameters.AddWithValue("@Gender", teacher.Gender);

            cmd.Parameters.AddWithValue("@BirthDate", teacher.BirthDate ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@BirthPlace", teacher.BirthPlace ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Nationality", teacher.Nationality ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@NationalID", teacher.NationalID ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Phone", teacher.Phone ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Email", teacher.Email ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Address", teacher.Address ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Qualification", teacher.Qualification ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Specialization", teacher.Specialization ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@HireDate", teacher.HireDate ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@BasicSalary", teacher.BasicSalary);

            cmd.Parameters.AddWithValue("@TransportAllowance", teacher.TransportAllowance);

            cmd.Parameters.AddWithValue("@HousingAllowance", teacher.HousingAllowance);

            cmd.Parameters.AddWithValue("@Status", teacher.Status ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Notes", teacher.Notes ?? (object)DBNull.Value);
        }
    }
}
