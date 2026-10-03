/*
    Migration 011
    إصلاح بنية تشغيل النظام في القواعد المنشأة من databaseschool.sql أو النسخ القديمة.
    ينشئ/يكمل جداول العام النشط وسجل النسخ، ثم ينشئ الكائنات التي تعتمد عليها
    لوحة التحكم والإغلاق السنوي.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

/* ============================================================
   1) مصدر العام الدراسي النشط
   ============================================================ */
IF OBJECT_ID(N'dbo.AcademicYears', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AcademicYears
    (
        AcademicYear NVARCHAR(20) NOT NULL,
        StartDate DATE NULL,
        EndDate DATE NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_AcademicYears_Startup_IsActive DEFAULT (1),
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AcademicYears_Startup_CreatedAt DEFAULT GETDATE()
    );
END;

IF COL_LENGTH(N'dbo.AcademicYears', N'StartDate') IS NULL
    ALTER TABLE dbo.AcademicYears ADD StartDate DATE NULL;

IF COL_LENGTH(N'dbo.AcademicYears', N'EndDate') IS NULL
    ALTER TABLE dbo.AcademicYears ADD EndDate DATE NULL;

IF COL_LENGTH(N'dbo.AcademicYears', N'IsActive') IS NULL
    ALTER TABLE dbo.AcademicYears ADD IsActive BIT NULL;

IF COL_LENGTH(N'dbo.AcademicYears', N'CreatedAt') IS NULL
    ALTER TABLE dbo.AcademicYears ADD CreatedAt DATETIME NULL;

IF OBJECT_ID(N'dbo.AcademicYearCurrent', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AcademicYearCurrent
    (
        AcademicYear NVARCHAR(20) NOT NULL,
        StartDate DATE NULL,
        EndDate DATE NULL,
        IsCurrent BIT NOT NULL CONSTRAINT DF_AcademicYearCurrent_Startup_IsCurrent DEFAULT (1),
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AcademicYearCurrent_Startup_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL CONSTRAINT DF_AcademicYearCurrent_Startup_UpdatedAt DEFAULT GETDATE()
    );
END;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'StartDate') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD StartDate DATE NULL;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'EndDate') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD EndDate DATE NULL;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'IsCurrent') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD IsCurrent BIT NULL;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'Notes') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD Notes NVARCHAR(500) NULL;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'CreatedAt') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD CreatedAt DATETIME NULL;

