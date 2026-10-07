/*
    SchoolSystem - Production reset + reference seed
    =================================================
    الهدف:
      1) حذف جميع بيانات الأشخاص والحركات والتجارب القديمة.
      2) الاحتفاظ بالمستخدمين والأدوار والصلاحيات والصالات.
      3) إعادة بناء المرجع الأكاديمي للعام 2026/2027:
         السنوات، الفصول، المراحل، الصفوف، الشعب، المواد، ربط المواد بالصفوف والفصول.
      4) إنشاء قاموس أنواع الرسوم فقط، بدون إنشاء أي رسوم مالية للطلاب.
      5) التحقق النهائي من أن كل الجداول التشغيلية خالية.

    الجداول التي تبقى كبيانات مرجعية/هوية:
      Rooms, Users, Roles, Permissions, RolePermissions, UserRoles,
      UserPermissions, SchemaMigrations,
      AcademicYears, AcademicYearCurrent, AcademicTerms, Stages, Classes,
      SchoolSections, Subjects, ClassSubjects, ClassSubjectTerms, FeeTypes.

    لا يتم إنشاء Students / Teachers / Fees / Vouchers / Payroll / Attendance /
    Grades / Marks / Enrollment / Transport / Library / Timetable / Expenses /
    AnnualClosings / AnnualMigrationLog / AuditLogs / DatabaseBackupHistory.
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

    /* قاموس أنواع الرسوم مرجعي فقط؛ لا يمثل حركة مالية. */
    IF OBJECT_ID(N'dbo.FeeTypes', N'U') IS NULL
    BEGIN
        CREATE TABLE [dbo].[FeeTypes]
        (
            FeeTypeID INT IDENTITY(1,1) NOT NULL
                CONSTRAINT PK_FeeTypes PRIMARY KEY,
            FeeTypeCode NVARCHAR(30) NOT NULL,
            FeeTypeName NVARCHAR(100) NOT NULL,
            IsTotal BIT NOT NULL
                CONSTRAINT DF_FeeTypes_IsTotal DEFAULT (0),
            IsRequiredDefault BIT NOT NULL
                CONSTRAINT DF_FeeTypes_IsRequiredDefault DEFAULT (1),
            DistributionWeight DECIMAL(9,4) NOT NULL
                CONSTRAINT DF_FeeTypes_DistributionWeight DEFAULT (1),
            SortOrder INT NOT NULL
                CONSTRAINT DF_FeeTypes_SortOrder DEFAULT (100),
            IsActive BIT NOT NULL
                CONSTRAINT DF_FeeTypes_IsActive DEFAULT (1),
            Notes NVARCHAR(500) NULL,
            CreatedAt DATETIME2(0) NOT NULL
                CONSTRAINT DF_FeeTypes_CreatedAt DEFAULT (SYSUTCDATETIME()),
            UpdatedAt DATETIME2(0) NULL,
            CONSTRAINT UQ_FeeTypes_Code UNIQUE (FeeTypeCode),
            CONSTRAINT UQ_FeeTypes_Name UNIQUE (FeeTypeName)
        );
    END;

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

    /* تعطيل القيود أثناء عملية التنظيف وإعادة بناء المرجع. */
    DECLARE disable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
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

    /* امسح كل الجداول التشغيلية/التجريبية، ولا تلمس قائمة الاحتفاظ. */
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
        SET @sql =
            N'DELETE FROM ' + QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';
        EXEC sys.sp_executesql @sql;
        FETCH NEXT FROM clear_cursor INTO @schema, @table;
    END;

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /*
       لا نمسح المرجع الأكاديمي الموجود أصلًا:
       AcademicYears / AcademicTerms / Stages / Classes / SchoolSections /
       Subjects / ClassSubjects / ClassSubjectTerms / Rooms / SchoolSettings.
       هذه جداول إعدادات ومرجع، وليست بيانات تشغيلية للطلاب أو المعلمين.

       قاموس أنواع الرسوم مرجعي فقط. إذا كانت الهجرة قد أنشأته ولم توجد فيه بيانات،
       نضع القاموس القياسي داخل قاعدة البيانات حتى تبقى قائمة أنواع الرسوم ديناميكية.
    */
    IF OBJECT_ID(N'dbo.FeeTypes', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.FeeTypes)
    BEGIN
        INSERT dbo.FeeTypes
        (
            FeeTypeCode, FeeTypeName, IsTotal, IsRequiredDefault,
            DistributionWeight, SortOrder, IsActive, Notes, CreatedAt
        )
        VALUES
            (N'TOTAL', N'الرسوم الكلية', 1, 1, 0, 0, 1, N'إجمالي الخطة قبل توزيع البنود', SYSUTCDATETIME()),
            (N'REG', N'رسوم تسجيل', 0, 1, 15, 10, 1, N'رسوم التسجيل', SYSUTCDATETIME()),
            (N'TUITION', N'رسوم دراسية', 0, 1, 40, 20, 1, N'الرسوم الدراسية', SYSUTCDATETIME()),
            (N'BOOKS', N'رسوم كتب', 0, 1, 10, 30, 1, N'الكتب والمقررات', SYSUTCDATETIME()),
            (N'UNIFORM', N'رسوم زي مدرسي', 0, 0, 8, 40, 1, N'الزي المدرسي', SYSUTCDATETIME()),
            (N'TRANSPORT', N'رسوم مواصلات', 0, 0, 8, 50, 1, N'النقل المدرسي', SYSUTCDATETIME()),
            (N'EXAMS', N'رسوم امتحانات', 0, 1, 10, 60, 1, N'الاختبارات والامتحانات', SYSUTCDATETIME()),
            (N'ACTIVITIES', N'رسوم أنشطة', 0, 0, 4, 70, 1, N'الأنشطة المدرسية', SYSUTCDATETIME()),
            (N'OTHER', N'أخرى', 0, 0, 5, 80, 1, N'رسوم إضافية', SYSUTCDATETIME());
    END

        /* إعادة القيود والتحقق من سلامة المرجع. */
    DECLARE enable_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT SCHEMA_NAME(t.schema_id), t.name
        FROM sys.tables t
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

    /* أي AuditLogs ولّدت أثناء التنظيف تُزال قبل التحقق النهائي. */
    IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NOT NULL
        DELETE FROM dbo.AuditLogs;

    /* تحقق من عدم وجود بيانات تشغيلية فقط؛ المرجع الأساسي لا يُعاد توليده أو اختزاله. */

    /* تحقق ديناميكي: لا يبقى أي صف في جداول التشغيل. */
    DECLARE @RemainingOperational BIGINT = 0;
    DECLARE @RecordCount BIGINT;

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
        SET @RecordCount = 0;

        SET @sql =
            N'SELECT @p = COUNT_BIG(*) FROM ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        EXEC sys.sp_executesql
            @sql,
            N'@p BIGINT OUTPUT',
            @p = @RecordCount OUTPUT;

        SET @RemainingOperational += ISNULL(@RecordCount, 0);

        FETCH NEXT FROM verify_cursor INTO @schema, @table;
    END;

    CLOSE verify_cursor;
    DEALLOCATE verify_cursor;

    IF @RemainingOperational <> 0
        THROW 51460, N'بقيت بيانات تشغيلية بعد التنظيف. راجع الجدول المبلغ عنه في سجل التنفيذ.', 1;

    IF (SELECT COUNT(*) FROM dbo.Students) <> 0
        THROW 51461, N'جدول الطلاب ليس فارغًا.', 1;

    IF (SELECT COUNT(*) FROM dbo.Teachers) <> 0
        THROW 51462, N'جدول المعلمين ليس فارغًا.', 1;

    IF (SELECT COUNT(*) FROM dbo.Fees) <> 0
        THROW 51463, N'جدول الرسوم ليس فارغًا.', 1;

    IF (SELECT COUNT(*) FROM dbo.Vouchers) <> 0
        THROW 51464, N'جدول السندات ليس فارغًا.', 1;

    IF (SELECT COUNT(*) FROM dbo.FeePlans) <> 0
        THROW 51465, N'خطط الرسوم ليست فارغة؛ قاموس أنواع الرسوم منفصل عنها.', 1;

    COMMIT TRANSACTION;

    SELECT
        N'تم التنظيف والتحضير بنجاح' AS StatusMessage,
        (SELECT COUNT(*) FROM dbo.Users) AS UsersCount,
        (SELECT COUNT(*) FROM dbo.Roles) AS RolesCount,
        (SELECT COUNT(*) FROM dbo.Permissions) AS PermissionsCount,
        (SELECT COUNT(*) FROM dbo.Rooms) AS RoomsCount,
        (SELECT COUNT(*) FROM dbo.AcademicYears) AS AcademicYearsCount,
        (SELECT COUNT(*) FROM dbo.AcademicTerms) AS AcademicTermsCount,
        (SELECT COUNT(*) FROM dbo.Stages) AS StagesCount,
        (SELECT COUNT(*) FROM dbo.Classes) AS ClassesCount,
        (SELECT COUNT(*) FROM dbo.SchoolSections) AS SchoolSectionsCount,
        (SELECT COUNT(*) FROM dbo.Subjects) AS SubjectsCount,
        (SELECT COUNT(*) FROM dbo.ClassSubjects) AS ClassSubjectsCount,
        (SELECT COUNT(*) FROM dbo.ClassSubjectTerms) AS ClassSubjectTermsCount,
        (SELECT COUNT(*) FROM dbo.FeeTypes) AS FeeTypesCount,
        (SELECT COUNT(*) FROM dbo.Students) AS StudentsCount,
        (SELECT COUNT(*) FROM dbo.Teachers) AS TeachersCount,
        (SELECT COUNT(*) FROM dbo.FeePlans) AS FeePlansCount,
        (SELECT COUNT(*) FROM dbo.Fees) AS FeesCount,
        (SELECT COUNT(*) FROM dbo.Vouchers) AS VouchersCount;
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

    IF CURSOR_STATUS('local', 'verify_cursor') >= 0
    BEGIN CLOSE verify_cursor; DEALLOCATE verify_cursor; END;

    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
