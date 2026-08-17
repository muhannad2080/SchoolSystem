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
                    SELECT LTRIM(RTRIM(SectionName)) AS Section
                    FROM dbo.SchoolSections
                    WHERE (@ClassID = 0 OR ClassID = @ClassID)
                      AND REPLACE(AcademicYear, N'/', N'-') = REPLACE(@AcademicYear, N'/', N'-')
                      AND ISNULL(IsActive, 1) = 1
                      AND NULLIF(LTRIM(RTRIM(SectionName)), N'') IS NOT NULL
                    GROUP BY LTRIM(RTRIM(SectionName))
                    ORDER BY Section";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId;
                    cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim();

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

            if (request.ReportType == "تقرير الحركة المالية")
                return GetFinancialMovementReport(request);

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
                    s.StudentPhone AS [هاتف الطالب],
                    s.GuardianName AS [ولي الأمر],
                    s.GuardianPhone AS [هاتف الولي],
                    s.Status AS [الحالة],
                    s.CreatedAt AS [تاريخ الإضافة]
                FROM Students s
                LEFT JOIN Classes c ON s.ClassID = c.ClassID
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(s.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

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
                    s.FullName AS [اسم الطالب],
                    s.Gender AS [الجنس],
                    c.ClassName AS [الصف],
                    e.Section AS [الشعبة],
                    e.AcademicYear AS [العام الدراسي],
                    e.Status AS [حالة الطلب],
                    s.GuardianName AS [ولي الأمر],
                    s.GuardianPhone AS [هاتف الولي],
                    e.RegistrationFee AS [رسوم التسجيل],
                    e.PaidAmount AS [المدفوع],
                    (ISNULL(e.RegistrationFee, 0) - ISNULL(e.PaidAmount, 0)) AS [المتبقي],
                    e.PaymentMethod AS [طريقة الدفع],
                    e.ReceiptNo AS [رقم السند]
                FROM Enrollments e
                LEFT JOIN Students s ON e.StudentID = s.StudentID
                LEFT JOIN Classes c ON e.ClassID = c.ClassID
                WHERE e.ApplicationDate BETWEEN @FromDate AND @ToDate";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

            if (request.ClassID.HasValue)
                query += " AND e.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND e.Section = @Section";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND e.Status = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (ISNULL(s.FullName, '') LIKE @Search OR ISNULL(s.GuardianPhone, '') LIKE @Search OR ISNULL(s.NationalId, '') LIKE @Search)";

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
                    AND ISNULL(s.Status, N'نشط') = N'نشط'
                INNER JOIN Classes c ON sc.ClassID = c.ClassID
                    AND ISNULL(c.IsActive, 1) = 1
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

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
                    AND ISNULL(t.Status, N'نشط') <> N'غير نشط'
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
                    AND ISNULL(t.Status, N'نشط') <> N'غير نشط'
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
                    query += " AND ISNULL(IsActive, 1) = 1";
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
                string query = @"
                    SELECT
                        f.FeeID AS [المعرف],
                        f.StudentID AS [رقم الطالب],
                        s.FullName AS [اسم الطالب],
                        s.ClassID AS [رقم الصف],
                        c.ClassName AS [الصف],
                        f.FeePlanID AS [خطة الرسوم],
                        f.AcademicYear AS [العام الدراسي],
                        f.FeeType AS [نوع الرسوم],
                        f.TotalAmount AS [الإجمالي],
                        f.DiscountAmount AS [الخصم],
                        f.NetAmount AS [الصافي],
                        f.PaidAmount AS [المدفوع],
                        f.RemainingAmount AS [المتبقي],
                        f.DueDate AS [تاريخ الاستحقاق],
                        f.PaymentDate AS [تاريخ السداد],
                        f.PaymentMethod AS [طريقة الدفع],
                        f.ReceiptNumber AS [رقم الإيصال],
                        f.Status AS [الحالة],
                        f.Notes AS [ملاحظات],
                        f.CreatedAt AS [تاريخ الإنشاء],
                        f.UpdatedAt AS [تاريخ التعديل]
                    FROM Fees f
                    INNER JOIN Students s ON f.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'نشط') = N'نشط'
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                        AND ISNULL(c.IsActive, 1) = 1
                    WHERE f.DueDate >= @FromDate
                      AND f.DueDate <= @ToDate";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

                if (request.ClassID.HasValue)
                    query += " AND s.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND (s.Section = @Section OR EXISTS (SELECT 1 FROM StudentClasses sc WHERE sc.StudentID = s.StudentID AND sc.Section = @Section))";

                if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                    query += " AND f.Status = @Status";

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (s.FullName LIKE @Search OR f.FeeType LIKE @Search OR f.ReceiptNumber LIKE @Search)";

                query += " ORDER BY f.DueDate DESC, f.FeeID DESC";
                return ExecuteQuery(query, request);
            }

            if (TableExists("StudentFees"))
            {
                const string query = @"
                    SELECT
                        sf.StudentFeeID AS [المعرف],
                        sf.StudentID AS [رقم الطالب],
                        sf.FeeType AS [نوع الرسم],
                        sf.AcademicYear AS [العام الدراسي],
                        sf.Amount AS [المبلغ],
                        sf.PaidAmount AS [المدفوع],
                        sf.Status AS [الحالة],
                        sf.Notes AS [ملاحظات],
                        sf.CreatedAt AS [تاريخ الإنشاء]
                    FROM StudentFees sf
                    WHERE sf.CreatedAt >= @FromDate
                      AND sf.CreatedAt <= @ToDate
                    ORDER BY sf.CreatedAt DESC, sf.StudentFeeID DESC";
                return ExecuteQuery(query, request);
            }

            if (TableExists("Receipts"))
            {
                const string query = @"
                    SELECT
                        r.ReceiptID AS [المعرف],
                        r.ReceiptNumber AS [رقم الإيصال],
                        r.StudentID AS [رقم الطالب],
                        r.Amount AS [المبلغ],
                        r.ReceiptDate AS [التاريخ],
                        r.PaymentMethod AS [طريقة الدفع],
                        r.Description AS [البيان],
                        r.Notes AS [ملاحظات],
                        r.CreatedAt AS [تاريخ الإنشاء]
                    FROM Receipts r
                    WHERE r.ReceiptDate >= @FromDate
                      AND r.ReceiptDate <= @ToDate
                    ORDER BY r.ReceiptDate DESC, r.ReceiptID DESC";
                return ExecuteQuery(query, request);
            }

            return CreateMessageTable("لم يتم العثور على جدول رسوم معروف مثل Fees أو StudentFees أو Receipts.");
        }

        private DataTable GetFinancialMovementReport(ReportRequest request)
        {
            if (!TableExists("Vouchers"))
                return CreateMessageTable("جدول السندات Vouchers غير موجود. نفّذ ترحيل قاعدة البيانات أولًا.");

            bool hasFees = TableExists("Fees");
            bool hasExpenses = TableExists("Expenses");
            string feeJoin = hasFees ? "LEFT JOIN Fees f ON v.ReferenceType = N'رسوم' AND v.ReferenceID = f.FeeID" : string.Empty;
            string expenseJoin = hasExpenses ? "LEFT JOIN Expenses e ON v.ReferenceType = N'مصروفات' AND v.ReferenceID = e.ExpenseID" : string.Empty;
            string sourceDetails = "v.ReferenceType";
            if (hasFees && hasExpenses)
            {
                sourceDetails = "CASE WHEN v.ReferenceType = N'رسوم' THEN ISNULL(f.FeeType, N'رسوم') " +
                                "WHEN v.ReferenceType = N'مصروفات' THEN ISNULL(e.Category, N'مصروفات') " +
                                "ELSE ISNULL(v.ReferenceType, N'عام') END";
            }
            else if (hasFees)
            {
                sourceDetails = "CASE WHEN v.ReferenceType = N'رسوم' THEN ISNULL(f.FeeType, N'رسوم') ELSE ISNULL(v.ReferenceType, N'عام') END";
            }
            else if (hasExpenses)
            {
                sourceDetails = "CASE WHEN v.ReferenceType = N'مصروفات' THEN ISNULL(e.Category, N'مصروفات') ELSE ISNULL(v.ReferenceType, N'عام') END";
            }

            string query = @"
                SELECT
                    v.VoucherID AS [المعرف],
                    v.VoucherNumber AS [رقم السند],
                    v.VoucherType AS [نوع الحركة],
                    v.VoucherDate AS [التاريخ],
                    v.PartyName AS [الطرف],
                    v.Description AS [البيان],
                    v.PaymentMethod AS [طريقة الدفع],
                    ISNULL(v.ReferenceType, N'عام') AS [نوع المصدر],
                    v.ReferenceID AS [رقم المصدر],
                    " + sourceDetails + @" AS [تفاصيل المصدر],
                    v.Amount AS [المبلغ],
                    CASE WHEN v.VoucherType = N'قبض' THEN v.Amount ELSE 0 END AS [القبض],
                    CASE WHEN v.VoucherType = N'صرف' THEN v.Amount ELSE 0 END AS [الصرف],
                    CASE WHEN v.VoucherType = N'قبض' THEN v.Amount ELSE -v.Amount END AS [الصافي],
                    CASE WHEN ISNULL(v.IsAutoGenerated, 0) = 1 THEN N'نعم' ELSE N'لا' END AS [تلقائي],
                    v.CreatedAt AS [تاريخ الإنشاء]
                FROM Vouchers v
                " + feeJoin + " " + expenseJoin + @"
                WHERE v.VoucherDate >= @FromDate
                  AND v.VoucherDate <= @ToDate";

            if (request.Status == "قبض" || request.Status == "صرف")
                query += " AND v.VoucherType = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (v.VoucherNumber LIKE @Search OR v.PartyName LIKE @Search OR v.Description LIKE @Search OR v.ReferenceType LIKE @Search)";

            query += " ORDER BY v.VoucherDate DESC, v.VoucherID DESC";
            return ExecuteQuery(query, request);
        }

        private DataTable GetMarksReport(ReportRequest request)
        {
            if (!TableExists("Grades") && TableExists("StudentGrades"))
            {
                string query = @"
                    SELECT
                        sg.StudentGradeID AS [المعرف],
                        sg.StudentID AS [رقم الطالب],
                        s.FullName AS [اسم الطالب],
                        sg.ClassID AS [رقم الصف],
                        c.ClassName AS [الصف],
                        sg.Section AS [الشعبة],
                        sg.AcademicYear AS [العام الدراسي],
                        sg.SubjectID AS [رقم المادة],
                        sub.SubjectName AS [المادة],
                        sg.TermName AS [الفصل الدراسي],
                        sg.Quiz1 AS [اختبار 1],
                        sg.Quiz2 AS [اختبار 2],
                        sg.CourseWork AS [أعمال السنة],
                        sg.FinalExam AS [الاختبار النهائي],
                        sg.Total AS [المجموع],
                        sg.GradeLetter AS [التقدير],
                        sg.ResultStatus AS [النتيجة],
                        sg.Notes AS [ملاحظات],
                        sg.CreatedAt AS [تاريخ الإنشاء],
                        sg.UpdatedAt AS [تاريخ التعديل]
                    FROM StudentGrades sg
                    INNER JOIN Students s ON sg.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'نشط') = N'نشط'
                    LEFT JOIN Classes c ON sg.ClassID = c.ClassID
                        AND ISNULL(c.IsActive, 1) = 1
                    INNER JOIN Subjects sub ON sg.SubjectID = sub.SubjectID
                        AND ISNULL(sub.IsActive, 1) = 1
                    WHERE sg.CreatedAt >= @FromDate
                      AND sg.CreatedAt <= @ToDate";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(sg.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

                if (request.ClassID.HasValue)
                    query += " AND sg.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND sg.Section = @Section";

                if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                    query += " AND sg.ResultStatus = @Status";

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (s.FullName LIKE @Search OR sub.SubjectName LIKE @Search OR sg.TermName LIKE @Search)";

                query += " ORDER BY sg.CreatedAt DESC, sg.StudentGradeID DESC";
                return ExecuteQuery(query, request);
            }

            if (TableExists("Marks"))
            {
                string query = @"
                    SELECT
                        m.MarkID AS [المعرف],
                        m.StudentID AS [رقم الطالب],
                        s.FullName AS [اسم الطالب],
                        s.ClassID AS [رقم الصف],
                        c.ClassName AS [الصف],
                        m.SubjectID AS [رقم المادة],
                        sub.SubjectName AS [المادة],
                        m.TeacherID AS [رقم المعلم],
                        t.FullName AS [المعلم],
                        m.Mark AS [الدرجة],
                        m.ExamType AS [نوع الاختبار],
                        m.CreatedAt AS [تاريخ الإنشاء]
                    FROM Marks m
                    INNER JOIN Students s ON m.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'نشط') = N'نشط'
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                        AND ISNULL(c.IsActive, 1) = 1
                    INNER JOIN Subjects sub ON m.SubjectID = sub.SubjectID
                        AND ISNULL(sub.IsActive, 1) = 1
                    LEFT JOIN Teachers t ON m.TeacherID = t.TeacherID
                        AND ISNULL(t.Status, N'نشط') <> N'غير نشط'
                    WHERE m.CreatedAt >= @FromDate
                      AND m.CreatedAt <= @ToDate";

                if (request.ClassID.HasValue)
                    query += " AND s.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (s.FullName LIKE @Search OR sub.SubjectName LIKE @Search OR m.ExamType LIKE @Search)";

                query += " ORDER BY m.CreatedAt DESC, m.MarkID DESC";
                return ExecuteQuery(query, request);
            }

            if (TableExists("Grades"))
            {
                string query = @"
                    SELECT
                        g.GradeID AS [المعرف],
                        g.StudentID AS [رقم الطالب],
                        s.FullName AS [اسم الطالب],
                        g.SubjectID AS [رقم المادة],
                        sub.SubjectName AS [المادة],
                        g.ClassID AS [رقم الصف],
                        c.ClassName AS [الصف],
                        g.Section AS [الشعبة],
                        g.AcademicYear AS [العام الدراسي],
                        g.TermName AS [الفصل الدراسي],
                        g.Quiz1 AS [الاختبار الأول],
                        g.Quiz2 AS [الاختبار الثاني],
                        g.CourseWork AS [أعمال السنة],
                        g.FinalExam AS [الاختبار النهائي],
                        g.GradeValue AS [الدرجة],
                        g.GradeLetter AS [التقدير],
                        g.ResultStatus AS [الحالة],
                        g.Notes AS [ملاحظات],
                        g.CreatedAt AS [تاريخ الإنشاء]
                    FROM Grades g
                    INNER JOIN Students s ON g.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'نشط') = N'نشط'
                    LEFT JOIN Classes c ON g.ClassID = c.ClassID
                        AND ISNULL(c.IsActive, 1) = 1
                    LEFT JOIN Subjects sub ON g.SubjectID = sub.SubjectID
                        AND ISNULL(sub.IsActive, 1) = 1
                    WHERE g.CreatedAt >= @FromDate
                      AND g.CreatedAt <= @ToDate";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(g.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

                if (request.ClassID.HasValue)
                    query += " AND g.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND LTRIM(RTRIM(ISNULL(g.Section, N''))) = LTRIM(RTRIM(@Section))";

                if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                    query += " AND ISNULL(g.ResultStatus, N'') = @Status";

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (ISNULL(s.FullName, N'') LIKE @Search OR ISNULL(sub.SubjectName, N'') LIKE @Search OR ISNULL(g.TermName, N'') LIKE @Search)";

                query += " ORDER BY g.CreatedAt DESC, g.GradeID DESC";
                return ExecuteQuery(query, request);
            }

            return CreateMessageTable("لم يتم العثور على جدول درجات معروف مثل StudentGrades أو Marks أو Grades.");
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
