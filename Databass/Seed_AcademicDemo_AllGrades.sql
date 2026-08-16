/*
    SchoolSystem - Academic demo seed for all grades.
    يضيف بيانات تجريبية مترابطة من الأول الإعدادي حتى الثالث الثانوي.
    آمن للتشغيل المتكرر: يستخدم معرفات DEMO-* ولا يحذف بيانات الأمان أو البيانات الحقيقية.
    الترتيب الموصى به:
      1) Migration_MissingApplicationTables.sql
      2) Migration_SeedAcademicCatalog.sql
      3) Migration_CreateSchoolSections.sql
      4) هذا الملف
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 52000, N'قاعدة SchoolDB غير موجودة.', 1;
GO
USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Subjects', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Students', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
   OR OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
   OR OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
    THROW 52001, N'شغّل ترحيلات الجداول والكتالوج والشعب أولاً.', 1;

IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL
    THROW 52002, N'عمود Section غير موجود في StudentClasses. شغّل Migration_MissingApplicationTables.sql.', 1;

DECLARE @AcademicYear NVARCHAR(20) = N'1447-1448';
DECLARE @Today DATE = CONVERT(date, GETDATE());

BEGIN TRY
    BEGIN TRANSACTION;

    /* معلمون تجريبيون ثابتون للدرجات والجداول والتقارير. */
    INSERT INTO dbo.Teachers
    (
        EmployeeNumber, FullName, Gender, Nationality, Phone, Email,
        Qualification, Specialization, HireDate, BasicSalary, Status, Notes
    )
    SELECT v.EmployeeNumber, v.FullName, v.Gender, v.Nationality, v.Phone, v.Email,
           v.Qualification, v.Specialization, v.HireDate, v.BasicSalary, N'نشط', N'بيانات تجريبية أكاديمية'
    FROM (VALUES
        (N'DEMO-TCH-101', N'أحمد محمد العريقي', N'ذكر', N'يمني', N'777510101', N'tch101@schoolsystem.local', N'بكالوريوس تربية', N'الرياضيات', CONVERT(date,'2021-09-01'), 850),
        (N'DEMO-TCH-102', N'سارة علي الحكيمي', N'أنثى', N'يمنية', N'777510102', N'tch102@schoolsystem.local', N'بكالوريوس لغة', N'اللغة الإنجليزية', CONVERT(date,'2022-09-01'), 900),
        (N'DEMO-TCH-103', N'خالد عبدالله الصلوي', N'ذكر', N'يمني', N'777510103', N'tch103@schoolsystem.local', N'ماجستير علوم', N'الفيزياء', CONVERT(date,'2020-09-01'), 980),
        (N'DEMO-TCH-104', N'مريم صالح النعماني', N'أنثى', N'يمنية', N'777510104', N'tch104@schoolsystem.local', N'بكالوريوس تربية', N'اللغة العربية', CONVERT(date,'2021-09-01'), 870),
        (N'DEMO-TCH-105', N'علي يحيى القاسمي', N'ذكر', N'يمني', N'777510105', N'tch105@schoolsystem.local', N'ماجستير رياضيات', N'الرياضيات', CONVERT(date,'2019-09-01'), 1100),
        (N'DEMO-TCH-106', N'نجلاء حسن الشامي', N'أنثى', N'يمنية', N'777510106', N'tch106@schoolsystem.local', N'بكالوريوس علوم', N'الكيمياء', CONVERT(date,'2022-09-01'), 930)
    ) v(EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Teachers t WHERE t.EmployeeNumber = v.EmployeeNumber
    );

    DECLARE @Students TABLE
    (
        StudentNumber NVARCHAR(30) NOT NULL PRIMARY KEY,
        FullName NVARCHAR(200) NOT NULL,
        Gender NVARCHAR(20) NOT NULL,
        BirthDate DATE NOT NULL,
        ClassCode NVARCHAR(30) NOT NULL,
        SectionName NVARCHAR(50) NOT NULL,
        SeatNumber NVARCHAR(20) NOT NULL
    );

    INSERT INTO @Students
    (StudentNumber, FullName, Gender, BirthDate, ClassCode, SectionName, SeatNumber)
    VALUES
        (N'DEMO-ALL-001', N'محمد أحمد صالح', N'ذكر', '2012-02-14', N'PREP-01', N'Section A', N'SEAT-001'),
        (N'DEMO-ALL-002', N'ريم عبدالله حسن', N'أنثى', '2012-05-22', N'PREP-01', N'Section B', N'SEAT-002'),
        (N'DEMO-ALL-003', N'عمر خالد القحطاني', N'ذكر', '2012-08-05', N'PREP-01', N'Section C', N'SEAT-003'),
        (N'DEMO-ALL-004', N'ليان محمد يحيى', N'أنثى', '2012-11-18', N'PREP-01', N'Section D', N'SEAT-004'),
        (N'DEMO-ALL-005', N'ياسر عبدالله حسن', N'ذكر', '2011-02-14', N'PREP-02', N'Section A', N'SEAT-005'),
        (N'DEMO-ALL-006', N'سلمى أحمد ناصر', N'أنثى', '2011-05-22', N'PREP-02', N'Section B', N'SEAT-006'),
        (N'DEMO-ALL-007', N'إبراهيم علي قاسم', N'ذكر', '2011-08-05', N'PREP-02', N'Section C', N'SEAT-007'),
        (N'DEMO-ALL-008', N'نور محمد سعيد', N'أنثى', '2011-11-18', N'PREP-02', N'Section D', N'SEAT-008'),
        (N'DEMO-ALL-009', N'حسام عبدالملك عوض', N'ذكر', '2010-02-14', N'PREP-03', N'Section A', N'SEAT-009'),
        (N'DEMO-ALL-010', N'أروى يحيى غالب', N'أنثى', '2010-05-22', N'PREP-03', N'Section B', N'SEAT-010'),
        (N'DEMO-ALL-011', N'مازن عبدالله سالم', N'ذكر', '2010-08-05', N'PREP-03', N'Section C', N'SEAT-011'),
        (N'DEMO-ALL-012', N'غادة محمد عبدالله', N'أنثى', '2010-11-18', N'PREP-03', N'Section D', N'SEAT-012'),
        (N'DEMO-ALL-013', N'طارق حسن قاسم', N'ذكر', '2009-02-19', N'SEC-01', N'Section A', N'SEAT-013'),
        (N'DEMO-ALL-014', N'إيمان علي يحيى', N'أنثى', '2009-04-15', N'SEC-01', N'Section B', N'SEAT-014'),
        (N'DEMO-ALL-015', N'سعيد أحمد حسن', N'ذكر', '2009-06-27', N'SEC-01', N'Section C', N'SEAT-015'),
        (N'DEMO-ALL-016', N'سارة فؤاد محمد', N'أنثى', '2009-09-05', N'SEC-01', N'Section D', N'SEAT-016'),
        (N'DEMO-ALL-017', N'عبدالرحمن فهد حسين', N'ذكر', '2008-02-19', N'SEC-02', N'Section A', N'SEAT-017'),
        (N'DEMO-ALL-018', N'غادة محمد عبدالله', N'أنثى', '2008-04-15', N'SEC-02', N'Section B', N'SEAT-018'),
        (N'DEMO-ALL-019', N'فهد أحمد المذعوري', N'ذكر', '2008-06-27', N'SEC-02', N'Section C', N'SEAT-019'),
        (N'DEMO-ALL-020', N'هدى محمد الوصابي', N'أنثى', '2008-09-05', N'SEC-02', N'Section D', N'SEAT-020'),
        (N'DEMO-ALL-021', N'نبيل أحمد سالم', N'ذكر', '2007-02-19', N'SEC-03', N'Section A', N'SEAT-021'),
        (N'DEMO-ALL-022', N'أماني علي حسن', N'أنثى', '2007-04-15', N'SEC-03', N'Section B', N'SEAT-022'),
        (N'DEMO-ALL-023', N'رامي خالد صالح', N'ذكر', '2007-06-27', N'SEC-03', N'Section C', N'SEAT-023'),
        (N'DEMO-ALL-024', N'مها محمد قاسم', N'أنثى', '2007-09-05', N'SEC-03', N'Section D', N'SEAT-024');

    INSERT INTO dbo.Students
    (
        StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality,
        NationalId, StudentPhone, Status, GuardianName, GuardianRelation,
        GuardianPhone, GuardianEmail, Governorate, District, Address
    )
    SELECT s.StudentNumber, s.FullName, s.Gender, s.BirthDate, N'صنعاء',
           CASE WHEN s.Gender = N'أنثى' THEN N'يمنية' ELSE N'يمني' END,
           N'DEMO-ID-' + RIGHT(s.StudentNumber, 3),
           N'77752' + RIGHT(s.StudentNumber, 4), N'نشط',
           N'ولي أمر ' + s.FullName, N'والد',
           N'77753' + RIGHT(s.StudentNumber, 4),
           LOWER(s.StudentNumber) + N'@schoolsystem.local', N'أمانة العاصمة',
           N'التحرير', N'حي المدارس التجريبي'
    FROM @Students s
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Students x WHERE x.StudentNumber = s.StudentNumber
    );

    /* ربط كل طالب بصفه وشعبته الإنجليزية للعام الدراسي. */
    UPDATE sc
       SET sc.ClassID = c.ClassID,
           sc.Section = s.SectionName,
           sc.AcademicYear = @AcademicYear,
           sc.AssignedDate = GETDATE()
    FROM dbo.StudentClasses sc
    INNER JOIN dbo.Students st ON st.StudentID = sc.StudentID
    INNER JOIN @Students s ON s.StudentNumber = st.StudentNumber
    INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
    WHERE sc.AcademicYear = @AcademicYear;

    INSERT INTO dbo.StudentClasses (StudentID, ClassID, Section, AcademicYear, AssignedDate)
    SELECT st.StudentID, c.ClassID, s.SectionName, @AcademicYear, GETDATE()
    FROM @Students s
    INNER JOIN dbo.Students st ON st.StudentNumber = s.StudentNumber
    INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.StudentClasses sc
        WHERE sc.StudentID = st.StudentID AND sc.AcademicYear = @AcademicYear
    );

    /* إبقاء الأعمدة المرآتية في Students متوافقة مع شاشة الطلاب إن كانت موجودة. */
    IF COL_LENGTH(N'dbo.Students', N'ClassID') IS NOT NULL
       AND COL_LENGTH(N'dbo.Students', N'Section') IS NOT NULL
       AND COL_LENGTH(N'dbo.Students', N'AcademicYear') IS NOT NULL
    BEGIN
        UPDATE st
           SET st.ClassID = c.ClassID,
               st.Section = s.SectionName,
               st.AcademicYear = @AcademicYear
        FROM dbo.Students st
        INNER JOIN @Students s ON s.StudentNumber = st.StudentNumber
        INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode;
    END;

    /* تسجيلات قبول واحدة لكل طالب/عام دراسي. */
    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Enrollments
        (
            StudentID, ApplicationDate, ApplicationType, AcademicYear, ClassID,
            Section, SeatNumber, Status, RegistrationFee, PaidAmount, PaymentMethod,
            HasBirthCertificate, HasGuardianId, HasPhoto, HasLastCertificate,
            HasMedicalReport, GeneralNotes
        )
        SELECT st.StudentID, DATEADD(day, -ROW_NUMBER() OVER (ORDER BY st.StudentID), @Today),
               N'قبول جديد', @AcademicYear, c.ClassID, s.SectionName, s.SeatNumber,
               N'مقبول', 150, 150, N'نقدي', 1, 1, 1, 1, 1, N'ملف تجريبي مكتمل'
        FROM @Students s
        INNER JOIN dbo.Students st ON st.StudentNumber = s.StudentNumber
        INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.Enrollments e
            WHERE e.StudentID = st.StudentID AND e.AcademicYear = @AcademicYear
        );
    END;

    /* درجتان لكل طالب لضمان ظهور بيانات في شاشة الدرجات والتقارير. */
    IF OBJECT_ID(N'dbo.Marks', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType)
        SELECT st.StudentID, sub.SubjectID,
               (SELECT TOP (1) TeacherID FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-101'),
               CONVERT(decimal(5,2), 70 + (st.StudentID % 26)), N'اختبار منتصف الفصل'
        FROM @Students s
        INNER JOIN dbo.Students st ON st.StudentNumber = s.StudentNumber
        INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
        INNER JOIN dbo.Subjects sub ON sub.ClassID = c.ClassID AND sub.SubjectName = N'الرياضيات'
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.Marks m
            WHERE m.StudentID = st.StudentID AND m.SubjectID = sub.SubjectID
              AND m.ExamType = N'اختبار منتصف الفصل'
        );

        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType)
        SELECT st.StudentID, sub.SubjectID,
               (SELECT TOP (1) TeacherID FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-102'),
               CONVERT(decimal(5,2), 75 + (st.StudentID % 21)), N'اختبار منتصف الفصل'
        FROM @Students s
        INNER JOIN dbo.Students st ON st.StudentNumber = s.StudentNumber
        INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
        INNER JOIN dbo.Subjects sub ON sub.ClassID = c.ClassID AND sub.SubjectName = N'اللغة الإنجليزية'
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.Marks m
            WHERE m.StudentID = st.StudentID AND m.SubjectID = sub.SubjectID
              AND m.ExamType = N'اختبار منتصف الفصل'
        );
    END;

    /* حضور يومين لكل طالب مع شعبته الصحيحة، مع احترام القيد الفريد StudentID/Date. */
    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
    BEGIN
        INSERT INTO dbo.StudentAttendance
        (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, Notes)
        SELECT st.StudentID, c.ClassID, s.SectionName, @AcademicYear,
               DATEADD(day, -d.DayOffset, @Today),
               CASE WHEN (st.StudentID + d.DayOffset) % 9 = 0 THEN N'غائب'
                    WHEN (st.StudentID + d.DayOffset) % 5 = 0 THEN N'متأخر'
                    ELSE N'حاضر' END,
               CASE WHEN (st.StudentID + d.DayOffset) % 9 = 0 THEN NULL ELSE CONVERT(time, '07:15') END,
               N'حضور تجريبي للصفوف الستة'
        FROM @Students s
        INNER JOIN dbo.Students st ON st.StudentNumber = s.StudentNumber
        INNER JOIN dbo.Classes c ON c.ClassCode = s.ClassCode
        CROSS JOIN (VALUES (1), (2)) d(DayOffset)
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.StudentAttendance a
            WHERE a.StudentID = st.StudentID
              AND a.AttendanceDate = DATEADD(day, -d.DayOffset, @Today)
        );
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

