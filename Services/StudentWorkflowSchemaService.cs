using System;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;

namespace SchoolSystem.Services
{
    /// <summary>
    /// Ensures the minimum schema required by the student/enrollment/assignment/fees workflow.
    /// It is intentionally idempotent so it can run on every application start.
    /// </summary>
    public static class StudentWorkflowSchemaService
    {
        public static void EnsureReady()
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(BuildSql(), connection))
            {
                command.CommandTimeout = 60;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static string BuildSql()
        {
            return @"
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51300, N'جدول الطلاب Students غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;
IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
    THROW 51301, N'جدول توزيع الطلاب StudentClasses غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;
IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
    THROW 51302, N'جدول الصفوف Classes غير موجود. نفّذ مخطط قاعدة البيانات الأساسي أولاً.', 1;

BEGIN TRANSACTION;

/* StudentClasses compatibility columns. */
IF COL_LENGTH(N'dbo.StudentClasses', N'StudentID') IS NULL
    ALTER TABLE dbo.StudentClasses ADD StudentID INT NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'ClassID') IS NULL
    ALTER TABLE dbo.StudentClasses ADD ClassID INT NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL
    ALTER TABLE dbo.StudentClasses ADD Section NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AcademicYear') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AcademicYear NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedDate') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedDate DATETIME NOT NULL CONSTRAINT DF_StudentClasses_Startup_AssignedDate DEFAULT GETDATE() WITH VALUES;
IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedBy') IS NULL
    ALTER TABLE dbo.StudentClasses ADD AssignedBy INT NULL;

/* Students summary columns used by legacy screens. */
IF COL_LENGTH(N'dbo.Students', N'ClassID') IS NULL
    ALTER TABLE dbo.Students ADD ClassID INT NULL;
IF COL_LENGTH(N'dbo.Students', N'Section') IS NULL
    ALTER TABLE dbo.Students ADD Section NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.Students', N'AcademicYear') IS NULL
    ALTER TABLE dbo.Students ADD AcademicYear NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.Students', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Students ADD UpdatedAt DATETIME NULL;

/* SchoolSections may exist in older installations with only a subset of fields. */
IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSections
    (
        SectionID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolSections_Startup PRIMARY KEY,
        ClassID INT NOT NULL,
        SectionName NVARCHAR(50) NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_SchoolSections_Startup_IsActive DEFAULT (1),
        Capacity INT NULL,
        AllowedGender NVARCHAR(20) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_SchoolSections_Startup_CreatedAt DEFAULT GETDATE()
    );
END;
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.SchoolSections', N'ClassID') IS NULL
        ALTER TABLE dbo.SchoolSections ADD ClassID INT NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'SectionName') IS NULL
        ALTER TABLE dbo.SchoolSections ADD SectionName NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'AcademicYear') IS NULL
        ALTER TABLE dbo.SchoolSections ADD AcademicYear NVARCHAR(20) NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'IsActive') IS NULL
        ALTER TABLE dbo.SchoolSections ADD IsActive BIT NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'Capacity') IS NULL
        ALTER TABLE dbo.SchoolSections ADD Capacity INT NULL;
    IF COL_LENGTH(N'dbo.SchoolSections', N'AllowedGender') IS NULL
        ALTER TABLE dbo.SchoolSections ADD AllowedGender NVARCHAR(20) NULL;
END;

COMMIT TRANSACTION;";
        }
    }
}
