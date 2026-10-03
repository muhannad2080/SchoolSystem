/*
    Migration 005
    يمنع تكرار درجة الطالب للمادة ونوع الاختبار نفسه.
    يتم التطبيع لمنع اختلاف المسافات من إنشاء تكرارات منطقية.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Marks', N'U') IS NULL
    THROW 53010, N'جدول Marks غير موجود؛ لا يمكن تطبيق قيد تفرد الدرجات التفصيلية.', 1;

IF COL_LENGTH(N'dbo.Marks', N'MarkExamTypeNormalized') IS NULL
    ALTER TABLE dbo.Marks ADD MarkExamTypeNormalized AS LTRIM(RTRIM(ISNULL(ExamType, N''))) PERSISTED;

IF EXISTS
(
    SELECT StudentID, SubjectID,
           LTRIM(RTRIM(ISNULL(ExamType, N''))) AS ExamTypeNormalized,
           COUNT(*) AS DuplicateCount
    FROM dbo.Marks
    GROUP BY StudentID, SubjectID, LTRIM(RTRIM(ISNULL(ExamType, N'')))
    HAVING COUNT(*) > 1
)
    THROW 53011, N'توجد درجات تفصيلية مكررة لنفس الطالب والمادة ونوع الاختبار. عالج التكرارات قبل تطبيق Migration 005.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Marks')
      AND name = N'UX_Marks_Student_Subject_Exam'
)
BEGIN
    CREATE UNIQUE INDEX UX_Marks_Student_Subject_Exam
        ON dbo.Marks(StudentID, SubjectID, MarkExamTypeNormalized);
END;