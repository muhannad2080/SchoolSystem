/*
==============================================================================
  SchoolSystem - FIX_RBAC_SourceOfTruth.sql
  إصلاح مصدر الحقيقة لنظام الصلاحيات RBAC
  الإصدار: 2026-08-20

  الهدف:
  1. جعل عمود Users.Permissions (المحفوظ يدوياً) هو مصدر الحقيقة الوحيد.
  2. معالجة الحسابات القديمة:
       - Permissions = NULL  -> يُعاد تعبئته من دور المستخدم (GetRoleDefaults).
       - Permissions = ''    -> النص الفارغ اختيار يدوي عمدي، لا نلمسه.
       - القيمة القديمة 'Dashboard.View,Reports.View' فقط لدور غير التقارير/مدقق
         -> تُستبدل بصلاحيات الدور (أثر Migration_Step1.sql القديم).
  3. مزامنة الجدول المعياري UserRoles مع Users.RoleName لكل المستخدمين
     حتى يبقى مسار الاستعادة متسقاً مع الاختيار الحالي.
  4. عرض ملخص نهائي بحالة كل مستخدم.

  السكريبت آمن:
   - لا يحذف مستخدماً.
   - لا يستبدل الصلاحيات المخصصة المحفوظة فعلياً (غير NULL وليست القيمة القديمة).
   - مدير النظام يحصل دائماً على الكتالوج الكامل (كما يفعل الكود عند الدخول).
==============================================================================
*/

USE SchoolDB;
GO

IF DB_NAME() <> N'SchoolDB'
BEGIN
    RAISERROR(N'خطأ: يجب الاتصال بقاعدة SchoolDB.', 16, 1);
    RETURN;
END;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    RAISERROR(N'خطأ: جدول Users غير موجود!', 16, 1);
    RETURN;
END;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 1: ضمان NVARCHAR(MAX) لعمود الصلاحيات';
PRINT N'========================================';

IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.Users')
      AND c.name = N'Permissions'
      AND NOT (t.name = N'nvarchar' AND c.max_length = -1)
)
BEGIN
    ALTER TABLE dbo.Users ALTER COLUMN Permissions NVARCHAR(MAX) NULL;
    PRINT N'✓ تم تحويل عمود Permissions إلى NVARCHAR(MAX).';
END
ELSE
    PRINT N'✓ عمود Permissions بالفعل NVARCHAR(MAX).';
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 2: الحالة الحالية قبل الإصلاح';
PRINT N'========================================';

SELECT
    UserID,
    UserName,
    RoleName,
    CASE
        WHEN Permissions IS NULL THEN N'NULL (يحتاج تعبئة)'
        WHEN LTRIM(RTRIM(Permissions)) = N'' THEN N'فارغ (اختيار عمدي)'
        WHEN LTRIM(RTRIM(Permissions)) = N'Dashboard.View,Reports.View'
             AND LTRIM(RTRIM(RoleName)) NOT IN (N'التقارير', N'مدقق') THEN N'قيمة قديمة (تحتاج إصلاح)'
        ELSE N'محفوظ'
    END AS حالة_الصلاحيات,
    LEN(ISNULL(Permissions, N'')) AS الطول
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 3: تعبئة NULL و القيم القديمة بصلاحيات الدور';
PRINT N'(مطابقة لـ PermissionKeys.GetRoleDefaults في C#)';
PRINT N'========================================';

-- مدير النظام: الكتالوج الكامل
DECLARE @AdminFull NVARCHAR(MAX);

IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
    AND (SELECT COUNT(*) FROM dbo.Permissions WHERE IsActive = 1 OR IsActive IS NULL) > 50
BEGIN
    SELECT @AdminFull = STRING_AGG(CONVERT(NVARCHAR(MAX), PermissionKey), N',')
    FROM dbo.Permissions
    WHERE IsActive = 1 OR IsActive IS NULL;
    PRINT N'✓ الكتالوج الكامل مبني من جدول Permissions';
END;

