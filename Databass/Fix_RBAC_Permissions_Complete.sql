/*
==========================================================================
SchoolSystem - Fix RBAC Permissions Complete
سكريبت إصلاح شامل لنظام الصلاحيات - التشخيص والإصلاح الكامل
الإصدار: 2026-08-18
==========================================================================
تعليمات التشغيل:
1. قم بعمل نسخة احتياطية من قاعدة البيانات أولاً.
2. شغّل هذا الملف على قاعدة SchoolDB.
3. راجع نتائج الـ SELECT في النهاية للتحقق من الإصلاح.
==========================================================================
*/

USE SchoolDB;
GO

IF DB_NAME() <> N'SchoolDB'
BEGIN
    RAISERROR(N'خطأ: يجب الاتصال بقاعدة SchoolDB وليس master.', 16, 1);
    RETURN;
END;
GO

PRINT N'========================================';
PRINT N'خطوة 1: تشخيص حالة قاعدة البيانات';
PRINT N'========================================';

-- 1.1 التحقق من وجود جدول Users وعمود Permissions
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    PRINT N'خطأ: جدول Users غير موجود! نفّذ Migration_Step1.sql أولاً.';
    RETURN;
END;

IF COL_LENGTH(N'dbo.Users', N'Permissions') IS NULL
BEGIN
    PRINT N'خطأ: عمود Permissions غير موجود!';
    RETURN;
END;

-- 1.2 التحقق من نوع عمود Permissions (يجب NVARCHAR(MAX))
SELECT
    N'نوع عمود Permissions' AS الفحص,
    c.name AS اسم_العمود,
    t.name AS نوع_البيانات,
    c.max_length AS الطول_max,
    CASE WHEN t.name = N'nvarchar' AND c.max_length = -1
         THEN N'✓ صحيح NVARCHAR(MAX)'
         ELSE N'⚠ تحذير: العمود ليس NVARCHAR(MAX) - قد يتسبب في قطع النص!'
    END AS الحالة
FROM sys.columns c
JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'dbo.Users')
  AND c.name = N'Permissions';

-- 1.3 إصلاح نوع عمود Permissions إذا لم يكن NVARCHAR(MAX)
IF EXISTS
(
    SELECT 1
    FROM sys.columns c
    JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.Users')
      AND c.name = N'Permissions'
      AND NOT (t.name = N'nvarchar' AND c.max_length = -1)
)
BEGIN
    PRINT N'⚠ إصلاح: تحويل عمود Permissions إلى NVARCHAR(MAX)...';
    ALTER TABLE dbo.Users ALTER COLUMN Permissions NVARCHAR(MAX) NULL;
    PRINT N'✓ تم إصلاح نوع عمود Permissions';
END;
GO

PRINT N'========================================';
PRINT N'خطوة 2: عرض الحالة الحالية للمستخدمين';
PRINT N'========================================';

SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    LEN(ISNULL(Permissions, N'')) AS طول_نص_الصلاحيات,
    LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
        CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END AS عدد_الصلاحيات,
    LEFT(ISNULL(Permissions, N'لا يوجد'), 200) AS أول_200_حرف_من_الصلاحيات
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'========================================';
PRINT N'خطوة 3: قائمة الكتالوج الكامل للصلاحيات';
PRINT N'========================================';

