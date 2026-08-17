/*
    SchoolSystem - Initial academic catalog seed
    الهدف: تهيئة الصفوف من الأول إلى الثالث الإعدادي ومن الأول إلى الثالث الثانوي والمواد الأساسية لكل صف.
    آمن للتشغيل المتكرر: لا يحذف بيانات ولا يكرر الصفوف أو المواد الموجودة.
    شغّل هذا الملف بعد Migration_MissingApplicationTables.sql.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 50020, N'قاعدة SchoolDB غير موجودة. أنشئها أو نفّذ migrations الأساسية أولاً.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
    THROW 50021, N'جدول Classes غير موجود. نفّذ Migration_MissingApplicationTables.sql أولاً.', 1;

IF OBJECT_ID(N'dbo.Subjects', N'U') IS NULL
    THROW 50022, N'جدول Subjects غير موجود. نفّذ Migration_MissingApplicationTables.sql أولاً.', 1;
GO

/* توافق الأعمدة المطلوبة في installations القديمة. */
IF COL_LENGTH(N'dbo.Classes', N'ClassCode') IS NULL
    ALTER TABLE dbo.Classes ADD ClassCode NVARCHAR(30) NULL;
IF COL_LENGTH(N'dbo.Classes', N'StageName') IS NULL
    ALTER TABLE dbo.Classes ADD StageName NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.Classes', N'GradeOrder') IS NULL
    ALTER TABLE dbo.Classes ADD GradeOrder INT NOT NULL CONSTRAINT DF_Seed_Classes_GradeOrder DEFAULT 0 WITH VALUES;
IF COL_LENGTH(N'dbo.Classes', N'IsActive') IS NULL
    ALTER TABLE dbo.Classes ADD IsActive BIT NOT NULL CONSTRAINT DF_Seed_Classes_IsActive DEFAULT 1 WITH VALUES;
IF COL_LENGTH(N'dbo.Classes', N'Notes') IS NULL
    ALTER TABLE dbo.Classes ADD Notes NVARCHAR(MAX) NULL;

IF COL_LENGTH(N'dbo.Subjects', N'SubjectCode') IS NULL
    ALTER TABLE dbo.Subjects ADD SubjectCode NVARCHAR(30) NULL;
IF COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NULL
    ALTER TABLE dbo.Subjects ADD ClassID INT NULL;
IF COL_LENGTH(N'dbo.Subjects', N'MaxDegree') IS NULL
    ALTER TABLE dbo.Subjects ADD MaxDegree DECIMAL(10,2) NOT NULL CONSTRAINT DF_Seed_Subjects_MaxDegree DEFAULT 100 WITH VALUES;
IF COL_LENGTH(N'dbo.Subjects', N'PassDegree') IS NULL
    ALTER TABLE dbo.Subjects ADD PassDegree DECIMAL(10,2) NOT NULL CONSTRAINT DF_Seed_Subjects_PassDegree DEFAULT 50 WITH VALUES;
IF COL_LENGTH(N'dbo.Subjects', N'IsActive') IS NULL
    ALTER TABLE dbo.Subjects ADD IsActive BIT NOT NULL CONSTRAINT DF_Seed_Subjects_IsActive DEFAULT 1 WITH VALUES;
IF COL_LENGTH(N'dbo.Subjects', N'Notes') IS NULL
    ALTER TABLE dbo.Subjects ADD Notes NVARCHAR(MAX) NULL;
IF COL_LENGTH(N'dbo.Subjects', N'CreatedAt') IS NULL
    ALTER TABLE dbo.Subjects ADD CreatedAt DATETIME NOT NULL CONSTRAINT DF_Seed_Subjects_CreatedAt DEFAULT GETDATE() WITH VALUES;
