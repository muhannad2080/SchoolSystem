/*
    SchoolSystem - Production data reset
    ------------------------------------
    الهدف:
      إبقاء البيانات الأساسية المطلوبة للتشغيل فقط:
        1) Subjects
        2) AcademicYears
        3) AcademicTerms
        4) AcademicYearCurrent
        5) Rooms
        6) Users
        7) Roles
        8) Permissions
        9) RolePermissions
       10) UserRoles
       11) UserPermissions
       12) SchoolSettings
       13) SchemaMigrations (بنية تشغيلية للنظام ولا تُعد بيانات مدرسية)

    تُحذف جميع البيانات التشغيلية/التجريبية الأخرى، بما فيها:
      الطلاب، ملفاتهم، التسجيل، التوزيع، المعلمون، العقود، الرواتب،
      الحضور، الدرجات، الجدول الدراسي، الرسوم، خطط الرسوم، السندات،
      المصروفات، النقل، المكتبة، السجلات السنوية والتدقيق والتقارير
      وأي جدول تشغيلي جديد غير مدرج ضمن قائمة الاحتفاظ.

    ملاحظة:
      هذا السكربت يمسح البيانات من قاعدة البيانات فقط.
      ملفات PDF/Excel التي صدرتها سابقًا وحُفظت في نظام الملفات لا توجد
      في SQL Server، لذلك احذفها من مجلد التصدير إن كانت موجودة.

    مهم:
      خذ نسخة احتياطية قبل التنفيذ.
      لا يُستدعى هذا السكربت تلقائيًا عند تشغيل البرنامج.
*/

IF DB_ID(N'SchoolDB') IS NULL
    THROW 52000, N'قاعدة البيانات SchoolDB غير موجودة.', 1;
GO

