/*
    تهيئة الشعب الدراسية لبيئة SchoolSystem.
    السكربت قابل لإعادة التنفيذ ولا ينشئ سجلات مكررة.
    يعتمد على الطلاب التجريبيين DEMO-STU-001 إلى DEMO-STU-004
    الذين ينشئهم Databass/Seed_DemoReportingData.sql.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
    THROW 51001, N'جدول StudentClasses غير موجود. شغّل ترحيلات قاعدة البيانات أولاً.', 1;

IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL
    THROW 51002, N'عمود Section غير موجود في StudentClasses. شغّل Migration_Step1.sql أولاً.', 1;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51003, N'جدول Students غير موجود.', 1;

DECLARE @AcademicYear NVARCHAR(20) = N'1447-1448';
DECLARE @Class1 INT = (SELECT TOP (1) ClassID FROM dbo.Classes WHERE ClassCode = N'SEC-01' AND ISNULL(IsActive, 1) = 1);
DECLARE @Class2 INT = (SELECT TOP (1) ClassID FROM dbo.Classes WHERE ClassCode = N'SEC-02' AND ISNULL(IsActive, 1) = 1);
DECLARE @Class3 INT = (SELECT TOP (1) ClassID FROM dbo.Classes WHERE ClassCode = N'SEC-03' AND ISNULL(IsActive, 1) = 1);

IF @Class1 IS NULL OR @Class2 IS NULL OR @Class3 IS NULL
    THROW 51004, N'الصفوف الثانوية SEC-01 وSEC-02 وSEC-03 غير موجودة. شغّل Migration_SeedAcademicCatalog.sql أولاً.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    /*
       الشعب التجريبية:
       الصف الأول الثانوي: Section A، Section B
       الصف الثاني الثانوي: Section C
       الصف الثالث الثانوي: Section D
    */
    DECLARE @Assignments TABLE
    (
        StudentNumber NVARCHAR(50) NOT NULL,
        ClassID INT NOT NULL,
        Section NVARCHAR(50) NOT NULL
    );

    INSERT INTO @Assignments (StudentNumber, ClassID, Section)
    VALUES
        (N'DEMO-STU-001', @Class1, N'Section A'),
        (N'DEMO-STU-002', @Class1, N'Section B'),
        (N'DEMO-STU-003', @Class2, N'Section C'),
        (N'DEMO-STU-004', @Class3, N'Section D');

    /* تحديث التوزيع الموجود فقط؛ لا يتم إنشاء طالب وهمي أو تكرار توزيع. */
    UPDATE sc
       SET sc.ClassID = a.ClassID,
           sc.Section = a.Section,
           sc.AcademicYear = @AcademicYear
    FROM dbo.StudentClasses sc
    INNER JOIN dbo.Students st ON st.StudentID = sc.StudentID
    INNER JOIN @Assignments a ON a.StudentNumber = st.StudentNumber
    WHERE sc.AcademicYear = @AcademicYear;

    /* إذا لم يكن التوزيع موجوداً، أنشئه للطلاب التجريبيين الموجودين فقط. */
    INSERT INTO dbo.StudentClasses
    (
        StudentID,
        ClassID,
        Section,
        AcademicYear,
        AssignedDate
    )
    SELECT st.StudentID, a.ClassID, a.Section, @AcademicYear, GETDATE()
    FROM dbo.Students st
    INNER JOIN @Assignments a ON a.StudentNumber = st.StudentNumber
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.StudentClasses sc
        WHERE sc.StudentID = st.StudentID
          AND sc.AcademicYear = @AcademicYear
    );

    /* إبقاء Students متوافقاً مع أحدث توزيع كما يعتمد عليه العرض والتقارير. */
    IF COL_LENGTH(N'dbo.Students', N'ClassID') IS NOT NULL
       AND COL_LENGTH(N'dbo.Students', N'Section') IS NOT NULL
       AND COL_LENGTH(N'dbo.Students', N'AcademicYear') IS NOT NULL
    BEGIN
        UPDATE st
           SET st.ClassID = a.ClassID,
               st.Section = a.Section,
               st.AcademicYear = @AcademicYear
        FROM dbo.Students st
        INNER JOIN @Assignments a ON a.StudentNumber = st.StudentNumber;
    END;

    /* توحيد أسماء الشعب في سجلات الحضور التجريبية عند وجودها. */
    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
    BEGIN
        UPDATE sa
           SET sa.Section = CASE sa.StudentID
                                WHEN (SELECT StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-001') THEN N'Section A'
                                WHEN (SELECT StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-002') THEN N'Section B'
                                WHEN (SELECT StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-003') THEN N'Section C'
                                WHEN (SELECT StudentID FROM dbo.Students WHERE StudentNumber = N'DEMO-STU-004') THEN N'Section D'
                                ELSE sa.Section
                            END
        WHERE sa.StudentID IN
        (
            SELECT StudentID
            FROM dbo.Students
            WHERE StudentNumber IN
            (N'DEMO-STU-001', N'DEMO-STU-002', N'DEMO-STU-003', N'DEMO-STU-004')
        );
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;

/* تحقق عملي من الشعب التي ستظهر في القوائم التي تعتمد على StudentClasses. */
SELECT
    sc.ClassID,
    c.ClassName,
    sc.Section,
    sc.AcademicYear,
    COUNT(*) AS StudentCount
FROM dbo.StudentClasses sc
INNER JOIN dbo.Classes c ON c.ClassID = sc.ClassID
WHERE sc.AcademicYear = @AcademicYear
  AND NULLIF(LTRIM(RTRIM(sc.Section)), N'') IS NOT NULL
GROUP BY sc.ClassID, c.ClassName, sc.Section, sc.AcademicYear
ORDER BY c.ClassName, sc.Section;

PRINT N'تمت إضافة الشعب التجريبية: Section A، Section B، Section C، Section D.';
GO
