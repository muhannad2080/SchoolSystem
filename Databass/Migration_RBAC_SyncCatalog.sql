/*
    SchoolSystem - Sync normalized RBAC tables with the code catalog
    مزامنة الجداول المعيارية (Permissions, UserRoles, RolePermissions)
    مع كتالوج الصلاحيات في الكود (PermissionKeys.cs). Idempotent وآمنة لإعادة التشغيل.
    ملاحظة: طبقة التشغيل الحالية تقرأ Users.Permissions؛ هذه المزامنة تجعل القاعدة
    متطابقة مع الكود ولا تحذف أي بيانات.
*/
IF DB_ID(N'SchoolDB') IS NULL
    THROW 51220, N'قاعدة البيانات SchoolDB غير موجودة.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
    THROW 51221, N'جدول Permissions غير موجود. نفّذ Migration_RBAC_NormalizedCatalog.sql أولاً.', 1;
GO

SET NOCOUNT ON;

/* 1) إضافة مفاتيح الكتالوج الناقصة (ExportCsv لجميع الوحدات + مفاتيح Users/Roles/Permissions). */
INSERT INTO dbo.Permissions(PermissionKey, DisplayName, ModuleName, ActionName)
SELECT V.PermissionKey, V.DisplayName, V.ModuleName, V.ActionName
FROM (VALUES
    (N'Attendance.ExportCsv', N'حضور الطلاب - تصدير CSV', N'Attendance', N'ExportCsv'),
    (N'AuditLogs.ExportCsv', N'سجل التدقيق - تصدير CSV', N'AuditLogs', N'ExportCsv'),
    (N'ClassAssignment.ExportCsv', N'توزيع الطلاب - تصدير CSV', N'ClassAssignment', N'ExportCsv'),
    (N'Classes.ExportCsv', N'الفصول الدراسية - تصدير CSV', N'Classes', N'ExportCsv'),
    (N'Dashboard.ExportCsv', N'لوحة التحكم - تصدير CSV', N'Dashboard', N'ExportCsv'),
    (N'Enrollment.ExportCsv', N'القبول والتسجيل - تصدير CSV', N'Enrollment', N'ExportCsv'),
    (N'Expenses.ExportCsv', N'المصروفات - تصدير CSV', N'Expenses', N'ExportCsv'),
    (N'FeePlans.ExportCsv', N'خطط الرسوم - تصدير CSV', N'FeePlans', N'ExportCsv'),
    (N'Fees.ExportCsv', N'الرسوم - تصدير CSV', N'Fees', N'ExportCsv'),
    (N'Grades.ExportCsv', N'الدرجات - تصدير CSV', N'Grades', N'ExportCsv'),
    (N'Library.ExportCsv', N'المكتبة - تصدير CSV', N'Library', N'ExportCsv'),
    (N'Payroll.ExportCsv', N'الرواتب - تصدير CSV', N'Payroll', N'ExportCsv'),
    (N'Permissions.Manage', N'إدارة الصلاحيات', N'Permissions', N'Manage'),
    (N'Reports.ExportCsv', N'التقارير - تصدير CSV', N'Reports', N'ExportCsv'),
    (N'Roles.Add', N'إضافة دور', N'Roles', N'Add'),
    (N'Roles.Delete', N'حذف دور', N'Roles', N'Delete'),
    (N'Roles.Edit', N'تعديل دور', N'Roles', N'Edit'),
    (N'Roles.Manage', N'إدارة الأدوار', N'Roles', N'Manage'),
    (N'Roles.View', N'عرض الأدوار', N'Roles', N'View'),
    (N'Rooms.ExportCsv', N'القاعات - تصدير CSV', N'Rooms', N'ExportCsv'),
    (N'Settings.ExportCsv', N'الإعدادات - تصدير CSV', N'Settings', N'ExportCsv'),
    (N'StaffAttendance.ExportCsv', N'حضور الموظفين - تصدير CSV', N'StaffAttendance', N'ExportCsv'),
    (N'StaffAttendance.Manage', N'إدارة حضور الموظفين القديمة', N'StaffAttendance', N'Manage'),
    (N'Students.ExportCsv', N'الطلاب - تصدير CSV', N'Students', N'ExportCsv'),
    (N'Subjects.ExportCsv', N'المواد - تصدير CSV', N'Subjects', N'ExportCsv'),
    (N'TeacherAttendance.ExportCsv', N'حضور المعلمين - تصدير CSV', N'TeacherAttendance', N'ExportCsv'),
    (N'TeacherContracts.ExportCsv', N'عقود المعلمين - تصدير CSV', N'TeacherContracts', N'ExportCsv'),
    (N'Teachers.ExportCsv', N'المعلمون - تصدير CSV', N'Teachers', N'ExportCsv'),
    (N'Timetable.ExportCsv', N'الجدول الدراسي - تصدير CSV', N'Timetable', N'ExportCsv'),
    (N'Transport.ExportCsv', N'النقل - تصدير CSV', N'Transport', N'ExportCsv'),
    (N'Users.Add', N'إضافة مستخدم', N'Users', N'Add'),
    (N'Users.Delete', N'حذف مستخدم', N'Users', N'Delete'),
    (N'Users.Edit', N'تعديل مستخدم', N'Users', N'Edit'),
    (N'Users.ManageRoles', N'إدارة أدوار المستخدمين', N'Users', N'ManageRoles'),
    (N'Users.View', N'عرض المستخدمين', N'Users', N'View'),
    (N'Vouchers.ExportCsv', N'السندات - تصدير CSV', N'Vouchers', N'ExportCsv')
) V(PermissionKey, DisplayName, ModuleName, ActionName)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Permissions P WHERE P.PermissionKey = V.PermissionKey
);
GO

