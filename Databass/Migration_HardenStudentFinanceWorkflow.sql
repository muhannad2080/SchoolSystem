/*
    Production hardening: Student -> Enrollment -> Assignment -> Fees
    Safe/idempotent migration. Execute against SchoolDB after the base schema.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51200, N'جدول dbo.Students غير موجود.', 1;
IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL
    THROW 51201, N'جدول dbo.Enrollments غير موجود.', 1;
IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
    THROW 51202, N'جدول dbo.StudentClasses غير موجود.', 1;
IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NULL
    THROW 51203, N'جدول dbo.FeePlans غير موجود.', 1;
IF OBJECT_ID(N'dbo.Fees', N'U') IS NULL
    THROW 51204, N'جدول dbo.Fees غير موجود.', 1;

BEGIN TRANSACTION;

/* Required compatibility columns used by the assignment workflow. */
IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL
    ALTER TABLE dbo.StudentClasses ADD Section NVARCHAR(50) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AcademicYear') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AcademicYear NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedDate') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedDate DATETIME NOT NULL CONSTRAINT DF_StudentClasses_Harden_AssignedDate DEFAULT GETDATE() WITH VALUES;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedBy') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedBy INT NULL;
IF COL_LENGTH(N'dbo.Students', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Students ADD UpdatedAt DATETIME NULL;

/* Supporting indexes: they improve lookup and make duplicate checks reliable. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Enrollments') AND name = N'IX_Enrollments_StudentYearStatus')
    CREATE INDEX IX_Enrollments_StudentYearStatus
        ON dbo.Enrollments(StudentID, AcademicYear, Status);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentClasses') AND name = N'IX_StudentClasses_StudentYear')
    CREATE INDEX IX_StudentClasses_StudentYear
        ON dbo.StudentClasses(StudentID, AcademicYear);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentClasses') AND name = N'IX_StudentClasses_ClassSectionYear')
    CREATE INDEX IX_StudentClasses_ClassSectionYear
        ON dbo.StudentClasses(ClassID, Section, AcademicYear);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.FeePlans') AND name = N'IX_FeePlans_ClassYear')
    CREATE INDEX IX_FeePlans_ClassYear
        ON dbo.FeePlans(ClassID, AcademicYear, FeeType);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Fees') AND name = N'IX_Fees_StudentYearPlan')
    CREATE INDEX IX_Fees_StudentYearPlan
        ON dbo.Fees(StudentID, AcademicYear, FeePlanID);

/* Prevent duplicate enrollments for the same student and normalized academic year. */
IF COL_LENGTH(N'dbo.Enrollments', N'AcademicYearKey') IS NULL
    ALTER TABLE dbo.Enrollments ADD AcademicYearKey AS (REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')) PERSISTED;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Enrollments') AND name = N'UX_Enrollments_StudentYear')
   AND NOT EXISTS
   (
       SELECT StudentID, AcademicYearKey
       FROM dbo.Enrollments
       WHERE AcademicYearKey <> N''
       GROUP BY StudentID, AcademicYearKey
       HAVING COUNT(*) > 1
   )
    CREATE UNIQUE INDEX UX_Enrollments_StudentYear
        ON dbo.Enrollments(StudentID, AcademicYearKey)
        WHERE AcademicYearKey <> N'';

/* Prevent duplicate plan-generated fees for the same student, year and plan. */
IF COL_LENGTH(N'dbo.Fees', N'AcademicYearKey') IS NULL
    ALTER TABLE dbo.Fees ADD AcademicYearKey AS (REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')) PERSISTED;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Fees') AND name = N'UX_Fees_StudentYearPlan')
   AND NOT EXISTS
   (
       SELECT StudentID, AcademicYearKey, FeePlanID
       FROM dbo.Fees
       WHERE FeePlanID IS NOT NULL AND AcademicYearKey <> N''
       GROUP BY StudentID, AcademicYearKey, FeePlanID
       HAVING COUNT(*) > 1
   )
    CREATE UNIQUE INDEX UX_Fees_StudentYearPlan
        ON dbo.Fees(StudentID, AcademicYearKey, FeePlanID)
        WHERE FeePlanID IS NOT NULL AND AcademicYearKey <> N'';

/* Prevent two active assignments for the same student and year. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentClasses') AND name = N'UX_StudentClasses_StudentYear')
   AND NOT EXISTS
   (
       SELECT StudentID, AcademicYear
       FROM dbo.StudentClasses
       GROUP BY StudentID, AcademicYear
       HAVING COUNT(*) > 1
   )
    CREATE UNIQUE INDEX UX_StudentClasses_StudentYear
        ON dbo.StudentClasses(StudentID, AcademicYear);

COMMIT TRANSACTION;
PRINT N'تم تعزيز مخطط دورة الطالب والرسوم بنجاح.';
GO

/* Verification queries: every result should be zero after cleanup. */
SELECT N'Orphan assignments' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.StudentClasses sc
LEFT JOIN dbo.Students s ON s.StudentID = sc.StudentID
WHERE s.StudentID IS NULL;

SELECT N'Assignments without accepted enrollment' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.StudentClasses sc
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Enrollments e
    WHERE e.StudentID = sc.StudentID
      AND REPLACE(ISNULL(e.AcademicYear,N''),N'-',N'/') = REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')
      AND LTRIM(RTRIM(ISNULL(e.Status,N''))) IN (N'مقبول', N'Accepted')
);

SELECT N'Duplicate assignments' AS CheckName, COUNT(*) AS IssueCount
FROM
(
    SELECT StudentID, AcademicYear
    FROM dbo.StudentClasses
    GROUP BY StudentID, AcademicYear
    HAVING COUNT(*) > 1
) d;

SELECT N'Fees without student' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.Fees f
LEFT JOIN dbo.Students s ON s.StudentID = f.StudentID
WHERE s.StudentID IS NULL;

SELECT N'Plan fees without plan' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.Fees f
WHERE f.FeePlanID IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.FeePlans fp WHERE fp.FeePlanID = f.FeePlanID);
GO
