/*
    Migration 004
    يمنع تكرار الدرجة لنفس الطالب والمادة والصف والشعبة والعام والفصل الدراسي.
    يتم التطبيع في أعمدة محسوبة حتى لا تسمح المسافات أو اختلاف - / بتكرار منطقي.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
    THROW 52010, N'جدول Grades غير موجود؛ لا يمكن تطبيق قيد تفرد الدرجات.', 1;

IF COL_LENGTH(N'dbo.Grades', N'GradeSectionNormalized') IS NULL
    ALTER TABLE dbo.Grades ADD GradeSectionNormalized AS LTRIM(RTRIM(ISNULL(Section, N''))) PERSISTED;

IF COL_LENGTH(N'dbo.Grades', N'GradeAcademicYearNormalized') IS NULL
    ALTER TABLE dbo.Grades ADD GradeAcademicYearNormalized AS REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') PERSISTED;

IF COL_LENGTH(N'dbo.Grades', N'GradeTermNormalized') IS NULL
    ALTER TABLE dbo.Grades ADD GradeTermNormalized AS LTRIM(RTRIM(ISNULL(TermName, N''))) PERSISTED;

IF EXISTS
(
    SELECT StudentID, SubjectID, ClassID,
           LTRIM(RTRIM(ISNULL(Section, N''))) AS SectionNormalized,
           REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
           LTRIM(RTRIM(ISNULL(TermName, N''))) AS TermNormalized,
           COUNT(*) AS DuplicateCount
    FROM dbo.Grades
    GROUP BY StudentID, SubjectID, ClassID,
             LTRIM(RTRIM(ISNULL(Section, N''))),
             REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/'),
             LTRIM(RTRIM(ISNULL(TermName, N'')))
    HAVING COUNT(*) > 1
)
    THROW 52011, N'توجد درجات مكررة لنفس الطالب والمادة والسياق الدراسي. عالج التكرارات قبل تطبيق Migration 004.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Grades')
      AND name = N'UX_Grades_Student_Subject_Context'
)
BEGIN
    CREATE UNIQUE INDEX UX_Grades_Student_Subject_Context
        ON dbo.Grades
        (StudentID, SubjectID, ClassID, GradeSectionNormalized, GradeAcademicYearNormalized, GradeTermNormalized);
END;