/* 2) ربط كل مستخدم بدوره في UserRoles إن لم يكن مربوطاً (بما في ذلك المستخدمين الجدد). */
INSERT INTO dbo.UserRoles(UserID, RoleID)
SELECT U.UserID, R.RoleID
FROM dbo.Users U
JOIN dbo.Roles R ON R.RoleName = LTRIM(RTRIM(U.RoleName))
WHERE U.UserID IS NOT NULL
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.UserRoles UR
      WHERE UR.UserID = U.UserID AND UR.RoleID = R.RoleID
  );
GO

/* 3) منح مدير النظام كامل الكتالوج (يشمل المفاتيح المضافة أعلاه). */
INSERT INTO dbo.RolePermissions(RoleID, PermissionID)
SELECT R.RoleID, P.PermissionID
FROM dbo.Roles R CROSS JOIN dbo.Permissions P
WHERE R.RoleName = N'مدير النظام'
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.RolePermissions RP
      WHERE RP.RoleID = R.RoleID AND RP.PermissionID = P.PermissionID
  );
GO

/* 4) مزامنة صلاحيات الأدوار القياسية مع GetRoleDefaults في الكود.
      تُحذف الاقتراحات القديمة غير المطابقة ثم يُعاد الإدخال للدور المحدد فقط،
      دون لمس الأدوار المخصصة أو بيانات المستخدمين. */
IF OBJECT_ID(N'tempdb..#RoleDefaults') IS NOT NULL
    DROP TABLE #RoleDefaults;
CREATE TABLE #RoleDefaults
(
    RoleName NVARCHAR(100) NOT NULL,
    PermissionKey NVARCHAR(150) NOT NULL
);