IF @AdminFull IS NULL OR LEN(@AdminFull) < 100
BEGIN
    SET @AdminFull =
        N'Dashboard.View,' +
        N'Students.View,Students.Manage,Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,Students.ExportExcel,Students.ExportCsv,Students.ExportPDF,Students.Approve,Students.Cancel,Students.ManageRoles,' +
        N'Enrollment.Manage,Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,Enrollment.ExportExcel,Enrollment.ExportCsv,Enrollment.ExportPDF,Enrollment.Approve,Enrollment.Cancel,Enrollment.ManageRoles,' +
        N'ClassAssignment.View,ClassAssignment.Manage,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,ClassAssignment.Print,ClassAssignment.ExportExcel,ClassAssignment.ExportCsv,ClassAssignment.ExportPDF,ClassAssignment.Approve,ClassAssignment.Cancel,ClassAssignment.ManageRoles,' +
        N'Teachers.Manage,Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,Teachers.Print,Teachers.ExportExcel,Teachers.ExportCsv,Teachers.ExportPDF,Teachers.Approve,Teachers.Cancel,Teachers.ManageRoles,' +
        N'TeacherAttendance.View,TeacherAttendance.Manage,TeacherAttendance.Add,TeacherAttendance.Edit,TeacherAttendance.Delete,TeacherAttendance.Search,TeacherAttendance.Print,TeacherAttendance.ExportExcel,TeacherAttendance.ExportCsv,TeacherAttendance.ExportPDF,TeacherAttendance.Approve,TeacherAttendance.Cancel,TeacherAttendance.ManageRoles,' +
        N'StaffAttendance.Manage,StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,StaffAttendance.ExportExcel,StaffAttendance.ExportCsv,StaffAttendance.ExportPDF,StaffAttendance.Approve,StaffAttendance.Cancel,StaffAttendance.ManageRoles,' +
        N'TeacherContracts.View,TeacherContracts.Manage,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,TeacherContracts.Print,TeacherContracts.ExportExcel,TeacherContracts.ExportCsv,TeacherContracts.ExportPDF,TeacherContracts.Approve,TeacherContracts.Cancel,TeacherContracts.ManageRoles,' +
        N'Payroll.Manage,Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Payroll.ExportCsv,Payroll.ExportPDF,Payroll.Approve,Payroll.Cancel,Payroll.ManageRoles,' +
        N'Subjects.Manage,Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,Subjects.Search,Subjects.Print,Subjects.ExportExcel,Subjects.ExportCsv,Subjects.ExportPDF,Subjects.Approve,Subjects.Cancel,Subjects.ManageRoles,' +
        N'Classes.Manage,Classes.View,Classes.Add,Classes.Edit,Classes.Delete,Classes.Search,Classes.Print,Classes.ExportExcel,Classes.ExportCsv,Classes.ExportPDF,Classes.Approve,Classes.Cancel,Classes.ManageRoles,' +
        N'Rooms.View,Rooms.Add,Rooms.Edit,Rooms.Delete,Rooms.Search,Rooms.Print,Rooms.ExportExcel,Rooms.ExportCsv,Rooms.ExportPDF,Rooms.Approve,Rooms.Cancel,Rooms.Manage,Rooms.ManageRoles,' +
        N'Timetable.Manage,Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Search,Timetable.Print,Timetable.ExportExcel,Timetable.ExportCsv,Timetable.ExportPDF,Timetable.Approve,Timetable.Cancel,Timetable.ManageRoles,' +
        N'Grades.Manage,Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Search,Grades.Print,Grades.ExportExcel,Grades.ExportCsv,Grades.ExportPDF,Grades.Approve,Grades.Cancel,Grades.ManageRoles,' +
        N'Attendance.Manage,Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Search,Attendance.Print,Attendance.ExportExcel,Attendance.ExportCsv,Attendance.ExportPDF,Attendance.Approve,Attendance.Cancel,Attendance.ManageRoles,' +
        N'Fees.Manage,Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,Fees.ExportCsv,Fees.ExportPDF,Fees.Approve,Fees.Cancel,Fees.ManageRoles,' +
        N'FeePlans.View,FeePlans.Manage,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,FeePlans.Search,FeePlans.Print,FeePlans.ExportExcel,FeePlans.ExportCsv,FeePlans.ExportPDF,FeePlans.Approve,FeePlans.Cancel,FeePlans.ManageRoles,' +
        N'Vouchers.Manage,Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Search,Vouchers.Print,Vouchers.ExportExcel,Vouchers.ExportCsv,Vouchers.ExportPDF,Vouchers.Approve,Vouchers.Cancel,Vouchers.ManageRoles,' +
        N'Expenses.Manage,Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Search,Expenses.Print,Expenses.ExportExcel,Expenses.ExportCsv,Expenses.ExportPDF,Expenses.Approve,Expenses.Cancel,Expenses.ManageRoles,' +
        N'Transport.Manage,Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,Transport.ExportExcel,Transport.ExportCsv,Transport.ExportPDF,Transport.Approve,Transport.Cancel,Transport.ManageRoles,' +
        N'Library.Manage,Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,Library.ExportExcel,Library.ExportCsv,Library.ExportPDF,Library.Approve,Library.Cancel,Library.ManageRoles,' +
        N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,Reports.Search,Reports.Approve,Reports.Add,Reports.Edit,Reports.Delete,Reports.Cancel,Reports.Manage,Reports.ManageRoles,' +
        N'Dashboard.Manage,Dashboard.View,Dashboard.Add,Dashboard.Edit,Dashboard.Delete,Dashboard.Search,Dashboard.Print,Dashboard.ExportExcel,Dashboard.ExportCsv,Dashboard.ExportPDF,Dashboard.Approve,Dashboard.Cancel,Dashboard.ManageRoles,' +
        N'AuditLogs.View,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportPDF,AuditLogs.ExportCsv,AuditLogs.Search,AuditLogs.Manage,AuditLogs.Add,AuditLogs.Edit,AuditLogs.Delete,AuditLogs.Approve,AuditLogs.Cancel,AuditLogs.ManageRoles,' +
        N'Settings.View,Settings.Edit,Settings.Manage,Settings.Print,Settings.ExportExcel,Settings.ExportCsv,Settings.ExportPDF,Settings.Add,Settings.Delete,Settings.Search,Settings.Approve,Settings.Cancel,Settings.ManageRoles,' +
        N'Users.Manage,Users.View,Users.Add,Users.Edit,Users.Delete,Users.ManageRoles,Users.Search,Users.Print,Users.ExportExcel,Users.ExportCsv,Users.ExportPDF,Users.Approve,Users.Cancel,Users.ManageRoles,' +
        N'Roles.View,Roles.Add,Roles.Edit,Roles.Delete,Roles.Manage,Roles.Search,Roles.Print,Roles.ExportExcel,Roles.ExportCsv,Roles.ExportPDF,Roles.Approve,Roles.Cancel,Roles.ManageRoles,' +
        N'Permissions.Manage,Permissions.View,Permissions.Add,Permissions.Edit,Permissions.Delete,Permissions.Search,Permissions.Print,Permissions.Approve,Permissions.Cancel,Permissions.ManageRoles';
    PRINT N'✓ الكتالوج الكامل مبني من القائمة المرجعية في السكريبت';
