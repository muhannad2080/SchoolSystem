/*
    Migration 010
    يفرض اسم شعبة فريدًا داخل الصف والعام الدراسي.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
    THROW 51160, N'جدول SchoolSections غير موجود؛ لا يمكن تطبيق Migration 010.', 1;

IF EXISTS
(
    SELECT
        ClassID,
        LTRIM(RTRIM(ISNULL(SectionName, N''))) AS SectionNameNormalized,
        REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
        COUNT(*) AS DuplicateCount
    FROM dbo.SchoolSections
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(SectionName, N''))), N'') IS NOT NULL
    GROUP BY
        ClassID,
        LTRIM(RTRIM(ISNULL(SectionName, N''))),
        REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
    HAVING COUNT(*) > 1
)
    THROW 51161, N'توجد شعب مكررة للصف والعام الدراسي نفسه. عالج التكرارات قبل تطبيق Migration 010.', 1;

IF COL_LENGTH(N'dbo.SchoolSections', N'SectionNameNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolSections
    ADD SectionNameNormalized AS
    (
        NULLIF(LTRIM(RTRIM(ISNULL(SectionName, N''))), N'')
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.SchoolSections', N'SectionAcademicYearNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolSections
    ADD SectionAcademicYearNormalized AS
    (
        REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
    ) PERSISTED;
END;

/*
   لا نستخدم Filtered Index هنا لأن SQL Server لا يسمح بمرجع العمود المحسوب
   داخل WHERE للفهرس المفلتر. العمود المحسوب يبقى ضمن مفتاح الفهرس نفسه.
   الاسم الفارغ يتحول إلى NULL، وهو غير صالح من طبقة الخدمة أيضاً.
*/
IF EXISTS
(
    SELECT 1
    FROM dbo.SchoolSections
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(SectionName, N''))), N'') IS NULL
)
    THROW 51162, N'توجد شعب بلا اسم في SchoolSections. أدخل اسم الشعبة قبل تطبيق Migration 010.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.SchoolSections')
      AND name = N'UX_SchoolSections_Class_Year_Name'
)
BEGIN
    CREATE UNIQUE INDEX UX_SchoolSections_Class_Year_Name
        ON dbo.SchoolSections
        (
            ClassID,
            SectionAcademicYearNormalized,
            SectionNameNormalized
        );
END;