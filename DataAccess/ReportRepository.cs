using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Helpers;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class ReportRepository
    {
        // تعريف الدالة GetSections لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة FindStudentId لتنفيذ العملية المرتبطة بها.
        public int? FindStudentId(string searchText, string academicYear, int? classId, string section)
        {
            if (string.IsNullOrWhiteSpace(searchText) || !TableExists("Students"))
                return null;

            bool hasAssignments = TableExists("StudentClasses")
                && ColumnExists("StudentClasses", "StudentID")
                && ColumnExists("StudentClasses", "ClassID")
                && ColumnExists("StudentClasses", "Section")
                && ColumnExists("StudentClasses", "AcademicYear");

            string assignmentJoin = hasAssignments
                ? @"
                    LEFT JOIN
                    (
                        SELECT StudentID, ClassID, Section, AcademicYear,
                               ROW_NUMBER() OVER
                               (
                                   PARTITION BY StudentID
                                   ORDER BY ISNULL(AssignedDate, CONVERT(datetime, '19000101')) DESC,
                                            StudentClassID DESC
                               ) AS AssignmentRank
                        FROM StudentClasses
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                    ) sc ON sc.StudentID = s.StudentID AND sc.AssignmentRank = 1"
                : string.Empty;

            string assignmentFilter = hasAssignments
                ? @"
                  AND
                  (
                      NOT EXISTS
                      (
                          SELECT 1
                          FROM StudentClasses scx
                          WHERE scx.StudentID = s.StudentID
                            AND REPLACE(ISNULL(scx.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      )
                      OR
                      (
                          (@ClassID = 0 OR sc.ClassID = @ClassID)
                          AND (@Section = N'' OR LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(@Section)))
                      )
                  )"
                : @"
                  AND (@ClassID = 0 OR s.ClassID = @ClassID)
                  AND (@Section = N'' OR LTRIM(RTRIM(ISNULL(s.Section, N''))) = LTRIM(RTRIM(@Section)))";

            bool searchIsStudentId;
            int parsedStudentId;
            searchIsStudentId = int.TryParse(searchText.Trim(), out parsedStudentId) && parsedStudentId > 0;

            string query = @"
                SELECT TOP 1 s.StudentID
                FROM Students s" + assignmentJoin + @"
                WHERE
                    (
                        LTRIM(RTRIM(ISNULL(s.FullName, N''))) = LTRIM(RTRIM(@SearchExact))
                        OR LTRIM(RTRIM(ISNULL(s.StudentNumber, N''))) = LTRIM(RTRIM(@SearchExact))
                        OR s.FullName LIKE @Search
                        OR (@SearchIsStudentId = 1 AND s.StudentID = @SearchStudentID)
                    )
                  AND
                    (
                        @AcademicYear = N''
                        OR
                        (
                            " + (hasAssignments
                                ? @"NOT EXISTS
                            (
                                SELECT 1
                                FROM StudentClasses scy
                                WHERE scy.StudentID = s.StudentID
                                  AND REPLACE(ISNULL(scy.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                            )
                            AND REPLACE(ISNULL(s.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')"
                                : @"REPLACE(ISNULL(s.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')") + @"
                        )
                    )" + assignmentFilter + @"
                ORDER BY
                    CASE WHEN LTRIM(RTRIM(ISNULL(s.FullName, N''))) = LTRIM(RTRIM(@SearchExact)) THEN 0 ELSE 1 END,
                    CASE WHEN LTRIM(RTRIM(ISNULL(s.StudentNumber, N''))) = LTRIM(RTRIM(@SearchExact)) THEN 0 ELSE 1 END,
                    CASE WHEN @SearchIsStudentId = 1 AND s.StudentID = @SearchStudentID THEN 0 ELSE 1 END,
                    s.StudentID";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.Add("@SearchExact", SqlDbType.NVarChar, 200).Value = searchText.Trim();

                cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 210).Value = "%" + searchText.Trim() + "%";

                cmd.Parameters.Add("@SearchIsStudentId", SqlDbType.Bit).Value = searchIsStudentId;

                cmd.Parameters.Add("@SearchStudentID", SqlDbType.Int).Value = searchIsStudentId ? parsedStudentId : 0;

                cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = (academicYear ?? string.Empty).Trim();

                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = classId ?? 0;

                cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 100).Value = (section ?? string.Empty).Trim();

                conn.Open();

                object value = cmd.ExecuteScalar();

                if (value == null || value == DBNull.Value)
                    return null;

                return Convert.ToInt32(value);
            }
        }

        // تعريف الدالة GetReportData لتنفيذ العملية المرتبطة بها.
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

            if (request.ReportType == "تقرير الرواتب" ||
                request.ReportType == "تقرير العقود والرواتب")
                return GetPayrollReport(request);

            if (request.ReportType == "تقرير المستخدمين والصلاحيات")
                return GetUsersReport(request);

            if (request.ReportType == "تقرير الرسوم")
                return GetFeesReport(request);

            if (request.ReportType == "تقرير الدرجات" || request.ReportType == "كشف درجات طالب")
                return GetMarksReport(request);

            if (request.ReportType == "كشف نتائج الفصل")
                return GetClassResultsReport(request);

            if (request.ReportType == "تقرير الحركة المالية")
                return GetFinancialMovementReport(request);


            return CreateMessageTable("نوع التقرير غير معروف.");
        }

        // توافق مع الكود القديم: Students / Teachers / Fees / Attendance / Marks
        // تعريف الدالة GetReport لتنفيذ العملية المرتبطة بها.
        public DataTable GetReport(string key, string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))

                throw new ArgumentException("العام الدراسي مطلوب للتقرير.", "academicYear");

            ReportRequest request = new ReportRequest
            {
                FromDate = DateTime.MinValue.Date,
                ToDate = DateTime.Today,
                AcademicYear = academicYear.Trim().Replace('-', '/'),
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

        // تعريف الدالة GetStudentsReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetTeachersReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetEnrollmentReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetClassAssignmentReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetTeacherAttendanceReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetPayrollReport لتنفيذ العملية المرتبطة بها.
        // تعريف الدالة GetPayrollReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetPayrollReport(ReportRequest request)
        {
            if (!TableExists("Payroll"))
                return CreateMessageTable("جدول الرواتب Payroll غير موجود.");

            string query = @"
                SELECT
                    p.PayrollID AS [رقم الراتب],
                    t.FullName AS [اسم المعلم],
                    p.SalaryMonth AS [الشهر],
                    p.SalaryYear AS [السنة],
                    p.BasicSalary AS [الراتب الأساسي],
                    p.Allowances AS [البدلات],
                    p.Deductions AS [الخصومات],
                    p.NetSalary AS [الصافي],
                    p.PaymentDate AS [تاريخ الصرف],
                    v.PaymentMethod AS [طريقة الصرف],
                    v.VoucherNumber AS [رقم سند الصرف],
                    p.Notes AS [ملاحظات],
                    p.CreatedAt AS [تاريخ الإنشاء]
                FROM Payroll p
                INNER JOIN Teachers t ON p.TeacherID = t.TeacherID
                    AND ISNULL(t.Status, N'نشط') <> N'غير نشط'
                LEFT JOIN Vouchers v ON v.ReferenceType = N'رواتب'
                    AND v.ReferenceID = p.PayrollID
                    AND v.IsAutoGenerated = 1
                    AND v.VoucherType = N'صرف'
                WHERE p.PaymentDate IS NULL
                   OR p.PaymentDate BETWEEN @FromDate AND @ToDate";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
            {
                if (request.Status == "مصروف")
                    query += " AND p.PaymentDate IS NOT NULL";
                else if (request.Status == "غير مصروف")
                    query += " AND p.PaymentDate IS NULL";
            }

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (t.FullName LIKE @Search OR v.VoucherNumber LIKE @Search OR p.Notes LIKE @Search)";

            query += " ORDER BY p.SalaryYear DESC, p.SalaryMonth DESC, p.PayrollID DESC";

            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetUsersReport لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetFeesReport لتنفيذ العملية المرتبطة بها.
        // تعريف الدالة GetFeesReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetFeesReport(ReportRequest request)
        {
            if (!TableExists("Fees"))
                return CreateMessageTable("جدول الرسوم الأساسي Fees غير موجود.");

            string query = @"
                SELECT
                    f.FeeID AS [المعرف],
                    f.StudentID AS [رقم الطالب],
                    s.FullName AS [اسم الطالب],
                    sc.ClassID AS [رقم الصف],
                    c.ClassName AS [الصف],
                    sc.Section AS [الشعبة],
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
                    v.VoucherNumber AS [رقم سند القبض],
                    f.Status AS [الحالة],
                    f.Notes AS [ملاحظات],
                    f.CreatedAt AS [تاريخ الإنشاء],
                    f.UpdatedAt AS [تاريخ التعديل]
                FROM Fees f
                INNER JOIN Students s ON f.StudentID = s.StudentID
                    AND ISNULL(s.Status, N'نشط') = N'نشط'
                LEFT JOIN StudentClasses sc ON sc.StudentClassID =
                (
                    SELECT TOP (1) sc2.StudentClassID
                    FROM StudentClasses sc2
                    WHERE sc2.StudentID = f.StudentID
                      AND REPLACE(ISNULL(sc2.AcademicYear, N''), N'-', N'/') =
                          REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')
                    ORDER BY sc2.AssignedDate DESC, sc2.StudentClassID DESC
                )
                LEFT JOIN Classes c ON c.ClassID = sc.ClassID
                    AND ISNULL(c.IsActive, 1) = 1
                OUTER APPLY
                (
                    SELECT TOP (1)
                        v1.VoucherNumber
                    FROM Vouchers v1
                    WHERE v1.ReferenceType = N'رسوم'
                      AND v1.ReferenceID = f.FeeID
                      AND v1.VoucherType = N'قبض'
                      AND v1.IsAutoGenerated = 1
                    ORDER BY v1.VoucherID DESC
                ) v
                WHERE f.DueDate >= @FromDate
                  AND f.DueDate <= @ToDate";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

            if (request.ClassID.HasValue)
                query += " AND sc.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(@Section))";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND f.Status = @Status";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (s.FullName LIKE @Search OR f.FeeType LIKE @Search OR f.ReceiptNumber LIKE @Search OR v.VoucherNumber LIKE @Search)";

            query += " ORDER BY f.DueDate DESC, f.FeeID DESC";

            return ExecuteQuery(query, request);
        }

        private DataTable GetFinancialMovementReport(ReportRequest request)
        {
            if (!TableExists("Vouchers"))
                return CreateMessageTable("جدول السندات Vouchers غير موجود. نفّذ ترحيل قاعدة البيانات أولًا.");

            bool hasFees = TableExists("Fees");
            bool hasExpenses = TableExists("Expenses");
            bool hasExpenseAcademicYear = hasExpenses && ColumnExists("Expenses", "AcademicYear");
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

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
            {
                string normalizedYear = "REPLACE(@AcademicYear, N'-', N'/')";
                string feeYearCondition = hasFees
                    ? "(v.ReferenceType <> N'رسوم' OR REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = " + normalizedYear + ")"
                    : "1 = 1";
                string expenseYearCondition = hasExpenses && hasExpenseAcademicYear
                    ? "(v.ReferenceType <> N'مصروفات' OR REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = " + normalizedYear + ")"
                    : "1 = 1";
                query += " AND " + feeYearCondition + " AND " + expenseYearCondition;
            }
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (v.VoucherNumber LIKE @Search OR v.PartyName LIKE @Search OR v.Description LIKE @Search OR v.ReferenceType LIKE @Search)";


            query += " ORDER BY v.VoucherDate DESC, v.VoucherID DESC";

            return ExecuteQuery(query, request);
        }


        // تعريف الدالة GetStudentAttendanceReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetStudentAttendanceReport(ReportRequest request)
        {
            if (!TableExists("StudentAttendance"))
                return CreateMessageTable("جدول حضور الطلاب StudentAttendance غير موجود.");

            string query = @"
                SELECT
                    a.AttendanceID AS [المعرف],
                    a.AttendanceDate AS [التاريخ],
                    a.StudentID AS [رقم الطالب],
                    s.StudentNumber AS [الرقم الأكاديمي],
                    s.FullName AS [اسم الطالب],
                    c.ClassName AS [الصف],
                    a.Section AS [الشعبة],
                    a.AcademicYear AS [العام الدراسي],
                    a.Status AS [الحالة],
                    a.ExcuseStatus AS [حالة العذر],
                    a.AbsenceReason AS [سبب الغياب],
                    a.ArrivalTime AS [وقت الحضور],
                    a.DepartureTime AS [وقت الانصراف],
                    a.Notes AS [ملاحظات]
                FROM StudentAttendance a
                INNER JOIN Students s ON s.StudentID = a.StudentID
                LEFT JOIN Classes c ON c.ClassID = a.ClassID
                WHERE a.AttendanceDate >= @FromDate
                  AND a.AttendanceDate <= @ToDate";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(a.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";
            if (request.ClassID.HasValue)
                query += " AND a.ClassID = @ClassID";
            if (!string.IsNullOrWhiteSpace(request.Section))
                query += " AND LTRIM(RTRIM(ISNULL(a.Section, N''))) = LTRIM(RTRIM(@Section))";
            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
                query += " AND ISNULL(a.Status, N'') = @Status";
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (s.FullName LIKE @Search OR s.StudentNumber LIKE @Search)";

            query += " ORDER BY a.AttendanceDate DESC, s.FullName";
            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetSubjectsReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetSubjectsReport(ReportRequest request)
        {
            if (!TableExists("Subjects"))
                return CreateMessageTable("جدول المواد Subjects غير موجود.");

            string query = @"
                SELECT
                    sub.SubjectID AS [رقم المادة],
                    sub.SubjectCode AS [كود المادة],
                    sub.SubjectName AS [اسم المادة],
                    c.ClassName AS [الصف],
                    sub.MaxDegree AS [الدرجة الكبرى],
                    sub.PassDegree AS [درجة النجاح],
                    sub.IsActive AS [نشطة],
                    sub.Notes AS [ملاحظات],
                    sub.CreatedAt AS [تاريخ الإضافة]
                FROM Subjects sub
                LEFT JOIN Classes c ON c.ClassID = sub.ClassID
                WHERE 1 = 1";

            if (request.ClassID.HasValue)
                query += " AND sub.ClassID = @ClassID";
            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
            {
                if (request.Status == "نشط" || request.Status == "غير نشط")
                    query += " AND sub.IsActive = @IsActive";
            }
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (sub.SubjectCode LIKE @Search OR sub.SubjectName LIKE @Search)";

            query += " ORDER BY c.ClassName, sub.SubjectName, sub.SubjectID";
            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetLibraryReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetLibraryReport(ReportRequest request)
        {
            if (!TableExists("Books"))
                return CreateMessageTable("جدول الكتب Books غير موجود.");
            if (!TableExists("BookBorrowings"))
                return CreateMessageTable("جدول الإعارات BookBorrowings غير موجود.");

            string query = @"
                SELECT
                    br.BorrowingID AS [رقم الإعارة],
                    b.BookID AS [رقم الكتاب],
                    b.Title AS [الكتاب],
                    b.Category AS [التصنيف],
                    br.BorrowerType AS [نوع المستعير],
                    br.BorrowerID AS [رقم المستعير],
                    CASE
                        WHEN br.BorrowerType = N'طالب' THEN ISNULL(s.FullName, N'غير معروف')
                        WHEN br.BorrowerType = N'معلم' THEN ISNULL(t.FullName, N'غير معروف')
                        ELSE N'غير معروف'
                    END AS [اسم المستعير],
                    br.BorrowDate AS [تاريخ الإعارة],
                    br.DueDate AS [تاريخ الاستحقاق],
                    br.ReturnDate AS [تاريخ الإرجاع],
                    CASE
                        WHEN br.Status = N'معار' AND br.DueDate < CAST(GETDATE() AS DATE) THEN N'متأخر'
                        ELSE br.Status
                    END AS [الحالة],
                    b.Copies AS [إجمالي النسخ],
                    b.Copies - ISNULL((SELECT COUNT(*) FROM BookBorrowings x WHERE x.BookID = b.BookID AND x.Status = N'معار'), 0) AS [النسخ المتاحة],
                    br.Notes AS [ملاحظات]
                FROM BookBorrowings br
                INNER JOIN Books b ON b.BookID = br.BookID
                LEFT JOIN Students s ON br.BorrowerType = N'طالب' AND br.BorrowerID = s.StudentID
                LEFT JOIN Teachers t ON br.BorrowerType = N'معلم' AND br.BorrowerID = t.TeacherID
                WHERE br.BorrowDate >= @FromDate
                  AND br.BorrowDate <= @ToDate";

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "الكل")
            {
                if (request.Status == "متأخر")
                    query += " AND br.Status = N'معار' AND br.DueDate < CAST(GETDATE() AS DATE)";
                else if (request.Status == "معار" || request.Status == "مسترجع")
                    query += " AND br.Status = @Status";
            }
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (b.Title LIKE @Search OR b.ISBN LIKE @Search OR s.FullName LIKE @Search OR t.FullName LIKE @Search)";

            query += " ORDER BY br.BorrowDate DESC, br.BorrowingID DESC";
            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetTransportReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetTransportReport(ReportRequest request)
        {
            if (!TableExists("StudentTransportAssignments"))
                return CreateMessageTable("جدول توزيع النقل StudentTransportAssignments غير موجود.");

            string query = @"
                SELECT
                    a.AssignmentID AS [رقم التوزيع],
                    a.StudentID AS [رقم الطالب],
                    s.StudentNumber AS [الرقم الأكاديمي],
                    s.FullName AS [اسم الطالب],
                    a.AcademicYear AS [العام الدراسي],
                    r.RouteName AS [المسار],
                    b.BusNumber AS [الحافلة],
                    a.PickupPoint AS [نقطة الصعود],
                    a.DropoffPoint AS [نقطة النزول],
                    a.StartDate AS [بداية النقل],
                    a.EndDate AS [نهاية النقل],
                    a.IsActive AS [نشط],
                    r.Fee AS [رسوم النقل],
                    a.Notes AS [ملاحظات]
                FROM StudentTransportAssignments a
                INNER JOIN Students s ON s.StudentID = a.StudentID
                LEFT JOIN BusRoutes r ON r.RouteID = a.RouteID
                LEFT JOIN Buses b ON b.BusID = r.BusID
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                query += " AND REPLACE(ISNULL(a.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";
            if (request.ClassID.HasValue)
                query += " AND s.ClassID = @ClassID";
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (request.Status == "نشط")
                    query += " AND a.IsActive = 1";
                else if (request.Status == "غير نشط")
                    query += " AND a.IsActive = 0";
            }
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (s.FullName LIKE @Search OR s.StudentNumber LIKE @Search OR r.RouteName LIKE @Search OR b.BusNumber LIKE @Search)";

            query += " ORDER BY s.FullName, r.RouteName, a.AssignmentID DESC";
            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetExpensesReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetExpensesReport(ReportRequest request)
        {
            if (!TableExists("Expenses"))
                return CreateMessageTable("جدول المصروفات Expenses غير موجود.");

            string query = @"
                SELECT
                    e.ExpenseID AS [رقم المصروف],
                    e.ExpenseNumber AS [رقم السند],
                    e.ExpenseDate AS [التاريخ],
                    e.Category AS [التصنيف],
                    e.PayeeName AS [المستفيد],
                    e.PaymentMethod AS [طريقة الدفع],
                    e.Amount AS [المبلغ],
                    e.Description AS [البيان],
                    e.Notes AS [ملاحظات],
                    e.CreatedAt AS [تاريخ الإنشاء]
                FROM Expenses e
                WHERE e.ExpenseDate >= @FromDate
                  AND e.ExpenseDate <= @ToDate";

            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query += " AND (e.ExpenseNumber LIKE @Search OR e.Category LIKE @Search OR e.PayeeName LIKE @Search OR e.Description LIKE @Search)";

            if (request.Status == "مصروف")
                query += " AND e.Amount > 0";

            query += " ORDER BY e.ExpenseDate DESC, e.ExpenseID DESC";
            return ExecuteQuery(query, request);
        }

        // تعريف الدالة GetMarksReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetMarksReport(ReportRequest request)
        {
            // Grades هو مصدر الإدخال الحالي؛ يُفحص أولاً حتى لا تختفي الدرجات عند وجود جداول توافق قديمة.
            if (TableExists("Grades"))
            {
                string query = @"
                    WITH RankedGrades AS
                    (
                        SELECT g.*,
                               ROW_NUMBER() OVER
                               (
                                   PARTITION BY g.StudentID, g.SubjectID, g.ClassID,
                                                LTRIM(RTRIM(ISNULL(g.Section, N''))),
                                                LTRIM(RTRIM(ISNULL(g.AcademicYear, N''))),
                                                LTRIM(RTRIM(ISNULL(g.TermName, N'')))
                                   ORDER BY ISNULL(g.UpdatedAt, g.CreatedAt) DESC, g.GradeID DESC
                               ) AS GradeRank
                        FROM Grades g
                    )
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
                        CAST(COALESCE(g.GradeValue,
                            ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                            ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0)) AS decimal(18,2)) AS [مجموع المادة],
                        CAST(COALESCE(g.GradeValue,
                            ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                            ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0)) AS decimal(18,2)) AS [معدل المادة],
                        CAST(SUM(COALESCE(g.GradeValue,
                            ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                            ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0))) OVER
                            (PARTITION BY g.StudentID, g.AcademicYear, g.TermName) AS decimal(18,2)) AS [مجموع الطالب في الفصل],
                        CAST(AVG(CAST(COALESCE(g.GradeValue,
                            ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                            ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0)) AS decimal(18,2))) OVER
                            (PARTITION BY g.StudentID, g.AcademicYear, g.TermName) AS decimal(18,2)) AS [معدل الطالب في الفصل],
                        CASE
                            WHEN SUM(CASE WHEN ISNULL(g.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) OVER
                                (PARTITION BY g.StudentID, g.AcademicYear, g.TermName) > 0 THEN N'غير مكتمل'
                            WHEN SUM(CASE WHEN ISNULL(g.ResultStatus, N'') IN (N'راسب', N'غير مجتاز')
                                               OR (ISNULL(g.ResultStatus, N'') = N'' AND COALESCE(g.GradeValue,
                                                    ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                                                    ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0)) < 50)
                                         THEN 1 ELSE 0 END) OVER
                                (PARTITION BY g.StudentID, g.AcademicYear, g.TermName) > 0 THEN N'راسب'
                            ELSE N'ناجح'
                        END AS [الحالة العامة للطالب],
                        g.GradeLetter AS [التقدير],
                        g.ResultStatus AS [الحالة],
                        g.Notes AS [ملاحظات],
                        g.CreatedAt AS [تاريخ الإنشاء],
                        g.UpdatedAt AS [تاريخ التعديل]
                    FROM RankedGrades g
                    INNER JOIN Students s ON g.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'') NOT IN (N'محذوف', N'Deleted')
                    LEFT JOIN Classes c ON g.ClassID = c.ClassID
                    LEFT JOIN Subjects sub ON g.SubjectID = sub.SubjectID
                    WHERE g.GradeRank = 1";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(g.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";


                if (request.ClassID.HasValue)
                    query += " AND g.ClassID = @ClassID";


                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND LTRIM(RTRIM(ISNULL(g.Section, N''))) = LTRIM(RTRIM(@Section))";


                if (!string.IsNullOrWhiteSpace(request.TermName))
                    query += " AND LTRIM(RTRIM(ISNULL(g.TermName, N''))) = LTRIM(RTRIM(@TermName))";


                if (IsGradeResultStatus(request.Status))
                    query += " AND ISNULL(g.ResultStatus, N'') = @Status";


                if (request.StudentID.HasValue)
                    query += " AND g.StudentID = @StudentID";

                else if (request.ReportType == "كشف درجات طالب" && !string.IsNullOrWhiteSpace(request.StudentSelection))

                    query += BuildStudentSelectionFilter(request, "CONVERT(nvarchar(30), s.StudentID)", "ISNULL(s.FullName, N'')", "ISNULL(s.StudentNumber, N'')");


                if (request.ReportType != "كشف درجات طالب" && !string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (ISNULL(s.FullName, N'') LIKE @Search OR ISNULL(s.StudentNumber, N'') LIKE @Search OR ISNULL(sub.SubjectName, N'') LIKE @Search OR ISNULL(g.TermName, N'') LIKE @Search)";


                query += " ORDER BY s.FullName, g.TermName, sub.SubjectName, g.GradeID";

                return ExecuteQuery(query, request);
            }

            if (TableExists("StudentGrades"))
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
                        sg.Quiz1 AS [الاختبار الأول],
                        sg.Quiz2 AS [الاختبار الثاني],
                        sg.CourseWork AS [أعمال السنة],
                        sg.FinalExam AS [الاختبار النهائي],
                        CAST(ISNULL(sg.Total, 0) AS decimal(18,2)) AS [مجموع المادة],
                        CAST(ISNULL(sg.Total, 0) AS decimal(18,2)) AS [معدل المادة],
                        CAST(SUM(ISNULL(sg.Total, 0)) OVER (PARTITION BY sg.StudentID, sg.AcademicYear, sg.TermName) AS decimal(18,2)) AS [مجموع الطالب في الفصل],
                        CAST(AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) OVER (PARTITION BY sg.StudentID, sg.AcademicYear, sg.TermName) AS decimal(18,2)) AS [معدل الطالب في الفصل],
                        CASE
                            WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) OVER (PARTITION BY sg.StudentID, sg.AcademicYear, sg.TermName) > 0 THEN N'غير مكتمل'
                            WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') IN (N'راسب', N'غير مجتاز') THEN 1 ELSE 0 END) OVER (PARTITION BY sg.StudentID, sg.AcademicYear, sg.TermName) > 0 THEN N'راسب'
                            ELSE N'ناجح'
                        END AS [الحالة العامة للطالب],
                        sg.GradeLetter AS [التقدير],
                        sg.ResultStatus AS [الحالة],
                        sg.Notes AS [ملاحظات],
                        sg.CreatedAt AS [تاريخ الإنشاء],
                        sg.UpdatedAt AS [تاريخ التعديل]
                    FROM StudentGrades sg
                    INNER JOIN Students s ON sg.StudentID = s.StudentID
                        AND ISNULL(s.Status, N'') NOT IN (N'محذوف', N'Deleted')
                    LEFT JOIN Classes c ON sg.ClassID = c.ClassID
                    LEFT JOIN Subjects sub ON sg.SubjectID = sub.SubjectID
                    WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(sg.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

                if (request.ClassID.HasValue)
                    query += " AND sg.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND LTRIM(RTRIM(ISNULL(sg.Section, N''))) = LTRIM(RTRIM(@Section))";

                if (!string.IsNullOrWhiteSpace(request.TermName))
                    query += " AND LTRIM(RTRIM(ISNULL(sg.TermName, N''))) = LTRIM(RTRIM(@TermName))";

                if (IsGradeResultStatus(request.Status))
                    query += " AND ISNULL(sg.ResultStatus, N'') = @Status";

                if (request.StudentID.HasValue)
                    query += " AND sg.StudentID = @StudentID";

                else if (request.ReportType == "كشف درجات طالب" && !string.IsNullOrWhiteSpace(request.StudentSelection))

                    query += BuildStudentSelectionFilter(request, "CONVERT(nvarchar(30), s.StudentID)", "ISNULL(s.FullName, N'')", "ISNULL(s.StudentNumber, N'')");

                if (request.ReportType != "كشف درجات طالب" && !string.IsNullOrWhiteSpace(request.SearchText))
                    query += " AND (ISNULL(s.FullName, N'') LIKE @Search OR ISNULL(s.StudentNumber, N'') LIKE @Search OR ISNULL(sub.SubjectName, N'') LIKE @Search OR ISNULL(sg.TermName, N'') LIKE @Search)";


                query += " ORDER BY s.FullName, sg.TermName, sub.SubjectName, sg.StudentGradeID";

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
                        AND ISNULL(s.Status, N'') NOT IN (N'محذوف', N'Deleted')
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                    LEFT JOIN Subjects sub ON m.SubjectID = sub.SubjectID
                    LEFT JOIN Teachers t ON m.TeacherID = t.TeacherID
                    WHERE ( @IsIndividualGradeReport = 1
                            OR (m.CreatedAt >= @FromDate AND m.CreatedAt <= @ToDate))";

                if (request.ClassID.HasValue)
                    query += " AND s.ClassID = @ClassID";

                if (request.ReportType == "كشف درجات طالب" && !string.IsNullOrWhiteSpace(request.StudentSelection))
                    query += BuildStudentSelectionFilter(request, "CONVERT(nvarchar(30), s.StudentID)", "ISNULL(s.FullName, N'')", "ISNULL(s.StudentNumber, N'')");

                else if (!string.IsNullOrWhiteSpace(request.SearchText))

                    query += " AND (ISNULL(s.FullName, N'') LIKE @Search OR ISNULL(s.StudentNumber, N'') LIKE @Search OR ISNULL(sub.SubjectName, N'') LIKE @Search OR ISNULL(m.ExamType, N'') LIKE @Search)";


                query += " ORDER BY s.FullName, m.CreatedAt DESC, m.MarkID DESC";

                return ExecuteQuery(query, request);
            }


            return CreateMessageTable("لم يتم العثور على جدول درجات معروف مثل Grades أو StudentGrades أو Marks.");
        }

        // تعريف الدالة IsGradeResultStatus لتنفيذ العملية المرتبطة بها.
        private bool IsGradeResultStatus(string status)
        {

            return status == "ناجح" || status == "راسب" || status == "غير مجتاز" || status == "غير مكتمل";
        }

        // تعريف الدالة BuildStudentSelectionFilter لتنفيذ العملية المرتبطة بها.
        private string BuildStudentSelectionFilter(ReportRequest request, string idExpression, string nameExpression, string numberExpression)
        {
            string[] terms = GetStudentSelectionTerms(request.StudentSelection);
            if (terms.Length == 0)
                return string.Empty;

            string filter = " AND (";

            for (int i = 0; i < terms.Length; i++)
            {
                if (i > 0)
                    filter += " OR ";

                filter += "(" + nameExpression + " LIKE @StudentSelection" + i
                    + " OR " + numberExpression + " LIKE @StudentSelection" + i
                    + " OR " + idExpression + " LIKE @StudentSelection" + i + ")";
            }


            return filter + ")";
        }

        // تعريف الدالة GetStudentSelectionTerms لتنفيذ العملية المرتبطة بها.
        private string[] GetStudentSelectionTerms(string selection)
        {
            if (string.IsNullOrWhiteSpace(selection))
                return new string[0];

            string[] rawTerms = selection.Split(new[] { ',', '،', ';', '؛', '|', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> terms = new List<string>();
            foreach (string rawTerm in rawTerms)
            {
                string term = (rawTerm ?? string.Empty).Trim();
                if (term.Length == 0)
                    continue;

                bool exists = false;
                foreach (string existing in terms)
                {
                    if (string.Equals(existing, term, StringComparison.OrdinalIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    terms.Add(term);
            }


            return terms.ToArray();
        }

        // تعريف الدالة GetClassResultsReport لتنفيذ العملية المرتبطة بها.
        private DataTable GetClassResultsReport(ReportRequest request)
        {
            // التقرير التجميعي يستخدم نفس مصدر شاشة إدخال الدرجات، ثم يعود إلى StudentGrades للتوافق الخلفي.
            if (TableExists("Grades"))
            {
                string query = @"
                    WITH RankedGrades AS
                    (
                        SELECT g.*,
                               ROW_NUMBER() OVER
                               (
                                   PARTITION BY g.StudentID, g.SubjectID, g.ClassID,
                                                LTRIM(RTRIM(ISNULL(g.Section, N''))),
                                                LTRIM(RTRIM(ISNULL(g.AcademicYear, N''))),
                                                LTRIM(RTRIM(ISNULL(g.TermName, N'')))
                                   ORDER BY ISNULL(g.UpdatedAt, g.CreatedAt) DESC, g.GradeID DESC
                               ) AS GradeRank
                        FROM Grades g
                    ),
                    GradeRows AS
                    (
                        SELECT
                            g.StudentID,
                            s.StudentNumber,
                            s.FullName,
                            g.ClassID,
                            c.ClassName,
                            LTRIM(RTRIM(ISNULL(g.Section, N''))) AS Section,
                            g.AcademicYear,
                            LTRIM(RTRIM(ISNULL(g.TermName, N''))) AS TermName,
                            g.SubjectID,
                            CAST(COALESCE(g.GradeValue,
                                ISNULL(g.Quiz1, 0) + ISNULL(g.Quiz2, 0) +
                                ISNULL(g.CourseWork, 0) + ISNULL(g.FinalExam, 0)) AS decimal(18,2)) AS Total,
                            g.ResultStatus
                        FROM RankedGrades g
                        INNER JOIN Students s ON g.StudentID = s.StudentID
                            AND ISNULL(s.Status, N'') NOT IN (N'محذوف', N'Deleted')
                        LEFT JOIN Classes c ON g.ClassID = c.ClassID
                        WHERE g.GradeRank = 1
                    )
                    SELECT
                        gr.StudentID AS [رقم الطالب],
                        gr.StudentNumber AS [رقم الملف],
                        gr.FullName AS [اسم الطالب],
                        gr.ClassID AS [رقم الصف],
                        gr.ClassName AS [الصف],
                        gr.Section AS [الشعبة],
                        gr.AcademicYear AS [العام الدراسي],
                        gr.TermName AS [الفصل الدراسي],
                        COUNT(DISTINCT gr.SubjectID) AS [عدد المواد],
                        CAST(SUM(gr.Total) AS decimal(18,2)) AS [المجموع الكلي],
                        CAST(AVG(gr.Total) AS decimal(18,2)) AS [المعدل العام],
                        CASE
                            WHEN SUM(CASE WHEN ISNULL(gr.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل'
                            WHEN SUM(CASE WHEN ISNULL(gr.ResultStatus, N'') IN (N'راسب', N'غير مجتاز')
                                               OR (ISNULL(gr.ResultStatus, N'') = N'' AND gr.Total < 50)
                                         THEN 1 ELSE 0 END) > 0 THEN N'راسب'
                            ELSE N'ناجح'
                        END AS [الحالة النهائية],
                        CASE
                            WHEN AVG(gr.Total) >= 90 THEN N'ممتاز'
                            WHEN AVG(gr.Total) >= 80 THEN N'جيد جداً'
                            WHEN AVG(gr.Total) >= 70 THEN N'جيد'
                            WHEN AVG(gr.Total) >= 60 THEN N'مقبول'
                            ELSE N'راسب'
                        END AS [التقدير العام]
                    FROM GradeRows gr
                    WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                    query += " AND REPLACE(ISNULL(gr.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

                if (request.ClassID.HasValue)
                    query += " AND gr.ClassID = @ClassID";

                if (!string.IsNullOrWhiteSpace(request.Section))
                    query += " AND LTRIM(RTRIM(ISNULL(gr.Section, N''))) = LTRIM(RTRIM(@Section))";

                if (!string.IsNullOrWhiteSpace(request.TermName))
                    query += " AND LTRIM(RTRIM(ISNULL(gr.TermName, N''))) = LTRIM(RTRIM(@TermName))";

                if (request.StudentID.HasValue)
                    query += " AND gr.StudentID = @StudentID";

                else if (!string.IsNullOrWhiteSpace(request.StudentSelection))

                    query += BuildStudentSelectionFilter(request, "CONVERT(nvarchar(30), gr.StudentID)", "ISNULL(gr.FullName, N'')", "ISNULL(gr.StudentNumber, N'')");


                query += " GROUP BY gr.StudentID, gr.StudentNumber, gr.FullName, gr.ClassID, gr.ClassName, gr.Section, gr.AcademicYear, gr.TermName";

                if (IsGradeResultStatus(request.Status))
                {

                    query += " HAVING (CASE WHEN SUM(CASE WHEN ISNULL(gr.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل' WHEN SUM(CASE WHEN ISNULL(gr.ResultStatus, N'') IN (N'راسب', N'غير مجتاز') OR (ISNULL(gr.ResultStatus, N'') = N'' AND gr.Total < 50) THEN 1 ELSE 0 END) > 0 THEN N'راسب' ELSE N'ناجح' END) = @Status";
                }
                query += " ORDER BY [المعدل العام] DESC, [اسم الطالب]";

                return ExecuteQuery(query, request);
            }

            if (!TableExists("StudentGrades"))
                return CreateMessageTable("لم يتم العثور على جدول درجات صالح. يجب أن يوجد Grades أو StudentGrades لتشغيل تقرير نتائج الفصل.");

            string legacyQuery = @"
                SELECT
                    sg.StudentID AS [رقم الطالب],
                    s.StudentNumber AS [رقم الملف],
                    s.FullName AS [اسم الطالب],
                    sg.ClassID AS [رقم الصف],
                    c.ClassName AS [الصف],
                    LTRIM(RTRIM(ISNULL(sg.Section, N''))) AS [الشعبة],
                    sg.AcademicYear AS [العام الدراسي],
                    LTRIM(RTRIM(ISNULL(sg.TermName, N''))) AS [الفصل الدراسي],
                    COUNT(DISTINCT sg.SubjectID) AS [عدد المواد],
                    CAST(SUM(ISNULL(sg.Total, 0)) AS decimal(18,2)) AS [المجموع الكلي],
                    CAST(AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) AS decimal(18,2)) AS [المعدل العام],
                    CASE
                        WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل'
                        WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') IN (N'راسب', N'غير مجتاز') THEN 1 ELSE 0 END) > 0 THEN N'راسب'
                        ELSE N'ناجح'
                    END AS [الحالة النهائية],
                    CASE
                        WHEN AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) >= 90 THEN N'ممتاز'
                        WHEN AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) >= 80 THEN N'جيد جداً'
                        WHEN AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) >= 70 THEN N'جيد'
                        WHEN AVG(CAST(ISNULL(sg.Total, 0) AS decimal(18,2))) >= 60 THEN N'مقبول'
                        ELSE N'راسب'
                    END AS [التقدير العام]
                FROM StudentGrades sg
                INNER JOIN Students s ON s.StudentID = sg.StudentID
                    AND ISNULL(s.Status, N'') NOT IN (N'محذوف', N'Deleted')
                LEFT JOIN Classes c ON c.ClassID = sg.ClassID
                WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(request.AcademicYear))
                legacyQuery += " AND REPLACE(ISNULL(sg.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')";

            if (request.ClassID.HasValue)
                legacyQuery += " AND sg.ClassID = @ClassID";

            if (!string.IsNullOrWhiteSpace(request.Section))
                legacyQuery += " AND LTRIM(RTRIM(ISNULL(sg.Section, N''))) = LTRIM(RTRIM(@Section))";

            if (!string.IsNullOrWhiteSpace(request.TermName))
                legacyQuery += " AND LTRIM(RTRIM(ISNULL(sg.TermName, N''))) = LTRIM(RTRIM(@TermName))";

            if (request.StudentID.HasValue)
                legacyQuery += " AND sg.StudentID = @StudentID";

            else if (!string.IsNullOrWhiteSpace(request.StudentSelection))

                legacyQuery += BuildStudentSelectionFilter(request, "CONVERT(nvarchar(30), s.StudentID)", "ISNULL(s.FullName, N'')", "ISNULL(s.StudentNumber, N'')");


            legacyQuery += " GROUP BY sg.StudentID, s.StudentNumber, s.FullName, sg.ClassID, c.ClassName, sg.Section, sg.AcademicYear, sg.TermName";

            if (IsGradeResultStatus(request.Status))
                legacyQuery += " HAVING (CASE WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') = N'غير مكتمل' THEN 1 ELSE 0 END) > 0 THEN N'غير مكتمل' WHEN SUM(CASE WHEN ISNULL(sg.ResultStatus, N'') IN (N'راسب', N'غير مجتاز') THEN 1 ELSE 0 END) > 0 THEN N'راسب' ELSE N'ناجح' END) = @Status";
            legacyQuery += " ORDER BY [المعدل العام] DESC, [اسم الطالب]";

            return ExecuteQuery(legacyQuery, request);
        }

        // تعريف الدالة ExecuteQuery لتنفيذ العملية المرتبطة بها.
        private DataTable ExecuteQuery(string query, ReportRequest request)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (query.Contains("@FromDate"))
                    cmd.Parameters.AddWithValue("@FromDate", request.FromDate.Date);


                if (query.Contains("@IsIndividualGradeReport"))
                    cmd.Parameters.AddWithValue("@IsIndividualGradeReport", request.ReportType == "كشف درجات طالب" ? 1 : 0);


                if (query.Contains("@ToDate"))
                    cmd.Parameters.AddWithValue("@ToDate", request.ToDate.Date.AddDays(1).AddSeconds(-1));


                if (query.Contains("@AcademicYear"))
                    cmd.Parameters.AddWithValue("@AcademicYear", request.AcademicYear.Trim());


                if (query.Contains("@ClassID") && request.ClassID.HasValue)
                    cmd.Parameters.AddWithValue("@ClassID", request.ClassID.Value);


                if (query.Contains("@StudentID") && request.StudentID.HasValue)
                    cmd.Parameters.AddWithValue("@StudentID", request.StudentID.Value);


                if (query.Contains("@Section"))
                    cmd.Parameters.AddWithValue("@Section", request.Section.Trim());


                if (query.Contains("@Status"))
                    cmd.Parameters.AddWithValue("@Status", request.Status.Trim());


                if (query.Contains("@TermName"))
                    cmd.Parameters.AddWithValue("@TermName", (request.TermName ?? string.Empty).Trim());


                if (query.Contains("@Search"))
                    cmd.Parameters.AddWithValue("@Search", "%" + request.SearchText.Trim() + "%");


                if (query.Contains("@StudentSelection"))
                {
                    string[] terms = GetStudentSelectionTerms(request.StudentSelection);

                    for (int i = 0; i < terms.Length; i++)
                        cmd.Parameters.Add("@StudentSelection" + i, SqlDbType.NVarChar, 200).Value = "%" + terms[i] + "%";
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {

                        adapter.Fill(dt);
                    }

                    catch (Exception ex)
                    {
                        ApplicationLogger.LogException("مستودع " + nameof(ReportRepository) + ": تنفيذ التقرير", ex);
                        return CreateMessageTable("تعذر تنفيذ التقرير: " + ex.Message);
                    }


                    return dt;
                }
            }
        }

        // تعريف الدالة TableExists لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة ColumnExists لتنفيذ العملية المرتبطة بها.
        private bool ColumnExists(string tableName, string columnName)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                      AND COLUMN_NAME = @ColumnName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    cmd.Parameters.AddWithValue("@ColumnName", columnName);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        // تعريف الدالة CreateMessageTable لتنفيذ العملية المرتبطة بها.
        private DataTable CreateMessageTable(string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ملاحظة", typeof(string));
            dt.Rows.Add(message);

            return dt;
        }
    }
}
