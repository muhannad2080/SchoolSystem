/*
 SchoolSystem - ResetOperationalData
 -----------------------------------
 Purpose:
   - Remove operational/demo/test data only.
   - Preserve master/reference/configuration data and all identity/access data.
   - Do NOT insert students, teachers, fees, vouchers, payroll, attendance, grades, etc.
   - Reset identity counters for deleted operational tables.

 Preserved:
   Rooms, SchoolSettings, Users, Roles, Permissions,
   RolePermissions, UserRoles, UserPermissions, SchemaMigrations,
   AcademicYears, AcademicYearCurrent, AcademicTerms, Stages,
   Classes, SchoolSections, Subjects, ClassSubjects, ClassSubjectTerms,
   FeeTypes (if the table exists).

 The reset does not overwrite existing reference data.
 It is intended to be run AFTER a database backup.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
BEGIN
    RAISERROR(N'قاعدة البيانات SchoolDB غير موجودة.', 16, 1);
    RETURN;
END;

USE [SchoolDB];

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Keep TABLE
    (
        SchemaName SYSNAME NOT NULL,
        TableName SYSNAME NOT NULL,
        PRIMARY KEY (SchemaName, TableName)
    );

    INSERT INTO @Keep (SchemaName, TableName)
    VALUES
        (N'dbo', N'Rooms'),
        (N'dbo', N'SchoolSettings'),
        (N'dbo', N'Users'),
        (N'dbo', N'Roles'),
        (N'dbo', N'Permissions'),
        (N'dbo', N'RolePermissions'),
        (N'dbo', N'UserRoles'),
        (N'dbo', N'UserPermissions'),
        (N'dbo', N'SchemaMigrations'),
        (N'dbo', N'AcademicYears'),
        (N'dbo', N'AcademicYearCurrent'),
        (N'dbo', N'AcademicTerms'),
        (N'dbo', N'Stages'),
        (N'dbo', N'Classes'),
        (N'dbo', N'SchoolSections'),
        (N'dbo', N'Subjects'),
        (N'dbo', N'ClassSubjects'),
        (N'dbo', N'ClassSubjectTerms'),
        (N'dbo', N'FeeTypes');

    DECLARE @schema SYSNAME;
    DECLARE @table SYSNAME;
    DECLARE @sql NVARCHAR(MAX);

    /* Disable all user-table constraints so child rows can be cleared safely. */
    DECLARE disable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        WHERE t.is_ms_shipped = 0;

    OPEN disable_cursor;
    FETCH NEXT FROM disable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'ALTER TABLE ' + QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N' NOCHECK CONSTRAINT ALL;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM disable_cursor INTO @schema, @table;
    END;

    CLOSE disable_cursor;
    DEALLOCATE disable_cursor;

    /* Delete every non-reference table. This removes old and new test data. */
    DECLARE clear_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        WHERE t.is_ms_shipped = 0
          AND NOT EXISTS
          (
              SELECT 1
              FROM @Keep AS k
              WHERE k.SchemaName = SCHEMA_NAME(t.schema_id)
                AND k.TableName = t.name
          );

    OPEN clear_cursor;
    FETCH NEXT FROM clear_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'DELETE FROM ' + QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM clear_cursor INTO @schema, @table;
    END;

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /* Reset IDENTITY values on deleted operational tables. */
    DECLARE identity_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        INNER JOIN sys.identity_columns AS ic
            ON ic.object_id = t.object_id
        WHERE t.is_ms_shipped = 0
          AND NOT EXISTS
          (
              SELECT 1
              FROM @Keep AS k
              WHERE k.SchemaName = SCHEMA_NAME(t.schema_id)
                AND k.TableName = t.name
          );

    OPEN identity_cursor;
    FETCH NEXT FROM identity_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'DBCC CHECKIDENT (' + QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N', RESEED, 0) WITH NO_INFOMSGS;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM identity_cursor INTO @schema, @table;
    END;

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* Re-enable and validate all constraints. */
    DECLARE enable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        WHERE t.is_ms_shipped = 0;

    OPEN enable_cursor;
    FETCH NEXT FROM enable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'ALTER TABLE ' + QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N' WITH CHECK CHECK CONSTRAINT ALL;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM enable_cursor INTO @schema, @table;
    END;

    CLOSE enable_cursor;
    DEALLOCATE enable_cursor;

    /* Access accounts must survive the reset. */
    IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
        THROW 51501, N'جدول المستخدمين غير موجود.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Users)
        THROW 51502, N'لم يبقَ أي مستخدم بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Roles)
        THROW 51503, N'لم تبقَ الأدوار بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        THROW 51504, N'لم تبقَ الصلاحيات بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Students)
        THROW 51510, N'جدول الطلاب غير فارغ.', 1;

    IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Teachers)
        THROW 51511, N'جدول المعلمين غير فارغ.', 1;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Fees)
        THROW 51512, N'جدول الرسوم غير فارغ.', 1;

    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Vouchers)
        THROW 51513, N'جدول السندات غير فارغ.', 1;

    IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.FeePlans)
        THROW 51514, N'خطط الرسوم غير فارغة.', 1;

    COMMIT TRANSACTION;

    SELECT
        N'تم تنظيف بيانات التشغيل والاختبار بنجاح.' AS StatusMessage,
        (SELECT COUNT(*) FROM dbo.Users) AS UsersCount,
        (SELECT COUNT(*) FROM dbo.Roles) AS RolesCount,
        (SELECT COUNT(*) FROM dbo.Permissions) AS PermissionsCount,
        (SELECT COUNT(*) FROM dbo.Rooms) AS RoomsCount,
        CASE WHEN OBJECT_ID(N'dbo.Students', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Students) END AS StudentsCount,
        CASE WHEN OBJECT_ID(N'dbo.Teachers', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Teachers) END AS TeachersCount,
        CASE WHEN OBJECT_ID(N'dbo.FeePlans', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.FeePlans) END AS FeePlansCount,
        CASE WHEN OBJECT_ID(N'dbo.Fees', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Fees) END AS FeesCount,
        CASE WHEN OBJECT_ID(N'dbo.Vouchers', N'U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.Vouchers) END AS VouchersCount;
END TRY
BEGIN CATCH
    IF CURSOR_STATUS('local', 'disable_cursor') >= 0
    BEGIN CLOSE disable_cursor; DEALLOCATE disable_cursor; END;

    IF CURSOR_STATUS('local', 'clear_cursor') >= 0
    BEGIN CLOSE clear_cursor; DEALLOCATE clear_cursor; END;

    IF CURSOR_STATUS('local', 'identity_cursor') >= 0
    BEGIN CLOSE identity_cursor; DEALLOCATE identity_cursor; END;

    IF CURSOR_STATUS('local', 'enable_cursor') >= 0
    BEGIN CLOSE enable_cursor; DEALLOCATE enable_cursor; END;

    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