-- كتالوج الصلاحيات الكامل المتطابق مع PermissionKeys.cs في الكود
-- هذه القائمة يجب أن تُطابق ما يُنتجه BuildCatalog() في C#
DECLARE @FullPermissionsCatalog NVARCHAR(MAX);
SET @FullPermissionsCatalog =
    -- المفاتيح الثابتة (Static keys)
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
    -- المفاتيح من Modules × StandardActions
    N'Students.View,Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,Students.ExportExcel,Students.ExportCsv,Students.ExportPDF,Students.Approve,Students.Cancel,' +
    N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,Enrollment.ExportExcel,Enrollment.ExportCsv,Enrollment.ExportPDF,Enrollment.Approve,Enrollment.Cancel,' +
    N'ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,ClassAssignment.Print,ClassAssignment.ExportExcel,ClassAssignment.ExportCsv,ClassAssignment.ExportPDF,ClassAssignment.Approve,ClassAssignment.Cancel,' +
    N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,Teachers.Print,Teachers.ExportExcel,Teachers.ExportCsv,Teachers.ExportPDF,Teachers.Approve,Teachers.Cancel,' +
    N'TeacherAttendance.View,TeacherAttendance.Add,TeacherAttendance.Edit,TeacherAttendance.Delete,TeacherAttendance.Search,TeacherAttendance.Print,TeacherAttendance.ExportExcel,TeacherAttendance.ExportCsv,TeacherAttendance.ExportPDF,TeacherAttendance.Approve,TeacherAttendance.Cancel,' +
    N'StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,StaffAttendance.ExportExcel,StaffAttendance.ExportCsv,StaffAttendance.ExportPDF,StaffAttendance.Approve,StaffAttendance.Cancel,' +
    N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,TeacherContracts.Print,TeacherContracts.ExportExcel,TeacherContracts.ExportCsv,TeacherContracts.ExportPDF,TeacherContracts.Approve,TeacherContracts.Cancel,' +
    N'Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Payroll.ExportCsv,Payroll.ExportPDF,Payroll.Approve,Payroll.Cancel,' +
    N'Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,Subjects.Search,Subjects.Print,Subjects.ExportExcel,Subjects.ExportCsv,Subjects.ExportPDF,Subjects.Approve,Subjects.Cancel,' +
    N'Classes.View,Classes.Add,Classes.Edit,Classes.Delete,Classes.Search,Classes.Print,Classes.ExportExcel,Classes.ExportCsv,Classes.ExportPDF,Classes.Approve,Classes.Cancel,' +
    N'Rooms.View,Rooms.Add,Rooms.Edit,Rooms.Delete,Rooms.Search,Rooms.Print,Rooms.ExportExcel,Rooms.ExportCsv,Rooms.ExportPDF,Rooms.Approve,Rooms.Cancel,' +
    N'Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Search,Timetable.Print,Timetable.ExportExcel,Timetable.ExportCsv,Timetable.ExportPDF,Timetable.Approve,Timetable.Cancel,' +
    N'Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Search,Grades.Print,Grades.ExportExcel,Grades.ExportCsv,Grades.ExportPDF,Grades.Approve,Grades.Cancel,' +
    N'Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Search,Attendance.Print,Attendance.ExportExcel,Attendance.ExportCsv,Attendance.ExportPDF,Attendance.Approve,Attendance.Cancel,' +
    N'Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,Fees.ExportCsv,Fees.ExportPDF,Fees.Approve,Fees.Cancel,' +
    N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,FeePlans.Search,FeePlans.Print,FeePlans.ExportExcel,FeePlans.ExportCsv,FeePlans.ExportPDF,FeePlans.Approve,FeePlans.Cancel,' +
    N'Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Search,Vouchers.Print,Vouchers.ExportExcel,Vouchers.ExportCsv,Vouchers.ExportPDF,Vouchers.Approve,Vouchers.Cancel,' +
    N'Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Search,Expenses.Print,Expenses.ExportExcel,Expenses.ExportCsv,Expenses.ExportPDF,Expenses.Approve,Expenses.Cancel,' +
    N'Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,Transport.ExportExcel,Transport.ExportCsv,Transport.ExportPDF,Transport.Approve,Transport.Cancel,' +
    N'Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,Library.ExportExcel,Library.ExportCsv,Library.ExportPDF,Library.Approve,Library.Cancel,' +
    N'Reports.View,Reports.Add,Reports.Edit,Reports.Delete,Reports.Search,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,Reports.Approve,Reports.Cancel,' +
    N'Dashboard.View,Dashboard.Add,Dashboard.Edit,Dashboard.Delete,Dashboard.Search,Dashboard.Print,Dashboard.ExportExcel,Dashboard.ExportCsv,Dashboard.ExportPDF,Dashboard.Approve,Dashboard.Cancel,' +
    N'AuditLogs.View,AuditLogs.Add,AuditLogs.Edit,AuditLogs.Delete,AuditLogs.Search,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportCsv,AuditLogs.ExportPDF,AuditLogs.Approve,AuditLogs.Cancel,' +
    N'Settings.View,Settings.Add,Settings.Edit,Settings.Delete,Settings.Search,Settings.Print,Settings.ExportExcel,Settings.ExportCsv,Settings.ExportPDF,Settings.Approve,Settings.Cancel';

-- إذهب للخطوة 4 مباشرة
SELECT LEN(@FullPermissionsCatalog) AS حجم_كتالوج_الصلاحيات_الكامل;
GO

PRINT N'========================================';
PRINT N'خطوة 4: إصلاح صلاحيات مدير النظام';
PRINT N'========================================';

-- الكتالوج الكامل لمدير النظام (مطابق لما يُنتجه PermissionKeys.GetRoleDefaults("مدير النظام"))
DECLARE @AdminFullPermissions NVARCHAR(MAX);

-- نبني الكتالوج من جدول Permissions إن وجد، وإلا من القائمة المرجعية
IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
    AND (SELECT COUNT(*) FROM dbo.Permissions) > 10
