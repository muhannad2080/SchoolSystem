/*
    SchoolSystem - Repair legacy report-only permissions
    يعالج الحسابات القديمة التي بقيت على Dashboard.View وReports.View فقط
    رغم أن دورها الحالي يملك صلاحيات أوسع.

    السكربت لا يغيّر الحسابات التي اختار مدير النظام منع صلاحياتها صراحةً،
    ولا يغيّر دور التقارير أو الحسابات ذات الصلاحيات المخصصة المختلفة.
*/

IF DB_ID(N'SchoolDB') IS NULL
    THROW 51000, N'قاعدة البيانات SchoolDB غير موجودة.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51001, N'جدول dbo.Users غير موجود.', 1;
GO

IF COL_LENGTH(N'dbo.Users', N'Permissions') IS NULL
    THROW 51002, N'عمود dbo.Users.Permissions غير موجود.', 1;
GO

IF EXISTS
(
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Users')
      AND name = N'Permissions'
      AND (system_type_id <> 231 OR max_length <> -1)
)
BEGIN
    ALTER TABLE dbo.Users ALTER COLUMN Permissions NVARCHAR(MAX) NULL;
END;
GO

DECLARE @ReportsOnly NVARCHAR(200) = N'Dashboard.View,Reports.View';
-- الوضع الآمن الافتراضي: لا نعدّل الحسابات تلقائيًا.
-- غيّر القيمة إلى 1 فقط بعد مراجعة النتائج والتأكد أن الحسابات قديمة فعلًا.
DECLARE @ApplyRepair BIT = 0;

IF @ApplyRepair = 1
BEGIN
;WITH LegacyUsers AS
(
    SELECT
        U.UserID,
        U.RoleName,
        NormalizedPermissions = REPLACE(REPLACE(LTRIM(RTRIM(ISNULL(U.Permissions, N''))), N' ', N''), N';', N',')
    FROM dbo.Users U
    WHERE ISNULL(U.Permissions, N'') <> N''
      AND LTRIM(RTRIM(U.RoleName)) NOT IN (N'التقارير', N'مدير النظام', N'Admin', N'Administrator')
)
UPDATE U
SET
    Permissions = CASE L.RoleName
        WHEN N'الإدارة' THEN N'Dashboard.View,Students.View,Students.Add,Students.Edit,Students.Search,Students.Print,Students.ExportExcel,Students.ExportPDF,Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Search,Enrollment.Print,ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Search,Teachers.View,Teachers.Add,Teachers.Edit,Teachers.Search,Subjects.View,Subjects.Add,Subjects.Edit,Classes.View,Classes.Add,Classes.Edit,Rooms.View,Rooms.Add,Rooms.Edit,Timetable.View,Timetable.Add,Timetable.Edit,Timetable.Print,Attendance.View,Attendance.Add,Attendance.Edit,Attendance.Print,Grades.View,Grades.Add,Grades.Edit,Grades.Approve,Grades.Print,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF'
        WHEN N'شؤون الطلاب' THEN N'Dashboard.View,Students.View,Students.Add,Students.Edit,Students.Search,Students.Print,Enrollment.View,Enrollment.Add,Enrollment.Edit,Enrollment.Search,ClassAssignment.View,ClassAssignment.Add,ClassAssignment.Edit,ClassAssignment.Search,Attendance.View,Attendance.Add,Attendance.Edit,Grades.View,Grades.Print,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv'
        WHEN N'المعلمون' THEN N'Dashboard.View,Students.View,Students.Search,Attendance.View,Attendance.Add,Attendance.Edit,Grades.View,Grades.Add,Grades.Edit,Timetable.View,Reports.View'
        WHEN N'المالية' THEN N'Dashboard.View,Fees.View,Fees.Add,Fees.Edit,Fees.Search,Fees.Print,Fees.ExportExcel,FeePlans.View,FeePlans.Add,FeePlans.Edit,Vouchers.View,Vouchers.Add,Vouchers.Edit,Vouchers.Print,Vouchers.ExportExcel,Expenses.View,Expenses.Add,Expenses.Edit,Expenses.Print,Expenses.ExportExcel,Payroll.View,Payroll.Search,Payroll.Print,Payroll.ExportExcel,Reports.View,Reports.Print,Reports.ExportExcel,Reports.ExportCsv,Reports.ExportPDF'
        WHEN N'المكتبة' THEN N'Dashboard.View,Library.View,Library.Add,Library.Edit,Library.Delete,Library.Search,Library.Print,Reports.View'
        WHEN N'النقل' THEN N'Dashboard.View,Transport.View,Transport.Add,Transport.Edit,Transport.Delete,Transport.Search,Transport.Print,Reports.View'
        ELSE U.Permissions
    END,
    UpdatedAt = GETDATE()
FROM dbo.Users U
INNER JOIN LegacyUsers L ON L.UserID = U.UserID
WHERE L.NormalizedPermissions = REPLACE(@ReportsOnly, N' ', N'');
END;

SELECT UserID, UserName, RoleName, Permissions
FROM dbo.Users
WHERE REPLACE(REPLACE(LTRIM(RTRIM(ISNULL(Permissions, N''))), N' ', N''), N';', N',') = REPLACE(@ReportsOnly, N' ', N',');

IF @ApplyRepair = 1
    PRINT N'تم إصلاح الحسابات القديمة المحددة فقط بعد تفعيل ApplyRepair.';
ELSE
    PRINT N'وضع التشخيص فقط: لم يتم تعديل أي صلاحيات. راجع النتائج ثم فعّل ApplyRepair عند الحاجة.';
GO