END;

-- تعبئة NULL للدور الإداري
UPDATE dbo.Users
SET Permissions = @AdminFull, UpdatedAt = GETDATE()
WHERE Permissions IS NULL
  AND LTRIM(RTRIM(RoleName)) IN (N'مدير النظام', N'Admin', N'Administrator');

-- تعبئة NULL لباقي الأدوار (مطابقة GetRoleDefaults)
UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,Students.ExportExcel,Students.ExportPDF,' +
    N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,' +
    N'ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,' +
    N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,' +
    N'Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,' +
    N'Classes.View,Classes.Add,Classes.Edit,Classes.Delete,' +
    N'Rooms.View,Rooms.Add,Rooms.Edit,' +
    N'Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Print,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Print,' +
    N'Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Approve,Grades.Print,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,' +
    N'AuditLogs.View',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) IN (N'الإدارة', N'مدير المدرسة', N'وكيل المدرسة');

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Add,Students.Edit,Students.Search,Students.Print,' +
    N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Search,' +
    N'ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Search,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,' +
    N'Grades.View,Grades.Print,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'شؤون الطلاب';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Search,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,' +
    N'Grades.View,Grades.Add,Grades.Edit,' +
    N'Timetable.View,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'المعلمون';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,' +
    N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,' +
    N'Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Print,Vouchers.ExportExcel,' +
    N'Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Print,Expenses.ExportExcel,' +
    N'Payroll.View,Payroll.Search,Payroll.Print,Payroll.ExportExcel,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'المالية';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) IN (N'المكتبة', N'أمين المكتبة');

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) IN (N'النقل', N'مسؤول النقل');

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'التقارير';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,' +
    N'AuditLogs.View,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportPDF',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'مدقق';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,' +
    N'StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,' +
    N'Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,' +
    N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'شؤون الموظفين';

UPDATE dbo.Users SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Search,' +
    N'Enrollment.View,Enrollment.Search,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE Permissions IS NULL AND LTRIM(RTRIM(RoleName)) = N'موظف الاستقبال';