USE [SchoolDB];
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Keep TABLE
    (
        TableName SYSNAME NOT NULL PRIMARY KEY
    );

    INSERT INTO @Keep (TableName)
    VALUES
        (N'Subjects'),
        (N'AcademicYears'),
        (N'AcademicTerms'),
        (N'AcademicYearCurrent'),
        (N'Rooms'),
        (N'Users'),
        (N'Roles'),
        (N'Permissions'),
        (N'RolePermissions'),
        (N'UserRoles'),
        (N'UserPermissions'),
        (N'SchoolSettings'),
        (N'SchemaMigrations');

    /* المادة تبقى، لكن أي ربط قديم بصف سيُلغى قبل تنظيف الصفوف. */
    IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL
       AND COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NOT NULL
    BEGIN
        UPDATE dbo.Subjects
        SET ClassID = NULL,
            UpdatedAt = GETDATE();
    END;

    /* تعطيل قيود FK مؤقتًا حتى يكون التنظيف شاملًا ولا يعتمد على ترتيب
       الجداول أو على الجداول التي قد تُضاف في الإصدارات المستقبلية. */
    DECLARE @sql NVARCHAR(MAX) = N'';
    DECLARE @schema SYSNAME;
    DECLARE @table SYSNAME;

    DECLARE fk_disable_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT SCHEMA_NAME(t.schema_id), t.name
    FROM sys.tables t
    WHERE t.is_ms_shipped = 0;

    OPEN fk_disable_cursor;
    FETCH NEXT FROM fk_disable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'ALTER TABLE ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N' NOCHECK CONSTRAINT ALL;';

        EXEC sp_executesql @sql;

        FETCH NEXT FROM fk_disable_cursor INTO @schema, @table;
    END

    CLOSE fk_disable_cursor;
    DEALLOCATE fk_disable_cursor;

    /* احذف كل جداول المستخدم التي ليست ضمن قائمة الاحتفاظ. */
    DECLARE clear_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT SCHEMA_NAME(t.schema_id), t.name
    FROM sys.tables t
    WHERE t.is_ms_shipped = 0
      AND NOT EXISTS
      (
          SELECT 1
          FROM @Keep k
          WHERE k.TableName = t.name
      );

    OPEN clear_cursor;
    FETCH NEXT FROM clear_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'DELETE FROM ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        EXEC sp_executesql @sql;

        FETCH NEXT FROM clear_cursor INTO @schema, @table;
    END

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /* إعادة ضبط أرقام ID للجداول التي تم تنظيفها فقط. */
    DECLARE identity_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT
        SCHEMA_NAME(t.schema_id),
        t.name
    FROM sys.tables t
    INNER JOIN sys.identity_columns ic
        ON ic.object_id = t.object_id
    WHERE t.is_ms_shipped = 0
      AND NOT EXISTS
      (
          SELECT 1
          FROM @Keep k
          WHERE k.TableName = t.name
      );

    OPEN identity_cursor;
    FETCH NEXT FROM identity_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'DBCC CHECKIDENT (' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N', RESEED, 0) WITH NO_INFOMSGS;';

        EXEC sp_executesql @sql;

        FETCH NEXT FROM identity_cursor INTO @schema, @table;
    END

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* إعادة تفعيل وفحص جميع القيود. */
    DECLARE fk_enable_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT SCHEMA_NAME(t.schema_id), t.name
    FROM sys.tables t
    WHERE t.is_ms_shipped = 0;

    OPEN fk_enable_cursor;
    FETCH NEXT FROM fk_enable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'ALTER TABLE ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
            N' WITH CHECK CHECK CONSTRAINT ALL;';

        EXEC sp_executesql @sql;

        FETCH NEXT FROM fk_enable_cursor INTO @schema, @table;
    END

    CLOSE fk_enable_cursor;
    DEALLOCATE fk_enable_cursor;

    /* تحقق: يجب ألا تبقى أي صفوف في جدول غير محفوظ. */
    DECLARE @RemainingOperational BIGINT = 0;

    DECLARE verify_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT SCHEMA_NAME(t.schema_id), t.name
    FROM sys.tables t
    WHERE t.is_ms_shipped = 0
      AND NOT EXISTS
      (
          SELECT 1
          FROM @Keep k
          WHERE k.TableName = t.name
      );

    OPEN verify_cursor;
    FETCH NEXT FROM verify_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql =
            N'SELECT @CountOut = COUNT_BIG(*) FROM ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        DECLARE @CountOut BIGINT = 0;

        EXEC sp_executesql
            @sql,
            N'@CountOut BIGINT OUTPUT',
            @CountOut = @CountOut OUTPUT;

        SET @RemainingOperational += ISNULL(@CountOut, 0);

        FETCH NEXT FROM verify_cursor INTO @schema, @table;
    END

    CLOSE verify_cursor;
    DEALLOCATE verify_cursor;

    IF @RemainingOperational <> 0
        THROW 52001, N'فشل تنظيف البيانات التشغيلية بالكامل.', 1;

    /* أمان إضافي للبيانات المرجعية. */
    IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM dbo.Subjects
           WHERE ClassID IS NOT NULL
       )
        THROW 52002, N'بقي ربط قديم بين المواد والصفوف.', 1;

    IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Users)
        THROW 52004, N'لم يبق مستخدمون بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Roles)
        THROW 52006, N'لم تبق الأدوار بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        THROW 52005, N'لم تبق الصلاحيات بعد التنظيف.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF CURSOR_STATUS('local', 'fk_disable_cursor') >= 0
    BEGIN
        CLOSE fk_disable_cursor;
        DEALLOCATE fk_disable_cursor;
    END;

    IF CURSOR_STATUS('local', 'clear_cursor') >= 0
    BEGIN
        CLOSE clear_cursor;
        DEALLOCATE clear_cursor;
    END;

    IF CURSOR_STATUS('local', 'identity_cursor') >= 0
    BEGIN
        CLOSE identity_cursor;
        DEALLOCATE identity_cursor;
    END;

    IF CURSOR_STATUS('local', 'fk_enable_cursor') >= 0
    BEGIN
        CLOSE fk_enable_cursor;
        DEALLOCATE fk_enable_cursor;
    END;

    IF CURSOR_STATUS('local', 'verify_cursor') >= 0
    BEGIN
        CLOSE verify_cursor;
        DEALLOCATE verify_cursor;
    END;

    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

/* تقرير نهائي للمرجعيات التي أبقيناها. */
SELECT N'Subjects' AS TableName, COUNT_BIG(*) AS RemainingRows FROM dbo.Subjects
UNION ALL
SELECT N'AcademicYears', COUNT_BIG(*) FROM dbo.AcademicYears
UNION ALL
SELECT N'AcademicTerms', COUNT_BIG(*) FROM dbo.AcademicTerms
UNION ALL
SELECT N'AcademicYearCurrent', COUNT_BIG(*) FROM dbo.AcademicYearCurrent
UNION ALL
SELECT N'Rooms', COUNT_BIG(*) FROM dbo.Rooms
UNION ALL
SELECT N'Users', COUNT_BIG(*) FROM dbo.Users
UNION ALL
SELECT N'Roles', COUNT_BIG(*) FROM dbo.Roles
UNION ALL
SELECT N'Permissions', COUNT_BIG(*) FROM dbo.Permissions
UNION ALL
SELECT N'RolePermissions', COUNT_BIG(*) FROM dbo.RolePermissions
UNION ALL
SELECT N'UserRoles', COUNT_BIG(*) FROM dbo.UserRoles
UNION ALL
SELECT N'UserPermissions', COUNT_BIG(*) FROM dbo.UserPermissions
UNION ALL
SELECT N'SchoolSettings', COUNT_BIG(*) FROM dbo.SchoolSettings
UNION ALL
SELECT N'SchemaMigrations', COUNT_BIG(*) FROM dbo.SchemaMigrations
ORDER BY TableName;
GO