INSERT INTO #RoleDefaults(RoleName, PermissionKey)
SELECT V.RoleName, V.PermissionKey
FROM (VALUES
    -- الإدارة
    (N'الإدارة', N'Dashboard.View'),
    (N'الإدارة', N'Students.View'),
    (N'الإدارة', N'Students.Add'),
    (N'الإدارة', N'Students.Edit'),
    (N'الإدارة', N'Students.Search'),
    (N'الإدارة', N'Students.Print'),
    (N'الإدارة', N'Students.ExportExcel'),
    (N'الإدارة', N'Students.ExportPDF'),
    (N'الإدارة', N'Enrollment.View'),
    (N'الإدارة', N'Enrollment.Add'),
    (N'الإدارة', N'Enrollment.Edit'),
    (N'الإدارة', N'Enrollment.Search'),
    (N'الإدارة', N'Enrollment.Print'),
    (N'الإدارة', N'ClassAssignment.View'),
    (N'الإدارة', N'ClassAssignment.Add'),
    (N'الإدارة', N'ClassAssignment.Edit'),
    (N'الإدارة', N'ClassAssignment.Search'),
    (N'الإدارة', N'Teachers.View'),
    (N'الإدارة', N'Teachers.Add'),
    (N'الإدارة', N'Teachers.Edit'),
    (N'الإدارة', N'Teachers.Search'),
    (N'الإدارة', N'Subjects.View'),
    (N'الإدارة', N'Subjects.Add'),
    (N'الإدارة', N'Subjects.Edit'),
    (N'الإدارة', N'Classes.View'),
    (N'الإدارة', N'Classes.Add'),
    (N'الإدارة', N'Classes.Edit'),
    (N'الإدارة', N'Rooms.View'),
    (N'الإدارة', N'Rooms.Add'),
    (N'الإدارة', N'Rooms.Edit'),
    (N'الإدارة', N'Timetable.View'),
    (N'الإدارة', N'Timetable.Add'),
    (N'الإدارة', N'Timetable.Edit'),
    (N'الإدارة', N'Timetable.Print'),
    (N'الإدارة', N'Attendance.View'),
    (N'الإدارة', N'Attendance.Add'),
    (N'الإدارة', N'Attendance.Edit'),
    (N'الإدارة', N'Attendance.Print'),
    (N'الإدارة', N'Grades.View'),
    (N'الإدارة', N'Grades.Add'),
    (N'الإدارة', N'Grades.Edit'),
    (N'الإدارة', N'Grades.Approve'),
    (N'الإدارة', N'Grades.Print'),
    (N'الإدارة', N'Reports.View'),
    (N'الإدارة', N'Reports.Print'),
    (N'الإدارة', N'Reports.ExportExcel'),
    (N'الإدارة', N'Reports.ExportPDF'),
    -- شؤون الطلاب
    (N'شؤون الطلاب', N'Dashboard.View'),
    (N'شؤون الطلاب', N'Students.View'),
    (N'شؤون الطلاب', N'Students.Add'),
    (N'شؤون الطلاب', N'Students.Edit'),
    (N'شؤون الطلاب', N'Students.Search'),
    (N'شؤون الطلاب', N'Students.Print'),
    (N'شؤون الطلاب', N'Enrollment.View'),
    (N'شؤون الطلاب', N'Enrollment.Add'),
    (N'شؤون الطلاب', N'Enrollment.Edit'),
    (N'شؤون الطلاب', N'Enrollment.Search'),
    (N'شؤون الطلاب', N'ClassAssignment.View'),
    (N'شؤون الطلاب', N'ClassAssignment.Add'),
    (N'شؤون الطلاب', N'ClassAssignment.Edit'),
    (N'شؤون الطلاب', N'ClassAssignment.Search'),
    (N'شؤون الطلاب', N'Attendance.View'),
    (N'شؤون الطلاب', N'Attendance.Add'),
    (N'شؤون الطلاب', N'Attendance.Edit'),
    (N'شؤون الطلاب', N'Grades.View'),
    (N'شؤون الطلاب', N'Grades.Print'),
    (N'شؤون الطلاب', N'Reports.View'),
    (N'شؤون الطلاب', N'Reports.Print'),
    (N'شؤون الطلاب', N'Reports.ExportExcel'),
    -- المعلمون
    (N'المعلمون', N'Dashboard.View'),
    (N'المعلمون', N'Students.View'),
    (N'المعلمون', N'Students.Search'),
    (N'المعلمون', N'Attendance.View'),
    (N'المعلمون', N'Attendance.Add'),
    (N'المعلمون', N'Attendance.Edit'),
    (N'المعلمون', N'Grades.View'),
    (N'المعلمون', N'Grades.Add'),
    (N'المعلمون', N'Grades.Edit'),
    (N'المعلمون', N'Timetable.View'),
    (N'المعلمون', N'Reports.View'),
    -- المالية
    (N'المالية', N'Dashboard.View'),
    (N'المالية', N'Fees.View'),
    (N'المالية', N'Fees.Add'),
    (N'المالية', N'Fees.Edit'),
    (N'المالية', N'Fees.Search'),
    (N'المالية', N'Fees.Print'),
    (N'المالية', N'Fees.ExportExcel'),
    (N'المالية', N'FeePlans.View'),
    (N'المالية', N'FeePlans.Add'),
    (N'المالية', N'FeePlans.Edit'),
    (N'المالية', N'Vouchers.View'),
    (N'المالية', N'Vouchers.Add'),
    (N'المالية', N'Vouchers.Edit'),
    (N'المالية', N'Vouchers.Print'),
    (N'المالية', N'Vouchers.ExportExcel'),
    (N'المالية', N'Expenses.View'),
    (N'المالية', N'Expenses.Add'),
    (N'المالية', N'Expenses.Edit'),
    (N'المالية', N'Expenses.Print'),
    (N'المالية', N'Expenses.ExportExcel'),
    (N'المالية', N'Payroll.View'),
    (N'المالية', N'Payroll.Search'),
    (N'المالية', N'Payroll.Print'),
    (N'المالية', N'Payroll.ExportExcel'),
    (N'المالية', N'Reports.View'),
    (N'المالية', N'Reports.Print'),
    (N'المالية', N'Reports.ExportExcel'),
    (N'المالية', N'Reports.ExportPDF'),
    -- المكتبة
    (N'المكتبة', N'Dashboard.View'),
    (N'المكتبة', N'Library.View'),
    (N'المكتبة', N'Library.Add'),
    (N'المكتبة', N'Library.Edit'),
    (N'المكتبة', N'Library.Delete'),
    (N'المكتبة', N'Library.Search'),
    (N'المكتبة', N'Library.Print'),
    (N'المكتبة', N'Reports.View'),
    -- النقل
    (N'النقل', N'Dashboard.View'),
    (N'النقل', N'Transport.View'),
    (N'النقل', N'Transport.Add'),
    (N'النقل', N'Transport.Edit'),
    (N'النقل', N'Transport.Delete'),
    (N'النقل', N'Transport.Search'),
    (N'النقل', N'Transport.Print'),
    (N'النقل', N'Reports.View'),
    -- التقارير
    (N'التقارير', N'Dashboard.View'),
    (N'التقارير', N'Reports.View'),
    (N'التقارير', N'Reports.Print'),
    (N'التقارير', N'Reports.ExportExcel'),
    (N'التقارير', N'Reports.ExportPDF')
) V(RoleName, PermissionKey)
WHERE EXISTS (SELECT 1 FROM dbo.Permissions P WHERE P.PermissionKey = V.PermissionKey);

