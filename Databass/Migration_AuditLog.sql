/* SchoolSystem - Audit Log migration
   Safe to run repeatedly on an existing SQL Server database. */
IF DB_ID(N'SchoolDB') IS NULL
    THROW 51000, N'قاعدة البيانات SchoolDB غير موجودة. نفّذ الترحيل داخل قاعدة البيانات الصحيحة.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditLogs
    (
        AuditLogID BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY,
        UserID INT NULL,
        UserName NVARCHAR(150) NULL,
        ActionName NVARCHAR(100) NOT NULL,
        EntityName NVARCHAR(100) NULL,
        EntityID NVARCHAR(100) NULL,
        Details NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_CreatedAt' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
    CREATE INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs(CreatedAt DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AuditLogs_Entity' AND object_id = OBJECT_ID(N'dbo.AuditLogs'))
    CREATE INDEX IX_AuditLogs_Entity ON dbo.AuditLogs(EntityName, EntityID);
GO