IF COL_LENGTH(N'dbo.Subjects', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Subjects ADD UpdatedAt DATETIME NULL;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* الصفوف الأساسية: الأول إلى الثالث الإعدادي والأول إلى الثالث الثانوي. */
    DECLARE @Classes TABLE
    (
        ClassCode NVARCHAR(30) NOT NULL,
        ClassName NVARCHAR(100) NOT NULL,
        StageName NVARCHAR(100) NOT NULL,
        GradeOrder INT NOT NULL
    );

    INSERT INTO @Classes (ClassCode, ClassName, StageName, GradeOrder)
    VALUES
        (N'PREP-01', N'الأول الإعدادي', N'المرحلة الإعدادية', 1),
        (N'PREP-02', N'الثاني الإعدادي', N'المرحلة الإعدادية', 2),
        (N'PREP-03', N'الثالث الإعدادي', N'المرحلة الإعدادية', 3),
        (N'SEC-01', N'الأول الثانوي', N'المرحلة الثانوية', 10),
        (N'SEC-02', N'الثاني الثانوي', N'المرحلة الثانوية', 11),
        (N'SEC-03', N'الثالث الثانوي', N'المرحلة الثانوية', 12);

    /* تحديث السجلات المطابقة وإضافة المفقود فقط. */
    UPDATE c
       SET c.ClassCode = s.ClassCode,
           c.StageName = s.StageName,
           c.GradeOrder = s.GradeOrder,
           c.IsActive = 1,
           c.UpdatedAt = GETDATE()
    FROM dbo.Classes c
    INNER JOIN @Classes s ON s.ClassName = c.ClassName;

    INSERT INTO dbo.Classes (ClassCode, ClassName, StageName, GradeOrder, IsActive, Notes)
    SELECT s.ClassCode, s.ClassName, s.StageName, s.GradeOrder, 1,
           N'بيانات أولية للكتالوج الأكاديمي - يمكن تعديلها من شاشة إدارة الفصول.'
    FROM @Classes s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Classes c
        WHERE c.ClassName = s.ClassName
           OR c.ClassCode = s.ClassCode
    );

    /* المواد الأساسية المشتركة للصفوف الإعدادية والثانوية؛ يمكن تخصيصها لاحقًا حسب المرحلة. */
    DECLARE @Subjects TABLE
    (
        SubjectCodeSuffix NVARCHAR(10) NOT NULL,
        SubjectName NVARCHAR(100) NOT NULL,
        MaxDegree DECIMAL(10,2) NOT NULL,
        PassDegree DECIMAL(10,2) NOT NULL
    );

    INSERT INTO @Subjects (SubjectCodeSuffix, SubjectName, MaxDegree, PassDegree)
    VALUES
        (N'AR', N'اللغة العربية', 100, 50),
        (N'EN', N'اللغة الإنجليزية', 100, 50),
        (N'MA', N'الرياضيات', 100, 50),
        (N'PH', N'الفيزياء', 100, 50),
        (N'CH', N'الكيمياء', 100, 50),
        (N'BI', N'الأحياء', 100, 50),
        (N'IS', N'التربية الإسلامية', 100, 50),
        (N'SO', N'الدراسات الاجتماعية', 100, 50),
        (N'CO', N'الحاسب الآلي وتقنية المعلومات', 100, 50),
        (N'LI', N'المهارات الحياتية', 100, 50),
        (N'PE', N'التربية البدنية والصحية', 100, 50);

    /* تحديث إعدادات المادة إن كانت موجودة بالاسم داخل الصف، ثم إدخال المفقود. */
    UPDATE sub
       SET sub.SubjectCode = c.ClassCode + N'-' + s.SubjectCodeSuffix,
           sub.MaxDegree = s.MaxDegree,
           sub.PassDegree = s.PassDegree,
           sub.IsActive = 1,
           sub.UpdatedAt = GETDATE()
    FROM dbo.Subjects sub
    INNER JOIN dbo.Classes c ON c.ClassID IS NOT NULL
    INNER JOIN @Subjects s ON s.SubjectName = sub.SubjectName
    WHERE sub.ClassID = c.ClassID
      AND c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03');

    INSERT INTO dbo.Subjects
    (
        SubjectCode, SubjectName, ClassID, MaxDegree, PassDegree,
        IsActive, Notes, CreatedAt
    )
    SELECT
        c.ClassCode + N'-' + s.SubjectCodeSuffix,
        s.SubjectName,
        c.ClassID,
        s.MaxDegree,
        s.PassDegree,
        1,
        N'مادة أولية مرتبطة بالصف ' + c.ClassName,
        GETDATE()
    FROM dbo.Classes c
    CROSS JOIN @Subjects s
    WHERE c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03')
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.Subjects existing
          WHERE existing.ClassID = c.ClassID
            AND existing.SubjectName = s.SubjectName
      );

    COMMIT TRANSACTION;

    SELECT
        c.ClassID,
        c.ClassCode,
        c.ClassName,
        c.StageName,
        c.GradeOrder,
        COUNT(s.SubjectID) AS SubjectCount
    FROM dbo.Classes c
    LEFT JOIN dbo.Subjects s ON s.ClassID = c.ClassID AND ISNULL(s.IsActive, 1) = 1
    WHERE c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03')
    GROUP BY c.ClassID, c.ClassCode, c.ClassName, c.StageName, c.GradeOrder
    ORDER BY c.GradeOrder;

    PRINT N'تمت تهيئة الصفوف والمواد بنجاح. كل صف إعدادي أو ثانوي يجب أن يحتوي على 11 مادة نشطة.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* تحقق نهائي مختصر. */
SELECT
    c.ClassName,
    COUNT(s.SubjectID) AS SubjectCount
FROM dbo.Classes c
LEFT JOIN dbo.Subjects s ON s.ClassID = c.ClassID AND ISNULL(s.IsActive, 1) = 1
    WHERE c.ClassCode IN (N'PREP-01', N'PREP-02', N'PREP-03', N'SEC-01', N'SEC-02', N'SEC-03')
GROUP BY c.ClassName, c.GradeOrder
ORDER BY c.GradeOrder;
GO

/*
    ملاحظة: إذا كانت النسخة الحالية من Subjects لا تحتوي على ClassID بعد،
    شغّل Migration_MissingApplicationTables.sql أولاً ثم أعد تشغيل هذا الملف.
*/
