/*
   Grades components hardening
   Safe to run repeatedly on SchoolDB.
   It preserves existing GradeValue/Notes data and adds the component
   columns used by GradeEntryForm when they are missing.
*/

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grades
    (
        GradeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades PRIMARY KEY,
        StudentID INT NULL,
        SubjectID INT NULL,
        ClassID INT NULL,
        Section NVARCHAR(100) NULL,
        AcademicYear NVARCHAR(20) NULL,
        TermName NVARCHAR(50) NULL,
        Quiz1 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz1 DEFAULT (0),
        Quiz2 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz2 DEFAULT (0),
        CourseWork DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_CourseWork DEFAULT (0),
        FinalExam DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_FinalExam DEFAULT (0),
        GradeValue DECIMAL(10,2) NULL,
        GradeLetter NVARCHAR(50) NULL,
        ResultStatus NVARCHAR(50) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Grades_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.Grades', N'Section') IS NULL
        ALTER TABLE dbo.Grades ADD Section NVARCHAR(100) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'Quiz1') IS NULL
        ALTER TABLE dbo.Grades ADD Quiz1 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz1 DEFAULT (0);
    IF COL_LENGTH(N'dbo.Grades', N'Quiz2') IS NULL
        ALTER TABLE dbo.Grades ADD Quiz2 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz2 DEFAULT (0);
    IF COL_LENGTH(N'dbo.Grades', N'CourseWork') IS NULL
        ALTER TABLE dbo.Grades ADD CourseWork DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_CourseWork DEFAULT (0);
    IF COL_LENGTH(N'dbo.Grades', N'FinalExam') IS NULL
        ALTER TABLE dbo.Grades ADD FinalExam DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_FinalExam DEFAULT (0);
    IF COL_LENGTH(N'dbo.Grades', N'GradeLetter') IS NULL
        ALTER TABLE dbo.Grades ADD GradeLetter NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'ResultStatus') IS NULL
        ALTER TABLE dbo.Grades ADD ResultStatus NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'UpdatedAt') IS NULL
        ALTER TABLE dbo.Grades ADD UpdatedAt DATETIME NULL;
END;

GO