-- إصلاح القيمة القديمة 'Dashboard.View,Reports.View' فقط
-- لدور غير التقارير/مدقق (أثر Migration_Step1.sql القديم):
-- نسخ صلاحيات الدور المطابقة من جدول Permissions المعياري إن أمكن،
-- وإلا تُعاد للدور عبر الدخول التالي (الكود يتولى التعبئة والاستمرار).
UPDATE u
SET Permissions = CASE
        WHEN LTRIM(RTRIM(u.RoleName)) IN (N'الإدارة', N'مدير المدرسة', N'وكيل المدرسة')
             THEN N'Dashboard.View,Students.View,Students.Add,Students.Edit,Students.Search,Students.Print,Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Search,Attendance.View,Attendance.Add,Attendance.Edit,Grades.View,Grades.Print,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv'
        WHEN LTRIM(RTRIM(u.RoleName)) = N'المعلمون'
             THEN N'Dashboard.View,Students.View,Students.Search,Attendance.View,Attendance.Add,Attendance.Edit,Grades.View,Grades.Add,Grades.Edit,Timetable.View,Reports.View'
        WHEN LTRIM(RTRIM(u.RoleName)) = N'المالية'
             THEN N'Dashboard.View,Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,FeePlans.View,FeePlans.Add,FeePlans.Edit,Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Print,Vouchers.ExportExcel,Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Print,Expenses.ExportExcel,Payroll.View,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF'
        ELSE NULL
    END,
    UpdatedAt = GETDATE()
FROM dbo.Users u
WHERE LTRIM(RTRIM(u.Permissions)) = N'Dashboard.View,Reports.View'
  AND LTRIM(RTRIM(u.RoleName)) NOT IN (N'التقارير', N'مدقق');
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 4: مزامنة UserRoles مع Users.RoleName';
PRINT N'========================================';

IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL
BEGIN
    -- ضمان وجود دور لكل قيمة RoleName نشطة في Users
    INSERT INTO dbo.Roles (RoleName, IsSystemRole, IsActive, CreatedAt)
    SELECT DISTINCT LTRIM(RTRIM(u.RoleName)), 0, 1, GETDATE()
    FROM dbo.Users u
    WHERE LTRIM(RTRIM(ISNULL(u.RoleName, N''))) <> N''
      AND NOT EXISTS (
          SELECT 1 FROM dbo.Roles r
          WHERE LTRIM(RTRIM(r.RoleName)) = LTRIM(RTRIM(u.RoleName))
      );

    -- حذف الروابط المعيارية القديمة
    DELETE FROM dbo.UserRoles
    WHERE UserID IN (SELECT UserID FROM dbo.Users);

    -- إعادة ربط كل مستخدم بدوره الحالي
    INSERT INTO dbo.UserRoles (UserID, RoleID, AssignedAt)
    SELECT u.UserID, r.RoleID, GETDATE()
    FROM dbo.Users u
    INNER JOIN dbo.Roles r ON LTRIM(RTRIM(r.RoleName)) = LTRIM(RTRIM(u.RoleName))
    WHERE LTRIM(RTRIM(ISNULL(u.RoleName, N''))) <> N'';

    PRINT N'✓ تمت مزامنة UserRoles مع Users.RoleName لجميع المستخدمين.';
END
ELSE
    PRINT N'● الجداول المعيارية (Roles/UserRoles) غير موجودة - يتم تخطي المزامنة.';
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 5: الملخص النهائي';
PRINT N'========================================';

SELECT
    u.UserID,
    u.UserName,
    u.RoleName,
    CASE
        WHEN u.Permissions IS NULL THEN N'⛔ NULL'
        WHEN LTRIM(RTRIM(u.Permissions)) = N'' THEN N'فارغ (عمدي)'
        ELSE N'✓ محفوظ'
    END AS صلاحيات,
    LEN(ISNULL(u.Permissions, N'')) AS طول_الصلاحيات,
    CASE WHEN r.RoleName IS NULL THEN N'—' ELSE r.RoleName END AS الدور_المعياري
FROM dbo.Users u
LEFT JOIN dbo.UserRoles ur ON ur.UserID = u.UserID
LEFT JOIN dbo.Roles r ON r.RoleID = ur.RoleID
ORDER BY u.UserID;
GO

PRINT N'';
PRINT N'======================================================';
PRINT N'✅ تم الانتهاء. بعد التنفيذ:';
PRINT N'1. أغلق التطبيق إن كان مفتوحاً ثم أعد تشغيله.';
PRINT N'2. سجّل الدخول بحساب المدير للتحقق من ظهور كل القوائم.';
PRINT N'3. أي حساب تظهر له صلاحيات غير متوقعة: افتح واجهة';
PRINT N'   المستخدمين وعدّل تخصيصه ثم احفظ.';
PRINT N'======================================================';
GO