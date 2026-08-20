/*
    SchoolSystem - Repair legacy report-only runtime permissions
    الإصدار: 2026-08-20

    الهدف:
    إصلاح الحسابات القديمة التي تحتوي Users.Permissions على:
        Dashboard.View,Reports.View
    فقط، رغم أن RoleName يملك إعدادات أوسع.

    التشغيل الآمن:
    1) خذ نسخة احتياطية من SchoolDB.
    2) شغّل الملف أولاً مع @ApplyRepair = 0 للمراجعة.
    3) إذا كانت النتائج صحيحة، غيّر القيمة إلى 1 ثم شغّل الملف مرة أخرى.

    ملاحظة: لا يلمس هذا الملف أدوار "التقارير" أو "مدقق"، ولا يغير التخصيصات
    التي تحتوي على أكثر من مفتاحين.
*/

USE SchoolDB;
GO

DECLARE @ApplyRepair BIT = 0;

IF DB_NAME() <> N'SchoolDB'
    THROW 51000, N'يجب تشغيل السكربت على قاعدة SchoolDB.', 1;

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51001, N'جدول dbo.Users غير موجود.', 1;

IF COL_LENGTH(N'dbo.Users', N'Permissions') IS NULL
    THROW 51002, N'عمود dbo.Users.Permissions غير موجود.', 1;

IF EXISTS
(
    SELECT 1
    FROM sys.columns c
    JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.Users')
      AND c.name = N'Permissions'
      AND NOT (t.name = N'nvarchar' AND c.max_length = -1)
)
BEGIN
    ALTER TABLE dbo.Users ALTER COLUMN Permissions NVARCHAR(MAX) NULL;
END;

;WITH CandidateUsers AS
(
    SELECT
        U.UserID,
        U.UserName,
        U.RoleName,
        U.Permissions,
        PermissionCount =
            CASE
                WHEN NULLIF(LTRIM(RTRIM(U.Permissions)), N'') IS NULL THEN 0
                ELSE LEN(U.Permissions) - LEN(REPLACE(U.Permissions, N',', N'')) + 1
            END
    FROM dbo.Users U
    WHERE U.Permissions IS NOT NULL
      AND U.RoleName NOT IN (N'التقارير', N'مدقق')
      AND U.Permissions LIKE N'%Dashboard.View%'
      AND U.Permissions LIKE N'%Reports.View%'
), ExactCandidates AS
(
    SELECT *
    FROM CandidateUsers
    WHERE PermissionCount = 2
)
SELECT
    UserID,
    UserName,
    RoleName,
    Permissions,
    CASE WHEN @ApplyRepair = 1
         THEN N'سيتم تحويل Permissions إلى NULL ليعيد التطبيق بناء صلاحيات الدور.'
         ELSE N'فحص فقط — لن يتم تعديل البيانات.'
    END AS RepairStatus
FROM ExactCandidates
ORDER BY UserID;

IF @ApplyRepair = 1
BEGIN
    UPDATE U
       SET U.Permissions = NULL,
           U.UpdatedAt = GETDATE()
    FROM dbo.Users U
    WHERE U.Permissions IS NOT NULL
      AND U.RoleName NOT IN (N'التقارير', N'مدقق')
      AND U.Permissions LIKE N'%Dashboard.View%'
      AND U.Permissions LIKE N'%Reports.View%'
      AND LEN(U.Permissions) - LEN(REPLACE(U.Permissions, N',', N'')) + 1 = 2;

    PRINT N'تم إصلاح الحسابات القديمة. سجّل الخروج ثم الدخول مرة أخرى لكل حساب متأثر.';
END
ELSE
BEGIN
    PRINT N'وضع الفحص فقط. إذا كانت القائمة صحيحة، غيّر @ApplyRepair إلى 1 ثم أعد التشغيل.';
END;
GO

SELECT
    UserID,
    UserName,
    RoleName,
    CASE WHEN Permissions IS NULL THEN N'NULL — سيُعاد بناؤها عند الدخول'
         ELSE Permissions END AS Permissions,
    LEN(ISNULL(Permissions, N'')) AS PermissionTextLength
FROM dbo.Users
ORDER BY UserID;
GO

/*
    بعد الإصلاح:
    - سجّل الخروج من التطبيق.
    - سجّل الدخول من جديد.
    - راجع القائمة الرئيسية.
    - تحقق من أن Users.Permissions أصبحت تحتوي صلاحيات الدور، وليس Dashboard/Reports فقط.
*/
