/*
   Production compatibility migration for student assignment.
   Safe to run repeatedly. Execute against SchoolDB before using ClassAssignmentForm.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
    THROW 51120, N'جدول dbo.Classes غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;
IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51121, N'جدول dbo.Students غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;
IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
    THROW 51122, N'جدول dbo.StudentClasses غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;

BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSections
    (
        SectionID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolSections_Ensure PRIMARY KEY,
        ClassID INT NOT NULL,
        SectionName NVARCHAR(50) NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_SchoolSections_Ensure_IsActive DEFAULT (1),
        Capacity INT NULL,
        AllowedGender NVARCHAR(20) NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SchoolSections_Ensure_CreatedAt DEFAULT (SYSDATETIME()),
        CONSTRAINT FK_SchoolSections_Ensure_Classes FOREIGN KEY (ClassID) REFERENCES dbo.Classes(ClassID),
        CONSTRAINT CK_SchoolSections_Ensure_Name CHECK (NULLIF(LTRIM(RTRIM(SectionName)), N'') IS NOT NULL),
        CONSTRAINT CK_SchoolSections_Ensure_Year CHECK (NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL),
        CONSTRAINT CK_SchoolSections_Ensure_Capacity CHECK (Capacity IS NULL OR Capacity > 0)
    );
END;

IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL
    ALTER TABLE dbo.StudentClasses ADD Section NVARCHAR(50) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AcademicYear') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AcademicYear NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedDate') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedDate DATETIME NOT NULL
        CONSTRAINT DF_StudentClasses_Ensure_AssignedDate DEFAULT GETDATE() WITH VALUES;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedBy') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedBy INT NULL;

IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.SchoolSections', N'Capacity') IS NULL
        ALTER TABLE dbo.SchoolSections ADD Capacity INT NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'AllowedGender') IS NULL
        ALTER TABLE dbo.SchoolSections ADD AllowedGender NVARCHAR(20) NULL;
END;

IF COL_LENGTH(N'dbo.Students', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Students ADD UpdatedAt DATETIME NULL;

IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.SchoolSections') AND name = N'UX_SchoolSections_ClassYearName')
   AND NOT EXISTS
   (
       SELECT ClassID, AcademicYear, SectionName
       FROM dbo.SchoolSections
       GROUP BY ClassID, AcademicYear, SectionName
       HAVING COUNT(*) > 1
   )
    CREATE UNIQUE INDEX UX_SchoolSections_ClassYearName
        ON dbo.SchoolSections(ClassID, AcademicYear, SectionName);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentClasses') AND name = N'IX_StudentClasses_ClassSectionYear')
    CREATE INDEX IX_StudentClasses_ClassSectionYear
        ON dbo.StudentClasses(ClassID, Section, AcademicYear);

COMMIT TRANSACTION;
PRINT N'تم تجهيز مخطط توزيع الطلاب والشعب بنجاح.';
