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

DECLARE @AdminPermissions NVARCHAR(MAX) =
    N'Dashboard.View,Students.View,Students.Manage,Enrollment.Manage,ClassAssignment.Manage,' +
    N'Teachers.Manage,StaffAttendance.Manage,Payroll.Manage,Subjects.Manage,Classes.Manage,' +
    N'Timetable.Manage,Attendance.Manage,Grades.Manage,Fees.Manage,Vouchers.Manage,' +
    N'Expenses.Manage,Library.Manage,Transport.Manage,Reports.View,Users.Manage,' +
    N'AuditLogs.View,Settings.Manage';

/*
   مدير النظام هو الدور الوحيد الذي يجب أن يطابق القاموس المركزي بالكامل.
   يشمل ذلك Admin وAdministrator والأسماء التي تحتوي على مسافات زائدة.
*/
UPDATE dbo.Users
SET RoleName = N'مدير النظام',
    Permissions = @AdminPermissions,
    UpdatedAt = GETDATE()
WHERE LOWER(LTRIM(RTRIM(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator')
  AND (
        LTRIM(RTRIM(ISNULL(RoleName, N''))) <> N'مدير النظام'
        OR LTRIM(RTRIM(ISNULL(Permissions, N''))) <> @AdminPermissions
      );
GO

/*
   إكمال الصلاحيات للحسابات ذات الدور المعروف إذا كانت القيمة فارغة فقط.
   لا يتم استبدال أي تخصيص يدوي غير فارغ.
*/
UPDATE dbo.Users
SET Permissions = CASE LTRIM(RTRIM(RoleName))
    WHEN N'الإدارة' THEN N'Dashboard.View,Students.View,Students.Manage,Enrollment.Manage,ClassAssignment.Manage,Teachers.Manage,Subjects.Manage,Classes.Manage,Timetable.Manage,Attendance.Manage,Grades.Manage,Reports.View'
    WHEN N'شؤون الطلاب' THEN N'Dashboard.View,Students.View,Students.Manage,Enrollment.Manage,ClassAssignment.Manage,Attendance.Manage,Grades.Manage,Reports.View'
    WHEN N'المعلمون' THEN N'Dashboard.View,Students.View,Attendance.Manage,Grades.Manage,Timetable.Manage,Reports.View'
    WHEN N'المالية' THEN N'Dashboard.View,Fees.Manage,Vouchers.Manage,Expenses.Manage,Payroll.Manage,Reports.View'
    WHEN N'المكتبة' THEN N'Dashboard.View,Library.Manage,Reports.View'
    WHEN N'النقل' THEN N'Dashboard.View,Transport.Manage,Reports.View'
    WHEN N'التقارير' THEN N'Dashboard.View,Reports.View'
    ELSE Permissions
END,
UpdatedAt = GETDATE()
WHERE (Permissions IS NULL OR LTRIM(RTRIM(Permissions)) = N'')
  AND LTRIM(RTRIM(RoleName)) IN
      (N'الإدارة', N'شؤون الطلاب', N'المعلمون', N'المالية', N'المكتبة', N'النقل', N'التقارير');
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
