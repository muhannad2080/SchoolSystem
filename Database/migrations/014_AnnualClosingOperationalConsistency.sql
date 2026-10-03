/*
    Migration 014
    توحيد مصادر الإغلاق السنوي والنسخ الاحتياطية وتفعيل العام التالي
    في معاملة واحدة بعد نجاح النسخة الاحتياطية.

    الهدف:
    - استخدام DatabaseBackupHistory كمصدر النسخ الاحتياطي الرسمي.
    - استخدام AcademicYearCurrent كمصدر العام النشط الرسمي.
    - إبقاء الإجراءات القديمة متوافقة مع الخدمة.
    - إضافة CloseAndActivateAcademicYear لضمان عدم بقاء العام المغلق نشطاً
      إذا فشل تغيير العام التالي.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

/* ============================================================
   1) التأكد من وجود الجداول التي تعتمد عليها الإجراءات
   ============================================================ */
IF OBJECT_ID(N'dbo.AcademicYears', N'U') IS NULL
    THROW 51400, N'جدول AcademicYears غير موجود؛ لا يمكن تطبيق Migration 014.', 1;

IF OBJECT_ID(N'dbo.AcademicYearCurrent', N'U') IS NULL
    THROW 51401, N'جدول AcademicYearCurrent غير موجود؛ لا يمكن تطبيق Migration 014.', 1;

IF OBJECT_ID(N'dbo.DatabaseBackupHistory', N'U') IS NULL
    THROW 51402, N'جدول DatabaseBackupHistory غير موجود؛ لا يمكن تطبيق Migration 014.', 1;

IF OBJECT_ID(N'dbo.AnnualClosings', N'U') IS NULL
    THROW 51403, N'جدول AnnualClosings غير موجود؛ لا يمكن تطبيق Migration 014.', 1;

/* ============================================================
   2) توحيد إجراء تحديد العام النشط
   ============================================================ */
