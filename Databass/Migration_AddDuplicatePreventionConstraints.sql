/*
    Migration: Duplicate prevention constraints
    Purpose: Prevent duplicate business records at the database level.
    Run against SchoolDB, not master.

    The migration never deletes data automatically. If legacy duplicates exist,
    it stops and reports the affected business key so the data can be reviewed
    before the unique index is created.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

/* Users: login names must be unique, case-insensitively under the database collation. */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Users', N'UserName') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT UserName
        FROM dbo.Users
        WHERE NULLIF(LTRIM(RTRIM(UserName)), N'') IS NOT NULL
        GROUP BY UserName
        HAVING COUNT(*) > 1
    )
        THROW 51101, N'لا يمكن إنشاء قيد المستخدمين: توجد أسماء مستخدمين مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = N'UX_Users_UserName')
        CREATE UNIQUE INDEX UX_Users_UserName ON dbo.Users(UserName)
        WHERE UserName IS NOT NULL AND LTRIM(RTRIM(UserName)) <> N'';
END;

/* Students: generated student number and supplied national ID are unique. */
IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT StudentNumber
        FROM dbo.Students
        WHERE NULLIF(LTRIM(RTRIM(StudentNumber)), N'') IS NOT NULL
        GROUP BY StudentNumber
        HAVING COUNT(*) > 1
    )
        THROW 51102, N'لا يمكن إنشاء قيد الطلاب: توجد أرقام طلاب مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Students') AND name = N'UX_Students_StudentNumber')
        CREATE UNIQUE INDEX UX_Students_StudentNumber ON dbo.Students(StudentNumber)
        WHERE StudentNumber IS NOT NULL AND LTRIM(RTRIM(StudentNumber)) <> N'';

    IF COL_LENGTH(N'dbo.Students', N'NationalId') IS NOT NULL
    BEGIN
        IF EXISTS
        (
            SELECT NationalId
            FROM dbo.Students
            WHERE NULLIF(LTRIM(RTRIM(NationalId)), N'') IS NOT NULL
            GROUP BY NationalId
            HAVING COUNT(*) > 1
        )
            THROW 51103, N'لا يمكن إنشاء قيد الطلاب: توجد أرقام هوية وطنية مكررة.', 1;

        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Students') AND name = N'UX_Students_NationalId')
            CREATE UNIQUE INDEX UX_Students_NationalId ON dbo.Students(NationalId)
            WHERE NationalId IS NOT NULL AND LTRIM(RTRIM(NationalId)) <> N'';
    END;
END;

/* Teachers: employee number and national ID are unique. */
IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT EmployeeNumber
        FROM dbo.Teachers
        WHERE NULLIF(LTRIM(RTRIM(EmployeeNumber)), N'') IS NOT NULL
        GROUP BY EmployeeNumber
        HAVING COUNT(*) > 1
    )
        THROW 51104, N'لا يمكن إنشاء قيد المعلمين: توجد أرقام موظفين مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Teachers') AND name = N'UX_Teachers_EmployeeNumber')
        CREATE UNIQUE INDEX UX_Teachers_EmployeeNumber ON dbo.Teachers(EmployeeNumber)
        WHERE EmployeeNumber IS NOT NULL AND LTRIM(RTRIM(EmployeeNumber)) <> N'';

    IF COL_LENGTH(N'dbo.Teachers', N'NationalID') IS NOT NULL
    BEGIN
        IF EXISTS
        (
            SELECT NationalID
            FROM dbo.Teachers
            WHERE NULLIF(LTRIM(RTRIM(NationalID)), N'') IS NOT NULL
            GROUP BY NationalID
            HAVING COUNT(*) > 1
        )
            THROW 51105, N'لا يمكن إنشاء قيد المعلمين: توجد أرقام هوية وطنية مكررة.', 1;

        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Teachers') AND name = N'UX_Teachers_NationalID')
            CREATE UNIQUE INDEX UX_Teachers_NationalID ON dbo.Teachers(NationalID)
            WHERE NationalID IS NOT NULL AND LTRIM(RTRIM(NationalID)) <> N'';
    END;
END;

/* Rooms: generated room codes must not collide. */
IF OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Rooms', N'RoomCode') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT RoomCode
        FROM dbo.Rooms
        WHERE NULLIF(LTRIM(RTRIM(RoomCode)), N'') IS NOT NULL
        GROUP BY RoomCode
        HAVING COUNT(*) > 1
    )
        THROW 51106, N'لا يمكن إنشاء قيد القاعات: توجد أكواد قاعات مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Rooms') AND name = N'UX_Rooms_RoomCode')
        CREATE UNIQUE INDEX UX_Rooms_RoomCode ON dbo.Rooms(RoomCode)
        WHERE RoomCode IS NOT NULL AND LTRIM(RTRIM(RoomCode)) <> N'';
