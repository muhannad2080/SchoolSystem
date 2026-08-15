using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;
using SchoolSystem.DataAccess.Repositories;

namespace SchoolSystem.DataAccess
{
    public class StudentProfileRepository
    {
        private readonly StudentRepository studentRepository = new StudentRepository();

        public StudentProfile GetProfile(int studentId, bool includeFinancials)
        {
            Student student = studentRepository.GetById(studentId);
            if (student == null)
                return null;

            using (SqlConnection connection = DbConnection.GetConnection())
            {
                connection.Open();
                student.CurrentClassName = LoadClassName(connection, studentId);
                return new StudentProfile
                {
                    Student = student,
                    Attendance = LoadAttendance(connection, studentId),
                    Marks = LoadMarks(connection, studentId),
                    Fees = includeFinancials ? LoadFees(connection, studentId) : new DataTable(),
                    CanViewFinancials = includeFinancials
                };
            }
        }

        private string LoadClassName(SqlConnection connection, int studentId)
        {
            const string query = @"
                SELECT TOP 1 ISNULL(c.ClassName, N'')
                FROM Students s
                LEFT JOIN Classes c ON s.ClassID = c.ClassID
                WHERE s.StudentID = @StudentID;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }

        private DataTable LoadAttendance(SqlConnection connection, int studentId)
        {
            const string query = @"
                SELECT TOP 200
                    AttendanceDate,
                    Status,
                    ISNULL(ExcuseStatus, N'بدون عذر') AS ExcuseStatus,
                    ISNULL(Notes, N'') AS Notes,
                    CONVERT(VARCHAR(5), ArrivalTime, 108) AS ArrivalTime
                FROM StudentAttendance
                WHERE StudentID = @StudentID
                ORDER BY AttendanceDate DESC;";
            return ExecuteTable(connection, query, studentId);
        }

        private DataTable LoadMarks(SqlConnection connection, int studentId)
        {
            const string query = @"
                SELECT
                    ISNULL(sub.SubjectName, N'غير محدد') AS SubjectName,
                    ISNULL(m.ExamType, N'عام') AS ExamType,
                    m.Mark AS MarkValue,
                    m.CreatedAt
                FROM Marks m
                LEFT JOIN Subjects sub ON m.SubjectID = sub.SubjectID
                WHERE m.StudentID = @StudentID
                ORDER BY m.CreatedAt DESC, SubjectName;";
            return ExecuteTable(connection, query, studentId);
        }

        private DataTable LoadFees(SqlConnection connection, int studentId)
        {
            const string query = @"
                SELECT
                    ISNULL(AcademicYear, N'') AS AcademicYear,
                    ISNULL(FeeType, N'') AS FeeType,
                    TotalAmount,
                    DiscountAmount,
                    NetAmount,
                    PaidAmount,
                    RemainingAmount,
                    DueDate,
                    PaymentDate,
                    ISNULL(Status, N'') AS Status
                FROM Fees
                WHERE StudentID = @StudentID
                ORDER BY DueDate DESC, FeeID DESC;";
            return ExecuteTable(connection, query, studentId);
        }

        private DataTable ExecuteTable(SqlConnection connection, string query, int studentId)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentId;
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
