/*
    SchoolSystem - Runtime RBAC verification
    شغّل هذا الملف على SchoolDB فقط بعد حفظ نسخة احتياطية.
    لا يغيّر صلاحيات المستخدمين العاديين؛ يصلح المديرين فقط إلى القاموس المركزي.
*/

USE SchoolDB;
GO

IF DB_NAME() <> N'SchoolDB'
    THROW 51210, N'العملية موقوفة: يجب الاتصال بقاعدة SchoolDB وليس master.', 1;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51211, N'جدول Users غير موجود. نفّذ Migration_Step1.sql أولاً.', 1;
GO

IF COL_LENGTH(N'dbo.Users', N'Permissions') IS NULL
    THROW 51212, N'عمود Permissions غير موجود. نفّذ Migration_Step1.sql أولاً.', 1;
GO

/* ضمان أن العمود يستوعب الكتالوج الكامل دون قصّ النص. */
IF EXISTS
(
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.Users')
      AND c.name = N'Permissions'
      AND NOT (t.name = N'nvarchar' AND c.max_length = -1)
)
BEGIN
    ALTER TABLE dbo.Users ALTER COLUMN Permissions NVARCHAR(MAX) NULL;
END;
GO

/* منح مدير النظام كامل الكتالوج الحالي في الكود (PermissionKeys.cs).
   إذا وُجد جدول Permissions المعياري المزامن مع الكود، نُحمّل منه؛
   وإلا نستخدم القائمة المرجعية المختصرة الموافقة لنفس الكود. */
DECLARE @AdminPermissions NVARCHAR(MAX);
IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
    AND EXISTS (SELECT 1 FROM dbo.Permissions)
BEGIN
    SELECT @AdminPermissions = STRING_AGG(CONVERT(NVARCHAR(MAX), PermissionKey), N',')
           WITHIN GROUP (ORDER BY PermissionKey)
    FROM dbo.Permissions;
END;

IF @AdminPermissions IS NULL
BEGIN
    SET @AdminPermissions =
        N'Dashboard.View,Students.View,Students.Manage,Enrollment.Manage,ClassAssignment.View,ClassAssignment.Manage,' +
        N'Teachers.Manage,StaffAttendance.Manage,Payroll.Manage,Subjects.Manage,Classes.Manage,' +
        N'Timetable.Manage,Attendance.Manage,Grades.Manage,Fees.Manage,Vouchers.Manage,' +
        N'Expenses.Manage,Library.Manage,Transport.Manage,Reports.View,Users.Manage,Users.View,Users.Add,' +
        N'Users.Edit,Users.Delete,Users.ManageRoles,Roles.View,Roles.Add,Roles.Edit,Roles.Delete,Roles.Manage,' +
        N'Permissions.Manage,AuditLogs.View,AuditLogs.ExportExcel,AuditLogs.ExportPDF,AuditLogs.Print,' +
        N'Settings.View,Settings.Manage,Settings.Edit,FeePlans.View,FeePlans.Add,FeePlans.Edit,TeacherAttendance.View,' +
        N'TeacherContracts.View,Rooms.View,Classes.View,Subjects.View,Teachers.View,Timetable.View';
END;

UPDATE dbo.Users
SET RoleName = N'مدير النظام',
    Permissions = @AdminPermissions,
    UpdatedAt = GETDATE()
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator');

SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    LEN(ISNULL(Permissions, N'')) AS PermissionTextLength,
    LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
        CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END AS PermissionTokenCount,
    Permissions
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'تم فحص RBAC. إذا ظهرت للمستخدم العادي قيمتان فقط، فالصلاحيات المحفوظة لذلك الحساب هي Dashboard.View وReports.View وليست مشكلة في MainForm.';
GO
