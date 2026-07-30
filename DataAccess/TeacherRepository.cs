using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class TeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllTeachers()
        {
            DataTable dt = new DataTable();
            const string query = @"SELECT TeacherID, EmployeeNumber, FullName, Gender, BirthDate, BirthPlace, 
                                  Nationality, NationalID, Phone, Email, Address, Qualification, 
                                  Specialization, HireDate, BasicSalary, TransportAllowance, 
                                  HousingAllowance, Status, Notes, CreatedAt 
                                  FROM Teachers ORDER BY TeacherID DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }
            return dt;
        }

        public void AddTeacher(Teacher teacher)
        {
            const string query = @"INSERT INTO Teachers (EmployeeNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, 
                                    NationalID, Phone, Email, Address, Qualification, Specialization, HireDate, 
                                    BasicSalary, TransportAllowance, HousingAllowance, Status, Notes, CreatedAt)
                                  VALUES (@EmployeeNumber, @FullName, @Gender, @BirthDate, @BirthPlace, @Nationality,
                                    @NationalID, @Phone, @Email, @Address, @Qualification, @Specialization, @HireDate,
                                    @BasicSalary, @TransportAllowance, @HousingAllowance, @Status, @Notes, GETDATE())";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                AddTeacherParameters(cmd, teacher);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            const string query = @"UPDATE Teachers SET 
                                    EmployeeNumber = @EmployeeNumber, FullName = @FullName, Gender = @Gender,
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

        public void DeleteTeacher(int teacherId)
        {
            const string query = "DELETE FROM Teachers WHERE TeacherID = @TeacherID";
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

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