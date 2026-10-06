/*
    SchoolSystem - One-time production data reset
    ----------------------------------------------
    This script removes operational/test data while KEEPING reference/setup data:
      - Academic years/current academic-year history
      - Academic terms
      - Stages/classes/sections
      - Subjects and class-subject mappings
      - Rooms
      - Users, roles, permissions and their assignments
      - School settings
      - Library books are also cleared; this reset keeps only the explicitly requested reference data.

    Cleared:
      Students/profiles/enrollments/distribution/attendance/grades/marks/transport assignments
      Teachers/contracts/attendance/payroll/timetable
      Fees/fee plans/student fees/vouchers/receipts
      Expenses/borrowings/audit/closing/migration operational logs
      Buses/routes are cleared because they are operational master data that the user requested
      to re-enter dynamically from the application.

    IMPORTANT:
      Run once against SchoolDB after taking a backup.
      This script does NOT run automatically from the application.
*/

IF DB_ID(N'SchoolDB') IS NULL
    THROW 52000, N'قاعدة البيانات SchoolDB غير موجودة.', 1;
GO

USE [SchoolDB];
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* ================================================================
       1) Child/transactional data first
       ================================================================ */
    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL DELETE FROM dbo.StudentAttendance;
    IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL DELETE FROM dbo.TeacherAttendance;
    IF OBJECT_ID(N'dbo.StaffAttendance', N'U') IS NOT NULL DELETE FROM dbo.StaffAttendance;

    IF OBJECT_ID(N'dbo.Grades', N'U') IS NOT NULL DELETE FROM dbo.Grades;
    IF OBJECT_ID(N'dbo.Marks', N'U') IS NOT NULL DELETE FROM dbo.Marks;

    IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL DELETE FROM dbo.SchoolTimetable;
    IF OBJECT_ID(N'dbo.Timetables', N'U') IS NOT NULL DELETE FROM dbo.Timetables;

    IF OBJECT_ID(N'dbo.VoucherLines', N'U') IS NOT NULL DELETE FROM dbo.VoucherLines;
    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL DELETE FROM dbo.Vouchers;
    IF OBJECT_ID(N'dbo.Receipts', N'U') IS NOT NULL DELETE FROM dbo.Receipts;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL DELETE FROM dbo.Fees;
    IF OBJECT_ID(N'dbo.StudentFees', N'U') IS NOT NULL DELETE FROM dbo.StudentFees;
    IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NOT NULL DELETE FROM dbo.FeePlans;

    IF OBJECT_ID(N'dbo.StudentTransport', N'U') IS NOT NULL DELETE FROM dbo.StudentTransport;
    IF OBJECT_ID(N'dbo.StudentTransportAssignments', N'U') IS NOT NULL DELETE FROM dbo.StudentTransportAssignments;

    IF OBJECT_ID(N'dbo.BookBorrowings', N'U') IS NOT NULL DELETE FROM dbo.BookBorrowings;
    IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL DELETE FROM dbo.Books;

    IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NOT NULL DELETE FROM dbo.TeacherContracts;
    IF OBJECT_ID(N'dbo.Payroll', N'U') IS NOT NULL DELETE FROM dbo.Payroll;

    IF OBJECT_ID(N'dbo.Expenses', N'U') IS NOT NULL DELETE FROM dbo.Expenses;

    IF OBJECT_ID(N'dbo.AnnualMigrationLog', N'U') IS NOT NULL DELETE FROM dbo.AnnualMigrationLog;
    IF OBJECT_ID(N'dbo.AnnualClosings', N'U') IS NOT NULL DELETE FROM dbo.AnnualClosings;
    IF OBJECT_ID(N'dbo.DatabaseBackupHistory', N'U') IS NOT NULL DELETE FROM dbo.DatabaseBackupHistory;
    IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NOT NULL DELETE FROM dbo.AuditLogs;

    /* ================================================================
       2) Operational people and their dependent records
       ================================================================ */
    IF OBJECT_ID(N'dbo.StudentProfiles', N'U') IS NOT NULL DELETE FROM dbo.StudentProfiles;
    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL DELETE FROM dbo.Enrollments;
    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL DELETE FROM dbo.StudentClasses;
    IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL DELETE FROM dbo.Students;

    IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL DELETE FROM dbo.Teachers;

    /* ================================================================
       3) Operational transport
       ================================================================ */
    IF OBJECT_ID(N'dbo.BusRoutes', N'U') IS NOT NULL DELETE FROM dbo.BusRoutes;
    IF OBJECT_ID(N'dbo.Buses', N'U') IS NOT NULL DELETE FROM dbo.Buses;

    /* ================================================================
       4) Reset identity counters for cleared operational tables only
       ================================================================ */
    DECLARE @TableName SYSNAME;

    DECLARE identity_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT v.TableName
    FROM (VALUES
        (N'Students'),
        (N'StudentProfiles'),
        (N'Enrollments'),
        (N'StudentClasses'),
        (N'StudentAttendance'),
        (N'Grades'),
        (N'Marks'),
        (N'Teachers'),
        (N'TeacherAttendance'),
        (N'TeacherContracts'),
        (N'Payroll'),
        (N'SchoolTimetable'),
        (N'Fees'),
        (N'StudentFees'),
        (N'FeePlans'),
        (N'Vouchers'),
        (N'Receipts'),
        (N'Expenses'),
        (N'BookBorrowings'),
        (N'Books'),
        (N'Buses'),
        (N'BusRoutes'),
        (N'StudentTransport'),
        (N'StudentTransportAssignments')
    ) v(TableName);

    OPEN identity_cursor;
    FETCH NEXT FROM identity_cursor INTO @TableName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF OBJECT_ID(N'dbo.' + @TableName, N'U') IS NOT NULL
        BEGIN
            IF EXISTS
            (
                SELECT 1
                FROM sys.identity_columns
                WHERE object_id = OBJECT_ID(N'dbo.' + @TableName)
            )
            BEGIN
                EXEC(
                    N'DBCC CHECKIDENT (''' +
                    REPLACE(N'dbo.' + @TableName, '''', '''''') +
                    N''', RESEED, 0) WITH NO_INFOMSGS;'
                );
            END
        END

        FETCH NEXT FROM identity_cursor INTO @TableName;
    END

    CLOSE identity_cursor;
    DEALLOCATE identity_cursor;

    /* ================================================================
       5) Verification:
          Operational records must be zero.
          Reference records must still exist.
       ================================================================ */
    DECLARE @RemainingOperational BIGINT = 0;

    SELECT @RemainingOperational =
          ISNULL((SELECT COUNT_BIG(*) FROM dbo.Students), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Teachers), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Enrollments), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.StudentClasses), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Fees), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.FeePlans), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Vouchers), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Receipts), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.Payroll), 0)
        + ISNULL((SELECT COUNT_BIG(*) FROM dbo.TeacherContracts), 0);

    IF @RemainingOperational <> 0
        THROW 52001, N'فشل تنظيف البيانات التشغيلية بالكامل.', 1;

    IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Subjects)
        THROW 52002, N'لم تبقَ المواد الدراسية المرجعية كما هو متوقع.', 1;

    IF OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Rooms)
        THROW 52003, N'لم تبقَ القاعات المرجعية كما هو متوقع.', 1;

    IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Users)
        THROW 52004, N'لم يبقَ مستخدمون. تم إيقاف العملية لحماية الدخول والصلاحيات.', 1;

    IF OBJECT_ID(N'dbo.Permissions', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.Permissions)
        THROW 52005, N'لم تبقَ الصلاحيات. تم إيقاف العملية لحماية نظام الوصول.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF CURSOR_STATUS('local', 'identity_cursor') >= 0
    BEGIN
        CLOSE identity_cursor;
        DEALLOCATE identity_cursor;
    END

    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

