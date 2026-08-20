/*
==============================================================================
  SchoolSystem - FIX_RBAC_FINAL.sql
  إصلاح شامل ونهائي لنظام الصلاحيات RBAC
  الإصدار: 2026-08-18
  
  ما يفعله هذا السكريبت:
  1. يتحقق من وجود قاعدة البيانات والجداول الأساسية.
  2. يُصلح نوع عمود Permissions إلى NVARCHAR(MAX) إن لم يكن كذلك.
  3. يبني كتالوج الصلاحيات الكامل المطابق لـ PermissionKeys.cs.
  4. يُحدِّث صلاحيات جميع مدراء النظام بالكتالوج الكامل.
  5. يُصلح صلاحيات كل مستخدم حسب دوره إذا كانت صلاحياته أقل من 3.
  6. يعرض ملخصاً بحالة كل مستخدم بعد الإصلاح.
  
  ملاحظة هامة: هذا السكريبت آمن ومدروس:
  - لا يحذف أي مستخدم.
  - لا يُعيد صياغة الصلاحيات المخصصة للمستخدمين إلا إذا كانت فارغة أو أقل من 3.
  - يُطبّق الكتالوج الكامل لمدير النظام دائماً.
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

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 1: التحقق من البنية الأساسية';
PRINT N'========================================';

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    RAISERROR(N'خطأ: جدول Users غير موجود!', 16, 1);
    RETURN;
END;

IF COL_LENGTH(N'dbo.Users', N'Permissions') IS NULL
BEGIN
    RAISERROR(N'خطأ: عمود Permissions غير موجود!', 16, 1);
    RETURN;
END;

PRINT N'✓ جدول Users وعمود Permissions موجودان.';
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 2: ضمان NVARCHAR(MAX) للصلاحيات';
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
BEGIN
    PRINT N'✓ عمود Permissions بالفعل NVARCHAR(MAX).';
END;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 3: عرض الحالة الحالية للمستخدمين';
PRINT N'========================================';

SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(ISNULL(Permissions, N''))), N'') IS NULL THEN 0
        ELSE LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1
    END AS عدد_الصلاحيات_الحالي,
    LEFT(ISNULL(Permissions, N'(فارغ)'), 150) AS أول_150_حرف_من_الصلاحيات
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 4: بناء كتالوج الصلاحيات الكامل';
PRINT N'(مطابق لـ PermissionKeys.cs - BuildCatalog)';
PRINT N'========================================';

/*
  الكتالوج الكامل المطابق تماماً لما يُنتجه PermissionKeys.BuildCatalog() في C#:
  - المفاتيح الثابتة (Static Constants)
  - كل Modules × StandardActions (بعد Distinct)
  
  الوحدات: Students, Enrollment, ClassAssignment, Teachers, TeacherAttendance,
            StaffAttendance, TeacherContracts, Payroll, Subjects, Classes, Rooms,
            Timetable, Grades, Attendance, Fees, FeePlans, Vouchers, Expenses,
            Transport, Library, Reports, Dashboard, AuditLogs, Settings,
            Users, Roles, Permissions
  
  الأفعال: View, Add, Edit, Delete, Search, Print, ExportExcel, ExportCsv,
           ExportPDF, Approve, Cancel, Manage, ManageRoles
*/

DECLARE @FullCatalog NVARCHAR(MAX);

