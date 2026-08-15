/*
    SchoolSystem - Comprehensive Demo Data
    الهدف: تعبئة بيئة الاختبار ببيانات مترابطة ومنطقية دون تكرار.
    المتطلبات: تشغيل Migration_MissingApplicationTables.sql وMigration_OperationalTables.sql
    وMigration_SeedAcademicCatalog.sql وMigration_SeedClassSections.sql أولاً.
*/
IF DB_ID(N'SchoolDB') IS NULL
    THROW 51002, N'قاعدة SchoolDB غير موجودة. شغل سكربت إنشاء القاعدة أولاً.', 1;
GO
USE SchoolDB;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @AcademicYear NVARCHAR(20) = N'1447-1448';
DECLARE @Today DATE = CONVERT(date, GETDATE());

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.Students', N'U') IS NULL OR OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
        THROW 51000, N'يجب تشغيل ترحيلات الطلاب والمعلمين أولاً.', 1;

    /* معلمون تجريبيون موزعون على المواد الأساسية. */
    INSERT INTO dbo.Teachers
        (EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary, Status, Notes)
    SELECT v.EmployeeNumber, v.FullName, v.Gender, v.Nationality, v.Phone, v.Email, v.Qualification, v.Specialization,
           v.HireDate, v.BasicSalary, N'نشط', N'بيانات تجريبية شاملة'
    FROM (VALUES
        (N'DEMO-TCH-004', N'مريم صالح النعماني', N'أنثى', N'يمنية', N'777100004', N'maryam.demo@schoolsystem.local', N'بكالوريوس تربية', N'اللغة العربية', CONVERT(date,'2021-09-01'), 870),
        (N'DEMO-TCH-005', N'علي يحيى القاسمي', N'ذكر', N'يمني', N'777100005', N'ali.demo@schoolsystem.local', N'ماجستير رياضيات', N'الرياضيات', CONVERT(date,'2019-09-01'), 1100),
        (N'DEMO-TCH-006', N'نجلاء حسن الشامي', N'أنثى', N'يمنية', N'777100006', N'najla.demo@schoolsystem.local', N'بكالوريوس علوم', N'الكيمياء', CONVERT(date,'2022-09-01'), 930),
        (N'DEMO-TCH-007', N'سمير عبدالرحمن اليافعي', N'ذكر', N'يمني', N'777100007', N'samir.demo@schoolsystem.local', N'بكالوريوس حاسوب', N'الحاسوب', CONVERT(date,'2020-09-01'), 960),
        (N'DEMO-TCH-008', N'هدى محمد الوصابي', N'أنثى', N'يمنية', N'777100008', N'huda.demo@schoolsystem.local', N'بكالوريوس لغة', N'اللغة الإنجليزية', CONVERT(date,'2023-09-01'), 840),
        (N'DEMO-TCH-009', N'فهد أحمد المذعوري', N'ذكر', N'يمني', N'777100009', N'fahad.demo@schoolsystem.local', N'ماجستير علوم', N'الأحياء', CONVERT(date,'2018-09-01'), 1150)
    ) v(EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Teachers t WHERE t.EmployeeNumber = v.EmployeeNumber);

    /* طلاب تجريبيون: أربعة طلاب لكل شعبة من الشعب الجديدة. */
    INSERT INTO dbo.Students
        (StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, NationalId, StudentPhone, Address, GuardianName, GuardianPhone, GuardianEmail, Status)
    SELECT v.StudentNumber, v.FullName, v.Gender, v.BirthDate, N'صنعاء', N'يمني', v.NationalID, v.Phone,
           N'حي المدارس', v.GuardianName, v.GuardianPhone, v.Email, N'نشط'
    FROM (VALUES
        (N'DEMO-STU-005', N'ياسر عبدالله حسن', N'ذكر', CONVERT(date,'2010-02-14'), N'1000000005', N'777200005', N'stu005@schoolsystem.local', N'عبدالله حسن', N'777300005'),
        (N'DEMO-STU-006', N'محمد فؤاد علي', N'ذكر', CONVERT(date,'2010-04-20'), N'1000000006', N'777200006', N'stu006@schoolsystem.local', N'فؤاد علي', N'777300006'),
        (N'DEMO-STU-007', N'ريم خالد صالح', N'أنثى', CONVERT(date,'2010-06-09'), N'1000000007', N'777200007', N'stu007@schoolsystem.local', N'خالد صالح', N'777300007'),
        (N'DEMO-STU-008', N'سلمى أحمد ناصر', N'أنثى', CONVERT(date,'2010-08-11'), N'1000000008', N'777200008', N'stu008@schoolsystem.local', N'أحمد ناصر', N'777300008'),
        (N'DEMO-STU-009', N'إبراهيم علي قاسم', N'ذكر', CONVERT(date,'2009-03-18'), N'1000000009', N'777200009', N'stu009@schoolsystem.local', N'علي قاسم', N'777300009'),
        (N'DEMO-STU-010', N'نور محمد سعيد', N'أنثى', CONVERT(date,'2009-05-22'), N'1000000010', N'777200010', N'stu010@schoolsystem.local', N'محمد سعيد', N'777300010'),
        (N'DEMO-STU-011', N'حسام عبدالملك عوض', N'ذكر', CONVERT(date,'2009-07-03'), N'1000000011', N'777200011', N'stu011@schoolsystem.local', N'عبدالملك عوض', N'777300011'),
        (N'DEMO-STU-012', N'أروى يحيى غالب', N'أنثى', CONVERT(date,'2009-09-17'), N'1000000012', N'777200012', N'stu012@schoolsystem.local', N'يحيى غالب', N'777300012'),
        (N'DEMO-STU-013', N'مازن عبدالله سالم', N'ذكر', CONVERT(date,'2008-01-12'), N'1000000013', N'777200013', N'stu013@schoolsystem.local', N'عبدالله سالم', N'777300013'),
        (N'DEMO-STU-014', N'ليان صالح أحمد', N'أنثى', CONVERT(date,'2008-03-26'), N'1000000014', N'777200014', N'stu014@schoolsystem.local', N'صالح أحمد', N'777300014'),
        (N'DEMO-STU-015', N'عبدالرحمن فهد حسين', N'ذكر', CONVERT(date,'2008-05-30'), N'1000000015', N'777200015', N'stu015@schoolsystem.local', N'فهد حسين', N'777300015'),
        (N'DEMO-STU-016', N'غادة محمد عبدالله', N'أنثى', CONVERT(date,'2008-08-08'), N'1000000016', N'777200016', N'stu016@schoolsystem.local', N'محمد عبدالله', N'777300016'),
        (N'DEMO-STU-017', N'طارق حسن قاسم', N'ذكر', CONVERT(date,'2007-02-19'), N'1000000017', N'777200017', N'stu017@schoolsystem.local', N'حسن قاسم', N'777300017'),
        (N'DEMO-STU-018', N'إيمان علي يحيى', N'أنثى', CONVERT(date,'2007-04-15'), N'1000000018', N'777200018', N'stu018@schoolsystem.local', N'علي يحيى', N'777300018'),
        (N'DEMO-STU-019', N'سعيد أحمد حسن', N'ذكر', CONVERT(date,'2007-06-27'), N'1000000019', N'777200019', N'stu019@schoolsystem.local', N'أحمد حسن', N'777300019'),
        (N'DEMO-STU-020', N'سارة فؤاد محمد', N'أنثى', CONVERT(date,'2007-09-05'), N'1000000020', N'777200020', N'stu020@schoolsystem.local', N'فؤاد محمد', N'777300020')
    ) v(StudentNumber, FullName, Gender, BirthDate, NationalID, Phone, Email, GuardianName, GuardianPhone)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Students s WHERE s.StudentNumber = v.StudentNumber);

    DECLARE @Class1 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassCode = N'G10' OR ClassName = N'الأول الثانوي' ORDER BY ClassID);
    DECLARE @Class2 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassCode = N'G11' OR ClassName = N'الثاني الثانوي' ORDER BY ClassID);
    DECLARE @Class3 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassCode = N'G12' OR ClassName = N'الثالث الثانوي' ORDER BY ClassID);
    IF @Class1 IS NULL OR @Class2 IS NULL OR @Class3 IS NULL
        THROW 51001, N'لم يتم العثور على الصفوف الثلاثة. شغل Migration_SeedAcademicCatalog.sql أولاً.', 1;

    /* ربط الطلاب بالشعب: 005-008 ألف، 009-012 باء، 013-016 جيم، 017-020 دال. */
    INSERT INTO dbo.StudentClasses (StudentID, ClassID, Section, AcademicYear)
    SELECT s.StudentID, x.ClassID, x.Section, @AcademicYear
    FROM (VALUES
        (N'DEMO-STU-005', @Class1, N'أ'), (N'DEMO-STU-006', @Class1, N'أ'), (N'DEMO-STU-007', @Class1, N'أ'), (N'DEMO-STU-008', @Class1, N'أ'),
        (N'DEMO-STU-009', @Class1, N'ب'), (N'DEMO-STU-010', @Class1, N'ب'), (N'DEMO-STU-011', @Class1, N'ب'), (N'DEMO-STU-012', @Class1, N'ب'),
        (N'DEMO-STU-013', @Class2, N'ج'), (N'DEMO-STU-014', @Class2, N'ج'), (N'DEMO-STU-015', @Class2, N'ج'), (N'DEMO-STU-016', @Class2, N'ج'),
        (N'DEMO-STU-017', @Class3, N'د'), (N'DEMO-STU-018', @Class3, N'د'), (N'DEMO-STU-019', @Class3, N'د'), (N'DEMO-STU-020', @Class3, N'د')
    ) x(StudentNumber, ClassID, Section)
    INNER JOIN dbo.Students s ON s.StudentNumber = x.StudentNumber
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.StudentClasses sc
        WHERE sc.StudentID = s.StudentID AND sc.AcademicYear = @AcademicYear
    );

    /* تسجيلات قبول مرتبطة بالطلاب والشعب عند توفر جدول التسجيل. */
    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Enrollments
            (StudentID, ApplicationDate, ApplicationType, AcademicYear, ClassID, Section, SeatNumber, Status,
             RegistrationFee, PaidAmount, PaymentMethod, HasBirthCertificate, HasGuardianId, HasPhoto, HasLastCertificate, HasMedicalReport, GeneralNotes)
        SELECT s.StudentID, DATEADD(day, -ROW_NUMBER() OVER (ORDER BY s.StudentID), @Today), N'قبول جديد', @AcademicYear,
               sc.ClassID, sc.Section, N'SEAT-' + RIGHT(s.StudentNumber, 3), N'مقبول', 150, 150, N'نقدي', 1, 1, 1, 1, 1, N'ملف تجريبي مكتمل'
        FROM dbo.Students s
        INNER JOIN dbo.StudentClasses sc ON sc.StudentID = s.StudentID AND sc.AcademicYear = @AcademicYear
        WHERE s.StudentNumber LIKE N'DEMO-STU-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.Enrollments e WHERE e.StudentID = s.StudentID AND e.AcademicYear = @AcademicYear);
    END;

    /* درجات وحضور للطلاب المضافين. */
    IF OBJECT_ID(N'dbo.Marks', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType)
        SELECT s.StudentID, sub.SubjectID, t.TeacherID,
               CONVERT(decimal(10,2), 68 + (ABS(CHECKSUM(s.StudentNumber + CONVERT(nvarchar(10), sub.SubjectID))) % 29)), N'اختبار منتصف الفصل'
        FROM dbo.Students s
        INNER JOIN dbo.StudentClasses sc ON sc.StudentID = s.StudentID AND sc.AcademicYear = @AcademicYear
        CROSS JOIN (SELECT TOP 4 SubjectID FROM dbo.Subjects ORDER BY SubjectID) sub
        OUTER APPLY (SELECT TOP 1 TeacherID FROM dbo.Teachers WHERE EmployeeNumber LIKE N'DEMO-TCH-0%' ORDER BY TeacherID) t
        WHERE s.StudentNumber LIKE N'DEMO-STU-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.Marks m WHERE m.StudentID = s.StudentID AND m.SubjectID = sub.SubjectID AND m.ExamType = N'اختبار منتصف الفصل');
    END;

    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.StudentAttendance (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, ExcuseStatus, Notes)
        SELECT s.StudentID, sc.ClassID,
               COALESCE(NULLIF(sc.Section, N''),
                   CASE
                       WHEN TRY_CONVERT(INT, RIGHT(s.StudentNumber, 3)) BETWEEN 5 AND 8 THEN N'أ'
                       WHEN TRY_CONVERT(INT, RIGHT(s.StudentNumber, 3)) BETWEEN 9 AND 12 THEN N'ب'
                       WHEN TRY_CONVERT(INT, RIGHT(s.StudentNumber, 3)) BETWEEN 13 AND 16 THEN N'ج'
                       WHEN TRY_CONVERT(INT, RIGHT(s.StudentNumber, 3)) BETWEEN 17 AND 20 THEN N'د'
                       ELSE N'أ'
                   END),
               @AcademicYear, DATEADD(day, -d.DayOffset, @Today),
               CASE WHEN (s.StudentID + d.DayOffset) % 10 = 0 THEN N'غائب' WHEN (s.StudentID + d.DayOffset) % 7 = 0 THEN N'متأخر' ELSE N'حاضر' END,
               CASE WHEN (s.StudentID + d.DayOffset) % 10 = 0 THEN NULL ELSE CONVERT(time, '07:1' + CONVERT(varchar(1), (s.StudentID + d.DayOffset) % 9) + ':00') END,
               CASE WHEN (s.StudentID + d.DayOffset) % 10 = 0 THEN N'بدون عذر' ELSE N'لا ينطبق' END,
               N'سجل حضور تجريبي'
        FROM dbo.Students s
        INNER JOIN dbo.StudentClasses sc ON sc.StudentID = s.StudentID AND sc.AcademicYear = @AcademicYear
        CROSS JOIN (VALUES (1), (2), (3)) d(DayOffset)
        WHERE s.StudentNumber LIKE N'DEMO-STU-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance a WHERE a.StudentID = s.StudentID AND a.AttendanceDate = DATEADD(day, -d.DayOffset, @Today));
    END;

    /* عقود ورواتب وحضور المعلمين. */
    IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.TeacherContracts
            (TeacherID, ContractNumber, ContractType, ContractStatus, BasicSalary, HousingAllowance, TransportAllowance, OtherAllowances, Deductions, StartDate, PaymentMethod, Notes)
        SELECT t.TeacherID, N'DEMO-CON-' + RIGHT(t.EmployeeNumber, 3), N'دوام كامل', N'ساري', t.BasicSalary, 150, 75, 25, 0, '2024-09-01', N'تحويل بنكي', N'عقد تجريبي'
        FROM dbo.Teachers t
        WHERE t.EmployeeNumber LIKE N'DEMO-TCH-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.TeacherContracts c WHERE c.TeacherID = t.TeacherID AND c.ContractNumber = N'DEMO-CON-' + RIGHT(t.EmployeeNumber, 3));
    END;

    IF OBJECT_ID(N'dbo.Payroll', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Payroll (TeacherID, SalaryMonth, SalaryYear, BasicSalary, Allowances, Deductions, PaymentDate, Notes)
        SELECT t.TeacherID, MONTH(@Today), YEAR(@Today), t.BasicSalary, 250, 25, @Today, N'راتب تجريبي'
        FROM dbo.Teachers t
        WHERE t.EmployeeNumber LIKE N'DEMO-TCH-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.Payroll p WHERE p.TeacherID = t.TeacherID AND p.SalaryMonth = MONTH(@Today) AND p.SalaryYear = YEAR(@Today));
    END;

    IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.TeacherAttendance (TeacherID, AttendanceDate, Status, LateMinutes, EarlyLeaveMinutes, WorkHours, Notes)
        SELECT t.TeacherID, DATEADD(day, -d.DayOffset, @Today), CASE WHEN (t.TeacherID + d.DayOffset) % 8 = 0 THEN N'إجازة' ELSE N'حاضر' END,
               CASE WHEN (t.TeacherID + d.DayOffset) % 8 = 0 THEN 0 ELSE 5 END, 0, 7.5, N'حضور معلم تجريبي'
        FROM dbo.Teachers t CROSS JOIN (VALUES (1), (2), (3)) d(DayOffset)
        WHERE t.EmployeeNumber LIKE N'DEMO-TCH-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.TeacherAttendance a WHERE a.TeacherID = t.TeacherID AND a.AttendanceDate = DATEADD(day, -d.DayOffset, @Today));
    END;

    /* جدول دراسي يربط المعلمين بالمواد والشعب. */
    IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.SchoolTimetable
            (ClassID, Section, SubjectID, TeacherID, AcademicYear, TermName, DayName, PeriodNo, StartTime, EndTime, RoomName, Notes, IsActive)
        SELECT x.ClassID, x.Section, sub.SubjectID, t.TeacherID, @AcademicYear, N'الفصل الأول', x.DayName, x.PeriodNo,
               DATEADD(MINUTE, (x.PeriodNo - 1) * 45, CAST('08:00' AS TIME)),
               DATEADD(MINUTE, x.PeriodNo * 45, CAST('08:00' AS TIME)),
               x.RoomName, N'حصة تجريبية مرتبطة بالشعبة', 1
        FROM (VALUES
            (@Class1, N'أ', N'الأحد', 1, N'قاعة 101', N'DEMO-TCH-005'), (@Class1, N'ب', N'الاثنين', 2, N'قاعة 102', N'DEMO-TCH-004'),
            (@Class2, N'ج', N'الثلاثاء', 3, N'مختبر العلوم', N'DEMO-TCH-006'), (@Class3, N'د', N'الأربعاء', 4, N'مختبر الحاسوب', N'DEMO-TCH-007')
        ) x(ClassID, Section, DayName, PeriodNo, RoomName, EmployeeNumber)
        INNER JOIN dbo.Teachers t ON t.EmployeeNumber = x.EmployeeNumber
        CROSS APPLY (SELECT TOP 1 SubjectID FROM dbo.Subjects ORDER BY SubjectID) sub
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.SchoolTimetable st
            WHERE st.ClassID = x.ClassID AND ISNULL(st.Section, N'') = x.Section AND st.TeacherID = t.TeacherID
              AND st.AcademicYear = @AcademicYear AND REPLACE(st.DayName, N'الإثنين', N'الاثنين') = x.DayName AND st.PeriodNo = x.PeriodNo
        );
    END;

    /* بيانات مالية للطلاب والمعلمين. */
    IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.FeePlans (AcademicYear, ClassID, FeeType, Amount, DueDate, IsRequired, Notes)
        SELECT @AcademicYear, c.ClassID, v.FeeType, v.Amount, DATEADD(day, v.DaysToDue, @Today), 1, N'خطة تجريبية'
        FROM (VALUES (N'رسوم دراسية', 1200, 30), (N'نشاط مدرسي', 150, 45), (N'نقل مدرسي', 300, 60)) v(FeeType, Amount, DaysToDue)
        CROSS JOIN (VALUES (@Class1), (@Class2), (@Class3)) c(ClassID)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.FeePlans fp WHERE fp.AcademicYear = @AcademicYear AND fp.ClassID = c.ClassID AND fp.FeeType = v.FeeType);
    END;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Fees
            (StudentID, AcademicYear, FeeType, TotalAmount, DiscountAmount, NetAmount, PaidAmount, RemainingAmount, DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes)
        SELECT s.StudentID, @AcademicYear, N'رسوم دراسية', 1200, 100, 1100, CASE WHEN s.StudentID % 2 = 0 THEN 1100 ELSE 500 END,
               CASE WHEN s.StudentID % 2 = 0 THEN 0 ELSE 600 END, DATEADD(day, 30, @Today), CASE WHEN s.StudentID % 2 = 0 THEN @Today ELSE NULL END,
               CASE WHEN s.StudentID % 2 = 0 THEN N'نقدي' ELSE NULL END, CASE WHEN s.StudentID % 2 = 0 THEN N'DEMO-RCP-' + RIGHT(s.StudentNumber,3) ELSE NULL END,
               CASE WHEN s.StudentID % 2 = 0 THEN N'مدفوع' ELSE N'جزئي' END, N'رسوم تجريبية'
        FROM dbo.Students s
        WHERE s.StudentNumber LIKE N'DEMO-STU-0%'
          AND NOT EXISTS (SELECT 1 FROM dbo.Fees f WHERE f.StudentID = s.StudentID AND f.AcademicYear = @AcademicYear AND f.FeeType = N'رسوم دراسية');
    END;

    IF OBJECT_ID(N'dbo.Receipts', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Receipts (ReceiptNumber, StudentID, Amount, ReceiptDate, PaymentMethod, Description, Notes)
        SELECT N'DEMO-RCP-' + RIGHT(s.StudentNumber, 3), s.StudentID, 500, @Today, N'نقدي', N'دفعة رسوم دراسية', N'إيصال تجريبي'
        FROM dbo.Students s
        WHERE s.StudentNumber LIKE N'DEMO-STU-0%' AND s.StudentID % 2 = 1
          AND NOT EXISTS (SELECT 1 FROM dbo.Receipts r WHERE r.ReceiptNumber = N'DEMO-RCP-' + RIGHT(s.StudentNumber, 3));
    END;

    IF OBJECT_ID(N'dbo.Expenses', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Expenses (ExpenseNumber, Amount, ExpenseDate, Category, PayeeName, PaymentMethod, Description, Notes)
        SELECT v.ExpenseNumber, v.Amount, DATEADD(day, -v.DayOffset, @Today), v.Category, v.Payee, N'تحويل بنكي', v.Description, N'مصروف تجريبي'
        FROM (VALUES
            (N'DEMO-EXP-001', 450, 3, N'صيانة', N'مؤسسة الصيانة الحديثة', N'صيانة مختبر العلوم'),
            (N'DEMO-EXP-002', 280, 7, N'مستلزمات تعليمية', N'مكتبة المعرفة', N'شراء أدوات مدرسية'),
            (N'DEMO-EXP-003', 600, 12, N'نقل', N'شركة النقل المدرسي', N'وقود الحافلات')
        ) v(ExpenseNumber, Amount, DayOffset, Category, Payee, Description)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Expenses e WHERE e.ExpenseNumber = v.ExpenseNumber);
    END;

    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Vouchers (VoucherNumber, VoucherType, Amount, VoucherDate, PartyName, Description, PaymentMethod, ReferenceType, Notes)
        SELECT v.VoucherNumber, v.VoucherType, v.Amount, DATEADD(day, -v.DayOffset, @Today), v.PartyName, v.Description, N'نقدي', v.ReferenceType, N'سند تجريبي'
        FROM (VALUES
            (N'DEMO-VCH-REC-001', N'قبض', 500, 2, N'ولي أمر تجريبي', N'تحصيل رسوم دراسية', N'رسوم'),
            (N'DEMO-VCH-PAY-001', N'صرف', 450, 3, N'مؤسسة الصيانة الحديثة', N'سداد صيانة', N'مصروف')
        ) v(VoucherNumber, VoucherType, Amount, DayOffset, PartyName, Description, ReferenceType)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Vouchers x WHERE x.VoucherNumber = v.VoucherNumber);
    END;

    /* المكتبة والغرف عند توفر الجداول. */
    IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Books (Title, Author, ISBN, Category, Publisher, PublicationYear, Copies, AvailableCopies, ShelfLocation, Notes, IsActive)
        SELECT v.Title, v.Author, v.ISBN, v.Category, v.Publisher, v.PublicationYear, v.Copies, v.Copies, v.ShelfLocation, N'كتاب تجريبي', 1
        FROM (VALUES
            (N'أساسيات الرياضيات', N'قسم الرياضيات', N'DEMO-ISBN-001', N'رياضيات', N'المناهج المدرسية', 2024, 10, N'A-01'),
            (N'مدخل إلى الفيزياء', N'قسم العلوم', N'DEMO-ISBN-002', N'فيزياء', N'المناهج المدرسية', 2024, 8, N'A-02'),
            (N'اللغة العربية للثانوي', N'قسم اللغة العربية', N'DEMO-ISBN-003', N'لغة عربية', N'المناهج المدرسية', 2024, 12, N'B-01')
        ) v(Title, Author, ISBN, Category, Publisher, PublicationYear, Copies, ShelfLocation)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Books b WHERE b.ISBN = v.ISBN);
    END;

    IF OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Rooms (RoomCode, RoomName, RoomType, Capacity, Location, IsActive, Notes)
        SELECT v.RoomCode, v.RoomName, v.RoomType, v.Capacity, N'المبنى الرئيسي', 1, N'قاعة تجريبية'
        FROM (VALUES (N'R-101', N'قاعة 101', N'فصل دراسي', 30, N'الجناح الشرقي'), (N'LAB-01', N'مختبر العلوم', N'مختبر', 20, N'الجناح العلمي')) v(RoomCode, RoomName, RoomType, Capacity, Location)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Rooms r WHERE r.RoomCode = v.RoomCode);
    END;

    COMMIT TRANSACTION;
    SELECT N'اكتملت تعبئة البيانات التجريبية الشاملة.' AS ResultMessage;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* استعلامات تحقق مختصرة */
SELECT N'Students' AS Entity, COUNT(*) AS DemoCount FROM dbo.Students WHERE StudentNumber LIKE N'DEMO-STU-%'
UNION ALL SELECT N'Teachers', COUNT(*) FROM dbo.Teachers WHERE EmployeeNumber LIKE N'DEMO-TCH-%'
UNION ALL SELECT N'StudentClasses', COUNT(*) FROM dbo.StudentClasses sc INNER JOIN dbo.Students s ON s.StudentID = sc.StudentID WHERE s.StudentNumber LIKE N'DEMO-STU-%'
UNION ALL SELECT N'Marks', COUNT(*) FROM dbo.Marks m INNER JOIN dbo.Students s ON s.StudentID = m.StudentID WHERE s.StudentNumber LIKE N'DEMO-STU-%';
GO

