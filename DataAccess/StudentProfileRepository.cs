using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;
using SchoolSystem.DataAccess.Repositories;


namespace SchoolSystem.DataAccess
{

    public class StudentProfileRepository
    {
        // تعريف الدالة StudentRepository لتنفيذ العملية المرتبطة بها.
        private readonly StudentRepository studentRepository = new StudentRepository();

        // تعريف الدالة GetProfile لتنفيذ العملية المرتبطة بها.
        public StudentProfile GetProfile(int studentId, bool includeFinancials)
        {
            Student student = studentRepository.GetById(studentId);
            if (student == null)
                return null;

            using (SqlConnection connection = DbConnection.GetConnection())
            {
                connection.Open();

                student.CurrentClassName = LoadClassName(connection, studentId);

                DataTable attendance = TryLoadOptionalSection(
                    "الحضور والغياب",
                    () => LoadAttendance(connection, studentId));

                DataTable grades = TryLoadOptionalSection(
                    "الدرجات الأكاديمية",
                    () => LoadGrades(connection, studentId));

                DataTable fees = includeFinancials
                    ? TryLoadOptionalSection(
                        "الرسوم والمدفوعات",
                        () => LoadFees(connection, studentId))
                    : new DataTable();

                return new StudentProfile
                {
                    Student = student,
                    Attendance = attendance,
                    Grades = grades,
                    Fees = fees,
                    CanViewFinancials = includeFinancials
                };
            }
        }

        /// <summary>
        /// يعزل أخطاء الجداول الثانوية عن بيانات الطالب الأساسية.
        /// إذا كان مخطط قاعدة البيانات أقدم أو كان أحد الجداول غير متاح،
        /// يستمر ملف الطالب في الفتح وتظهر ملاحظة داخل القسم المتعذر تحميله.
        /// </summary>
        private static DataTable TryLoadOptionalSection(
            string sectionName,
            Func<DataTable> loader)
        {
            try
            {
                DataTable table = loader();

                return table ?? new DataTable();
            }
            catch (Exception ex)
            {
                SchoolSystem.Helpers.ApplicationLogger.LogException(
                    "شاشة ملف الطالب: تحميل " + sectionName,
                    ex);

                DataTable fallback = new DataTable();
                fallback.Columns.Add(
                    "ملاحظة",
                    typeof(string));

                fallback.Rows.Add(
                    "تعذر تحميل " + sectionName +
                    ". تم فتح ملف الطالب بالبيانات الأساسية.");

                return fallback;
            }
        }

        // تعريف الدالة LoadClassName لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة LoadAttendance لتنفيذ العملية المرتبطة بها.
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

        // الدرجات الأكاديمية تُقرأ من جدول Grades، وهو المصدر الذي تستخدمه شاشة إدخال الدرجات الحالية.
        private DataTable LoadGrades(SqlConnection connection, int studentId)
        {
            const string query = @"
                SELECT
                    ISNULL(sub.SubjectName, N'غير محدد') AS SubjectName,
                    ISNULL(g.TermName, N'غير محدد') AS TermName,
                    ISNULL(g.Quiz1, 0) AS Quiz1,
                    ISNULL(g.Quiz2, 0) AS Quiz2,
                    ISNULL(g.CourseWork, 0) AS CourseWork,
                    ISNULL(g.FinalExam, 0) AS FinalExam,
                    ISNULL(g.GradeValue, 0) AS Total,
                    ISNULL(g.GradeLetter, N'') AS GradeLetter,
                    ISNULL(g.ResultStatus, N'') AS ResultStatus,
                    ISNULL(g.Notes, N'') AS Notes,
                    g.CreatedAt,
                    g.UpdatedAt
                FROM Grades g
                LEFT JOIN Subjects sub ON g.SubjectID = sub.SubjectID
                WHERE g.StudentID = @StudentID
                ORDER BY ISNULL(g.UpdatedAt, g.CreatedAt) DESC, SubjectName;";

            return ExecuteTable(connection, query, studentId);
        }

        // تعريف الدالة LoadFees لتنفيذ العملية المرتبطة بها.
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
                    ISNULL(PaymentMethod, N'') AS PaymentMethod,
                    ISNULL(ReceiptNumber, N'') AS ReceiptNumber,
                    ISNULL(Status, N'') AS Status
                FROM Fees
                WHERE StudentID = @StudentID
                ORDER BY DueDate DESC, FeeID DESC;";

            return ExecuteTable(connection, query, studentId);
        }

        // تعريف الدالة ExecuteTable لتنفيذ العملية المرتبطة بها.
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