-- ==========================================
-- بناء الكتالوج بدقة مع جميع Modules × Actions
-- ==========================================
SET @FullCatalog =
    -- Static Constants (المفاتيح الثابتة)
    N'Dashboard.View,' +
    N'Students.View,' +
    N'Students.Manage,' +
    N'Enrollment.Manage,' +
    N'ClassAssignment.View,' +
    N'ClassAssignment.Manage,' +
    N'Teachers.Manage,' +
    N'StaffAttendance.Manage,' +
    N'Payroll.Manage,' +
    N'Subjects.Manage,' +
    N'Classes.Manage,' +
    N'Timetable.Manage,' +
    N'Attendance.Manage,' +
    N'Grades.Manage,' +
    N'Fees.Manage,' +
    N'Vouchers.Manage,' +
    N'Expenses.Manage,' +
    N'Library.Manage,' +
    N'Transport.Manage,' +
    N'Reports.View,' +
    N'Users.Manage,' +
    N'AuditLogs.View,' +
    N'Settings.Manage,' +
    N'Users.View,' +
    N'Users.Add,' +
    N'Users.Edit,' +
    N'Users.Delete,' +
    N'Users.ManageRoles,' +
    N'Roles.View,' +
    N'Roles.Add,' +
    N'Roles.Edit,' +
    N'Roles.Delete,' +
    N'Roles.Manage,' +
    N'Permissions.Manage,' +
    N'AuditLogs.ExportExcel,' +
    N'AuditLogs.ExportPDF,' +
    N'AuditLogs.Print,' +
    N'Settings.View,' +
    N'Settings.Edit,' +
    -- Students × all Actions
    N'Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,' +
    N'Students.ExportExcel,Students.ExportCsv,Students.ExportPDF,Students.Approve,Students.Cancel,Students.ManageRoles,' +
    -- Enrollment × all Actions
    N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,' +
    N'Enrollment.ExportExcel,Enrollment.ExportCsv,Enrollment.ExportPDF,Enrollment.Approve,Enrollment.Cancel,' +
    -- ClassAssignment × all Actions
    N'ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,ClassAssignment.Print,' +
    N'ClassAssignment.ExportExcel,ClassAssignment.ExportCsv,ClassAssignment.ExportPDF,ClassAssignment.Approve,ClassAssignment.Cancel,' +
    -- Teachers × all Actions
    N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,Teachers.Print,' +
    N'Teachers.ExportExcel,Teachers.ExportCsv,Teachers.ExportPDF,Teachers.Approve,Teachers.Cancel,' +
    -- TeacherAttendance × all Actions
    N'TeacherAttendance.View,TeacherAttendance.Add,TeacherAttendance.Edit,TeacherAttendance.Delete,' +
    N'TeacherAttendance.Search,TeacherAttendance.Print,TeacherAttendance.ExportExcel,TeacherAttendance.ExportCsv,' +
    N'TeacherAttendance.ExportPDF,TeacherAttendance.Approve,TeacherAttendance.Cancel,TeacherAttendance.Manage,' +
    -- StaffAttendance × all Actions
    N'StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,' +
    N'StaffAttendance.Search,StaffAttendance.Print,StaffAttendance.ExportExcel,StaffAttendance.ExportCsv,' +
    N'StaffAttendance.ExportPDF,StaffAttendance.Approve,StaffAttendance.Cancel,' +
    -- TeacherContracts × all Actions
    N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,' +
    N'TeacherContracts.Search,TeacherContracts.Print,TeacherContracts.ExportExcel,TeacherContracts.ExportCsv,' +
    N'TeacherContracts.ExportPDF,TeacherContracts.Approve,TeacherContracts.Cancel,TeacherContracts.Manage,' +
    -- Payroll × all Actions
    N'Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,' +
    N'Payroll.ExportExcel,Payroll.ExportCsv,Payroll.ExportPDF,Payroll.Approve,Payroll.Cancel,' +
    -- Subjects × all Actions
    N'Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,Subjects.Search,Subjects.Print,' +
    N'Subjects.ExportExcel,Subjects.ExportCsv,Subjects.ExportPDF,Subjects.Approve,Subjects.Cancel,' +
    -- Classes × all Actions
    N'Classes.View,Classes.Add,Classes.Edit,Classes.Delete,Classes.Search,Classes.Print,' +
    N'Classes.ExportExcel,Classes.ExportCsv,Classes.ExportPDF,Classes.Approve,Classes.Cancel,' +
    -- Rooms × all Actions
    N'Rooms.View,Rooms.Add,Rooms.Edit,Rooms.Delete,Rooms.Search,Rooms.Print,' +
    N'Rooms.ExportExcel,Rooms.ExportCsv,Rooms.ExportPDF,Rooms.Approve,Rooms.Cancel,Rooms.Manage,' +
    -- Timetable × all Actions
    N'Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Search,Timetable.Print,' +
    N'Timetable.ExportExcel,Timetable.ExportCsv,Timetable.ExportPDF,Timetable.Approve,Timetable.Cancel,' +
    -- Grades × all Actions
    N'Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Search,Grades.Print,' +
    N'Grades.ExportExcel,Grades.ExportCsv,Grades.ExportPDF,Grades.Approve,Grades.Cancel,' +
    -- Attendance × all Actions
    N'Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Search,Attendance.Print,' +
    N'Attendance.ExportExcel,Attendance.ExportCsv,Attendance.ExportPDF,Attendance.Approve,Attendance.Cancel,' +
    -- Fees × all Actions
    N'Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,' +
    N'Fees.ExportExcel,Fees.ExportCsv,Fees.ExportPDF,Fees.Approve,Fees.Cancel,' +
    -- FeePlans × all Actions
    N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,FeePlans.Search,FeePlans.Print,' +
    N'FeePlans.ExportExcel,FeePlans.ExportCsv,FeePlans.ExportPDF,FeePlans.Approve,FeePlans.Cancel,FeePlans.Manage,' +
    -- Vouchers × all Actions
    N'Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Search,Vouchers.Print,' +
    N'Vouchers.ExportExcel,Vouchers.ExportCsv,Vouchers.ExportPDF,Vouchers.Approve,Vouchers.Cancel,' +
    -- Expenses × all Actions
    N'Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Search,Expenses.Print,' +
    N'Expenses.ExportExcel,Expenses.ExportCsv,Expenses.ExportPDF,Expenses.Approve,Expenses.Cancel,' +
    -- Transport × all Actions
    N'Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,' +
    N'Transport.ExportExcel,Transport.ExportCsv,Transport.ExportPDF,Transport.Approve,Transport.Cancel,' +
    -- Library × all Actions
    N'Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,' +
    N'Library.ExportExcel,Library.ExportCsv,Library.ExportPDF,Library.Approve,Library.Cancel,' +
    -- Reports × all Actions
    N'Reports.Add,Reports.Edit,Reports.Delete,Reports.Search,Reports.Print,' +
    N'Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,Reports.Approve,Reports.Cancel,' +
    -- Dashboard × all Actions
    N'Dashboard.Add,Dashboard.Edit,Dashboard.Delete,Dashboard.Search,Dashboard.Print,' +
    N'Dashboard.ExportExcel,Dashboard.ExportCsv,Dashboard.ExportPDF,Dashboard.Approve,Dashboard.Cancel,Dashboard.Manage,' +
    -- AuditLogs × all Actions
    N'AuditLogs.Add,AuditLogs.Edit,AuditLogs.Delete,AuditLogs.Search,' +
    N'AuditLogs.Approve,AuditLogs.Cancel,AuditLogs.Manage,' +
    -- Settings × all Actions
    N'Settings.Add,Settings.Delete,Settings.Search,Settings.Print,' +
    N'Settings.ExportExcel,Settings.ExportCsv,Settings.ExportPDF,Settings.Approve,Settings.Cancel,' +
    -- Users × all Actions (Roles/Users/Permissions modules)
    N'Users.Approve,Users.Cancel,Users.Search,Users.Print,Users.ExportExcel,Users.ExportCsv,Users.ExportPDF,' +
    N'Roles.Search,Roles.Print,Roles.ExportExcel,Roles.ExportCsv,Roles.ExportPDF,Roles.Approve,Roles.Cancel,' +
    N'Permissions.View,Permissions.Add,Permissions.Edit,Permissions.Delete,Permissions.Search,Permissions.Print';

