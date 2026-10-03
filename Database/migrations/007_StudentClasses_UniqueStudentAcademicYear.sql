/*
    Migration 007
    يفرض سجل توزيع واحد فقط للطالب في العام الدراسي نفسه.
    يستخدم عمودًا محسوبًا لتوحيد 2026-2027 و2026/2027.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
    THROW 51130, N'جدول StudentClasses غير موجود؛ لا يمكن تطبيق Migration 007.', 1;

IF EXISTS
(
    SELECT StudentID,
           REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
           COUNT(*) AS DuplicateCount
    FROM dbo.StudentClasses
    WHERE StudentID IS NOT NULL
    GROUP BY StudentID,
             REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
    HAVING COUNT(*) > 1
)
    THROW 51131, N'توجد توزيعات مكررة للطالب في العام الدراسي نفسه. عالج التكرارات قبل تطبيق Migration 007.', 1;

IF COL_LENGTH(N'dbo.StudentClasses', N'AcademicYearNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.StudentClasses
    ADD AcademicYearNormalized AS
    (
        REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
    ) PERSISTED;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.StudentClasses')
      AND name = N'UX_StudentClasses_Student_AcademicYear'
)
BEGIN
    CREATE UNIQUE INDEX UX_StudentClasses_Student_AcademicYear
        ON dbo.StudentClasses(StudentID, AcademicYearNormalized)
        WHERE StudentID IS NOT NULL;
END;