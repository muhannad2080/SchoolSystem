/*
    AuditLog enrichment migration
    Adds module, machine and network metadata without deleting existing audit rows.
    Safe to run repeatedly.
*/

IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditLogs
    (
        AuditLogID BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY,
        UserID INT NULL,
        UserName NVARCHAR(150) NULL,
        Module NVARCHAR(100) NULL,
        MachineName NVARCHAR(150) NULL,
        IpAddress NVARCHAR(64) NULL,
        ActionName NVARCHAR(100) NOT NULL,
        EntityName NVARCHAR(100) NULL,
        EntityID NVARCHAR(100) NULL,
        Details NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())
    );
END;

IF COL_LENGTH(N'dbo.AuditLogs', N'Module') IS NULL
    ALTER TABLE dbo.AuditLogs ADD Module NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.AuditLogs', N'MachineName') IS NULL
    ALTER TABLE dbo.AuditLogs ADD MachineName NVARCHAR(150) NULL;
IF COL_LENGTH(N'dbo.AuditLogs', N'IpAddress') IS NULL
    ALTER TABLE dbo.AuditLogs ADD IpAddress NVARCHAR(64) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_CreatedAt' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
    CREATE INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs(CreatedAt DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_Entity' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
    CREATE INDEX IX_AuditLogs_Entity ON dbo.AuditLogs(EntityName, EntityID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_Module' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
    CREATE INDEX IX_AuditLogs_Module ON dbo.AuditLogs(Module, CreatedAt DESC);

UPDATE dbo.AuditLogs
SET Module = CASE
    WHEN EntityName = N'Student' THEN N'Students'
    WHEN EntityName = N'Enrollment' THEN N'Enrollment'
    WHEN EntityName = N'Fee' THEN N'Fees'
    WHEN EntityName = N'Voucher' THEN N'Vouchers'
    WHEN EntityName = N'User' THEN N'Users'
    ELSE ISNULL(NULLIF(LTRIM(RTRIM(EntityName)), N''), N'System')
END
WHERE NULLIF(LTRIM(RTRIM(Module)), N'') IS NULL;
GO
