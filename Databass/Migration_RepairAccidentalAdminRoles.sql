/*
    إصلاح الحسابات التي حصلت على دور مدير النظام بالخطأ.
    شغّل الملف على SchoolDB وليس master.

    السياسة:
      - الحساب admin فقط يبقى مدير النظام.
      - أي حساب آخر يحمل مدير النظام أو Admin أو Administrator
        يتحول إلى دور التقارير بصلاحيات محدودة وآمنة.
      - لا يتم حذف المستخدمين ولا كلمات المرور.
      - يمكن تغيير اسم الحساب الإداري الأساسي في المتغير أدناه قبل التنفيذ.
*/

/*
    يمكن تشغيل الملف من SSMS حتى لو كانت قاعدة الاتصال الحالية master.
    يتم التحويل صراحةً إلى SchoolDB قبل فحص dbo.Users.
*/
IF DB_ID(N'SchoolDB') IS NULL
    THROW 51001, N'قاعدة البيانات SchoolDB غير موجودة على خادم SQL الحالي. تحقق من اسم الخادم واتصال التطبيق.', 1;
GO

USE [SchoolDB];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT N'قاعدة التنفيذ الحالية: ' + DB_NAME();

DECLARE @CanonicalAdminUserName NVARCHAR(100) = N'admin';
DECLARE @LimitedPermissions NVARCHAR(MAX) =
    N'Dashboard.View,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportPDF';

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51000, N'جدول dbo.Users غير موجود. تأكد من الاتصال بقاعدة SchoolDB.', 1;

/* معاينة الحسابات المتأثرة قبل التعديل. */
SELECT UserID, UserName, FullName, RoleName, IsActive
FROM dbo.Users
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN
      (N'مدير النظام', N'admin', N'administrator')
ORDER BY UserID;

BEGIN TRANSACTION;

/* توحيد الحساب الإداري الأساسي إذا كان موجودًا. */
UPDATE dbo.Users
SET RoleName = N'مدير النظام',
    Permissions = NULL,
    MustChangePassword = 0,
    UpdatedAt = GETDATE()
WHERE LOWER(LTRIM(RTRIM(UserName))) = LOWER(LTRIM(RTRIM(@CanonicalAdminUserName)));

/* الحسابات الأخرى لا تبقى بامتيازات المدير العام. */
UPDATE dbo.Users
SET RoleName = N'التقارير',
    Permissions = @LimitedPermissions,
    UpdatedAt = GETDATE()
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN
      (N'مدير النظام', N'admin', N'administrator')
  AND LOWER(LTRIM(RTRIM(UserName))) <> LOWER(LTRIM(RTRIM(@CanonicalAdminUserName)));

COMMIT TRANSACTION;

/* نتيجة التحقق بعد الإصلاح. */
SELECT UserID, UserName, FullName, RoleName, Permissions, IsActive
FROM dbo.Users
ORDER BY UserID;