BEGIN
    SELECT @AdminFullPermissions = STRING_AGG(CONVERT(NVARCHAR(MAX), PermissionKey), N',')
    FROM dbo.Permissions
    WHERE IsActive = 1 OR IsActive IS NULL;
    PRINT N'✓ تم بناء كتالوج الصلاحيات من جدول Permissions';
END;

IF @AdminFullPermissions IS NULL OR LEN(@AdminFullPermissions) < 50
BEGIN
    -- قائمة مرجعية مناسبة للكود الحالي
    SET @AdminFullPermissions =
        N'Dashboard.View,' +
        N'Students.View,Students.Add,Students.Edit,Students.Delete,Students.Search,Students.Print,Students.ExportExcel,Students.ExportCsv,Students.ExportPDF,Students.Approve,Students.Cancel,Students.Manage,' +
        N'Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Delete,Enrollment.Search,Enrollment.Print,Enrollment.ExportExcel,Enrollment.ExportCsv,Enrollment.ExportPDF,Enrollment.Approve,Enrollment.Cancel,Enrollment.Manage,' +
        N'ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Delete,ClassAssignment.Search,ClassAssignment.Print,ClassAssignment.ExportExcel,ClassAssignment.ExportCsv,ClassAssignment.ExportPDF,ClassAssignment.Approve,ClassAssignment.Cancel,ClassAssignment.Manage,' +
        N'Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Delete,Teachers.Search,Teachers.Print,Teachers.ExportExcel,Teachers.ExportCsv,Teachers.ExportPDF,Teachers.Approve,Teachers.Cancel,Teachers.Manage,' +
        N'TeacherAttendance.View,TeacherAttendance.Add,TeacherAttendance.Edit,TeacherAttendance.Delete,TeacherAttendance.Search,TeacherAttendance.Print,TeacherAttendance.ExportExcel,TeacherAttendance.ExportCsv,TeacherAttendance.ExportPDF,TeacherAttendance.Approve,TeacherAttendance.Cancel,' +
        N'StaffAttendance.View,StaffAttendance.Add,StaffAttendance.Edit,StaffAttendance.Delete,StaffAttendance.Search,StaffAttendance.Print,StaffAttendance.ExportExcel,StaffAttendance.ExportCsv,StaffAttendance.ExportPDF,StaffAttendance.Approve,StaffAttendance.Cancel,StaffAttendance.Manage,' +
        N'TeacherContracts.View,TeacherContracts.Add,TeacherContracts.Edit,TeacherContracts.Delete,TeacherContracts.Search,TeacherContracts.Print,TeacherContracts.ExportExcel,TeacherContracts.ExportCsv,TeacherContracts.ExportPDF,TeacherContracts.Approve,TeacherContracts.Cancel,' +
        N'Payroll.View,Payroll.Add,Payroll.Edit,Payroll.Delete,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Payroll.ExportCsv,Payroll.ExportPDF,Payroll.Approve,Payroll.Cancel,Payroll.Manage,' +
        N'Subjects.View,Subjects.Add,Subjects.Edit,Subjects.Delete,Subjects.Search,Subjects.Print,Subjects.ExportExcel,Subjects.ExportCsv,Subjects.ExportPDF,Subjects.Approve,Subjects.Cancel,Subjects.Manage,' +
        N'Classes.View,Classes.Add,Classes.Edit,Classes.Delete,Classes.Search,Classes.Print,Classes.ExportExcel,Classes.ExportCsv,Classes.ExportPDF,Classes.Approve,Classes.Cancel,Classes.Manage,' +
        N'Rooms.View,Rooms.Add,Rooms.Edit,Rooms.Delete,Rooms.Search,Rooms.Print,Rooms.ExportExcel,Rooms.ExportCsv,Rooms.ExportPDF,Rooms.Approve,Rooms.Cancel,' +
        N'Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Delete,Timetable.Search,Timetable.Print,Timetable.ExportExcel,Timetable.ExportCsv,Timetable.ExportPDF,Timetable.Approve,Timetable.Cancel,Timetable.Manage,' +
        N'Grades.View,Grades.Add,Grades.Edit,Grades.Delete,Grades.Search,Grades.Print,Grades.ExportExcel,Grades.ExportCsv,Grades.ExportPDF,Grades.Approve,Grades.Cancel,Grades.Manage,' +
        N'Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Delete,Attendance.Search,Attendance.Print,Attendance.ExportExcel,Attendance.ExportCsv,Attendance.ExportPDF,Attendance.Approve,Attendance.Cancel,Attendance.Manage,' +
        N'Fees.View,Fees.Add,Fees.Edit,Fees.Delete,Fees.Search,Fees.Print,Fees.ExportExcel,Fees.ExportCsv,Fees.ExportPDF,Fees.Approve,Fees.Cancel,Fees.Manage,' +
        N'FeePlans.View,FeePlans.Add,FeePlans.Edit,FeePlans.Delete,FeePlans.Search,FeePlans.Print,FeePlans.ExportExcel,FeePlans.ExportCsv,FeePlans.ExportPDF,FeePlans.Approve,FeePlans.Cancel,' +
        N'Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Delete,Vouchers.Search,Vouchers.Print,Vouchers.ExportExcel,Vouchers.ExportCsv,Vouchers.ExportPDF,Vouchers.Approve,Vouchers.Cancel,Vouchers.Manage,' +
        N'Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Delete,Expenses.Search,Expenses.Print,Expenses.ExportExcel,Expenses.ExportCsv,Expenses.ExportPDF,Expenses.Approve,Expenses.Cancel,Expenses.Manage,' +
        N'Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,Transport.ExportExcel,Transport.ExportCsv,Transport.ExportPDF,Transport.Approve,Transport.Cancel,Transport.Manage,' +
        N'Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,Library.ExportExcel,Library.ExportCsv,Library.ExportPDF,Library.Approve,Library.Cancel,Library.Manage,' +
        N'Reports.View,Reports.Add,Reports.Edit,Reports.Delete,Reports.Search,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF,Reports.Approve,Reports.Cancel,' +
        N'AuditLogs.View,AuditLogs.Print,AuditLogs.ExportExcel,AuditLogs.ExportPDF,AuditLogs.ExportCsv,' +
        N'Settings.View,Settings.Edit,Settings.Manage,' +
        N'Users.View,Users.Add,Users.Edit,Users.Delete,Users.Manage,Users.ManageRoles,' +
        N'Roles.View,Roles.Add,Roles.Edit,Roles.Delete,Roles.Manage,' +
        N'Permissions.Manage';

    PRINT N'✓ تم استخدام القائمة المرجعية للكتالوج';