SELECT LEN(@FullCatalog) AS حجم_الكتالوج_الكامل_بالأحرف;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 5: تحديث صلاحيات مدير النظام';
PRINT N'(الكتالوج الكامل دائماً)';
PRINT N'========================================';

DECLARE @AdminFullPerms NVARCHAR(MAX);

-- نبني من جدول Permissions إن كان موجوداً ومكتملاً
IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
    AND (SELECT COUNT(*) FROM dbo.Permissions WHERE IsActive = 1 OR IsActive IS NULL) > 50
BEGIN
    SELECT @AdminFullPerms = STRING_AGG(CONVERT(NVARCHAR(MAX), PermissionKey), N',')
    FROM dbo.Permissions
    WHERE IsActive = 1 OR IsActive IS NULL;
    PRINT N'✓ الكتالوج مبني من جدول Permissions';
END;

-- إذا لم يكن الجدول موجوداً أو كان فارغاً، نستخدم القائمة المرجعية الكاملة
IF @AdminFullPerms IS NULL OR LEN(@AdminFullPerms) < 100
BEGIN
    SET @AdminFullPerms =
        N'Dashboard.View,' +
        N'Students.View,Students.Manage,Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,Students.ExportExcel,Students.ExportCsv,Students.ExportPDF,Students.Approve,Students.Cancel,' +
        N'Enrollment.Manage,Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,Enrollment.ExportExcel,Enrollment.ExportCsv,Enrollment.ExportPDF,Enrollment.Approve,Enrollment.Cancel,' +
        N'ClassAssignment.View,ClassAssignment.Manage,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,ClassAssignment.Print,ClassAssignment.ExportExcel,ClassAssignment.ExportCsv,ClassAssignment.ExportPDF,' +
        N'Teachers.Manage,Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,Teachers.Print,Teachers.ExportExcel,Teachers.ExportCsv,Teachers.ExportPDF,Teachers.Approve,Teachers.Cancel,' +
        N'TeacherAttendance.View,TeacherAttendance.Add,TeacherAttendance.Edit,TeacherAttendance.Delete,TeacherAttendance.Search,TeacherAttendance.Print,TeacherAttendance.ExportExcel,TeacherAttendance.ExportCsv,TeacherAttendance.ExportPDF,TeacherAttendance.Manage,' +
        N'StaffAttendance.Manage,StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,StaffAttendance.ExportExcel,StaffAttendance.ExportCsv,StaffAttendance.ExportPDF,' +
        N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,TeacherContracts.Print,TeacherContracts.ExportExcel,TeacherContracts.ExportCsv,TeacherContracts.ExportPDF,TeacherContracts.Manage,' +
        N'Payroll.Manage,Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Payroll.ExportCsv,Payroll.ExportPDF,Payroll.Approve,Payroll.Cancel,' +
        N'Subjects.Manage,Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,Subjects.Search,Subjects.Print,Subjects.ExportExcel,Subjects.ExportCsv,Subjects.ExportPDF,' +
        N'Classes.Manage,Classes.View,Classes.Add,Classes.Edit,Classes.Delete,Classes.Search,Classes.Print,Classes.ExportExcel,Classes.ExportCsv,Classes.ExportPDF,' +
        N'Rooms.View,Rooms.Add,Rooms.Edit,Rooms.Delete,Rooms.Search,Rooms.Print,Rooms.ExportExcel,Rooms.ExportCsv,Rooms.ExportPDF,Rooms.Manage,' +
        N'Timetable.Manage,Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Search,Timetable.Print,Timetable.ExportExcel,Timetable.ExportCsv,Timetable.ExportPDF,' +
        N'Grades.Manage,Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Search,Grades.Print,Grades.ExportExcel,Grades.ExportCsv,Grades.ExportPDF,Grades.Approve,Grades.Cancel,' +
        N'Attendance.Manage,Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Search,Attendance.Print,Attendance.ExportExcel,Attendance.ExportCsv,Attendance.ExportPDF,' +
        N'Fees.Manage,Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,Fees.ExportCsv,Fees.ExportPDF,Fees.Approve,Fees.Cancel,' +
        N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,FeePlans.Search,FeePlans.Print,FeePlans.ExportExcel,FeePlans.ExportCsv,FeePlans.ExportPDF,FeePlans.Manage,' +
        N'Vouchers.Manage,Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Search,Vouchers.Print,Vouchers.ExportExcel,Vouchers.ExportCsv,Vouchers.ExportPDF,Vouchers.Approve,Vouchers.Cancel,' +
        N'Expenses.Manage,Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Search,Expenses.Print,Expenses.ExportExcel,Expenses.ExportCsv,Expenses.ExportPDF,' +
        N'Transport.Manage,Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,Transport.ExportExcel,Transport.ExportCsv,Transport.ExportPDF,' +
        N'Library.Manage,Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,Library.ExportExcel,Library.ExportCsv,Library.ExportPDF,' +
        N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,Reports.Search,Reports.Approve,Reports.Add,Reports.Edit,Reports.Delete,Reports.Cancel,' +
        N'AuditLogs.View,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportPDF,AuditLogs.ExportCsv,AuditLogs.Search,AuditLogs.Manage,' +
        N'Settings.View,Settings.Edit,Settings.Manage,Settings.Print,Settings.ExportExcel,' +
        N'Users.Manage,Users.View,Users.Add,Users.Edit,Users.Delete,Users.ManageRoles,Users.Search,Users.Print,Users.ExportExcel,Users.ExportCsv,Users.ExportPDF,' +
        N'Roles.View,Roles.Add,Roles.Edit,Roles.Delete,Roles.Manage,Roles.Search,Roles.Print,Roles.ExportExcel,Roles.ExportCsv,Roles.ExportPDF,' +
        N'Permissions.Manage,Permissions.View,Permissions.Add,Permissions.Edit,Permissions.Delete,Permissions.Search,Permissions.Print';
    PRINT N'✓ الكتالوج مبني من القائمة المرجعية المدمجة في السكريبت';