/* إعادة بناء اقتراحات الأدوار القياسية فقط من القائمة المزامنة أعلاه. */
DELETE RP
FROM dbo.RolePermissions RP
JOIN dbo.Roles R ON R.RoleID = RP.RoleID
WHERE R.RoleName IN (N'الإدارة', N'شؤون الطلاب', N'المعلمون', N'المالية', N'المكتبة', N'النقل', N'التقارير')
  AND RP.PermissionID NOT IN
  (
      SELECT P.PermissionID
      FROM #RoleDefaults D
      JOIN dbo.Permissions P ON P.PermissionKey = D.PermissionKey
      WHERE D.RoleName = R.RoleName
  );

INSERT INTO dbo.RolePermissions(RoleID, PermissionID)
SELECT R.RoleID, P.PermissionID
FROM dbo.Roles R
JOIN #RoleDefaults D ON D.RoleName = R.RoleName
JOIN dbo.Permissions P ON P.PermissionKey = D.PermissionKey
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.RolePermissions RP
    WHERE RP.RoleID = R.RoleID AND RP.PermissionID = P.PermissionID
);

DROP TABLE #RoleDefaults;
GO

/* 5) ملخص الفحص بعد المزامنة. */
SELECT N'Permissions' AS EntityName, COUNT(*) AS EntityCount FROM dbo.Permissions
UNION ALL
SELECT N'Roles', COUNT(*) FROM dbo.Roles
UNION ALL
SELECT N'UserRoles', COUNT(*) FROM dbo.UserRoles
UNION ALL
SELECT N'RolePermissions', COUNT(*) FROM dbo.RolePermissions;
GO

SELECT U.UserID, U.UserName, U.RoleName,
       (SELECT COUNT(*) FROM dbo.RolePermissions RP JOIN dbo.Roles R ON R.RoleID = RP.RoleID WHERE R.RoleName = LTRIM(RTRIM(U.RoleName))) AS RolePermissionCount,
       LEN(ISNULL(U.Permissions, N'')) - LEN(REPLACE(ISNULL(U.Permissions, N''), N',', N'')) +
           CASE WHEN NULLIF(LTRIM(RTRIM(U.Permissions)), N'') IS NULL THEN 0 ELSE 1 END AS UserPermissionTokenCount
FROM dbo.Users U
ORDER BY U.UserID;
GO

PRINT N'تمت مزامنة الجداول المعيارية مع كتالوج الكود.';
GO
