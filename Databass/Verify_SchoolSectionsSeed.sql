/*
    تحقق من بيانات الصفوف والشعب التجريبية والارتباطات.
    نفّذ هذا الملف داخل قاعدة SchoolDB بعد تشغيل:
      1) Migration_SeedAcademicCatalog.sql
      2) Migration_CreateSchoolSections.sql
      3) Migration_SeedClassSections.sql (اختياري لتوزيع الطلاب التجريبيين)
*/
SET NOCOUNT ON;

DECLARE @CurrentDatabase sysname = DB_NAME();

IF @CurrentDatabase <> N'SchoolDB'
BEGIN
    RAISERROR(N'أوقف التنفيذ: يجب تشغيل هذا السكربت داخل قاعدة SchoolDB. قاعدة الاتصال الحالية: %s', 16, 1, @CurrentDatabase);
    RETURN;
END;

PRINT N'1) الصفوف الأكاديمية النشطة';
SELECT ClassID, ClassCode, ClassName, StageName, GradeOrder, IsActive
FROM dbo.Classes
WHERE ClassCode IN (N'SEC-01', N'SEC-02', N'SEC-03')
ORDER BY GradeOrder, ClassID;

PRINT N'2) عدد الصفوف النشطة وعدد الشعب حسب العام الدراسي';
SELECT
    ss.AcademicYear,
    COUNT(DISTINCT ss.ClassID) AS ActiveClassCount,
    COUNT(*) AS ActiveSectionCount,
    COUNT(DISTINCT ss.SectionName) AS DistinctSectionNameCount
FROM dbo.SchoolSections ss
INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
WHERE ISNULL(ss.IsActive, 1) = 1
  AND ISNULL(c.IsActive, 1) = 1
GROUP BY ss.AcademicYear
ORDER BY ss.AcademicYear;

PRINT N'3) تفاصيل الشعب المرتبطة بصفوف صحيحة';
SELECT
    ss.SectionID,
    c.ClassID,
    c.ClassCode,
    c.ClassName,
    ss.SectionName,
    ss.AcademicYear,
    ss.IsActive
FROM dbo.SchoolSections ss
INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
WHERE ISNULL(ss.IsActive, 1) = 1
ORDER BY ss.AcademicYear, c.GradeOrder, ss.SectionName;

PRINT N'4) الشعب المطلوبة المفقودة: يجب أن تكون النتيجة فارغة';
DECLARE @Years TABLE (AcademicYear NVARCHAR(20) NOT NULL PRIMARY KEY);
INSERT INTO @Years VALUES (N'1447-1448'), (N'2026/2027');
DECLARE @Sections TABLE (SectionName NVARCHAR(50) NOT NULL PRIMARY KEY);
INSERT INTO @Sections VALUES (N'ألف'), (N'باء'), (N'جيم'), (N'دال');

SELECT c.ClassCode, c.ClassName, y.AcademicYear, s.SectionName
FROM dbo.Classes c
CROSS JOIN @Years y
CROSS JOIN @Sections s
LEFT JOIN dbo.SchoolSections ss
    ON ss.ClassID = c.ClassID
   AND ss.AcademicYear = y.AcademicYear
   AND LTRIM(RTRIM(ss.SectionName)) = s.SectionName
   AND ISNULL(ss.IsActive, 1) = 1
WHERE c.ClassCode IN (N'SEC-01', N'SEC-02', N'SEC-03')
  AND ISNULL(c.IsActive, 1) = 1
  AND ss.SectionID IS NULL
ORDER BY c.GradeOrder, y.AcademicYear, s.SectionName;

PRINT N'5) تكرار غير مسموح: يجب أن تكون النتيجة فارغة';
SELECT ClassID, AcademicYear, LTRIM(RTRIM(SectionName)) AS SectionName, COUNT(*) AS DuplicateCount
FROM dbo.SchoolSections
GROUP BY ClassID, AcademicYear, LTRIM(RTRIM(SectionName))
HAVING COUNT(*) > 1;

PRINT N'6) شعب مرتبطة بصفوف غير موجودة أو غير نشطة: يجب أن تكون النتيجة فارغة';
SELECT ss.SectionID, ss.ClassID, ss.SectionName, ss.AcademicYear, c.ClassName, c.IsActive
FROM dbo.SchoolSections ss
LEFT JOIN dbo.Classes c ON c.ClassID = ss.ClassID
WHERE c.ClassID IS NULL OR ISNULL(c.IsActive, 1) = 0;

PRINT N'7) توزيعات الطلاب ذات صف أو شعبة غير صحيحة: يجب أن تكون النتيجة فارغة';
IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
BEGIN
    SELECT sc.StudentClassID, sc.StudentID, sc.ClassID, sc.Section, sc.AcademicYear
    FROM dbo.StudentClasses sc
    LEFT JOIN dbo.Students st ON st.StudentID = sc.StudentID
    LEFT JOIN dbo.Classes c ON c.ClassID = sc.ClassID
    LEFT JOIN dbo.SchoolSections ss
        ON ss.ClassID = sc.ClassID
       AND ss.AcademicYear = sc.AcademicYear
       AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(sc.Section))
       AND ISNULL(ss.IsActive, 1) = 1
    WHERE st.StudentID IS NULL OR c.ClassID IS NULL OR ss.SectionID IS NULL;
END;

PRINT N'8) نتيجة مختصرة للصفوف الثانوية';
SELECT
    c.ClassCode,
    c.ClassName,
    COUNT(DISTINCT ss.SectionID) AS ActiveSectionCount,
    COUNT(DISTINCT sub.SubjectID) AS ActiveSubjectCount
FROM dbo.Classes c
LEFT JOIN dbo.SchoolSections ss ON ss.ClassID = c.ClassID AND ISNULL(ss.IsActive, 1) = 1
LEFT JOIN dbo.Subjects sub ON sub.ClassID = c.ClassID AND ISNULL(sub.IsActive, 1) = 1
WHERE c.ClassCode IN (N'SEC-01', N'SEC-02', N'SEC-03')
GROUP BY c.ClassCode, c.ClassName, c.GradeOrder
ORDER BY c.GradeOrder;

PRINT N'اكتمل التحقق. النتائج التي تحمل عبارة يجب أن تكون النتيجة فارغة يجب ألا تعرض أي صف.';
GO
