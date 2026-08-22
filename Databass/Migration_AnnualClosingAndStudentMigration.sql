/*
    SchoolSystem - الإغلاق السنوي والترحيل
    هذا الملف آمن لإعادة التشغيل ولا يحذف بيانات السنوات السابقة.
    يجب تشغيله على قاعدة SchoolDB بعد تطبيق مخططات التشغيل الأساسية.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.AnnualClosings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AnnualClosings
    (
        ClosingID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AnnualClosings PRIMARY KEY,
        AcademicYear NVARCHAR(20) NOT NULL,
        ClosingStatus NVARCHAR(20) NOT NULL CONSTRAINT DF_AnnualClosings_Status DEFAULT N'مفتوح',
        VerifiedAt DATETIME NULL,
        ClosedAt DATETIME NULL,
        ClosedByUserID INT NULL,
        NextAcademicYear NVARCHAR(20) NULL,
        Notes NVARCHAR(1000) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AnnualClosings_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL,
        CONSTRAINT CK_AnnualClosings_Status CHECK (ClosingStatus IN (N'مفتوح', N'مغلق', N'مؤرشف')),
        CONSTRAINT CK_AnnualClosings_Year CHECK (NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.AnnualClosings')
      AND name = N'UX_AnnualClosings_AcademicYear'
)
BEGIN
    CREATE UNIQUE INDEX UX_AnnualClosings_AcademicYear
        ON dbo.AnnualClosings(AcademicYear);
END;
GO

IF OBJECT_ID(N'dbo.AnnualMigrationLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AnnualMigrationLog
    (
        MigrationID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AnnualMigrationLog PRIMARY KEY,
        StudentID INT NOT NULL,
        FromAcademicYear NVARCHAR(20) NOT NULL,
        ToAcademicYear NVARCHAR(20) NOT NULL,
        FromClassID INT NULL,
        FromSection NVARCHAR(50) NULL,
        ToClassID INT NULL,
        ToSection NVARCHAR(50) NULL,
        MigrationStatus NVARCHAR(20) NOT NULL CONSTRAINT DF_AnnualMigrationLog_Status DEFAULT N'مخطط',
        CreatedByUserID INT NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_AnnualMigrationLog_CreatedAt DEFAULT GETDATE(),
        Notes NVARCHAR(1000) NULL,
        CONSTRAINT CK_AnnualMigrationLog_Status CHECK (MigrationStatus IN (N'مخطط', N'منفذ', N'مستبعد', N'فشل'))
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.AnnualMigrationLog')
      AND name = N'UX_AnnualMigrationLog_StudentYears'
)
BEGIN
    CREATE UNIQUE INDEX UX_AnnualMigrationLog_StudentYears
        ON dbo.AnnualMigrationLog(StudentID, FromAcademicYear, ToAcademicYear);
END;
GO

IF OBJECT_ID(N'dbo.VerifyAnnualClosing', N'P') IS NOT NULL
    DROP PROCEDURE dbo.VerifyAnnualClosing;
GO
CREATE PROCEDURE dbo.VerifyAnnualClosing
    @AcademicYear NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET @AcademicYear = REPLACE(LTRIM(RTRIM(@AcademicYear)), N'-', N'/');
    IF NULLIF(@AcademicYear, N'') IS NULL
        THROW 51020, N'يجب تحديد العام الدراسي قبل الفحص.', 1;

    DECLARE @Issues TABLE
    (
        CheckCode NVARCHAR(80) NOT NULL,
        CheckName NVARCHAR(200) NOT NULL,
        IssueCount INT NOT NULL,
        Severity NVARCHAR(20) NOT NULL,
        Details NVARCHAR(500) NULL
    );

    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'STUDENT_ASSIGNMENT_DUPLICATE', N'توزيع الطالب المكرر في العام',
         ISNULL((SELECT COUNT(*) FROM (SELECT StudentID FROM dbo.StudentClasses WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear GROUP BY StudentID HAVING COUNT(*)>1) x),0), N'حرج', N'يجب أن يكون لكل طالب توزيع فعّال واحد في العام.'),
        (N'STUDENT_ASSIGNMENT_ORPHAN', N'توزيع يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.StudentClasses sc LEFT JOIN dbo.Students s ON s.StudentID=sc.StudentID WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'سجل توزيع بلا طالب.'),
        (N'STUDENT_ASSIGNMENT_ORPHAN_CLASS', N'توزيع يشير إلى صف غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.StudentClasses sc LEFT JOIN dbo.Classes c ON c.ClassID=sc.ClassID WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@AcademicYear AND c.ClassID IS NULL),0), N'حرج', N'سجل توزيع بلا صف.');
    END;

    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'ENROLLMENT_ORPHAN', N'تسجيل يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.Enrollments e LEFT JOIN dbo.Students s ON s.StudentID=e.StudentID WHERE REPLACE(ISNULL(e.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'سجل تسجيل بلا طالب.'),
        (N'ENROLLMENT_DUPLICATE', N'التسجيل المكرر للطالب في العام',
         ISNULL((SELECT COUNT(*) FROM (SELECT StudentID FROM dbo.Enrollments WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear GROUP BY StudentID HAVING COUNT(*)>1) x),0), N'حرج', N'يجب مراجعة التسجيلات المكررة قبل الإغلاق.');
    END;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'FEE_ORPHAN', N'رسم يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.Students s ON s.StudentID=f.StudentID WHERE REPLACE(ISNULL(f.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'الرسم بلا طالب.'),
        (N'FEE_NEGATIVE', N'مبلغ رسم سالب أو صافي غير صحيح',
         ISNULL((SELECT COUNT(*) FROM dbo.Fees WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear AND (ISNULL(TotalAmount,0)<0 OR ISNULL(PaidAmount,0)<0 OR ISNULL(RemainingAmount,0)<0 OR ISNULL(PaidAmount,0)>ISNULL(NetAmount,ISNULL(TotalAmount,0)))),0), N'حرج', N'مراجعة المبالغ والدفعات.');
    END;

    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'VOUCHER_DUPLICATE_NUMBER', N'رقم السند المكرر',
         ISNULL((SELECT COUNT(*) FROM (SELECT VoucherNumber FROM dbo.Vouchers WHERE NULLIF(LTRIM(RTRIM(VoucherNumber)),N'') IS NOT NULL GROUP BY VoucherNumber HAVING COUNT(*)>1) x),0), N'حرج', N'رقم السند يجب أن يكون فريداً.');
    END;

    SELECT CheckCode, CheckName, IssueCount, Severity, Details,
           CASE WHEN IssueCount=0 THEN N'سليم' ELSE N'يحتاج معالجة' END AS Result
    FROM @Issues ORDER BY CASE Severity WHEN N'حرج' THEN 1 ELSE 2 END, CheckCode;
END;
GO

IF OBJECT_ID(N'dbo.CloseAcademicYear', N'P') IS NOT NULL
    DROP PROCEDURE dbo.CloseAcademicYear;
GO
CREATE PROCEDURE dbo.CloseAcademicYear
    @AcademicYear NVARCHAR(20),
    @NextAcademicYear NVARCHAR(20) = NULL,
    @ClosedByUserID INT = NULL,
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @AcademicYear=REPLACE(LTRIM(RTRIM(@AcademicYear)),N'-',N'/');
    SET @NextAcademicYear=REPLACE(NULLIF(LTRIM(RTRIM(@NextAcademicYear)),N''),N'-',N'/');
    IF NULLIF(@AcademicYear,N'') IS NULL THROW 51021,N'العام الدراسي غير صالح.',1;
    IF @NextAcademicYear=@AcademicYear THROW 51022,N'لا يمكن أن يكون العام التالي مطابقاً للعام المغلق.',1;

    DECLARE @Critical INT=0;
    DECLARE @Issues TABLE (CheckCode NVARCHAR(80), CheckName NVARCHAR(200), IssueCount INT, Severity NVARCHAR(20), Details NVARCHAR(500), Result NVARCHAR(20));
    INSERT @Issues EXEC dbo.VerifyAnnualClosing @AcademicYear;
    SELECT @Critical=COUNT(*) FROM @Issues WHERE Severity=N'حرج' AND IssueCount>0;
    IF @Critical>0 THROW 51023,N'لا يمكن إغلاق العام لوجود أخطاء حرجة. أصلح نتائج الفحص أولاً.',1;

    BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@AcademicYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51024,N'العام الدراسي مغلق مسبقاً.',1;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@AcademicYear)
        UPDATE dbo.AnnualClosings SET ClosingStatus=N'مغلق',VerifiedAt=GETDATE(),ClosedAt=GETDATE(),ClosedByUserID=@ClosedByUserID,NextAcademicYear=@NextAcademicYear,Notes=@Notes,UpdatedAt=GETDATE() WHERE AcademicYear=@AcademicYear;
    ELSE
        INSERT dbo.AnnualClosings(AcademicYear,ClosingStatus,VerifiedAt,ClosedAt,ClosedByUserID,NextAcademicYear,Notes) VALUES(@AcademicYear,N'مغلق',GETDATE(),GETDATE(),@ClosedByUserID,@NextAcademicYear,@Notes);
    COMMIT TRANSACTION;
    SELECT CAST(1 AS BIT) AS Success, @AcademicYear AS AcademicYear, @NextAcademicYear AS NextAcademicYear;
END;
GO

IF OBJECT_ID(N'dbo.PlanStudentYearMigration', N'P') IS NOT NULL
    DROP PROCEDURE dbo.PlanStudentYearMigration;
GO
CREATE PROCEDURE dbo.PlanStudentYearMigration
    @FromAcademicYear NVARCHAR(20),
    @ToAcademicYear NVARCHAR(20),
    @CreatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @FromAcademicYear=REPLACE(LTRIM(RTRIM(@FromAcademicYear)),N'-',N'/');
    SET @ToAcademicYear=REPLACE(LTRIM(RTRIM(@ToAcademicYear)),N'-',N'/');
    IF NULLIF(@FromAcademicYear,N'') IS NULL OR NULLIF(@ToAcademicYear,N'') IS NULL OR @FromAcademicYear=@ToAcademicYear
        THROW 51025,N'يجب تحديد عامين مختلفين للترحيل.',1;
    IF NOT EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@FromAcademicYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51026,N'يجب إغلاق العام السابق قبل تخطيط الترحيل.',1;
    IF OBJECT_ID(N'dbo.StudentClasses',N'U') IS NULL THROW 51027,N'جدول توزيع الطلاب غير موجود.',1;

    INSERT dbo.AnnualMigrationLog(StudentID,FromAcademicYear,ToAcademicYear,FromClassID,FromSection,MigrationStatus,CreatedByUserID)
    SELECT sc.StudentID,@FromAcademicYear,@ToAcademicYear,sc.ClassID,sc.Section,N'مخطط',@CreatedByUserID
    FROM dbo.StudentClasses sc
    INNER JOIN dbo.Students s ON s.StudentID=sc.StudentID
    WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@FromAcademicYear
      /* يعتمد الترحيل على وجود توزيع للطالب في العام المغلق؛
         لا نستخدم Students.IsActive لأنه غير موجود في المخطط الفعلي. */
      AND NOT EXISTS (SELECT 1 FROM dbo.AnnualMigrationLog ml WHERE ml.StudentID=sc.StudentID AND ml.FromAcademicYear=@FromAcademicYear AND ml.ToAcademicYear=@ToAcademicYear);

    SELECT MigrationID,StudentID,FromAcademicYear,ToAcademicYear,FromClassID,FromSection,MigrationStatus FROM dbo.AnnualMigrationLog WHERE FromAcademicYear=@FromAcademicYear AND ToAcademicYear=@ToAcademicYear ORDER BY MigrationID;
