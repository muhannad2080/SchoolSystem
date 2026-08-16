/*
    مصدر ثابت للشعب الدراسية للقوائم المنسدلة.
    السكربت قابل لإعادة التنفيذ ولا يعتمد على وجود طلاب موزعين.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
    THROW 51101, N'جدول Classes غير موجود. شغّل ترحيلات قاعدة البيانات الأساسية أولاً.', 1;

IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSections
    (
        SectionID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolSections PRIMARY KEY,
        ClassID INT NOT NULL,
        SectionName NVARCHAR(50) NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_SchoolSections_IsActive DEFAULT (1),
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SchoolSections_CreatedAt DEFAULT (SYSDATETIME()),
        CONSTRAINT FK_SchoolSections_Classes FOREIGN KEY (ClassID) REFERENCES dbo.Classes(ClassID),
        CONSTRAINT CK_SchoolSections_SectionName_NotEmpty CHECK (NULLIF(LTRIM(RTRIM(SectionName)), N'') IS NOT NULL),
        CONSTRAINT CK_SchoolSections_AcademicYear_NotEmpty CHECK (NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_SchoolSections_ClassYearName'
      AND object_id = OBJECT_ID(N'dbo.SchoolSections')
)
BEGIN
    CREATE UNIQUE INDEX UX_SchoolSections_ClassYearName
        ON dbo.SchoolSections(ClassID, AcademicYear, SectionName);
END;
GO

DECLARE @Years TABLE (AcademicYear NVARCHAR(20) NOT NULL PRIMARY KEY);
INSERT INTO @Years (AcademicYear)
VALUES (N'1447-1448'), (N'2026/2027');

DECLARE @Sections TABLE (SectionName NVARCHAR(50) NOT NULL PRIMARY KEY);
INSERT INTO @Sections (SectionName)
VALUES (N'ألف'), (N'باء'), (N'جيم'), (N'دال');

INSERT INTO dbo.SchoolSections (ClassID, SectionName, AcademicYear, IsActive)
SELECT c.ClassID, s.SectionName, y.AcademicYear, 1
FROM dbo.Classes c
CROSS JOIN @Sections s
CROSS JOIN @Years y
WHERE ISNULL(c.IsActive, 1) = 1
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.SchoolSections existing
      WHERE existing.ClassID = c.ClassID
        AND existing.SectionName = s.SectionName
        AND existing.AcademicYear = y.AcademicYear
  );

SELECT
    ss.SectionID,
    ss.ClassID,
    c.ClassName,
    ss.SectionName,
    ss.AcademicYear,
    ss.IsActive
FROM dbo.SchoolSections ss
INNER JOIN dbo.Classes c ON c.ClassID = ss.ClassID
WHERE ss.IsActive = 1
ORDER BY ss.AcademicYear, c.ClassName, ss.SectionName;

PRINT N'تم إنشاء مصدر الشعب الثابت وإضافة ألف وباء وجيم ودال لكل صف نشط.';
GO
