/*
   SchoolSystem - Active year reporting integration verification
   Read-only verification. Run against SchoolDB after all active-year migrations.
*/
SET NOCOUNT ON;

DECLARE @ActiveYear NVARCHAR(20);
SELECT TOP (1) @ActiveYear = ActiveAcademicYear
FROM dbo.SystemAcademicSettings
ORDER BY SettingID;

DECLARE @CriticalIssueCount INT = 0;

IF NULLIF(LTRIM(RTRIM(@ActiveYear)), N'') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'ActiveYear' AS CheckName, N'لا يوجد عام نشط مضبوط.' AS Result;
END;

IF OBJECT_ID(N'dbo.GetDashboardOperationalStatus', N'P') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'DashboardStatusProcedure' AS CheckName, N'إجراء حالة لوحة التحكم غير موجود.' AS Result;
END;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'StudentClasses' AS CheckName, N'جدول التوزيع السنوي غير موجود.' AS Result;
END;

IF OBJECT_ID(N'dbo.Fees', N'U') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'Fees' AS CheckName, N'جدول الرسوم غير موجود.' AS Result;
END;

IF OBJECT_ID(N'dbo.Attendance', N'U') IS NULL
BEGIN
    SELECT N'Attendance' AS CheckName, N'SKIP: جدول الحضور غير موجود في هذا المخطط.' AS Result;
END;

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    SELECT N'Grades' AS CheckName, N'SKIP: جدول الدرجات غير موجود في هذا المخطط.' AS Result;
END;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Fees f
        LEFT JOIN dbo.StudentClasses sc
          ON sc.StudentID = f.StudentID
         AND REPLACE(sc.AcademicYear, N'-', N'/') = REPLACE(f.AcademicYear, N'-', N'/')
        WHERE REPLACE(f.AcademicYear, N'-', N'/') = REPLACE(@ActiveYear, N'-', N'/')
          AND sc.StudentClassID IS NULL
    )
    BEGIN
        SET @CriticalIssueCount += 1;
        SELECT N'OrphanFeesInActiveYear' AS CheckName, N'رسوم في العام النشط بلا توزيع سنوي مطابق.' AS Result;
    END;
END;

IF OBJECT_ID(N'dbo.GetDashboardOperationalStatus', N'P') IS NOT NULL
BEGIN
    EXEC dbo.GetDashboardOperationalStatus;
END;

SELECT @ActiveYear AS ActiveAcademicYear,
       @CriticalIssueCount AS CriticalIssueCount,
       CASE WHEN @CriticalIssueCount = 0 THEN N'سليم' ELSE N'يحتاج معالجة' END AS VerificationResult;
GO
