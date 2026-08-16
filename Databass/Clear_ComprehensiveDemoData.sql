/*
    SchoolSystem - Clear Comprehensive Demo Data
    الهدف: حذف البيانات التجريبية التي أنشأها Seed_ComprehensiveDemoData.sql فقط.

    مهم:
    - لا يحذف هذا السكربت dbo.Users ولا Roles ولا Permissions ولا UserRoles ولا AuditLogs.
    - لا يحذف السجلات غير المعلّمة DEMO.
    - يجب تشغيله على SchoolDB فقط، وليس على master.
    - يُفضّل أخذ نسخة احتياطية قبل التنفيذ.
*/

IF DB_NAME() <> N'SchoolDB'
    THROW 51100, N'العملية موقوفة: اتصل بقاعدة SchoolDB ثم أعد تشغيل السكربت.', 1;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51101, N'لم يتم العثور على dbo.Users؛ تم إيقاف السكربت للحماية.', 1;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* حفظ مفاتيح الطلاب والمعلمين التجريبيين قبل حذف أي سجل تابع. */
    IF OBJECT_ID(N'tempdb..#DemoStudents') IS NOT NULL DROP TABLE #DemoStudents;
    IF OBJECT_ID(N'tempdb..#DemoTeachers') IS NOT NULL DROP TABLE #DemoTeachers;

    CREATE TABLE #DemoStudents (StudentID INT NOT NULL PRIMARY KEY);
    CREATE TABLE #DemoTeachers (TeacherID INT NOT NULL PRIMARY KEY);

    IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
    BEGIN
        INSERT INTO #DemoStudents (StudentID)
        SELECT StudentID
        FROM dbo.Students
        WHERE StudentNumber LIKE N'DEMO-STU-%';
    END;

    IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL
    BEGIN
        INSERT INTO #DemoTeachers (TeacherID)
        SELECT TeacherID
        FROM dbo.Teachers
        WHERE EmployeeNumber LIKE N'DEMO-TCH-%';
    END;

    /* الجداول التابعة للطلاب والمعلمين. */
    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
        DELETE a FROM dbo.StudentAttendance a INNER JOIN #DemoStudents d ON d.StudentID = a.StudentID;

    IF OBJECT_ID(N'dbo.Marks', N'U') IS NOT NULL
        DELETE m FROM dbo.Marks m INNER JOIN #DemoStudents d ON d.StudentID = m.StudentID;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL
        DELETE f FROM dbo.Fees f INNER JOIN #DemoStudents d ON d.StudentID = f.StudentID;

    IF OBJECT_ID(N'dbo.Receipts', N'U') IS NOT NULL
        DELETE r FROM dbo.Receipts r INNER JOIN #DemoStudents d ON d.StudentID = r.StudentID;

    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
        DELETE e FROM dbo.Enrollments e INNER JOIN #DemoStudents d ON d.StudentID = e.StudentID;

    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
        DELETE sc FROM dbo.StudentClasses sc INNER JOIN #DemoStudents d ON d.StudentID = sc.StudentID;

    IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL
        DELETE a FROM dbo.TeacherAttendance a INNER JOIN #DemoTeachers d ON d.TeacherID = a.TeacherID;

    IF OBJECT_ID(N'dbo.Payroll', N'U') IS NOT NULL
        DELETE p FROM dbo.Payroll p INNER JOIN #DemoTeachers d ON d.TeacherID = p.TeacherID;

    IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NOT NULL
        DELETE c FROM dbo.TeacherContracts c INNER JOIN #DemoTeachers d ON d.TeacherID = c.TeacherID;

    IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL
    BEGIN
        DELETE t
        FROM dbo.SchoolTimetable t
        LEFT JOIN #DemoTeachers d ON d.TeacherID = t.TeacherID
        WHERE d.TeacherID IS NOT NULL
           OR ISNULL(t.Notes, N'') LIKE N'%تجريبية%'
           OR ISNULL(t.Notes, N'') LIKE N'%DEMO%';
    END;

    /* الجداول المالية والعامة التي تحمل معرف DEMO صريحاً. */
    IF OBJECT_ID(N'dbo.Expenses', N'U') IS NOT NULL
        DELETE FROM dbo.Expenses WHERE ExpenseNumber LIKE N'DEMO-%' OR ISNULL(Notes, N'') LIKE N'%تجريبي%';

    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
        DELETE FROM dbo.Vouchers WHERE VoucherNumber LIKE N'DEMO-%' OR ISNULL(Notes, N'') LIKE N'%تجريبي%';

    IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
        DELETE FROM dbo.Books WHERE ISBN LIKE N'DEMO-%' OR ISNULL(Notes, N'') LIKE N'%تجريبي%';

    IF OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
        DELETE FROM dbo.Rooms WHERE RoomCode IN (N'R-101', N'LAB-01') OR ISNULL(Notes, N'') LIKE N'%تجريبي%';

    IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NOT NULL
        DELETE FROM dbo.FeePlans WHERE ISNULL(Notes, N'') LIKE N'%تجريبي%';

    /* حذف الكيانات الأساسية التجريبية بعد تنظيف جميع توابعها. */
    IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
        DELETE s FROM dbo.Students s INNER JOIN #DemoStudents d ON d.StudentID = s.StudentID;

    IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL
        DELETE t FROM dbo.Teachers t INNER JOIN #DemoTeachers d ON d.TeacherID = t.TeacherID;

    /* تحقق داخل المعاملة: Users لم يتغير ولم يُستهدف. */
    IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
        THROW 51102, N'فشل تحقق الحماية: جدول Users غير موجود.', 1;

    COMMIT TRANSACTION;

    SELECT
        (SELECT COUNT(*) FROM #DemoStudents) AS DeletedStudentCandidates,
        (SELECT COUNT(*) FROM #DemoTeachers) AS DeletedTeacherCandidates,
        N'تم حذف البيانات التجريبية المعلّمة فقط. Users والأدوار وحسابات الإدارة محفوظة.' AS ResultMessage;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* تحقق بعد التنفيذ: يجب أن تكون النتائج صفراً، ولا يتم فحص Users بالحذف أو التعديل. */
USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
    SELECT N'DemoStudentsRemaining' AS CheckName, COUNT(*) AS RemainingCount
    FROM dbo.Students
    WHERE StudentNumber LIKE N'DEMO-STU-%';

IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL
    SELECT N'DemoTeachersRemaining' AS CheckName, COUNT(*) AS RemainingCount
    FROM dbo.Teachers
    WHERE EmployeeNumber LIKE N'DEMO-TCH-%';

IF OBJECT_ID(N'dbo.Expenses', N'U') IS NOT NULL
    SELECT N'DemoExpensesRemaining' AS CheckName, COUNT(*) AS RemainingCount
    FROM dbo.Expenses
    WHERE ExpenseNumber LIKE N'DEMO-%';

IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
    SELECT N'DemoVouchersRemaining' AS CheckName, COUNT(*) AS RemainingCount
    FROM dbo.Vouchers
    WHERE VoucherNumber LIKE N'DEMO-%';
GO

/* عرض عدد المستخدمين فقط للتحقق من بقائهم دون تعديلهم. */
SELECT N'UsersPreserved' AS CheckName, COUNT(*) AS CurrentCount
FROM dbo.Users;
GO
