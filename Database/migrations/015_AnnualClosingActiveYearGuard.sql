/*
    Migration 015
    تشديد الإغلاق السنوي:
    - لا يمكن إغلاق عام غير العام الدراسي النشط حالياً.
    - يجب أن يكون العام التالي هو العام الدراسي المتسلسل مباشرة.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.CloseAndActivateAcademicYear', N'P') IS NULL
    THROW 51500, N'الإجراء CloseAndActivateAcademicYear غير موجود؛ يجب تطبيق Migration 014 أولاً.', 1;

DECLARE @sql NVARCHAR(MAX) = N'
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
        THROW 51510, N''العام الدراسي المطلوب إغلاقه غير صالح.'', 1;

    IF @NextYear IS NULL
        THROW 51511, N''العام الدراسي التالي مطلوب لإتمام الإغلاق.'', 1;

    DECLARE @YearSeparator INT = CHARINDEX(N''/'', @Year);
    DECLARE @NextSeparator INT = CHARINDEX(N''/'', @NextYear);

    IF @YearSeparator <> 5 OR @NextSeparator <> 5 OR
       LEN(@Year) <> 9 OR LEN(@NextYear) <> 9 OR
       TRY_CONVERT(INT, LEFT(@Year, 4)) IS NULL OR
       TRY_CONVERT(INT, RIGHT(@Year, 4)) IS NULL OR
       TRY_CONVERT(INT, LEFT(@NextYear, 4)) IS NULL OR
       TRY_CONVERT(INT, RIGHT(@NextYear, 4)) IS NULL
        THROW 51512, N''صيغة العام الدراسي غير صالحة.'', 1;

    DECLARE @YearStart INT = TRY_CONVERT(INT, LEFT(@Year, 4));
    DECLARE @YearEnd INT = TRY_CONVERT(INT, RIGHT(@Year, 4));
    DECLARE @NextStart INT = TRY_CONVERT(INT, LEFT(@NextYear, 4));
    DECLARE @NextEnd INT = TRY_CONVERT(INT, RIGHT(@NextYear, 4));

    IF @YearEnd <> @YearStart + 1 OR @NextEnd <> @NextStart + 1
        THROW 51513, N''العام الدراسي يجب أن يكون متسلسلاً مثل 2026/2027.'', 1;

    IF @NextStart <> @YearEnd
        THROW 51514, N''العام التالي يجب أن يكون العام الدراسي المتسلسل مباشرة بعد العام المغلق.'', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYearCurrent
        WHERE ISNULL(IsCurrent, 0) = 1
          AND REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @Year
    )
        THROW 51515, N''لا يمكن إغلاق عام غير محدد حالياً كعام دراسي نشط.'', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYears
        WHERE REPLACE(LTRIM(RTRIM(AcademicYear)), N''-'', N''/'') = @NextYear
    )
        THROW 51516, N''العام الدراسي التالي غير موجود. أنشئه أولاً قبل الإغلاق.'', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @NextYear
          AND ClosingStatus IN (N''مغلق'', N''مؤرشف'')
    )
        THROW 51517, N''لا يمكن التفعيل إلى عام مغلق أو مؤرشف.'', 1;

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
        THROW 51518, N''لا يمكن إغلاق العام لوجود أخطاء حرجة. عالج نتائج الفحص أولاً.'', 1;

    BEGIN TRANSACTION;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AnnualClosings
        WHERE REPLACE(ISNULL(AcademicYear, N''''), N''-'', N''/'') = @Year
          AND ClosingStatus IN (N''مغلق'', N''مؤرشف'')
    )
        THROW 51519, N''العام الدراسي مغلق مسبقاً.'', 1;

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

    UPDATE dbo.AcademicYears
       SET IsActive = 1
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
EXEC sp_executesql @sql;
