/*
   SchoolSystem - Dashboard operational status verification
   Read-only. Run against SchoolDB after Migration_ActiveYearBackupAudit.sql.
*/
SET NOCOUNT ON;

DECLARE @CriticalIssueCount INT = 0;

IF OBJECT_ID(N'dbo.SystemAcademicSettings', N'U') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'SystemAcademicSettings' AS ObjectName, N'جدول العام النشط غير موجود.' AS Issue;
END;

IF OBJECT_ID(N'dbo.DatabaseBackupHistory', N'U') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'DatabaseBackupHistory' AS ObjectName, N'جدول سجل النسخ الاحتياطية غير موجود.' AS Issue;
END;

IF OBJECT_ID(N'dbo.AnnualClosings', N'U') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'AnnualClosings' AS ObjectName, N'جدول الإغلاق السنوي غير موجود.' AS Issue;
END;

IF OBJECT_ID(N'dbo.GetDashboardOperationalStatus', N'P') IS NULL
BEGIN
    SET @CriticalIssueCount += 1;
    SELECT N'GetDashboardOperationalStatus' AS ObjectName, N'إجراء ملخص لوحة التحكم غير موجود.' AS Issue;
END;

SELECT @CriticalIssueCount AS CriticalIssueCount,
       CASE WHEN @CriticalIssueCount = 0 THEN N'سليم' ELSE N'يحتاج معالجة' END AS VerificationResult;
GO

