/*
    Verify_User_RBAC_Permissions.sql
    الغرض: تشخيص سبب ظهور Dashboard وReports فقط لحساب محدد.
    الاستخدام:
      1) غيّر قيمة @UserName.
      2) شغّل السكربت على قاعدة SchoolDB.
      3) راجع Permissions وPermissionCount وModules.
*/
USE SchoolDB;
GO

DECLARE @UserName NVARCHAR(100) = N'ضع_اسم_المستخدم_هنا';

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Users' AND schema_id = SCHEMA_ID(N'dbo'))
    THROW 51000, N'جدول dbo.Users غير موجود. اختر قاعدة SchoolDB الصحيحة.', 1;

SELECT
    u.UserID,
    u.UserName,
    u.FullName,
    u.RoleName,
    u.IsActive,
    u.Permissions,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(u.Permissions)), N'') IS NULL THEN 0
        ELSE LEN(u.Permissions) - LEN(REPLACE(u.Permissions, N',', N'')) + 1
    END AS PermissionCount,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(u.Permissions)), N'') IS NULL THEN N'لا توجد صلاحيات محفوظة'
        WHEN u.Permissions LIKE N'%Dashboard.View%' AND u.Permissions LIKE N'%Reports.View%'
             AND u.Permissions NOT LIKE N'%,%.%' THEN N'تحتاج مراجعة: قد تكون صلاحيات قديمة أو مختصرة'
        ELSE N'راجع القائمة الكاملة للمفاتيح'
    END AS DiagnosticStatus
FROM dbo.Users u
WHERE u.UserName = @UserName;

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserName = @UserName)
    PRINT N'لم يتم العثور على مستخدم بهذا الاسم.';

/* تفكيك الصلاحيات المحفوظة إلى صفوف مفردة. */
SELECT
    u.UserName,
    LTRIM(RTRIM(s.value)) AS PermissionKey
FROM dbo.Users u
CROSS APPLY STRING_SPLIT(REPLACE(REPLACE(COALESCE(u.Permissions, N''), N';', N','), CHAR(13) + CHAR(10), N','), N',') s
WHERE u.UserName = @UserName
  AND NULLIF(LTRIM(RTRIM(s.value)), N'') IS NOT NULL
ORDER BY LTRIM(RTRIM(s.value));

/* إظهار ما إذا كانت الصلاحيات القديمة ذات القائمتين فقط ما زالت موجودة. */
SELECT
    u.UserName,
    u.RoleName,
    u.Permissions
FROM dbo.Users u
WHERE u.UserName = @UserName
  AND (
      LTRIM(RTRIM(u.Permissions)) = N'Dashboard.View,Reports.View'
      OR LTRIM(RTRIM(u.Permissions)) = N'Reports.View,Dashboard.View'
  );
GO

/*
    التفسير:
    - إذا كانت PermissionCount = 2، شغّل Migration_RepairLegacyReportOnlyPermissions_Runtime.sql.
    - إذا كانت PermissionCount كبيرة، فالحفظ صحيح، ويجب فحص تسجيل الدخول وMainForm على Windows.
    - إذا كانت Permissions فارغة، فالسبب أن الصلاحيات لم تُحدد أو لم يُحفظ التعديل.
*/