/* تقرير نهائي للمستخدم */
SELECT 'Students' AS TableName, COUNT_BIG(*) AS RemainingRows FROM dbo.Students
UNION ALL SELECT 'StudentProfiles', COUNT_BIG(*) FROM dbo.StudentProfiles
UNION ALL SELECT 'Enrollments', COUNT_BIG(*) FROM dbo.Enrollments
UNION ALL SELECT 'StudentClasses', COUNT_BIG(*) FROM dbo.StudentClasses
UNION ALL SELECT 'Teachers', COUNT_BIG(*) FROM dbo.Teachers
UNION ALL SELECT 'TeacherContracts', COUNT_BIG(*) FROM dbo.TeacherContracts
UNION ALL SELECT 'Payroll', COUNT_BIG(*) FROM dbo.Payroll
UNION ALL SELECT 'Fees', COUNT_BIG(*) FROM dbo.Fees
UNION ALL SELECT 'FeePlans', COUNT_BIG(*) FROM dbo.FeePlans
UNION ALL SELECT 'Vouchers', COUNT_BIG(*) FROM dbo.Vouchers
UNION ALL SELECT 'Receipts', COUNT_BIG(*) FROM dbo.Receipts
UNION ALL SELECT 'Subjects', COUNT_BIG(*) FROM dbo.Subjects
UNION ALL SELECT 'Rooms', COUNT_BIG(*) FROM dbo.Rooms
UNION ALL SELECT 'AcademicYears', COUNT_BIG(*) FROM dbo.AcademicYears
UNION ALL SELECT 'AcademicTerms', COUNT_BIG(*) FROM dbo.AcademicTerms
UNION ALL SELECT 'Users', COUNT_BIG(*) FROM dbo.Users
UNION ALL SELECT 'Roles', COUNT_BIG(*) FROM dbo.Roles
UNION ALL SELECT 'Permissions', COUNT_BIG(*) FROM dbo.Permissions
UNION ALL SELECT 'RolePermissions', COUNT_BIG(*) FROM dbo.RolePermissions
UNION ALL SELECT 'UserRoles', COUNT_BIG(*) FROM dbo.UserRoles
UNION ALL SELECT 'UserPermissions', COUNT_BIG(*) FROM dbo.UserPermissions
ORDER BY TableName;
GO
