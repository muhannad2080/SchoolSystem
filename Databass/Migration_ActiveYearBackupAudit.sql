/*
   SchoolSystem - Active academic year, pre-closing backup and audit integration
   Idempotent migration. Run against SchoolDB; it does not delete operational data.
*/
SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.SystemAcademicSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SystemAcademicSettings
    (
        SettingID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SystemAcademicSettings PRIMARY KEY,
        ActiveAcademicYear NVARCHAR(20) NOT NULL,
        UpdatedByUserID INT NULL,
        UpdatedAt DATETIME NOT NULL CONSTRAINT DF_SystemAcademicSettings_UpdatedAt DEFAULT(GETDATE()),
        SingletonKey AS (CONVERT(TINYINT, 1)) PERSISTED,
        CONSTRAINT CK_SystemAcademicSettings_Year CHECK (NULLIF(LTRIM(RTRIM(ActiveAcademicYear)), N'') IS NOT NULL)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'UX_SystemAcademicSettings_OnlyOne' AND object_id=OBJECT_ID(N'dbo.SystemAcademicSettings'))
BEGIN
    CREATE UNIQUE INDEX UX_SystemAcademicSettings_OnlyOne ON dbo.SystemAcademicSettings(SingletonKey);
END;
GO

IF OBJECT_ID(N'dbo.DatabaseBackupHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DatabaseBackupHistory
    (
        BackupHistoryID BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DatabaseBackupHistory PRIMARY KEY,
        BackupFile NVARCHAR(500) NOT NULL,
        BackupType NVARCHAR(50) NOT NULL,
        AcademicYear NVARCHAR(20) NULL,
        CreatedByUserID INT NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_DatabaseBackupHistory_CreatedAt DEFAULT(GETDATE()),
        IsVerified BIT NOT NULL CONSTRAINT DF_DatabaseBackupHistory_IsVerified DEFAULT(0),
        CONSTRAINT CK_DatabaseBackupHistory_Type CHECK (BackupType IN (N'يدوي', N'قبل الإغلاق', N'قبل الاستعادة'))
    );
    CREATE INDEX IX_DatabaseBackupHistory_Year ON dbo.DatabaseBackupHistory(AcademicYear, CreatedAt DESC);
END;
GO

IF OBJECT_ID(N'dbo.GetActiveAcademicYear', N'P') IS NOT NULL DROP PROCEDURE dbo.GetActiveAcademicYear;
GO
CREATE PROCEDURE dbo.GetActiveAcademicYear
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (1) ActiveAcademicYear, UpdatedByUserID, UpdatedAt
    FROM dbo.SystemAcademicSettings ORDER BY SettingID;
END;
GO

IF OBJECT_ID(N'dbo.SetActiveAcademicYear', N'P') IS NOT NULL DROP PROCEDURE dbo.SetActiveAcademicYear;
GO
CREATE PROCEDURE dbo.SetActiveAcademicYear
    @AcademicYear NVARCHAR(20),
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @AcademicYear=REPLACE(NULLIF(LTRIM(RTRIM(@AcademicYear)),N''),N'-',N'/');
    IF @AcademicYear IS NULL THROW 51101,N'العام الدراسي النشط غير صالح.',1;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@AcademicYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51102,N'لا يمكن جعل عام مغلق هو العام النشط.',1;
    BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM dbo.SystemAcademicSettings)
        UPDATE dbo.SystemAcademicSettings SET ActiveAcademicYear=@AcademicYear, UpdatedByUserID=@UserID, UpdatedAt=GETDATE();
    ELSE
        INSERT dbo.SystemAcademicSettings(ActiveAcademicYear,UpdatedByUserID) VALUES(@AcademicYear,@UserID);
    COMMIT TRANSACTION;
    SELECT @AcademicYear AS ActiveAcademicYear;
END;
GO

IF OBJECT_ID(N'dbo.RegisterDatabaseBackup', N'P') IS NOT NULL DROP PROCEDURE dbo.RegisterDatabaseBackup;
GO
CREATE PROCEDURE dbo.RegisterDatabaseBackup
    @BackupFile NVARCHAR(500), @BackupType NVARCHAR(50), @AcademicYear NVARCHAR(20)=NULL, @UserID INT=NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@BackupFile)),N'') IS NULL THROW 51103,N'ملف النسخة الاحتياطية غير صالح.',1;
    INSERT dbo.DatabaseBackupHistory(BackupFile,BackupType,AcademicYear,CreatedByUserID,IsVerified)
    VALUES(@BackupFile,@BackupType,NULLIF(LTRIM(RTRIM(@AcademicYear)),N''),@UserID,1);
    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS BackupHistoryID;
END;
GO
