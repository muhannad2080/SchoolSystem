/*
    SchoolSystem - Production data reset
    ====================================
    هذا السكربت يمسح كل البيانات التشغيلية/التجريبية ويُبقي فقط
    بيانات المرجع المطلوبة للنظام.

    يحتفظ بـ:
      Subjects
      AcademicYears
      AcademicTerms
      AcademicYearCurrent
      Rooms
      Users
      Roles
      Permissions
      RolePermissions
      UserRoles
      UserPermissions
      SchemaMigrations

    يمسح:
      الطلاب وتسجيلاتهم وتوزيعهم ورسومهم ودرجاتهم وحضورهم ونقلهم
      المعلمين وعقودهم ورواتبهم وحضورهم
      خطط الرسوم والسندات والإيصالات والمصروفات
      المكتبة والكتب والإعارات
      الحافلات وخطوط النقل
      الجدول الدراسي
      الإغلاقات والترحيل وسجل التدقيق وسجل النسخ الاحتياطية
      وإعدادات المدرسة التشغيلية، وأي جدول dbo آخر غير موجود في قائمة الاحتفاظ.

    ملاحظة:
      هذا السكربت لا يحتوي على GO حتى يعمل أيضًا عبر ADO.NET وأدوات
      تنفيذ SQL التي لا تفهم GO.
      لا تُدخل بيانات اختبارية إلى القاعدة.
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
        (N'dbo', N'Subjects'),
        (N'dbo', N'AcademicYears'),
        (N'dbo', N'AcademicTerms'),
        (N'dbo', N'AcademicYearCurrent'),
        (N'dbo', N'Rooms'),
        (N'dbo', N'Users'),
        (N'dbo', N'Roles'),
        (N'dbo', N'Permissions'),
        (N'dbo', N'RolePermissions'),
        (N'dbo', N'UserRoles'),
        (N'dbo', N'UserPermissions'),
        (N'dbo', N'SchemaMigrations');

    /* احفظ تعريف المواد فقط، وافصل أي ClassID قديم قبل حذف الصفوف. */
    IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL
       AND COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NOT NULL
    BEGIN
        UPDATE dbo.Subjects
        SET ClassID = NULL
        WHERE ClassID IS NOT NULL;
    END;

    DECLARE @sql NVARCHAR(MAX);
    DECLARE @schema SYSNAME;
    DECLARE @table SYSNAME;

    /* تعطيل قيود المستخدم مؤقتًا حتى يتم مسح كل البيانات التشغيلية. */
    DECLARE fk_disable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        WHERE t.is_ms_shipped = 0;

    OPEN fk_disable_cursor;
    FETCH NEXT FROM fk_disable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql = N'ALTER TABLE ' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
                   N' NOCHECK CONSTRAINT ALL;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM fk_disable_cursor INTO @schema, @table;
    END;

    CLOSE fk_disable_cursor;
    DEALLOCATE fk_disable_cursor;

    /* امسح كل جدول غير موجود في قائمة الاحتفاظ. */
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
        SET @sql = N'DELETE FROM ' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM clear_cursor INTO @schema, @table;
    END;

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /* إعادة ترقيم جداول التشغيل من الصفر. */
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
        SET @sql = N'DBCC CHECKIDENT (''' +
                   REPLACE(QUOTENAME(@schema) + N'.' + QUOTENAME(@table), N'''', N'''''') +
                   N''', RESEED, 0) WITH NO_INFOMSGS;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM identity_cursor INTO @schema, @table;
    END;

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* إعادة تفعيل القيود وفحصها فعليًا. */
    DECLARE fk_enable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables AS t
        WHERE t.is_ms_shipped = 0;

    OPEN fk_enable_cursor;
    FETCH NEXT FROM fk_enable_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql = N'ALTER TABLE ' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
                   N' WITH CHECK CHECK CONSTRAINT ALL;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM fk_enable_cursor INTO @schema, @table;
    END;

    CLOSE fk_enable_cursor;
    DEALLOCATE fk_enable_cursor;

    /* تحقق نهائي: كل الجداول التشغيلية يجب أن تكون فارغة. */
    DECLARE @RemainingOperational BIGINT = 0;
    DECLARE @CountOut BIGINT;

    DECLARE verify_cursor CURSOR LOCAL FAST_FORWARD FOR
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

    OPEN verify_cursor;
    FETCH NEXT FROM verify_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @CountOut = 0;

        SET @sql = N'SELECT @p = COUNT_BIG(*) FROM ' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        EXEC sys.sp_executesql
            @sql,
            N'@p BIGINT OUTPUT',
            @p = @CountOut OUTPUT;

        SET @RemainingOperational = @RemainingOperational + ISNULL(@CountOut, 0);

        FETCH NEXT FROM verify_cursor INTO @schema, @table;
    END;

    CLOSE verify_cursor;
    DEALLOCATE verify_cursor;

    IF @RemainingOperational <> 0
    BEGIN
        RAISERROR(N'فشل تنظيف البيانات التشغيلية بالكامل.', 16, 1);
    END;

    /* حماية حساب الدخول والصلاحيات والمرجع الأكاديمي. */
    IF OBJECT_ID(N'dbo.Users', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Users)
        RAISERROR(N'لم يبقَ أي مستخدم بعد التنظيف؛ تم إيقاف العملية حمايةً للدخول.', 16, 1);

    IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Roles)
        RAISERROR(N'لم تبقَ الأدوار بعد التنظيف؛ تم إيقاف العملية حمايةً للصلاحيات.', 16, 1);

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        RAISERROR(N'لم تبقَ الصلاحيات بعد التنظيف؛ تم إيقاف العملية حمايةً لنظام الوصول.', 16, 1);

    IF OBJECT_ID(N'dbo.AcademicYears', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.AcademicYears)
        RAISERROR(N'لم تبقَ السنوات الدراسية المرجعية.', 16, 1);

    IF OBJECT_ID(N'dbo.AcademicTerms', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.AcademicTerms)
        RAISERROR(N'لم تبقَ الفصول الدراسية المرجعية.', 16, 1);

    IF COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.Subjects WHERE ClassID IS NOT NULL)
        RAISERROR(N'بقي ربط قديم بين مادة وصف بعد التنظيف.', 16, 1);

    COMMIT TRANSACTION;

    /* تقرير تأكيد نهائي. */
    DECLARE @ReportSql NVARCHAR(MAX) = N'';

    SELECT @ReportSql = @ReportSql +
        CASE WHEN @ReportSql = N'' THEN N'' ELSE N' UNION ALL ' END +
        N'SELECT N''' + REPLACE(t.name,'''','''''') +
        N''' AS TableName, COUNT_BIG(*) AS RemainingRows FROM ' +
        QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
    FROM sys.tables AS t
    WHERE t.is_ms_shipped = 0
      AND EXISTS
      (
          SELECT 1
          FROM @Keep AS k
          WHERE k.SchemaName = SCHEMA_NAME(t.schema_id)
            AND k.TableName = t.name
      );

    IF @ReportSql <> N''
    BEGIN
        SET @ReportSql = @ReportSql + N' ORDER BY TableName;';
        EXEC sys.sp_executesql @ReportSql;
    END;
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