IF OBJECT_ID(N'dbo.SetActiveAcademicYear', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.SetActiveAcademicYear AS BEGIN SET NOCOUNT ON; END');

DECLARE @sqlSetActive NVARCHAR(MAX) = N'
ALTER PROCEDURE dbo.SetActiveAcademicYear
    @AcademicYear NVARCHAR(20),
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Year NVARCHAR(20) =
        REPLACE(NULLIF(LTRIM(RTRIM(@AcademicYear)), N''''), N''-'', N''/'');

    IF @Year IS NULL
        THROW 51410, N''العام الدراسي النشط غير صالح.'', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @Year
          AND ClosingStatus IN (N''مغلق'', N''مؤرشف'')
    )
        THROW 51411, N''لا يمكن جعل عام مغلق أو مؤرشف هو العام النشط.'', 1;

    BEGIN TRANSACTION;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYears
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year
    )
    BEGIN
        INSERT dbo.AcademicYears
        (
            AcademicYear, StartDate, EndDate, IsActive, CreatedAt
        )
        VALUES
        (
            @Year, NULL, NULL, 1, GETDATE()
        );
    END
    ELSE
    BEGIN
        UPDATE dbo.AcademicYears
           SET IsActive = 1
         WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year;
    END;

    UPDATE dbo.AcademicYearCurrent
       SET IsCurrent = 0,
           UpdatedAt = GETDATE();

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYearCurrent
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year
    )
    BEGIN
        UPDATE dbo.AcademicYearCurrent
           SET AcademicYear = @Year,
               IsCurrent = 1,
               UpdatedAt = GETDATE()
         WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year;
    END
    ELSE
    BEGIN
        INSERT dbo.AcademicYearCurrent
        (
            AcademicYear, IsCurrent, Notes, CreatedAt, UpdatedAt
        )
        VALUES
        (
            @Year, 1, N''العام النشط المحدد من النظام'', GETDATE(), GETDATE()
        );
    END;

    COMMIT TRANSACTION;

    SELECT @Year AS ActiveAcademicYear;
END;';
EXEC sp_executesql @sqlSetActive;

/* ============================================================
   3) النسخ الاحتياطية: المصدر الرسمي DatabaseBackupHistory
   ============================================================ */
IF OBJECT_ID(N'dbo.RegisterDatabaseBackup', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.RegisterDatabaseBackup AS BEGIN SET NOCOUNT ON; END');

DECLARE @sqlRegisterBackup NVARCHAR(MAX) = N'
ALTER PROCEDURE dbo.RegisterDatabaseBackup
    @BackupFile NVARCHAR(500),
    @BackupType NVARCHAR(50),
    @AcademicYear NVARCHAR(20) = NULL,
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NULLIF(LTRIM(RTRIM(@BackupFile)), N'''') IS NULL
        THROW 51420, N''مسار النسخة الاحتياطية مطلوب.'', 1;

    IF NULLIF(LTRIM(RTRIM(@BackupType)), N'''') IS NULL
        THROW 51421, N''نوع النسخة الاحتياطية مطلوب.'', 1;

    INSERT dbo.DatabaseBackupHistory
    (
        BackupFile, BackupType, AcademicYear,
        CreatedByUserID, CreatedAt, IsVerified
    )
    VALUES
    (
        LTRIM(RTRIM(@BackupFile)),
        LTRIM(RTRIM(@BackupType)),
        NULLIF(REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''''))), N''-'', N''/''), N''''),
        @UserID,
        GETDATE(),
        1
    );

    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS BackupHistoryID;
END;';
EXEC sp_executesql @sqlRegisterBackup;

/* ============================================================
   4) حالة التشغيل: الاعتماد على المصادر الرسمية
   ============================================================ */
IF OBJECT_ID(N'dbo.GetDashboardOperationalStatus', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.GetDashboardOperationalStatus AS BEGIN SET NOCOUNT ON; END');

DECLARE @sqlDashboardStatus NVARCHAR(MAX) = N'
ALTER PROCEDURE dbo.GetDashboardOperationalStatus
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dbo.GetActiveAcademicYear() AS ActiveAcademicYear,
        (SELECT TOP (1) BackupFile
           FROM dbo.DatabaseBackupHistory
          ORDER BY BackupHistoryID DESC) AS LastBackupFile,
        (SELECT TOP (1) CreatedAt
           FROM dbo.DatabaseBackupHistory
          ORDER BY BackupHistoryID DESC) AS LastBackupDate,
        (SELECT TOP (1) AcademicYear
           FROM dbo.AnnualClosings
          ORDER BY ClosingID DESC) AS LastClosingYear,
        (SELECT TOP (1) ClosingStatus
           FROM dbo.AnnualClosings
          ORDER BY ClosingID DESC) AS LastClosingStatus,
        (SELECT TOP (1) VerifiedAt
           FROM dbo.AnnualClosings
          ORDER BY ClosingID DESC) AS LastClosingVerifiedAt;
END;';
EXEC sp_executesql @sqlDashboardStatus;

/* ============================================================
   5) إغلاق العام + تفعيل العام التالي في معاملة واحدة
   ============================================================ */
IF OBJECT_ID(N'dbo.CloseAndActivateAcademicYear', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE dbo.CloseAndActivateAcademicYear AS BEGIN SET NOCOUNT ON; END');

DECLARE @sqlCloseAndActivate NVARCHAR(MAX) = N'
ALTER PROCEDURE dbo.CloseAndActivateAcademicYear
    @AcademicYear NVARCHAR(20),
    @NextAcademicYear NVARCHAR(20),
    @ClosedByUserID INT = NULL,
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Year NVARCHAR(20) =
        REPLACE(NULLIF(LTRIM(RTRIM(@AcademicYear)), N''''), N''-'', N''/'');
    DECLARE @NextYear NVARCHAR(20) =
        REPLACE(NULLIF(LTRIM(RTRIM(@NextAcademicYear)), N''''), N''-'', N''/'');

    IF @Year IS NULL
        THROW 51430, N''العام الدراسي المطلوب إغلاقه غير صالح.'', 1;

    IF @NextYear IS NULL
        THROW 51431, N''العام الدراسي التالي مطلوب لإتمام الإغلاق.'', 1;

    IF @Year = @NextYear
        THROW 51432, N''لا يمكن أن يكون العام التالي مطابقاً للعام المغلق.'', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYears
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear
    )
        THROW 51433, N''العام الدراسي التالي غير موجود. أنشئه أولاً قبل الإغلاق.'', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @NextYear
          AND ClosingStatus IN (N''مغلق'', N''مؤرشف'')
    )
        THROW 51434, N''لا يمكن التفعيل إلى عام مغلق أو مؤرشف.'', 1;

    DECLARE @Critical INT = 0;

    DECLARE @Issues TABLE
    (
        CheckCode NVARCHAR(80) NOT NULL,
        CheckName NVARCHAR(200) NOT NULL,
        IssueCount INT NOT NULL,
        Severity NVARCHAR(20) NOT NULL,
        Details NVARCHAR(500) NULL,
        Result NVARCHAR(20) NULL
    );

    INSERT @Issues
    EXEC dbo.VerifyAnnualClosing @Year;

    SELECT @Critical = COUNT(*)
    FROM @Issues
    WHERE Severity = N''حرج''
      AND IssueCount > 0;

    IF @Critical > 0
        THROW 51435, N''لا يمكن إغلاق العام لوجود أخطاء حرجة. عالج نتائج الفحص أولاً.'', 1;

    BEGIN TRANSACTION;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @Year
          AND ClosingStatus IN (N''مغلق'', N''مؤرشف'')
    )
        THROW 51436, N''العام الدراسي مغلق مسبقاً.'', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @Year
    )
    BEGIN
        UPDATE dbo.AnnualClosings
           SET ClosingStatus = N''مغلق'',
               VerifiedAt = GETDATE(),
               ClosedAt = GETDATE(),
               ClosedByUserID = @ClosedByUserID,
               NextAcademicYear = @NextYear,
               Notes = @Notes,
               UpdatedAt = GETDATE()
         WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @Year;
    END
    ELSE
    BEGIN
        INSERT dbo.AnnualClosings
        (
            AcademicYear, ClosingStatus, VerifiedAt, ClosedAt,
            ClosedByUserID, NextAcademicYear, Notes, CreatedAt, UpdatedAt
        )
        VALUES
        (
            @Year, N''مغلق'', GETDATE(), GETDATE(),
            @ClosedByUserID, @NextYear, @Notes, GETDATE(), GETDATE()
        );
    END;

    /* العام التالي يجب أن يكون جاهزًا للتشغيل */
    UPDATE dbo.AcademicYears
       SET IsActive = CASE
                         WHEN REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear THEN 1
                         ELSE IsActive
                      END
     WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear;

    UPDATE dbo.AcademicYearCurrent
       SET IsCurrent = 0,
           UpdatedAt = GETDATE();

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYearCurrent
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear
    )
    BEGIN
        UPDATE dbo.AcademicYearCurrent
           SET AcademicYear = @NextYear,
               IsCurrent = 1,
               UpdatedAt = GETDATE()
         WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear;
    END
    ELSE
    BEGIN
        INSERT dbo.AcademicYearCurrent
        (
            AcademicYear, IsCurrent, Notes, CreatedAt, UpdatedAt
        )
        VALUES
        (
            @NextYear, 1, N''العام التالي بعد الإغلاق السنوي'', GETDATE(), GETDATE()
        );
    END;

    COMMIT TRANSACTION;

    SELECT
        CAST(1 AS BIT) AS Success,
        @Year AS ClosedAcademicYear,
        @NextYear AS ActiveAcademicYear;
END;';
EXEC sp_executesql @sqlCloseAndActivate;
