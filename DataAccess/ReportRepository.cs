using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class ReportRepository
    {
        public DataTable GetSections(int classId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT DISTINCT Section
                    FROM StudentClasses
                    WHERE ClassID = @ClassID
                      AND AcademicYear = @AcademicYear
                      AND NULLIF(LTRIM(RTRIM(Section)), N'') IS NOT NULL
                    ORDER BY Section";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetReportData(ReportRequest request)
        {
            if (request.ReportType == "تقرير الطلاب")
                return GetStudentsReport(request);

            if (request.ReportType == "تقرير المعلمين")
                return GetTeachersReport(request);

            if (request.ReportType == "تقرير القبول والتسجيل")
                return GetEnrollmentReport(request);

            if (request.ReportType == "تقرير توزيع الفصول")
                return GetClassAssignmentReport(request);

            if (request.ReportType == "تقرير حضور المعلمين")
                return GetTeacherAttendanceReport(request);

            if (request.ReportType == "تقرير العقود والرواتب")
                return GetPayrollReport(request);

            if (request.ReportType == "تقرير المستخدمين والصلاحيات")
                return GetUsersReport(request);

            if (request.ReportType == "تقرير الرسوم")
                return GetFeesReport(request);

            if (request.ReportType == "تقرير الدرجات")
                return GetMarksReport(request);

            return CreateMessageTable("نوع التقرير غير معروف.");
        }

        // توافق مع الكود القديم: Students / Teachers / Fees / Attendance / Marks
        public DataTable GetReport(string key)
        {
            ReportRequest request = new ReportRequest
            {
                FromDate = new DateTime(DateTime.Now.Year, 1, 1),
                ToDate = DateTime.Today,
                AcademicYear = "",
                Section = "",
                Status = "الكل",
                SearchText = ""
            };

            if (key == "Students")
            {
                request.ReportType = "تقرير الطلاب";
                return GetStudentsReport(request);
            }

            if (key == "Teachers")
            {
                request.ReportType = "تقرير المعلمين";
                return GetTeachersReport(request);
            }

            if (key == "Fees")
            {
                request.ReportType = "تقرير الرسوم";
                return GetFeesReport(request);
            }

            if (key == "Attendance")
            {
                request.ReportType = "تقرير حضور المعلمين";
                return GetTeacherAttendanceReport(request);
            }

            if (key == "Marks")
            {
                request.ReportType = "تقرير الدرجات";
                return GetMarksReport(request);
            }

            return CreateMessageTable("مفتاح التقرير غير معروف.");
        }

        private DataTable GetStudentsReport(ReportRequest request)
        {
            if (!TableExists("Students"))
                return CreateMessageTable("جدول الطلاب Students غير موجود.");

            string query = @"
                SELECT
                    s.StudentID AS [رقم الطالب],
                    s.StudentNumber AS [الرقم الأكاديمي],
                    s.FullName AS [اسم الطالب],
                    s.Gender AS [الجنس],
                    s.BirthDate AS [تاريخ الميلاد],
                    c.ClassName AS [الصف],
                    s.Section AS [الشعبة],
                    s.AcademicYear AS [العام الدراسي],
                    s.Phone AS [هاتف الطالب],
                    s.GuardianName AS [ولي الأمر],
                    s.GuardianPhone AS [هاتف الولي],
                    s.Status AS [الحالة],
                    s.CreatedAt AS [تاريخ الإضافة]
                FROM Students s
                LEFT JOIN Classes c ON s.ClassID = c.ClassID
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND ISNULL(s.AcademicYear, '') = @AcademicYear";

            if (request.ClassID.HasValue)
                query += " AND s.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND ISNULL(s.Section, '') = @Section";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND ISNULL(s.Status, '') = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (s.FullName LIKE @Search OR s.StudentNumber LIKE @Search OR s.GuardianPhone LIKE @Search)";

            query += " ORDER BY s.FullName";

            return ExecuteQuery(query, request);
        }

        private DataTable GetTeachersReport(ReportRequest request)
        {
            if (!TableExists("Teachers"))
                return CreateMessageTable("جدول المعلمين Teachers غير موجود.");

            string query = @"
                SELECT
                    TeacherID AS [رقم المعلم],
                    FullName AS [اسم المعلم],
                    Gender AS [الجنس],
                    Phone AS [الهاتف],
                    Email AS [البريد],
                    Address AS [العنوان],
                    HireDate AS [تاريخ التوظيف],
                    Status AS [الحالة],
                    CreatedAt AS [تاريخ الإضافة]
                FROM Teachers
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND ISNULL(Status, '') = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (FullName LIKE @Search OR Phone LIKE @Search OR Email LIKE @Search)";

            query += " ORDER BY FullName";

            return ExecuteQuery(query, request);
        }

        private DataTable GetEnrollmentReport(ReportRequest request)
        {
            if (!TableExists("Enrollments"))
                return CreateMessageTable("جدول القبول والتسجيل Enrollments غير موجود.");

            string query = @"
                SELECT
                    e.EnrollmentID AS [رقم الطلب],
                    e.ApplicationDate AS [تاريخ التقديم],
                    e.ApplicationType AS [نوع التسجيل],
                    e.FullName AS [اسم الطالب],
                    e.Gender AS [الجنس],
                    c.ClassName AS [الصف],
                    e.Section AS [الشعبة],
                    e.AcademicYear AS [العام الدراسي],
                    e.Status AS [حالة الطلب],
                    e.GuardianName AS [ولي الأمر],
                    e.GuardianPhone AS [هاتف الولي],
                    e.RegistrationFee AS [رسوم التسجيل],
                    e.PaidAmount AS [المدفوع],
                    (ISNULL(e.RegistrationFee, 0) - ISNULL(e.PaidAmount, 0)) AS [المتبقي],
                    e.PaymentMethod AS [طريقة الدفع],
                    e.ReceiptNo AS [رقم السند]
                FROM Enrollments e
                LEFT JOIN Classes c ON e.ClassID = c.ClassID
                WHERE e.ApplicationDate BETWEEN @FromDate AND @ToDate";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND e.AcademicYear = @AcademicYear";

            if (request.ClassID.HasValue)
                query += " AND e.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND e.Section = @Section";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND e.Status = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (e.FullName LIKE @Search OR e.GuardianPhone LIKE @Search OR e.NationalId LIKE @Search)";

            query += " ORDER BY e.EnrollmentID DESC";

            return ExecuteQuery(query, request);
        }

        private DataTable GetClassAssignmentReport(ReportRequest request)
        {
            if (!TableExists("StudentClasses"))
                return CreateMessageTable("جدول توزيع الفصول StudentClasses غير موجود.");

            string query = @"
                SELECT
                    sc.StudentClassID AS [رقم التوزيع],
                    s.StudentNumber AS [الرقم الأكاديمي],
                    s.FullName AS [اسم الطالب],
                    s.Gender AS [الجنس],
                    c.ClassName AS [الصف],
                    sc.Section AS [الشعبة],
                    sc.AcademicYear AS [العام الدراسي],
                    sc.AssignedDate AS [تاريخ التوزيع]
                FROM StudentClasses sc
                INNER JOIN Students s ON sc.StudentID = s.StudentID
                INNER JOIN Classes c ON sc.ClassID = c.ClassID
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND sc.AcademicYear = @AcademicYear";

            if (request.ClassID.HasValue)
                query += " AND sc.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND sc.Section = @Section";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (s.FullName LIKE @Search OR s.StudentNumber LIKE @Search)";

            query += " ORDER BY c.ClassName, sc.Section, s.FullName";

            return ExecuteQuery(query, request);
        }

        private DataTable GetTeacherAttendanceReport(ReportRequest request)
        {
            if (!TableExists("TeacherAttendance"))
                return CreateMessageTable("جدول حضور المعلمين TeacherAttendance غير موجود.");

            string query = @"
                SELECT
                    ta.AttendanceID AS [رقم السجل],
                    t.FullName AS [اسم المعلم],
                    ta.AttendanceDate AS [تاريخ الحضور],
                    ta.Status AS [الحالة],
                    ta.CheckInTime AS [وقت الحضور],
                    ta.CheckOutTime AS [وقت الانصراف],
                    ta.LateMinutes AS [دقائق التأخير],
                    ta.EarlyLeaveMinutes AS [خروج مبكر],
                    ta.WorkHours AS [ساعات العمل],
                    ta.AbsenceReason AS [السبب],
                    ta.Notes AS [ملاحظات]
                FROM TeacherAttendance ta
                INNER JOIN Teachers t ON ta.TeacherID = t.TeacherID
                WHERE ta.AttendanceDate BETWEEN @FromDate AND @ToDate";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND ta.Status = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (t.FullName LIKE @Search OR ta.Status LIKE @Search OR ta.AbsenceReason LIKE @Search)";

            query += " ORDER BY ta.AttendanceDate DESC, t.FullName";

            return ExecuteQuery(query, request);
        }

        private DataTable GetPayrollReport(ReportRequest request)
        {
            if (!TableExists("TeacherContracts"))
                return CreateMessageTable("جدول العقود TeacherContracts غير موجود.");

            string query = @"
                SELECT
                    c.ContractID AS [رقم العقد],
                    c.ContractNumber AS [رقم العقد النصي],
                    t.FullName AS [اسم المعلم],
                    c.ContractType AS [نوع العقد],
                    c.ContractStatus AS [حالة العقد],
                    c.BasicSalary AS [الراتب الأساسي],
                    c.HousingAllowance AS [بدل السكن],
                    c.TransportAllowance AS [بدل النقل],
                    c.OtherAllowances AS [بدلات أخرى],
                    c.Deductions AS [الخصومات],
                    c.TotalSalary AS [الإجمالي],
                    c.NetSalary AS [الصافي],
                    c.StartDate AS [بداية العقد],
                    c.EndDate AS [نهاية العقد],
                    c.PaymentMethod AS [طريقة الصرف]
                FROM TeacherContracts c
                INNER JOIN Teachers t ON c.TeacherID = t.TeacherID
                WHERE c.StartDate <= @ToDate";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND c.ContractStatus = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (t.FullName LIKE @Search OR c.ContractNumber LIKE @Search OR c.ContractType LIKE @Search)";

            query += " ORDER BY c.ContractID DESC";

            return ExecuteQuery(query, request);
        }

        private DataTable GetUsersReport(ReportRequest request)
        {
            if (!TableExists("Users"))
                return CreateMessageTable("جدول المستخدمين Users غير موجود.");

            string query = @"
                SELECT
                    UserID AS [رقم المستخدم],
                    FullName AS [الاسم الكامل],
                    UserName AS [اسم المستخدم],
                    RoleName AS [الدور],
                    Permissions AS [الصلاحيات],
                    Email AS [البريد],
                    Phone AS [الهاتف],
                    IsActive AS [نشط],
                    MustChangePassword AS [تغيير المرور],
                    LastLoginAt AS [آخر دخول],
                    CreatedAt AS [تاريخ الإنشاء]
                FROM Users
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
            {
                if (request.Status == "نشط")
                    query += " AND IsActive = 1";
                else if (request.Status == "غير نشط")
                    query += " AND IsActive = 0";
            }

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (FullName LIKE @Search OR UserName LIKE @Search OR RoleName LIKE @Search)";

            query += " ORDER BY UserID DESC";

            return ExecuteQuery(query, request);
        }

        private DataTable GetFeesReport(ReportRequest request)
        {
            if (TableExists("Fees"))
            {
                string query = "SELECT * FROM Fees WHERE 1 = 1";
                return ExecuteQuery(query, request);
            }

            if (TableExists("StudentFees"))
            {
                string query = "SELECT * FROM StudentFees WHERE 1 = 1";
                return ExecuteQuery(query, request);
            }

            if (TableExists("Receipts"))
            {
                string query = "SELECT * FROM Receipts WHERE 1 = 1";
                return ExecuteQuery(query, request);
            }

            return CreateMessageTable("لم يتم العثور على جدول رسوم معروف مثل Fees أو StudentFees أو Receipts.");
        }

        private DataTable GetMarksReport(ReportRequest request)
        {
            if (TableExists("Marks"))
                return ExecuteQuery("SELECT * FROM Marks", request);

            if (TableExists("Grades"))
                return ExecuteQuery("SELECT * FROM Grades", request);

            if (TableExists("StudentGrades"))
                return ExecuteQuery("SELECT * FROM StudentGrades", request);

            return CreateMessageTable("لم يتم العثور على جدول درجات معروف مثل Marks أو Grades أو StudentGrades.");
        }

        private DataTable ExecuteQuery(string query, ReportRequest request)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (query.Contains("@FromDate"))
                    cmd.Parameters.AddWithValue("@FromDate", request.FromDate.Date);

                if (query.Contains("@ToDate"))
                    cmd.Parameters.AddWithValue("@ToDate", request.ToDate.Date.AddDays(1).AddSeconds(-1));

                if (query.Contains("@AcademicYear"))
                    cmd.Parameters.AddWithValue("@AcademicYear", request.AcademicYear.Trim());

                if (query.Contains("@ClassID") && request.ClassID.HasValue)
                    cmd.Parameters.AddWithValue("@ClassID", request.ClassID.Value);

                if (query.Contains("@Section"))
                    cmd.Parameters.AddWithValue("@Section", request.Section.Trim());

                if (query.Contains("@Status"))
                    cmd.Parameters.AddWithValue("@Status", request.Status.Trim());

                if (query.Contains("@Search"))
                    cmd.Parameters.AddWithValue("@Search", "%" + request.SearchText.Trim() + "%");

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    try
                    {
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        return CreateMessageTable("تعذر تنفيذ التقرير: " + ex.Message);
                    }

                    return dt;
                }
            }
        }

        private bool TableExists(string tableName)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = @TableName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    conn.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private DataTable CreateMessageTable(string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ملاحظة", typeof(string));
            dt.Rows.Add(message);
            return dt;
        }
    }
}
