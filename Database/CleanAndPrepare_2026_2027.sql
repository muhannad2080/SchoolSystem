/*
    SchoolSystem - Production reset + academic reference preparation
    ================================================================
    يمسح جميع البيانات التشغيلية/التجريبية، ثم يعيد بناء المرجع الأكاديمي
    المطلوب للعام 2026/2027:

    يبقى بدون مسح:
      Rooms
      Users
      Roles
      Permissions
      RolePermissions
      UserRoles
      UserPermissions
      SchemaMigrations

    ثم يعاد إنشاء:
      AcademicYears / AcademicYearCurrent
      AcademicTerms
      Stages (3)
      Classes (12)
      Subjects (15)
      ClassSubjects
      ClassSubjectTerms

    لا ينشئ أي:
      Students / Teachers / Fees / Vouchers / Payroll / Attendance /
      Marks / Grades / Enrollment / Transport / Library / Timetable /
      Expenses / Reports أو سجلات تشغيل قديمة.

    ملاحظة:
      السكربت لا يستخدم GO ولا XML .value، ويمكن تشغيله مباشرة في SSMS.
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
        (N'dbo', N'Users'),
        (N'dbo', N'Roles'),
        (N'dbo', N'Permissions'),
        (N'dbo', N'RolePermissions'),
        (N'dbo', N'UserRoles'),
        (N'dbo', N'UserPermissions'),
        (N'dbo', N'SchemaMigrations');

    DECLARE @schema SYSNAME;
    DECLARE @table SYSNAME;
    DECLARE @sql NVARCHAR(MAX);

    /* تعطيل القيود مؤقتًا حتى يمكن تنظيف كل الجداول التشغيلية. */
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
        EXEC sp_executesql @sql;

        FETCH NEXT FROM disable_cursor INTO @schema, @table;
    END;

    CLOSE disable_cursor;
    DEALLOCATE disable_cursor;

    /* حذف كل البيانات غير الموجودة في قائمة الاحتفاظ. */
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
        EXEC sp_executesql @sql;

        FETCH NEXT FROM clear_cursor INTO @schema, @table;
    END;

    CLOSE clear_cursor;
    DEALLOCATE clear_cursor;

    /* إعادة العدادات للجداول التي تم تنظيفها. */
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
        SET @sql =
            N'DBCC CHECKIDENT (' + QUOTENAME(@schema + N'.' + @table, '''') +
            N', RESEED, 0) WITH NO_INFOMSGS;';
        EXEC sp_executesql @sql;

        FETCH NEXT FROM identity_cursor INTO @schema, @table;
    END;

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* إعادة القيود مع فحصها. */
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
        EXEC sp_executesql @sql;

        FETCH NEXT FROM enable_cursor INTO @schema, @table;
    END;

    CLOSE enable_cursor;
    DEALLOCATE enable_cursor;

    /* حماية الحسابات والصلاحيات قبل بناء المرجع الأكاديمي. */
    IF OBJECT_ID(N'dbo.Users', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Users)
        THROW 51450, N'لم يبقَ أي مستخدم بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Roles)
        THROW 51451, N'لم تبقَ الأدوار بعد التنظيف.', 1;

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        THROW 51452, N'لم تبقَ الصلاحيات بعد التنظيف.', 1;

        /* -----------------------------------------------------------------
       1) إعادة بناء المرجع الأكاديمي المطلوب فقط
       ----------------------------------------------------------------- */
    DELETE FROM dbo.ClassSubjectTerms;
    DELETE FROM dbo.ClassSubjects;
    DELETE FROM dbo.Subjects;
    DELETE FROM dbo.Classes;
    DELETE FROM dbo.Stages;

    DELETE FROM dbo.AcademicYearCurrent;
    DELETE FROM dbo.AcademicYears;
    DELETE FROM dbo.AcademicTerms;

    INSERT dbo.AcademicYears
    (
        AcademicYear, StartDate, EndDate, IsActive, CreatedAt
    )
    VALUES
    (
        N'2026/2027',
        CONVERT(date, '2026-09-01'),
        CONVERT(date, '2027-06-30'),
        1,
        SYSUTCDATETIME()
    );

    INSERT dbo.AcademicYearCurrent
    (
        AcademicYear, StartDate, EndDate, IsCurrent, Notes, CreatedAt, UpdatedAt
    )
    VALUES
    (
        N'2026/2027',
        CONVERT(date, '2026-09-01'),
        CONVERT(date, '2027-06-30'),
        1,
        N'العام التشغيلي الحالي',
        GETDATE(),
        GETDATE()
    );

    INSERT dbo.AcademicTerms
    (
        TermCode, TermName, TermOrder, IsActive, CreatedAt
    )
    VALUES
        (N'TERM-1', N'الفصل الدراسي الأول', 1, 1, SYSUTCDATETIME()),
        (N'TERM-2', N'الفصل الدراسي الثاني', 2, 1, SYSUTCDATETIME());

    /* -----------------------------------------------------------------
       2) المراحل الثلاث
       ----------------------------------------------------------------- */
    INSERT dbo.Stages
    (
        StageCode, StageName, StageOrder, IsActive, CreatedAt
    )
    VALUES
        (N'PRIMARY',   N'المرحلة الابتدائية', 1, 1, SYSUTCDATETIME()),
        (N'MIDDLE',    N'المرحلة الإعدادية', 2, 1, SYSUTCDATETIME()),
        (N'SECONDARY', N'المرحلة الثانوية',   3, 1, SYSUTCDATETIME());

    /* -----------------------------------------------------------------
       3) الصفوف من الأول الأساسي إلى الثالث الثانوي
       GradeOrder عالمي = 1..12 لضمان ترتيب ثابت في النظام.
       ----------------------------------------------------------------- */
    INSERT dbo.Classes
    (
        ClassCode, ClassName, StageName, StageID, GradeOrder, IsActive, Notes, CreatedAt
    )
    SELECT
        v.ClassCode,
        v.ClassName,
        s.StageName,
        s.StageID,
        v.GradeOrder,
        1,
        N'صف تشغيلي مرجعي للعام 2026/2027',
        GETDATE()
    FROM
    (
        VALUES
            (N'PRI-01', N'الأول الأساسي',   1, N'PRIMARY'),
            (N'PRI-02', N'الثاني الأساسي',  2, N'PRIMARY'),
            (N'PRI-03', N'الثالث الأساسي',  3, N'PRIMARY'),
            (N'PRI-04', N'الرابع الأساسي',  4, N'PRIMARY'),
            (N'PRI-05', N'الخامس الأساسي',  5, N'PRIMARY'),
            (N'PRI-06', N'السادس الأساسي',  6, N'PRIMARY'),

            (N'MID-07', N'السابع الإعدادي',  7, N'MIDDLE'),
            (N'MID-08', N'الثامن الإعدادي',  8, N'MIDDLE'),
            (N'MID-09', N'التاسع الإعدادي',  9, N'MIDDLE'),

            (N'SEC-10', N'الأول الثانوي',    10, N'SECONDARY'),
            (N'SEC-11', N'الثاني الثانوي',   11, N'SECONDARY'),
            (N'SEC-12', N'الثالث الثانوي',   12, N'SECONDARY')
    ) v(ClassCode, ClassName, GradeOrder, StageCode)
    INNER JOIN dbo.Stages s
        ON s.StageCode = v.StageCode;

    /* -----------------------------------------------------------------
       4) قاموس المواد الموحد
       ClassID = NULL عمدًا: المادة كيان مرجعي واحد،
       والتخصيص الفعلي للصف يتم في ClassSubjects.
       ----------------------------------------------------------------- */
    INSERT dbo.Subjects
    (
        SubjectCode, SubjectName, ClassID, MaxDegree, PassDegree, IsActive, Notes, CreatedAt
    )
    VALUES
        (N'SUB-QUR',  N'القرآن الكريم',             NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-ISL',  N'التربية الإسلامية',         NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-ARB',  N'اللغة العربية',             NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-MAT',  N'الرياضيات',                 NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-ENG',  N'اللغة الإنجليزية',          NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-SCI',  N'العلوم',                    NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-SOC',  N'الدراسات الاجتماعية',       NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-CMP',  N'الحاسوب',                   NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-ART',  N'التربية الفنية',            NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-PE',   N'التربية البدنية',           NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-PHY',  N'الفيزياء',                  NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-CHM',  N'الكيمياء',                  NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-BIO',  N'الأحياء',                   NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-HIS',  N'التاريخ',                   NULL, 100, 50, 1, N'مادة مرجعية', GETDATE()),
        (N'SUB-GEO',  N'الجغرافيا',                NULL, 100, 50, 1, N'مادة مرجعية', GETDATE());

    /* -----------------------------------------------------------------
       5) تحديد المواد التابعة لكل مرحلة/صف.
       ----------------------------------------------------------------- */
    DECLARE @Curriculum TABLE
    (
        GradeOrder INT NOT NULL,
        SubjectCode NVARCHAR(30) NOT NULL,
        PRIMARY KEY (GradeOrder, SubjectCode)
    );

    /* الأول إلى الثالث الأساسي */
    INSERT @Curriculum (GradeOrder, SubjectCode)
    VALUES
        (1,N'SUB-QUR'), (1,N'SUB-ISL'), (1,N'SUB-ARB'), (1,N'SUB-MAT'),
        (1,N'SUB-SCI'), (1,N'SUB-ENG'), (1,N'SUB-PE'),  (1,N'SUB-ART'),

        (2,N'SUB-QUR'), (2,N'SUB-ISL'), (2,N'SUB-ARB'), (2,N'SUB-MAT'),
        (2,N'SUB-SCI'), (2,N'SUB-ENG'), (2,N'SUB-PE'),  (2,N'SUB-ART'),

        (3,N'SUB-QUR'), (3,N'SUB-ISL'), (3,N'SUB-ARB'), (3,N'SUB-MAT'),
        (3,N'SUB-SCI'), (3,N'SUB-ENG'), (3,N'SUB-PE'),  (3,N'SUB-ART');

    /* الرابع إلى السادس الأساسي */
    INSERT @Curriculum (GradeOrder, SubjectCode)
    VALUES
        (4,N'SUB-QUR'), (4,N'SUB-ISL'), (4,N'SUB-ARB'), (4,N'SUB-MAT'),
        (4,N'SUB-SCI'), (4,N'SUB-ENG'), (4,N'SUB-SOC'), (4,N'SUB-CMP'),
        (4,N'SUB-PE'),  (4,N'SUB-ART'),

        (5,N'SUB-QUR'), (5,N'SUB-ISL'), (5,N'SUB-ARB'), (5,N'SUB-MAT'),
        (5,N'SUB-SCI'), (5,N'SUB-ENG'), (5,N'SUB-SOC'), (5,N'SUB-CMP'),
        (5,N'SUB-PE'),  (5,N'SUB-ART'),

        (6,N'SUB-QUR'), (6,N'SUB-ISL'), (6,N'SUB-ARB'), (6,N'SUB-MAT'),
        (6,N'SUB-SCI'), (6,N'SUB-ENG'), (6,N'SUB-SOC'), (6,N'SUB-CMP'),
        (6,N'SUB-PE'),  (6,N'SUB-ART');

    /* السابع إلى التاسع الإعدادي */
    INSERT @Curriculum (GradeOrder, SubjectCode)
    VALUES
        (7,N'SUB-QUR'), (7,N'SUB-ISL'), (7,N'SUB-ARB'), (7,N'SUB-MAT'),
        (7,N'SUB-SCI'), (7,N'SUB-ENG'), (7,N'SUB-SOC'), (7,N'SUB-CMP'),
        (7,N'SUB-PE'),

        (8,N'SUB-QUR'), (8,N'SUB-ISL'), (8,N'SUB-ARB'), (8,N'SUB-MAT'),
        (8,N'SUB-SCI'), (8,N'SUB-ENG'), (8,N'SUB-SOC'), (8,N'SUB-CMP'),
        (8,N'SUB-PE'),

        (9,N'SUB-QUR'), (9,N'SUB-ISL'), (9,N'SUB-ARB'), (9,N'SUB-MAT'),
        (9,N'SUB-SCI'), (9,N'SUB-ENG'), (9,N'SUB-SOC'), (9,N'SUB-CMP'),
        (9,N'SUB-PE');

    /* الأول إلى الثالث الثانوي */
    INSERT @Curriculum (GradeOrder, SubjectCode)
    VALUES
        (10,N'SUB-QUR'), (10,N'SUB-ISL'), (10,N'SUB-ARB'), (10,N'SUB-MAT'),
        (10,N'SUB-ENG'), (10,N'SUB-PHY'), (10,N'SUB-CHM'), (10,N'SUB-BIO'),
        (10,N'SUB-CMP'), (10,N'SUB-HIS'), (10,N'SUB-GEO'), (10,N'SUB-PE'),

        (11,N'SUB-QUR'), (11,N'SUB-ISL'), (11,N'SUB-ARB'), (11,N'SUB-MAT'),
        (11,N'SUB-ENG'), (11,N'SUB-PHY'), (11,N'SUB-CHM'), (11,N'SUB-BIO'),
        (11,N'SUB-CMP'), (11,N'SUB-HIS'), (11,N'SUB-GEO'), (11,N'SUB-PE'),

        (12,N'SUB-QUR'), (12,N'SUB-ISL'), (12,N'SUB-ARB'), (12,N'SUB-MAT'),
        (12,N'SUB-ENG'), (12,N'SUB-PHY'), (12,N'SUB-CHM'), (12,N'SUB-BIO'),
        (12,N'SUB-CMP'), (12,N'SUB-HIS'), (12,N'SUB-GEO'), (12,N'SUB-PE');

    /* -----------------------------------------------------------------
       6) ربط المادة بالصف.
       ----------------------------------------------------------------- */
    INSERT dbo.ClassSubjects
    (
        ClassID, SubjectID, IsActive, CreatedAt
    )
    SELECT
        c.ClassID,
        s.SubjectID,
        1,
        SYSUTCDATETIME()
    FROM @Curriculum x
    INNER JOIN dbo.Classes c
        ON c.GradeOrder = x.GradeOrder
       AND ISNULL(c.IsActive, 1) = 1
    INNER JOIN dbo.Subjects s
        ON s.SubjectCode = x.SubjectCode
       AND ISNULL(s.IsActive, 1) = 1;

    /* -----------------------------------------------------------------
       7) ربط كل مادة بالترم الأول والثاني.
       ----------------------------------------------------------------- */
    INSERT dbo.ClassSubjectTerms
    (
        ClassSubjectID, TermID, IsActive, CreatedAt
    )
    SELECT
        cs.ClassSubjectID,
        t.TermID,
        1,
        SYSUTCDATETIME()
    FROM dbo.ClassSubjects cs
    CROSS JOIN dbo.AcademicTerms t
    WHERE ISNULL(cs.IsActive, 1) = 1
      AND ISNULL(t.IsActive, 1) = 1;

    /* -----------------------------------------------------------------
       8) تحقق تشغيلي صارم قبل الاعتماد.
       ----------------------------------------------------------------- */
    IF (SELECT COUNT(*) FROM dbo.Stages WHERE IsActive = 1) <> 3
        THROW 51410, N'يجب أن تكون المراحل النشطة 3 مراحل بالضبط.', 1;

    IF (SELECT COUNT(*) FROM dbo.Classes WHERE IsActive = 1) <> 12
        THROW 51411, N'يجب أن تكون الصفوف النشطة 12 صفًا بالضبط.', 1;

    IF (SELECT COUNT(*) FROM dbo.AcademicTerms WHERE IsActive = 1) <> 2
        THROW 51412, N'يجب أن يكون هناك فصلان دراسيان نشطان.', 1;

    IF (SELECT COUNT(*) FROM dbo.Subjects WHERE IsActive = 1) <> 15
        THROW 51413, N'عدد المواد المرجعية غير متوقع.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.ClassSubjects cs
        LEFT JOIN dbo.Classes c ON c.ClassID = cs.ClassID
        LEFT JOIN dbo.Subjects s ON s.SubjectID = cs.SubjectID
        WHERE c.ClassID IS NULL OR s.SubjectID IS NULL
    )
        THROW 51414, N'يوجد ربط مادة/صف غير صالح.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.ClassSubjects cs
        WHERE
        (
            SELECT COUNT(*)
            FROM dbo.ClassSubjectTerms cst
            INNER JOIN dbo.AcademicTerms t ON t.TermID = cst.TermID
            WHERE cst.ClassSubjectID = cs.ClassSubjectID
              AND ISNULL(cst.IsActive, 1) = 1
              AND ISNULL(t.IsActive, 1) = 1
        ) <> 2
    )
        THROW 51415, N'هناك مادة مرتبطة بصف لا تعمل في الترمين الأول والثاني.', 1;

    IF EXISTS (SELECT 1 FROM dbo.AcademicYears WHERE IsActive = 1 AND AcademicYear <> N'2026/2027')
        THROW 51416, N'يوجد عام دراسي نشط غير العام 2026/2027.', 1;



    /* لا تسمح ببقاء أي سجل في الجداول التشغيلية التي تم تنظيفها. */
    DECLARE @RemainingOperational BIGINT = 0;
    DECLARE @CountOut BIGINT;

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
        SET @CountOut = 0;

        SET @sql =
            N'SELECT @p = COUNT_BIG(*) FROM ' +
            QUOTENAME(@schema) + N'.' + QUOTENAME(@table) + N';';

        EXEC sp_executesql
            @sql,
            N'@p BIGINT OUTPUT',
            @p = @CountOut OUTPUT;

        SET @RemainingOperational =
            @RemainingOperational + ISNULL(@CountOut, 0);

        FETCH NEXT FROM verify_cursor INTO @schema, @table;
    END;

    CLOSE verify_cursor;
    DEALLOCATE verify_cursor;

    IF @RemainingOperational <> 0
        THROW 51460, N'بقيت بيانات تشغيلية بعد التنظيف.', 1;

    COMMIT TRANSACTION;

    SELECT
        N'تنظيف وتحضير مكتمل' AS StatusMessage,
        (SELECT COUNT(*) FROM dbo.Stages WHERE IsActive = 1) AS ActiveStages,
        (SELECT COUNT(*) FROM dbo.Classes WHERE IsActive = 1) AS ActiveClasses,
        (SELECT COUNT(*) FROM dbo.Subjects WHERE IsActive = 1) AS ActiveSubjects,
        (SELECT COUNT(*) FROM dbo.ClassSubjects WHERE IsActive = 1) AS ClassSubjectLinks,
        (SELECT COUNT(*) FROM dbo.ClassSubjectTerms WHERE IsActive = 1) AS ClassSubjectTermLinks,
        (SELECT COUNT(*) FROM dbo.AcademicTerms WHERE IsActive = 1) AS ActiveTerms,
        (SELECT COUNT(*) FROM dbo.AcademicYears WHERE IsActive = 1) AS ActiveAcademicYears,
        (SELECT COUNT(*) FROM dbo.Rooms) AS RoomsCount,
        (SELECT COUNT(*) FROM dbo.Users) AS UsersCount,
        (SELECT COUNT(*) FROM dbo.Roles) AS RolesCount,
        (SELECT COUNT(*) FROM dbo.Permissions) AS PermissionsCount,
        (SELECT COUNT(*) FROM dbo.Students) AS StudentsCount,
        (SELECT COUNT(*) FROM dbo.Teachers) AS TeachersCount,
        (SELECT COUNT(*) FROM dbo.Fees) AS FeesCount,
        (SELECT COUNT(*) FROM dbo.Vouchers) AS VouchersCount;
END TRY
BEGIN CATCH
    IF CURSOR_STATUS('local', 'disable_cursor') >= 0
    BEGIN
        CLOSE disable_cursor;
        DEALLOCATE disable_cursor;
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

    IF CURSOR_STATUS('local', 'enable_cursor') >= 0
    BEGIN
        CLOSE enable_cursor;
        DEALLOCATE enable_cursor;
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
