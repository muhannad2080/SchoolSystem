/*
    SchoolSystem - Demo reporting data
    الغرض: إضافة بيانات تجريبية مترابطة لاختبار التقارير والطباعة والتصدير.
    آمن للتشغيل المتكرر: لا يحذف بيانات ولا يكرر السجلات الموجودة بالاسم/الرقم.
    شغّل Migration_SeedAcademicCatalog.sql أولاً.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 50030, N'قاعدة SchoolDB غير موجودة.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Classes', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Subjects', N'U') IS NULL
   OR OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Marks', N'U') IS NULL
   OR OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NULL
    THROW 50031, N'الجداول الأكاديمية الأساسية غير مكتملة. نفذ ملفات الترحيل أولاً.', 1;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* الصفوف الأساسية حتى تعمل النسخة أيضاً إذا لم ينفذ كتالوج الصفوف بعد. */
    IF NOT EXISTS (SELECT 1 FROM dbo.Classes WHERE ClassName = N'الأول الثانوي')
        INSERT INTO dbo.Classes (ClassName) VALUES (N'الأول الثانوي');
    IF NOT EXISTS (SELECT 1 FROM dbo.Classes WHERE ClassName = N'الثاني الثانوي')
        INSERT INTO dbo.Classes (ClassName) VALUES (N'الثاني الثانوي');
    IF NOT EXISTS (SELECT 1 FROM dbo.Classes WHERE ClassName = N'الثالث الثانوي')
        INSERT INTO dbo.Classes (ClassName) VALUES (N'الثالث الثانوي');

    DECLARE @Class1 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassName = N'الأول الثانوي' ORDER BY ClassID);
    DECLARE @Class2 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassName = N'الثاني الثانوي' ORDER BY ClassID);
    DECLARE @Class3 INT = (SELECT TOP 1 ClassID FROM dbo.Classes WHERE ClassName = N'الثالث الثانوي' ORDER BY ClassID);

    /* المواد الأساسية. */
    DECLARE @Subjects TABLE (SubjectName NVARCHAR(100) NOT NULL);
    INSERT INTO @Subjects VALUES
        (N'اللغة العربية'), (N'اللغة الإنجليزية'), (N'الرياضيات'),
        (N'الفيزياء'), (N'الكيمياء'), (N'الأحياء'),
        (N'التربية الإسلامية'), (N'الدراسات الاجتماعية'),
        (N'الحاسب الآلي وتقنية المعلومات');

    INSERT INTO dbo.Subjects (SubjectName)
    SELECT s.SubjectName
    FROM @Subjects s
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Subjects x WHERE x.SubjectName = s.SubjectName);

    /* معلمون تجريبيون. */
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-001')
        INSERT INTO dbo.Teachers
            (EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary, Status, Notes)
        VALUES
            (N'DEMO-TCH-001', N'أحمد محمد العريقي', N'ذكر', N'يمني', N'777100001', N'ahmad.demo@schoolsystem.local', N'بكالوريوس تربية', N'رياضيات', '2021-09-01', 850, N'نشط', N'بيانات تجريبية للتقارير');
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-002')
        INSERT INTO dbo.Teachers
            (EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary, Status, Notes)
        VALUES
            (N'DEMO-TCH-002', N'سارة علي الحكيمي', N'أنثى', N'يمنية', N'777100002', N'sara.demo@schoolsystem.local', N'بكالوريوس لغة', N'اللغة الإنجليزية', '2022-09-01', 900, N'نشط', N'بيانات تجريبية للتقارير');
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-003')
        INSERT INTO dbo.Teachers
            (EmployeeNumber, FullName, Gender, Nationality, Phone, Email, Qualification, Specialization, HireDate, BasicSalary, Status, Notes)
        VALUES
            (N'DEMO-TCH-003', N'خالد عبSection Dله الصلوي', N'ذكر', N'يمني', N'777100003', N'khaled.demo@schoolsystem.local', N'ماجستير علوم', N'الفيزياء', '2020-09-01', 980, N'نشط', N'بيانات تجريبية للتقارير');

    DECLARE @Teacher1 INT = (SELECT TOP 1 TeacherID FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-001');
    DECLARE @Teacher2 INT = (SELECT TOP 1 TeacherID FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-002');
    DECLARE @Teacher3 INT = (SELECT TOP 1 TeacherID FROM dbo.Teachers WHERE EmployeeNumber = N'DEMO-TCH-003');

    /* طلاب تجريبيون بأرقام واضحة حتى تظهر في بطاقة الطالب والتقارير. */
    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-001')
        INSERT INTO dbo.Students
            (StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, NationalId, StudentPhone, Status, GuardianName, GuardianRelation, GuardianPhone, GuardianEmail, Governorate, District, Address)
        VALUES
            (N'DEMO-STU-001', N'محمد أحمد صالح', N'ذكر', '2009-03-14', N'صنعاء', N'يمني', N'DEMO-ID-001', N'777200001', N'نشط', N'أحمد صالح', N'والد', N'777300001', N'guardian1@schoolsystem.local', N'أمانة العاصمة', N'التحرير', N'حي المدارس');
    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-002')
        INSERT INTO dbo.Students
            (StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, NationalId, StudentPhone, Status, GuardianName, GuardianRelation, GuardianPhone, GuardianEmail, Governorate, District, Address)
        VALUES
            (N'DEMO-STU-002', N'ريم عبSection Dله حسن', N'أنثى', '2009-08-22', N'تعز', N'يمنية', N'DEMO-ID-002', N'777200002', N'نشط', N'عبSection Dله حسن', N'والد', N'777300002', N'guardian2@schoolsystem.local', N'تعز', N'المظفر', N'شارع الجامعة');
    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-003')
        INSERT INTO dbo.Students
            (StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, NationalId, StudentPhone, Status, GuardianName, GuardianRelation, GuardianPhone, GuardianEmail, Governorate, District, Address)
        VALUES
            (N'DEMO-STU-003', N'عمر خالد القحطاني', N'ذكر', '2008-12-05', N'عدن', N'يمني', N'DEMO-ID-003', N'777200003', N'نشط', N'خالد القحطاني', N'والد', N'777300003', N'guardian3@schoolsystem.local', N'عدن', N'المنصورة', N'حي النور');
    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-004')
        INSERT INTO dbo.Students
            (StudentNumber, FullName, Gender, BirthDate, BirthPlace, Nationality, NationalId, StudentPhone, Status, GuardianName, GuardianRelation, GuardianPhone, GuardianEmail, Governorate, District, Address)
        VALUES
            (N'DEMO-STU-004', N'ليان محمد يحيى', N'أنثى', '2008-06-18', N'إب', N'يمنية', N'DEMO-ID-004', N'777200004', N'نشط', N'محمد يحيى', N'والد', N'777300004', N'guardian4@schoolsystem.local', N'إب', N'الظهار', N'المدينة القديمة');

    DECLARE @Student1 INT = (SELECT TOP 1 StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-001');
    DECLARE @Student2 INT = (SELECT TOP 1 StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-002');
    DECLARE @Student3 INT = (SELECT TOP 1 StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-003');
    DECLARE @Student4 INT = (SELECT TOP 1 StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-004');

    /* ربط الطلاب بالفصول للعام الحالي. */
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID = @Student1 AND ClassID = @Class1 AND AcademicYear = N'1447-1448')
        INSERT INTO dbo.StudentClasses (StudentID, ClassID, AcademicYear) VALUES (@Student1, @Class1, N'1447-1448');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID = @Student2 AND ClassID = @Class1 AND AcademicYear = N'1447-1448')
        INSERT INTO dbo.StudentClasses (StudentID, ClassID, AcademicYear) VALUES (@Student2, @Class1, N'1447-1448');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID = @Student3 AND ClassID = @Class2 AND AcademicYear = N'1447-1448')
        INSERT INTO dbo.StudentClasses (StudentID, ClassID, AcademicYear) VALUES (@Student3, @Class2, N'1447-1448');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID = @Student4 AND ClassID = @Class3 AND AcademicYear = N'1447-1448')
        INSERT INTO dbo.StudentClasses (StudentID, ClassID, AcademicYear) VALUES (@Student4, @Class3, N'1447-1448');

    DECLARE @Math INT = (SELECT TOP 1 SubjectID FROM dbo.Subjects WHERE SubjectName = N'الرياضيات');
    DECLARE @English INT = (SELECT TOP 1 SubjectID FROM dbo.Subjects WHERE SubjectName = N'اللغة الإنجليزية');
    DECLARE @Physics INT = (SELECT TOP 1 SubjectID FROM dbo.Subjects WHERE SubjectName = N'الفيزياء');
    DECLARE @Arabic INT = (SELECT TOP 1 SubjectID FROM dbo.Subjects WHERE SubjectName = N'اللغة العربية');

    /* درجات متنوعة لاختبار التقارير والرسوم البيانية. */
    IF NOT EXISTS (SELECT 1 FROM dbo.Marks WHERE StudentID = @Student1 AND SubjectID = @Math AND ExamType = N'اختبار منتصف الفصل')
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType) VALUES (@Student1, @Math, @Teacher1, 88, N'اختبار منتصف الفصل');
    IF NOT EXISTS (SELECT 1 FROM dbo.Marks WHERE StudentID = @Student1 AND SubjectID = @English AND ExamType = N'اختبار منتصف الفصل')
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType) VALUES (@Student1, @English, @Teacher2, 92, N'اختبار منتصف الفصل');
    IF NOT EXISTS (SELECT 1 FROM dbo.Marks WHERE StudentID = @Student2 AND SubjectID = @Math AND ExamType = N'اختبار منتصف الفصل')
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType) VALUES (@Student2, @Math, @Teacher1, 76, N'اختبار منتصف الفصل');
    IF NOT EXISTS (SELECT 1 FROM dbo.Marks WHERE StudentID = @Student3 AND SubjectID = @Physics AND ExamType = N'اختبار منتصف الفصل')
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType) VALUES (@Student3, @Physics, @Teacher3, 84, N'اختبار منتصف الفصل');
    IF NOT EXISTS (SELECT 1 FROM dbo.Marks WHERE StudentID = @Student4 AND SubjectID = @Arabic AND ExamType = N'اختبار منتصف الفصل')
        INSERT INTO dbo.Marks (StudentID, SubjectID, TeacherID, Mark, ExamType) VALUES (@Student4, @Arabic, @Teacher2, 95, N'اختبار منتصف الفصل');

    /* حضور يومين مختلفين لكل طالب، مع يوم غياب لإظهار الحالات في التقرير. */
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID = @Student1 AND AttendanceDate = '2026-08-10')
        INSERT INTO dbo.StudentAttendance (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, Notes) VALUES (@Student1, @Class1, N'Section A', N'1447-1448', '2026-08-10', N'حاضر', '07:05', N'حضور منتظم');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID = @Student2 AND AttendanceDate = '2026-08-10')
        INSERT INTO dbo.StudentAttendance (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, Notes) VALUES (@Student2, @Class1, N'Section A', N'1447-1448', '2026-08-10', N'غائب', NULL, N'غياب تجريبي');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID = @Student3 AND AttendanceDate = '2026-08-11')
        INSERT INTO dbo.StudentAttendance (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, Notes) VALUES (@Student3, @Class2, N'Section B', N'1447-1448', '2026-08-11', N'حاضر', '07:12', N'حضور منتظم');
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID = @Student4 AND AttendanceDate = '2026-08-11')
        INSERT INTO dbo.StudentAttendance (StudentID, ClassID, Section, AcademicYear, AttendanceDate, Status, ArrivalTime, Notes) VALUES (@Student4, @Class3, N'Section A', N'1447-1448', '2026-08-11', N'متأخر', '07:35', N'تأخر تجريبي');

    /* بيانات النقل لاختبار تقارير الحافلات إذا كانت الجداول مثبتة. */
    IF OBJECT_ID(N'dbo.Buses', N'U') IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Buses WHERE BusNumber = N'DEMO-BUS-01')
        INSERT INTO dbo.Buses (BusNumber, DriverName, DriverPhone, Capacity, Notes) VALUES (N'DEMO-BUS-01', N'إبراهيم حسن', N'777400001', 30, N'حافلة تجريبية');

    IF OBJECT_ID(N'dbo.BusRoutes', N'U') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.Buses WHERE BusNumber = N'DEMO-BUS-01')
       AND NOT EXISTS (SELECT 1 FROM dbo.BusRoutes WHERE RouteName = N'خط المدينة - المدرسة التجريبي')
        INSERT INTO dbo.BusRoutes (RouteName, BusID, StartPoint, EndPoint, DepartureTime, ArrivalTime, Fee, Notes)
        SELECT N'خط المدينة - المدرسة التجريبي', BusID, N'حي الجامعة', N'مدرسة SchoolSystem', '06:30', '07:10', 250, N'مسار تجريبي'
        FROM dbo.Buses WHERE BusNumber = N'DEMO-BUS-01';

    COMMIT TRANSACTION;

    SELECT N'طلاب' AS ItemName, COUNT(*) AS ItemCount FROM dbo.Students WHERE StudentNumber LIKE N'DEMO-%'
    UNION ALL SELECT N'معلمون', COUNT(*) FROM dbo.Teachers WHERE EmployeeNumber LIKE N'DEMO-%'
    UNION ALL SELECT N'ربط فصول', COUNT(*) FROM dbo.StudentClasses WHERE AcademicYear = N'1447-1448'
    UNION ALL SELECT N'درجات تجريبية', COUNT(*) FROM dbo.Marks WHERE ExamType = N'اختبار منتصف الفصل'
    UNION ALL SELECT N'حضور تجريبي', COUNT(*) FROM dbo.StudentAttendance WHERE AcademicYear = N'1447-1448';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