SELECT c.ClassCode, c.ClassName, c.StageName,
       COUNT(DISTINCT st.StudentID) AS DemoStudentCount,
       COUNT(DISTINCT sub.SubjectID) AS SubjectCount
FROM dbo.Classes c
LEFT JOIN dbo.StudentClasses sc ON sc.ClassID = c.ClassID AND sc.AcademicYear = @AcademicYear
LEFT JOIN dbo.Students st ON st.StudentID = sc.StudentID AND st.StudentNumber LIKE N'DEMO-ALL-%'
LEFT JOIN dbo.Subjects sub ON sub.ClassID = c.ClassID AND ISNULL(sub.IsActive, 1) = 1
WHERE c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03')
GROUP BY c.ClassCode, c.ClassName, c.StageName, c.GradeOrder
ORDER BY c.GradeOrder;

SELECT ss.ClassID, c.ClassCode, ss.SectionName, ss.AcademicYear
FROM dbo.SchoolSections ss
INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
WHERE ss.AcademicYear = @AcademicYear
  AND ss.SectionName LIKE N'Section %'
  AND c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03')
ORDER BY c.GradeOrder, ss.SectionName;

PRINT N'تمت إضافة البيانات التجريبية الأكاديمية للصفوف الستة دون تكرار أو تعديل المستخدمين والصلاحيات.';
GO