END;
GO

IF OBJECT_ID(N'dbo.GetStudentMigrationReport', N'P') IS NOT NULL
    DROP PROCEDURE dbo.GetStudentMigrationReport;
GO
CREATE PROCEDURE dbo.GetStudentMigrationReport
    @FromAcademicYear NVARCHAR(20),
    @ToAcademicYear NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET @FromAcademicYear=REPLACE(LTRIM(RTRIM(@FromAcademicYear)),N'-',N'/');
    SET @ToAcademicYear=REPLACE(LTRIM(RTRIM(@ToAcademicYear)),N'-',N'/');
    IF NULLIF(@FromAcademicYear,N'') IS NULL OR NULLIF(@ToAcademicYear,N'') IS NULL OR @FromAcademicYear=@ToAcademicYear
        THROW 51028,N'يجب تحديد عامين مختلفين لتقرير الترحيل.',1;
    IF OBJECT_ID(N'dbo.StudentClasses',N'U') IS NULL OR OBJECT_ID(N'dbo.Students',N'U') IS NULL
        THROW 51029,N'جداول الطلاب والتوزيع غير موجودة.',1;

    ;WITH SourceAssignments AS
    (
        SELECT sc.StudentID, sc.ClassID, sc.Section,
               COUNT(*) OVER (PARTITION BY sc.StudentID) AS AssignmentCount
        FROM dbo.StudentClasses sc
        WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@FromAcademicYear
    )
    SELECT sa.StudentID,
           s.FullName AS StudentName,
           sa.ClassID AS FromClassID,
           sa.Section AS FromSection,
           sa.AssignmentCount,
           ml.MigrationID,
           ml.MigrationStatus,
           CASE
             WHEN sa.AssignmentCount > 1 THEN N'مستبعد - توزيع مكرر'
             WHEN ml.MigrationID IS NOT NULL AND ml.MigrationStatus=N'منفذ' THEN N'منقول'
             WHEN ml.MigrationID IS NOT NULL AND ml.MigrationStatus=N'مستبعد' THEN N'مستبعد'
             WHEN ml.MigrationID IS NOT NULL THEN N'مخطط'
             ELSE N'مرشح - يحتاج اعتماداً'
           END AS MigrationResult
    FROM SourceAssignments sa
    INNER JOIN dbo.Students s ON s.StudentID=sa.StudentID
    OUTER APPLY
    (
        SELECT TOP (1) m.MigrationID, m.MigrationStatus
        FROM dbo.AnnualMigrationLog m
        WHERE m.StudentID=sa.StudentID AND m.FromAcademicYear=@FromAcademicYear AND m.ToAcademicYear=@ToAcademicYear
        ORDER BY m.MigrationID DESC
    ) ml
    ORDER BY MigrationResult, s.FullName, sa.StudentID;
