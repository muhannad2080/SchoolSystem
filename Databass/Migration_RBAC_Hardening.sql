/*
    SchoolSystem - RBAC hardening migration
    الهدف: إصلاح حسابات مدير النظام التي لا تحتوي على كامل الصلاحيات،
    وتوحيد الأسماء المستعارة للدور، مع الحفاظ على الصلاحيات المخصصة لبقية الأدوار.
    هذه العملية idempotent ويمكن تشغيلها أكثر من مرة بأمان.
*/

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    PRINT N'لم يتم العثور على جدول Users. شغّل Migration_Step1.sql أولاً.';
    RETURN;
END
GO

/*
   مدير النظام هو الدور الوحيد الذي يحصل على الكتالوج المركزي الكامل.
   نترك Permissions فارغًا عمدًا؛ يقوم UserService ببناء PermissionKeys.All
   عند تسجيل الدخول، وبذلك لا توجد قائمة SQL قديمة أو ناقصة للصلاحيات.
*/
UPDATE dbo.Users
SET RoleName = N'مدير النظام',
    Permissions = NULL,
    UpdatedAt = GETDATE()
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator')
  AND (
        LTRIM(RTRIM(ISNULL(RoleName, N''))) <> N'مدير النظام'
        OR Permissions IS NOT NULL
      );
GO

/*
   لا نعيد ملء Permissions للحسابات العادية هنا.
   القيمة الفارغة قد تكون اختيارًا يدويًا صحيحًا لمنع جميع الصلاحيات،
   وأي UPDATE تلقائي سيعيد المشكلة القديمة Dashboard.View,Reports.View.
   الحساب NULL فقط يعالجه UserService عند تسجيل الدخول من كتالوج الدور.
*/
PRINT N'لم تتم إعادة كتابة صلاحيات الحسابات العادية؛ التخصيص اليدوي محفوظ.';
GO

/*
   تشخيص غير تعديلي بعد الترحيل. يجب أن يعرض مديرو النظام القيمة الكاملة.
*/
SELECT UserID, UserName, RoleName, Permissions, IsActive
FROM dbo.Users
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) = LOWER(N'مدير النظام');
GO

PRINT N'تم تطبيق Migration_RBAC_Hardening.sql بنجاح.';
GO