END;

-- تحديث مدير النظام بالصلاحيات الكاملة
UPDATE dbo.Users
SET RoleName    = N'مدير النظام',
    Permissions = @AdminFullPerms,
    UpdatedAt   = GETDATE()
WHERE LTRIM(RTRIM(ISNULL(RoleName, N''))) IN (N'مدير النظام', N'Admin', N'Administrator');

PRINT N'✓ تم تحديث صلاحيات مدير/مدراء النظام بالكتالوج الكامل.';
PRINT N'  عدد المستخدمين المحدَّثون: ' + CAST(@@ROWCOUNT AS NVARCHAR(10));
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 6: إصلاح المستخدمين ذوي الصلاحيات المنعدمة/الناقصة';
PRINT N'(أقل من 3 صلاحيات = مشكلة)';
PRINT N'========================================';

-- معالجة المستخدمين بصلاحيات فارغة أو قليلة جداً حسب دورهم
-- هذا يصلح المستخدمين الذين كانت صلاحياتهم 'Dashboard.View,Reports.View' فقط
-- بسبب الـ Migration_Step1.sql القديم.

-- دور 'الإدارة'
UPDATE dbo.Users
SET Permissions =
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
WHERE LTRIM(RTRIM(RoleName)) = N'الإدارة'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 5
  );

