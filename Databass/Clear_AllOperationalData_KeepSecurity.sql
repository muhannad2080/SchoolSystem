/*
    SchoolSystem - Full operational data reset
    الهدف: تفريغ جميع بيانات التشغيل والبيانات التجريبية من SchoolDB.

    يتم الإبقاء على جداول الأمان حتى يبقى تسجيل الدخول وحساب المدير والصلاحيات
    كما هي، ولا يتم إسقاط أي جدول أو تغيير بنية قاعدة البيانات.

    الجداول المحمية:
      Users, Roles, Permissions, UserRoles, RolePermissions,
      AuditLogs, AppSettings, Settings, DatabaseSettings

    تحذير: هذا السكربت يحذف كل الصفوف من الجداول غير المحمية.
    خذ نسخة احتياطية قبل التنفيذ، ولا تغيّر قيمة التأكيد إلا بعد المراجعة.
*/

IF DB_NAME() <> N'SchoolDB'
    THROW 51200, N'العملية موقوفة: اتصل بقاعدة SchoolDB فقط، وليس master.', 1;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Confirmation NVARCHAR(30) = N'REVIEW-ONLY';
IF @Confirmation <> N'CLEAR-SCHOOLDB'
    THROW 51201, N'السكربت محمي. راجع النسخة الاحتياطية ثم غيّر @Confirmation إلى CLEAR-SCHOOLDB للتنفيذ.', 1;

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51202, N'لم يتم العثور على dbo.Users؛ تم إيقاف العملية لحماية حسابات الدخول.', 1;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Protected TABLE
    (
        SchemaName SYSNAME NOT NULL,
        TableName SYSNAME NOT NULL,
        PRIMARY KEY (SchemaName, TableName)
    );

    INSERT INTO @Protected (SchemaName, TableName)
    VALUES
        (N'dbo', N'Users'),
        (N'dbo', N'Roles'),
        (N'dbo', N'Permissions'),
        (N'dbo', N'UserRoles'),
        (N'dbo', N'RolePermissions'),
        (N'dbo', N'AuditLogs'),
        (N'dbo', N'AppSettings'),
        (N'dbo', N'Settings'),
        (N'dbo', N'DatabaseSettings');

    /* لا نتعامل إلا مع جداول المستخدمين الموجودة فعلياً. */
    DECLARE @Targets TABLE
    (
        SchemaName SYSNAME NOT NULL,
        TableName SYSNAME NOT NULL,
        PRIMARY KEY (SchemaName, TableName)
    );

    INSERT INTO @Targets (SchemaName, TableName)
    SELECT s.name, t.name
    FROM sys.tables t
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    LEFT JOIN @Protected p ON p.SchemaName = s.name AND p.TableName = t.name
    WHERE t.is_ms_shipped = 0
      AND p.TableName IS NULL
      AND t.name NOT IN (N'sysdiagrams', N'__EFMigrationsHistory');

    DECLARE @ProtectedUserCount INT = (SELECT COUNT(*) FROM dbo.Users);
    DECLARE @ProtectedRoleCount INT = CASE WHEN OBJECT_ID(N'dbo.Roles', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Roles) END;
    DECLARE @ProtectedPermissionCount INT = CASE WHEN OBJECT_ID(N'dbo.Permissions', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Permissions) END;

    /* تعطيل القيود على الجداول المستهدفة فقط؛ الجداول الأمنية لا تُعطّل. */
    DECLARE @Sql NVARCHAR(MAX) = N'';
    SELECT @Sql = @Sql +
        N'ALTER TABLE ' + QUOTENAME(SchemaName) + N'.' + QUOTENAME(TableName) + N' NOCHECK CONSTRAINT ALL;' + CHAR(13) + CHAR(10)
    FROM @Targets;
    IF @Sql <> N'' EXEC sys.sp_executesql @Sql;

    /* حذف الصفوف فقط، مع الاحتفاظ بكل تعريفات الجداول والفهارس. */
    SET @Sql = N'';
    SELECT @Sql = @Sql +
        N'DELETE FROM ' + QUOTENAME(SchemaName) + N'.' + QUOTENAME(TableName) + N';' + CHAR(13) + CHAR(10)
    FROM @Targets;
    IF @Sql <> N'' EXEC sys.sp_executesql @Sql;

    /* إعادة تفعيل القيود والتحقق منها قبل اعتماد المعاملة. */
    SET @Sql = N'';
    SELECT @Sql = @Sql +
        N'ALTER TABLE ' + QUOTENAME(SchemaName) + N'.' + QUOTENAME(TableName) + N' WITH CHECK CHECK CONSTRAINT ALL;' + CHAR(13) + CHAR(10)
    FROM @Targets;
    IF @Sql <> N'' EXEC sys.sp_executesql @Sql;

    /* تحقق من أن الجداول الأمنية لم تتغير أثناء المعاملة. */
    IF (SELECT COUNT(*) FROM dbo.Users) <> @ProtectedUserCount
        THROW 51203, N'فشل تحقق Users؛ تم إلغاء العملية.', 1;

    IF OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL AND (SELECT COUNT(*) FROM dbo.Roles) <> @ProtectedRoleCount
        THROW 51204, N'فشل تحقق Roles؛ تم إلغاء العملية.', 1;

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL AND (SELECT COUNT(*) FROM dbo.Permissions) <> @ProtectedPermissionCount
        THROW 51205, N'فشل تحقق Permissions؛ تم إلغاء العملية.', 1;

    COMMIT TRANSACTION;

    SELECT
        (SELECT COUNT(*) FROM @Targets) AS ClearedTableCount,
        @ProtectedUserCount AS UsersPreserved,
        @ProtectedRoleCount AS RolesPreserved,
        @ProtectedPermissionCount AS PermissionsPreserved,
        N'تم تفريغ بيانات التشغيل والتجربة، مع إبقاء المستخدمين وحسابات الإدارة والصلاحيات.' AS ResultMessage;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* تحقق مستقل بعد التنفيذ: عدد المستخدمين والصلاحيات محفوظ كما هو. */
USE SchoolDB;
GO

SELECT N'UsersPreserved' AS CheckName, COUNT(*) AS CurrentCount FROM dbo.Users
UNION ALL
SELECT N'RolesPreserved', CASE WHEN OBJECT_ID(N'dbo.Roles', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Roles) END
UNION ALL
SELECT N'PermissionsPreserved', CASE WHEN OBJECT_ID(N'dbo.Permissions', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Permissions) END;
GO