END;
GO

IF OBJECT_ID(N'dbo.ApproveStudentYearMigration', N'P') IS NOT NULL
    DROP PROCEDURE dbo.ApproveStudentYearMigration;
GO
CREATE PROCEDURE dbo.ApproveStudentYearMigration
    @MigrationID INT,
    @ToClassID INT,
    @ToSection NVARCHAR(50),
    @ApprovedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @ToSection = LTRIM(RTRIM(ISNULL(@ToSection, N'')));
    IF @MigrationID IS NULL OR @ToClassID IS NULL OR NULLIF(@ToSection, N'') IS NULL
        THROW 51030, N'يجب تحديد سجل الترحيل والصف والشعبة الجديدة.', 1;

    BEGIN TRANSACTION;
    DECLARE @StudentID INT, @FromYear NVARCHAR(20), @ToYear NVARCHAR(20), @FromClassID INT, @FromSection NVARCHAR(50), @Status NVARCHAR(20);
    SELECT @StudentID=StudentID, @FromYear=FromAcademicYear, @ToYear=ToAcademicYear,
           @FromClassID=FromClassID, @FromSection=FromSection, @Status=MigrationStatus
    FROM dbo.AnnualMigrationLog WITH (UPDLOCK, HOLDLOCK)
    WHERE MigrationID=@MigrationID;
    IF @StudentID IS NULL THROW 51031, N'سجل الترحيل غير موجود.', 1;
    IF @Status <> N'مخطط' THROW 51032, N'لا يمكن اعتماد سجل غير مخطط أو سبق تنفيذه.', 1;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@ToYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51033, N'لا يمكن الترحيل إلى عام مغلق أو مؤرشف.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.Classes WHERE ClassID=@ToClassID)
        THROW 51034, N'الصف الجديد غير موجود.', 1;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Enrollments e
        WHERE e.StudentID=@StudentID AND REPLACE(ISNULL(e.AcademicYear,N''),N'-',N'/')=REPLACE(@ToYear,N'-',N'/')
          AND LTRIM(RTRIM(ISNULL(e.Status,N''))) IN (N'مقبول',N'Accepted')
    )
        THROW 51035, N'لا يمكن اعتماد الترحيل قبل قبول تسجيل الطالب في العام الجديد.', 1;
    IF OBJECT_ID(N'dbo.SchoolSections',N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.SchoolSections WHERE ClassID=@ToClassID AND SectionName=@ToSection AND AcademicYear=@ToYear AND ISNULL(IsActive,1)=1)
        THROW 51036, N'الشعبة الجديدة غير موجودة أو غير فعالة في العام الجديد.', 1;
    IF EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@StudentID AND REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=REPLACE(@ToYear,N'-',N'/'))
        THROW 51037, N'للطالب توزيع مسجل مسبقاً في العام الجديد.', 1;
    IF OBJECT_ID(N'dbo.SchoolSections',N'U') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.SchoolSections ss WHERE ss.ClassID=@ToClassID AND ss.SectionName=@ToSection AND ss.AcademicYear=@ToYear AND ss.Capacity IS NOT NULL
           AND (SELECT COUNT(*) FROM dbo.StudentClasses sc WHERE sc.ClassID=@ToClassID AND sc.Section=@ToSection AND REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=REPLACE(@ToYear,N'-',N'/')) >= ss.Capacity)
        THROW 51038, N'لا توجد مقاعد شاغرة في الشعبة الجديدة.', 1;

    INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
    VALUES(@StudentID,@ToClassID,@ToSection,@ToYear,GETDATE(),@ApprovedByUserID);

    /* حافظ على مرآة الطالب التي تعتمد عليها الشاشات القديمة، مع بقاء StudentClasses هو المصدر التاريخي لكل عام. */
    UPDATE dbo.Students
       SET ClassID=@ToClassID, Section=@ToSection, AcademicYear=@ToYear
     WHERE StudentID=@StudentID;

    UPDATE dbo.AnnualMigrationLog
       SET ToClassID=@ToClassID, ToSection=@ToSection, MigrationStatus=N'منفذ', CreatedByUserID=COALESCE(@ApprovedByUserID,CreatedByUserID), Notes=COALESCE(Notes,N'') + CASE WHEN LEN(ISNULL(Notes,N''))>0 THEN N' ' ELSE N'' END + N'تم الاعتماد والتنفيذ.'
     WHERE MigrationID=@MigrationID;
    COMMIT TRANSACTION;
    SELECT @MigrationID AS MigrationID, @StudentID AS StudentID, @ToYear AS AcademicYear, N'منفذ' AS MigrationStatus;
END;
GO

PRINT N'تم تجهيز بنية الإغلاق السنوي وإجراءات الفحص والإغلاق وتخطيط الترحيل وتقرير الترحيل واعتماد الترحيل.';
GO
