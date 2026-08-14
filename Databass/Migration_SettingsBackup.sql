/*
   SchoolSystem - Settings and Database Backup permission migration
   Safe to run more than once. It does not create tables or modify user passwords.
*/
SET NOCOUNT ON;

IF EXISTS (SELECT 1 FROM dbo.Users WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator'))
BEGIN
    UPDATE dbo.Users
    SET RoleName = N'مدير النظام',
        Permissions = CASE
            WHEN Permissions IS NULL OR LTRIM(RTRIM(Permissions)) = N'' THEN N'Settings.Manage'
            WHEN Permissions LIKE N'%Settings.Manage%' THEN Permissions
            ELSE Permissions + N',Settings.Manage'
        END,
        UpdatedAt = GETDATE()
    WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator');
END;

SELECT UserID, UserName, RoleName, Permissions, IsActive
FROM dbo.Users
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) = LOWER(N'مدير النظام');
