/*
    Migration 009
    يمنع تكرار اسم الصف داخل المرحلة نفسها.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
    THROW 51150, N'جدول Classes غير موجود؛ لا يمكن تطبيق Migration 009.', 1;

IF EXISTS
(
    SELECT
        LTRIM(RTRIM(ISNULL(ClassName, N''))) AS ClassNameNormalized,
        LTRIM(RTRIM(ISNULL(StageName, N''))) AS StageNameNormalized,
        COUNT(*) AS DuplicateCount
    FROM dbo.Classes
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(ClassName, N''))), N'') IS NOT NULL
    GROUP BY
        LTRIM(RTRIM(ISNULL(ClassName, N''))),
        LTRIM(RTRIM(ISNULL(StageName, N'')))
    HAVING COUNT(*) > 1
)
    THROW 51151, N'توجد صفوف مكررة بالاسم داخل المرحلة نفسها. عالج التكرارات قبل تطبيق Migration 009.', 1;

IF COL_LENGTH(N'dbo.Classes', N'ClassNameNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.Classes
    ADD ClassNameNormalized AS
    (
        LTRIM(RTRIM(ISNULL(ClassName, N'')))
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.Classes', N'StageNameNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.Classes
    ADD StageNameNormalized AS
    (
        LTRIM(RTRIM(ISNULL(StageName, N'')))
    ) PERSISTED;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Classes')
      AND name = N'UX_Classes_Name_Stage'
)
BEGIN
    CREATE UNIQUE INDEX UX_Classes_Name_Stage
        ON dbo.Classes(ClassNameNormalized, StageNameNormalized);
END;