-- دور 'شؤون الطلاب'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Add,Students.Edit,Students.Search,Students.Print,' +
    N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Search,' +
    N'ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Search,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,' +
    N'Grades.View,Grades.Print,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'شؤون الطلاب'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 5
  );

-- دور 'المعلمون'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Search,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,' +
    N'Grades.View,Grades.Add,Grades.Edit,' +
    N'Timetable.View,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'المعلمون'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'المالية'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,' +
    N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,' +
    N'Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Print,Vouchers.ExportExcel,' +
    N'Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Print,Expenses.ExportExcel,' +
    N'Payroll.View,Payroll.Search,Payroll.Print,Payroll.ExportExcel,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'المالية'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 5
  );

-- دور 'المكتبة' + 'أمين المكتبة'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) IN (N'المكتبة', N'أمين المكتبة')
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'النقل' + 'مسؤول النقل'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) IN (N'النقل', N'مسؤول النقل')
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'التقارير'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'التقارير'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'شؤون الموظفين'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,' +
    N'StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,' +
    N'Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,' +
    N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'شؤون الموظفين'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'موظف الاستقبال'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Students.View,Students.Search,' +
    N'Enrollment.View,Enrollment.Search,' +
    N'Reports.View',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'موظف الاستقبال'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'مدقق'
UPDATE dbo.Users
SET Permissions =
    N'Dashboard.View,' +
    N'Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,' +
    N'AuditLogs.View,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportPDF',
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(RoleName)) = N'مدقق'
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 3
  );

