/*
   SchoolSystem - Settings and Database Backup permission migration
   Safe to run more than once. It does not create tables or modify user passwords.
*/
SET NOCOUNT ON;

IF EXISTS (SELECT 1 FROM dbo.Users WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator'))
BEGIN
    UPDATE dbo.Users
    SET RoleName = N'مدير النظام',
        /*
           لا نكتب صلاحية جزئية للمدير. يحمّل UserService الكتالوج الكامل
           مركزيًا عند تسجيل الدخول، ولذلك NULL هو الوضع الآمن هنا.
        */
        Permissions = NULL,
        UpdatedAt = GETDATE()
    WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator');
END;

SELECT UserID, UserName, RoleName, Permissions, IsActive
FROM dbo.Users
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) = LOWER(N'مدير النظام');
