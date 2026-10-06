/*
    SchoolSystem - Production data reset
    ====================================
    الهدف:
      تنظيف قاعدة SchoolDB من كل البيانات التشغيلية/التجريبية مع الاحتفاظ
      فقط بالبيانات المرجعية/الإعدادية التي طلبها المستخدم.

    الجداول المحتفظ بها:
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
      SchoolSettings
      SchemaMigrations

    كل جدول dbo آخر يُعتبر تشغيليًا ويُفرغ بالكامل.
    بذلك تُحذف:
      الطلاب وملفاتهم وتسجيلاتهم وتوزيعهم وحضورهم ودرجاتهم ونقلهم،
      المعلمون وعقودهم ورواتبهم وحضورهم،
      الرسوم وخطط الرسوم والسندات والإيصالات،
      المصروفات والمكتبة والحافلات والجدول الدراسي،
      الإغلاقات السنوية وسجلات الترحيل والتدقيق ونسخ قاعدة البيانات
      وأي جدول تشغيلي جديد أُضيف لاحقًا.

    مهم:
      1) خذ نسخة احتياطية قبل التنفيذ.
      2) هذا السكربت لا يُنفَّذ تلقائيًا عند تشغيل البرنامج.
      3) ملفات PDF/Excel/CSV التي تم تصديرها سابقًا خارج SQL Server
         يجب حذفها من مجلدات التصدير يدويًا.
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
        (N'dbo', N'SchoolSettings'),
        (N'dbo', N'SchemaMigrations');

    DECLARE @sql NVARCHAR(MAX);
    DECLARE @schema SYSNAME;
    DECLARE @table SYSNAME;

    /* تعطيل FK داخل جداول المستخدم مؤقتًا لضمان تنظيف شامل. */
    DECLARE fk_disable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
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
    END

    CLOSE fk_disable_cursor;
    DEALLOCATE fk_disable_cursor;

    /* تفريغ كل الجداول غير المحتفظ بها. */
    DECLARE clear_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
        WHERE t.is_ms_shipped = 0
          AND NOT EXISTS
          (
              SELECT 1
              FROM @Keep k
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
    END

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /* تصفير الهوية لكل جدول تم تنظيفه فقط. */
    DECLARE identity_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
        INNER JOIN sys.identity_columns ic
            ON ic.object_id = t.object_id
        WHERE t.is_ms_shipped = 0
          AND NOT EXISTS
          (
              SELECT 1
              FROM @Keep k
              WHERE k.SchemaName = SCHEMA_NAME(t.schema_id)
                AND k.TableName = t.name
          );

    OPEN identity_cursor;
    FETCH NEXT FROM identity_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql = N'DBCC CHECKIDENT (' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) +
                   N', RESEED, 0) WITH NO_INFOMSGS;';
        EXEC sys.sp_executesql @sql;

        FETCH NEXT FROM identity_cursor INTO @schema, @table;
    END

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* إعادة تفعيل FK والتحقق منها. */
    DECLARE fk_enable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
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
    END

    CLOSE fk_enable_cursor;
    DEALLOCATE fk_enable_cursor;

    /* تحقق صارم: لا توجد أي صفوف في جدول غير محتفظ به. */
    DECLARE @RemainingOperational BIGINT = 0;

    DECLARE verify_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
        WHERE t.is_ms_shipped = 0
          AND NOT EXISTS
          (
              SELECT 1
              FROM @Keep k
              WHERE k.SchemaName = SCHEMA_NAME(t.schema_id)
                AND k.TableName = t.name
          );

    OPEN verify_cursor;
    FETCH NEXT FROM verify_cursor INTO @schema, @table;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql = N'SELECT @CountOut = COUNT_BIG(*) FROM ' +
                   QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        DECLARE @CountOut BIGINT = 0;

        EXEC sys.sp_executesql
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

    /* تحقق من المرجعيات التي يجب أن تبقى. */
    IF NOT EXISTS (SELECT 1 FROM dbo.Users)
        THROW 52004, N'لم يبقَ أي مستخدم بعد التنظيف؛ تم إيقاف العملية حمايةً للدخول.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Roles)
        THROW 52006, N'لم تبقَ الأدوار بعد التنظيف؛ تم إيقاف العملية حمايةً للصلاحيات.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        THROW 52005, N'لم تبقَ الصلاحيات بعد التنظيف؛ تم إيقاف العملية حمايةً لنظام الوصول.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicYears)
        THROW 52007, N'لم تبقَ السنوات الدراسية المرجعية.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicTerms)
        THROW 52008, N'لم تبقَ الفصول الدراسية المرجعية.', 1;

    /* أي علاقة قديمة للمادة بالصف يجب أن تكون قد زالت مع تنظيف الصفوف. */
    IF COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.Subjects WHERE ClassID IS NOT NULL)
        THROW 52009, N'بقي ربط قديم بين مادة وصف بعد التنظيف.', 1;

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

/* تقرير نهائي ديناميكي: يعرض فقط الجداول المرجعية الموجودة فعلياً في القاعدة. */
DECLARE @ReportSql NVARCHAR(MAX) = N'';

SELECT @ReportSql = @ReportSql +
    CASE WHEN @ReportSql = N'' THEN N'' ELSE N' UNION ALL ' END +
    N'SELECT N''' + REPLACE(t.name,'''','''''') + N''' AS TableName, COUNT_BIG(*) AS RemainingRows FROM ' +
    QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
FROM sys.tables t
WHERE t.is_ms_shipped = 0
  AND t.name IN
  (
      N'Subjects',
      N'AcademicYears',
      N'AcademicTerms',
      N'AcademicYearCurrent',
      N'Rooms',
      N'Users',
      N'Roles',
      N'Permissions',
      N'RolePermissions',
      N'UserRoles',
      N'UserPermissions',
      N'SchoolSettings',
      N'SchemaMigrations'
  )
ORDER BY t.name;

IF @ReportSql <> N''
BEGIN
    SET @ReportSql = @ReportSql + N' ORDER BY TableName;';
    EXEC sp_executesql @ReportSql;
END;
GO