END;

/* Vouchers: every generated or manually entered voucher number is unique. */
IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Vouchers', N'VoucherNumber') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT VoucherNumber
        FROM dbo.Vouchers
        WHERE NULLIF(LTRIM(RTRIM(VoucherNumber)), N'') IS NOT NULL
        GROUP BY VoucherNumber
        HAVING COUNT(*) > 1
    )
        THROW 51107, N'لا يمكن إنشاء قيد السندات: توجد أرقام سندات مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Vouchers') AND name = N'UX_Vouchers_VoucherNumber')
        CREATE UNIQUE INDEX UX_Vouchers_VoucherNumber ON dbo.Vouchers(VoucherNumber)
        WHERE VoucherNumber IS NOT NULL AND LTRIM(RTRIM(VoucherNumber)) <> N'';
END;

/* One enrollment/assignment per student in the same normalized academic year. */
IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.StudentClasses', N'StudentID') IS NOT NULL
   AND COL_LENGTH(N'dbo.StudentClasses', N'AcademicYear') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.StudentClasses', N'AcademicYearKey') IS NULL
        ALTER TABLE dbo.StudentClasses ADD AcademicYearKey AS
            (REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')) PERSISTED;

    IF EXISTS
    (
        SELECT StudentID, AcademicYearKey
        FROM dbo.StudentClasses
        WHERE AcademicYearKey <> N''
        GROUP BY StudentID, AcademicYearKey
        HAVING COUNT(*) > 1
    )
        THROW 51108, N'لا يمكن إنشاء قيد التوزيع: يوجد أكثر من توزيع للطالب في العام الدراسي نفسه.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentClasses') AND name = N'UX_StudentClasses_StudentYear')
        CREATE UNIQUE INDEX UX_StudentClasses_StudentYear ON dbo.StudentClasses(StudentID, AcademicYearKey);
END;

/* One attendance record per student and calendar date, regardless of time. */
IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.StudentAttendance', N'StudentID') IS NOT NULL
   AND COL_LENGTH(N'dbo.StudentAttendance', N'AttendanceDate') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.StudentAttendance', N'AttendanceDay') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD AttendanceDay AS (CONVERT(date, AttendanceDate)) PERSISTED;

    IF EXISTS
    (
        SELECT StudentID, AttendanceDay
        FROM dbo.StudentAttendance
        GROUP BY StudentID, AttendanceDay
        HAVING COUNT(*) > 1
    )
        THROW 51109, N'لا يمكن إنشاء قيد حضور الطلاب: يوجد أكثر من سجل حضور للطالب في اليوم نفسه.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.StudentAttendance') AND name = N'UX_StudentAttendance_StudentDate')
        CREATE UNIQUE INDEX UX_StudentAttendance_StudentDate ON dbo.StudentAttendance(StudentID, AttendanceDay);
END;

/* One attendance record per teacher and calendar date, regardless of time. */
IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.TeacherAttendance', N'TeacherID') IS NOT NULL
   AND COL_LENGTH(N'dbo.TeacherAttendance', N'AttendanceDate') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.TeacherAttendance', N'AttendanceDay') IS NULL
        ALTER TABLE dbo.TeacherAttendance ADD AttendanceDay AS (CONVERT(date, AttendanceDate)) PERSISTED;

    IF EXISTS
    (
        SELECT TeacherID, AttendanceDay
        FROM dbo.TeacherAttendance
        GROUP BY TeacherID, AttendanceDay
        HAVING COUNT(*) > 1
    )
        THROW 51110, N'لا يمكن إنشاء قيد حضور الموظفين: يوجد أكثر من سجل حضور للمعلم في اليوم نفسه.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.TeacherAttendance') AND name = N'UX_TeacherAttendance_TeacherDate')
        CREATE UNIQUE INDEX UX_TeacherAttendance_TeacherDate ON dbo.TeacherAttendance(TeacherID, AttendanceDay);
END;

/* Contract numbers are business identifiers and must be unique. */
IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.TeacherContracts', N'ContractNumber') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT ContractNumber
        FROM dbo.TeacherContracts
        WHERE NULLIF(LTRIM(RTRIM(ContractNumber)), N'') IS NOT NULL
        GROUP BY ContractNumber
        HAVING COUNT(*) > 1
    )
        THROW 51111, N'لا يمكن إنشاء قيد العقود: توجد أرقام عقود مكررة.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.TeacherContracts') AND name = N'UX_TeacherContracts_ContractNumber')
        CREATE UNIQUE INDEX UX_TeacherContracts_ContractNumber ON dbo.TeacherContracts(ContractNumber)
        WHERE ContractNumber IS NOT NULL AND LTRIM(RTRIM(ContractNumber)) <> N'';
END;

COMMIT TRANSACTION;
PRINT N'تم إنشاء قيود منع التكرار بنجاح.';