IF COL_LENGTH(N'dbo.AcademicYearCurrent', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.AcademicYearCurrent ADD UpdatedAt DATETIME NULL;

/* تنظيف غير صالح بسيط: صفوف NULL/فارغة لا تصلح كمصدر للعام النشط. */
DELETE FROM dbo.AcademicYearCurrent
WHERE NULLIF(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'') IS NULL;

/* إذا كان هناك أكثر من عام نشط، نحتفظ بأحدث/أعلى عام فقط. */
;WITH RankedCurrent AS
(
    SELECT
        AcademicYear,
        ROW_NUMBER() OVER
        (
            ORDER BY
                CASE WHEN ISNULL(IsCurrent, 0) = 1 THEN 0 ELSE 1 END,
                AcademicYear DESC
        ) AS rn
    FROM dbo.AcademicYearCurrent
)
DELETE c
FROM dbo.AcademicYearCurrent c
INNER JOIN RankedCurrent r
    ON r.AcademicYear = c.AcademicYear
WHERE r.rn > 1;

/* مزامنة من AcademicYears عند غياب المصدر الحالي. */
IF NOT EXISTS (SELECT 1 FROM dbo.AcademicYearCurrent WHERE IsCurrent = 1)
BEGIN
    DECLARE @SeedYear NVARCHAR(20);

    SELECT TOP (1)
        @SeedYear = REPLACE(LTRIM(RTRIM(AcademicYear)), N'-', N'/')
    FROM dbo.AcademicYears
    WHERE ISNULL(IsActive, 1) = 1
      AND NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
    ORDER BY AcademicYear DESC;

    IF NULLIF(@SeedYear, N'') IS NOT NULL
    BEGIN
        INSERT dbo.AcademicYearCurrent
        (
            AcademicYear, StartDate, EndDate, IsCurrent, Notes, CreatedAt, UpdatedAt
        )
        SELECT TOP (1)
            @SeedYear, ay.StartDate, ay.EndDate, 1,
            N'تمت تهيئة العام النشط تلقائياً بواسطة النظام',
            GETDATE(), GETDATE()
        FROM dbo.AcademicYears ay
        WHERE REPLACE(LTRIM(RTRIM(ay.AcademicYear)), N'-', N'/') = @SeedYear
        ORDER BY ay.AcademicYear DESC;
    END;
END;

/* ============================================================
   2) سجل نسخ مستقل وموثوق للخدمات الجديدة.
   ============================================================ */
IF OBJECT_ID(N'dbo.SchoolSystemBackupHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSystemBackupHistory
    (
        BackupID INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_SchoolSystemBackupHistory PRIMARY KEY,
        BackupFile NVARCHAR(500) NOT NULL,
        BackupType NVARCHAR(50) NULL,
        AcademicYear NVARCHAR(20) NULL,
        UserID INT NULL,
        CreatedAt DATETIME NOT NULL
            CONSTRAINT DF_SchoolSystemBackupHistory_CreatedAt DEFAULT GETDATE()
    );
END;

/* ============================================================
   3) الإجراء: تغيير العام الدراسي النشط
   ============================================================ */
IF OBJECT_ID(N'dbo.SetActiveAcademicYear', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.SetActiveAcademicYear AS BEGIN SET NOCOUNT ON; END');

DECLARE @sql1 NVARCHAR(MAX) = N'ALTER PROCEDURE dbo.SetActiveAcademicYear
    @AcademicYear NVARCHAR(20),
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Year NVARCHAR(20) = REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''''))), N''-'', N''/'');
    IF @Year = N'''' THROW 51170, N''العام الدراسي المطلوب غير صالح.'', 1;
    BEGIN TRANSACTION;
    UPDATE dbo.AcademicYearCurrent SET IsCurrent = 0, UpdatedAt = GETDATE();
    IF EXISTS (SELECT 1 FROM dbo.AcademicYearCurrent WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year)
        UPDATE dbo.AcademicYearCurrent SET AcademicYear = @Year, IsCurrent = 1, UpdatedAt = GETDATE()
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year;
    ELSE
        INSERT dbo.AcademicYearCurrent (AcademicYear, IsCurrent, Notes, CreatedAt, UpdatedAt)
        VALUES (@Year, 1, N''العام النشط المحدد من النظام'', GETDATE(), GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicYears WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year)
        INSERT dbo.AcademicYears (AcademicYear, StartDate, EndDate, IsActive, CreatedAt)
        VALUES (@Year, NULL, NULL, 1, GETDATE());
    ELSE
        UPDATE dbo.AcademicYears SET IsActive = 1
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year;
    COMMIT TRANSACTION;
END;';
EXEC sp_executesql @sql1;
/* ============================================================
   4) الإجراء: تسجيل النسخ الاحتياطية
   ============================================================ */
IF OBJECT_ID(N'dbo.RegisterDatabaseBackup', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.RegisterDatabaseBackup AS BEGIN SET NOCOUNT ON; END');

DECLARE @sql2 NVARCHAR(MAX) = N'ALTER PROCEDURE dbo.RegisterDatabaseBackup
    @BackupFile NVARCHAR(500),
    @BackupType NVARCHAR(50) = NULL,
    @AcademicYear NVARCHAR(20) = NULL,
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NULLIF(LTRIM(RTRIM(ISNULL(@BackupFile, N''''))), N'''') IS NULL
        THROW 51171, N''مسار النسخة الاحتياطية مطلوب.'', 1;
    INSERT dbo.SchoolSystemBackupHistory (BackupFile, BackupType, AcademicYear, UserID, CreatedAt)
    VALUES
    (LTRIM(RTRIM(@BackupFile)),
     NULLIF(LTRIM(RTRIM(ISNULL(@BackupType, N''''))), N''''),
     NULLIF(REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''''))), N''-'', N''/''), N''''),
     @UserID, GETDATE());
END;';
EXEC sp_executesql @sql2;
/* ============================================================
   5) الإجراء: حالة التشغيل للوحة التحكم
   ============================================================ */
IF OBJECT_ID(N'dbo.GetDashboardOperationalStatus', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.GetDashboardOperationalStatus AS BEGIN SET NOCOUNT ON; END');

DECLARE @sql3 NVARCHAR(MAX) = N'ALTER PROCEDURE dbo.GetDashboardOperationalStatus
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.SchoolSystemBackupHistory)
        SELECT TOP (1)
            b.BackupFile AS LastBackupFile,
            b.CreatedAt AS LastBackupDate,
            CAST(NULL AS NVARCHAR(20)) AS LastClosingYear,
            CAST(NULL AS NVARCHAR(100)) AS LastClosingStatus
        FROM dbo.SchoolSystemBackupHistory b
        ORDER BY b.BackupID DESC;
    ELSE
        SELECT
            CAST(NULL AS NVARCHAR(500)) AS LastBackupFile,
            CAST(NULL AS DATETIME) AS LastBackupDate,
            CAST(NULL AS NVARCHAR(20)) AS LastClosingYear,
            CAST(NULL AS NVARCHAR(100)) AS LastClosingStatus;
END;';
EXEC sp_executesql @sql3;
/* ============================================================
   6) الدالة: العام الدراسي النشط
   ============================================================ */
IF OBJECT_ID(N'dbo.GetActiveAcademicYear', N'FN') IS NULL
    EXEC(N'CREATE FUNCTION dbo.GetActiveAcademicYear() RETURNS NVARCHAR(20) AS
          BEGIN
              RETURN N'''';
          END');

DECLARE @sql4 NVARCHAR(MAX) = N'ALTER FUNCTION dbo.GetActiveAcademicYear()
RETURNS NVARCHAR(20)
AS
BEGIN
    DECLARE @Year NVARCHAR(20);
    SELECT TOP (1)
        @Year = REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'')
    FROM dbo.AcademicYearCurrent
    WHERE ISNULL(IsCurrent, 1) = 1
      AND NULLIF(LTRIM(RTRIM(AcademicYear)), N'''') IS NOT NULL
    ORDER BY UpdatedAt DESC, CreatedAt DESC;
    RETURN ISNULL(@Year, N'''');
END;';
EXEC sp_executesql @sql4;
