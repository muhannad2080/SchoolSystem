SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
    RETURN;

UPDATE P
SET
    ModuleName = LEFT(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) - 1),
    ActionName = SUBSTRING(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) + 1, 100),
    DisplayName =
        CASE
            WHEN P.PermissionKey = N'Dashboard.View' THEN N'عرض لوحة التحكم'
            WHEN P.PermissionKey = N'Users.ManageRoles' THEN N'إدارة أدوار المستخدمين'
            WHEN P.PermissionKey = N'Roles.Manage' THEN N'إدارة الأدوار'
            WHEN P.PermissionKey = N'Permissions.Manage' THEN N'إدارة الصلاحيات'
            WHEN P.PermissionKey = N'Users.Manage' THEN N'إدارة المستخدمين (شاملة)'
            ELSE
                CASE LEFT(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) - 1)
                    WHEN N'Students' THEN N'الطلاب'
                    WHEN N'Enrollment' THEN N'القبول والتسجيل'
                    WHEN N'ClassAssignment' THEN N'توزيع الطلاب'
                    WHEN N'Teachers' THEN N'المعلمون'
                    WHEN N'TeacherAttendance' THEN N'حضور المعلمين'
                    WHEN N'StaffAttendance' THEN N'حضور الموظفين'
                    WHEN N'TeacherContracts' THEN N'عقود المعلمين'
                    WHEN N'Payroll' THEN N'الرواتب'
                    WHEN N'Subjects' THEN N'المواد'
                    WHEN N'Classes' THEN N'الفصول الدراسية'
                    WHEN N'Rooms' THEN N'القاعات'
                    WHEN N'Sections' THEN N'الشعب الدراسية'
                    WHEN N'Timetable' THEN N'الجدول الدراسي'
                    WHEN N'Grades' THEN N'الدرجات'
                    WHEN N'Attendance' THEN N'حضور الطلاب'
                    WHEN N'Fees' THEN N'الرسوم'
                    WHEN N'FeePlans' THEN N'خطط الرسوم'
                    WHEN N'Vouchers' THEN N'السندات'
                    WHEN N'Expenses' THEN N'المصروفات'
                    WHEN N'Transport' THEN N'النقل'
                    WHEN N'Library' THEN N'المكتبة'
                    WHEN N'Reports' THEN N'التقارير'
                    WHEN N'Dashboard' THEN N'لوحة التحكم'
                    WHEN N'AuditLogs' THEN N'سجل التدقيق'
                    WHEN N'Settings' THEN N'الإعدادات'
                    WHEN N'AnnualClosing' THEN N'الإغلاق السنوي والترحيل'
                    WHEN N'Users' THEN N'المستخدمون'
                    WHEN N'Roles' THEN N'الأدوار'
                    WHEN N'Permissions' THEN N'الصلاحيات'
                    ELSE LEFT(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) - 1)
                END
                + N' - ' +
                CASE SUBSTRING(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) + 1, 100)
                    WHEN N'View' THEN N'عرض'
                    WHEN N'Add' THEN N'إضافة'
                    WHEN N'Edit' THEN N'تعديل'
                    WHEN N'Delete' THEN N'حذف'
                    WHEN N'Search' THEN N'بحث'
                    WHEN N'Print' THEN N'طباعة'
                    WHEN N'ExportExcel' THEN N'تصدير Excel'
                    WHEN N'ExportCsv' THEN N'تصدير CSV'
                    WHEN N'ExportPDF' THEN N'تصدير PDF'
                    WHEN N'Approve' THEN N'اعتماد'
                    WHEN N'Cancel' THEN N'إلغاء'
                    WHEN N'Manage' THEN N'إدارة'
                    WHEN N'ManageRoles' THEN N'إدارة الأدوار'
                    ELSE SUBSTRING(P.PermissionKey, CHARINDEX(N'.', P.PermissionKey) + 1, 100)
                END
        END,
    IsActive = 1
FROM dbo.Permissions P
WHERE CHARINDEX(N'.', P.PermissionKey) > 1
  AND NULLIF(LTRIM(RTRIM(P.PermissionKey)), N'') IS NOT NULL;