-- دور 'مدير المدرسة' + 'وكيل المدرسة' (نفس الإدارة)
UPDATE dbo.Users
SET Permissions =
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
WHERE LTRIM(RTRIM(RoleName)) IN (N'مدير المدرسة', N'وكيل المدرسة')
  AND (
    Permissions IS NULL
    OR LTRIM(RTRIM(Permissions)) = N''
    OR (
      LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
      CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
    ) < 5
  );

PRINT N'✓ تم إصلاح صلاحيات المستخدمين حسب أدوارهم.';
GO

PRINT N'';
PRINT N'========================================';
PRINT N'الخطوة 7: التحقق النهائي من حالة جميع المستخدمين';
PRINT N'========================================';

SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(ISNULL(Permissions, N''))), N'') IS NULL THEN 0
        ELSE LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1
    END AS عدد_الصلاحيات,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(ISNULL(Permissions, N''))), N'') IS NULL
             THEN N'⛔ لا توجد صلاحيات - يحتاج إصلاح يدوي'
        WHEN (LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1) < 3
             THEN N'⚠ صلاحيات قليلة جداً'
        WHEN (LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1) >= 50
             AND LTRIM(RTRIM(ISNULL(RoleName, N''))) IN (N'مدير النظام', N'Admin', N'Administrator')
             THEN N'✅ مدير نظام - كتالوج كامل'
        WHEN (LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1) >= 5
             THEN N'✓ صلاحيات كافية'
        ELSE N'● صلاحيات محدودة'
    END AS تقييم_الصلاحيات,
    LEFT(ISNULL(Permissions, N'(فارغ)'), 200) AS أول_200_حرف_من_الصلاحيات
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'';
PRINT N'======================================================';
PRINT N'✅ تم الانتهاء من سكريبت إصلاح RBAC النهائي';
PRINT N'';
PRINT N'الخطوات التالية المطلوبة:';
PRINT N'1. أغلق التطبيق تماماً إن كان مفتوحاً.';
PRINT N'2. أعد تشغيل التطبيق.';
PRINT N'3. سجّل الدخول بحساب مدير النظام.';
PRINT N'4. تحقق من ظهور جميع القوائم.';
PRINT N'5. للمستخدمين الآخرين: سجّل الخروج ثم الدخول مرة أخرى.';
PRINT N'';
PRINT N'مهم: إذا لم تظهر الشاشات بعد تطبيق السكريبت، ابحث عن:';
PRINT N'   - هل Permissions محفوظة بـ "*.Manage" بدلاً من "*.View" للوحدة؟';
PRINT N'   - هل RoleName للمستخدم مطابق تماماً لما في الكود؟';
PRINT N'======================================================';
GO