END;

-- تحديث مدير النظام دائماً بالصلاحيات الكاملة
UPDATE dbo.Users
SET RoleName = N'مدير النظام',
    Permissions = @AdminFullPermissions,
    UpdatedAt = GETDATE()
WHERE LTRIM(RTRIM(LOWER(ISNULL(RoleName, N'')))) IN (N'مدير النظام', N'admin', N'administrator');

PRINT N'✓ تم تحديث صلاحيات مدير النظام';
GO

PRINT N'========================================';
PRINT N'خطوة 5: فحص المستخدمين بصلاحيات قليلة';
PRINT N'========================================';

-- عرض المستخدمين الذين لديهم أقل من 5 صلاحيات (مشبوهون)
SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
        CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END AS عدد_الصلاحيات,
    ISNULL(Permissions, N'NULL') AS الصلاحيات_الكاملة
FROM dbo.Users
WHERE (
    LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
    CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END
) < 5
ORDER BY UserID;
GO

PRINT N'========================================';
PRINT N'خطوة 6: التحقق النهائي من الحالة';
PRINT N'========================================';

SELECT
    UserID,
    UserName,
    RoleName,
    IsActive,
    LEN(ISNULL(Permissions, N'')) AS طول_نص_الصلاحيات,
    LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) +
        CASE WHEN NULLIF(LTRIM(RTRIM(Permissions)), N'') IS NULL THEN 0 ELSE 1 END AS عدد_الصلاحيات,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(ISNULL(Permissions, N''))), N'') IS NULL THEN N'⚠ لا يوجد صلاحيات'
        WHEN (LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1) < 3
             THEN N'⚠ صلاحيات قليلة جداً'
        WHEN (LEN(ISNULL(Permissions, N'')) - LEN(REPLACE(ISNULL(Permissions, N''), N',', N'')) + 1) >= 30
             THEN N'✓ صلاحيات كاملة'
        ELSE N'● صلاحيات محدودة عادية'
    END AS تقييم_الصلاحيات
FROM dbo.Users
ORDER BY UserID;
GO

PRINT N'';
PRINT N'==========================================================';
PRINT N'تم الانتهاء من سكريبت إصلاح RBAC';
PRINT N'';
PRINT N'الخطوات التالية:';
PRINT N'1. راجع النتائج أعلاه للتحقق من صحة الصلاحيات';
PRINT N'2. أعد تشغيل التطبيق لتجديد الجلسات';
PRINT N'3. سجّل الدخول مرة أخرى لتطبيق الصلاحيات الجديدة';
PRINT N'4. إذا كان مستخدم محدد لا يزال يعاني من المشكلة،';
PRINT N'   انظر إلى قيمة Permissions في الجدول وقارنها';
PRINT N'   بالمفاتيح المتوقعة في PermissionKeys.cs';
PRINT N'==========================================================';
GO
