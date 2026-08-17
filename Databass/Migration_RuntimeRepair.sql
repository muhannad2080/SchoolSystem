/*
  SchoolSystem runtime repair for an existing SQL Server database.
  Run this file in SSMS against the same SQL Server instance used by App.config.
  Safe to run repeatedly: it only creates missing objects or adds missing columns.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 51000, N'قاعدة SchoolDB غير موجودة على هذا الخادم. تحقق من اسم Server في App.config.', 1;
GO
USE [SchoolDB];
GO

PRINT N'الخادم المتصل: ' + @@SERVERNAME;
PRINT N'قاعدة البيانات الحالية: ' + DB_NAME();

/* Existing minimal schema compatibility. */
IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Subjects', N'SubjectCode') IS NULL ALTER TABLE dbo.Subjects ADD SubjectCode NVARCHAR(30) NULL;
    IF COL_LENGTH(N'dbo.Subjects', N'ClassID') IS NULL ALTER TABLE dbo.Subjects ADD ClassID INT NULL;
    IF COL_LENGTH(N'dbo.Subjects', N'MaxDegree') IS NULL ALTER TABLE dbo.Subjects ADD MaxDegree DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_Runtime_MaxDegree DEFAULT 100 WITH VALUES;
    IF COL_LENGTH(N'dbo.Subjects', N'PassDegree') IS NULL ALTER TABLE dbo.Subjects ADD PassDegree DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_Runtime_PassDegree DEFAULT 50 WITH VALUES;
    IF COL_LENGTH(N'dbo.Subjects', N'IsActive') IS NULL ALTER TABLE dbo.Subjects ADD IsActive BIT NOT NULL CONSTRAINT DF_Subjects_Runtime_IsActive DEFAULT 1 WITH VALUES;
    IF COL_LENGTH(N'dbo.Subjects', N'Notes') IS NULL ALTER TABLE dbo.Subjects ADD Notes NVARCHAR(MAX) NULL;
    IF COL_LENGTH(N'dbo.Subjects', N'CreatedAt') IS NULL ALTER TABLE dbo.Subjects ADD CreatedAt DATETIME NOT NULL CONSTRAINT DF_Subjects_Runtime_CreatedAt DEFAULT GETDATE() WITH VALUES;
    IF COL_LENGTH(N'dbo.Subjects', N'UpdatedAt') IS NULL ALTER TABLE dbo.Subjects ADD UpdatedAt DATETIME NULL;
END;

IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Students', N'ClassID') IS NULL ALTER TABLE dbo.Students ADD ClassID INT NULL;
    IF COL_LENGTH(N'dbo.Students', N'Section') IS NULL ALTER TABLE dbo.Students ADD Section NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Students', N'AcademicYear') IS NULL ALTER TABLE dbo.Students ADD AcademicYear NVARCHAR(20) NULL;
    IF COL_LENGTH(N'dbo.Students', N'Phone') IS NULL ALTER TABLE dbo.Students ADD Phone NVARCHAR(30) NULL;
    /* Dynamic SQL is required because SQL Server compiles a batch before ALTER TABLE runs. */
    IF COL_LENGTH(N'dbo.Students', N'StudentPhone') IS NOT NULL
        EXEC(N'UPDATE dbo.Students SET Phone = StudentPhone WHERE Phone IS NULL AND StudentPhone IS NOT NULL;');
END;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.StudentClasses', N'Section') IS NULL ALTER TABLE dbo.StudentClasses ADD Section NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedDate') IS NULL ALTER TABLE dbo.StudentClasses ADD AssignedDate DATETIME NOT NULL CONSTRAINT DF_StudentClasses_Runtime_AssignedDate DEFAULT GETDATE() WITH VALUES;
    IF COL_LENGTH(N'dbo.StudentClasses', N'AssignedBy') IS NULL ALTER TABLE dbo.StudentClasses ADD AssignedBy INT NULL;
END;

/* Grades compatibility for installations created before component columns existed. */
IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grades
    (
        GradeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades_Runtime PRIMARY KEY,
        StudentID INT NULL,
        SubjectID INT NULL,
        ClassID INT NULL,
        Section NVARCHAR(100) NULL,
        AcademicYear NVARCHAR(20) NULL,
        TermName NVARCHAR(50) NULL,
        Quiz1 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_Quiz1 DEFAULT 0,
        Quiz2 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_Quiz2 DEFAULT 0,
        CourseWork DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_CourseWork DEFAULT 0,
        FinalExam DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_FinalExam DEFAULT 0,
        GradeValue DECIMAL(10,2) NULL,
        GradeLetter NVARCHAR(50) NULL,
        ResultStatus NVARCHAR(50) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Grades_Runtime_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.Grades', N'Section') IS NULL ALTER TABLE dbo.Grades ADD Section NVARCHAR(100) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'Quiz1') IS NULL ALTER TABLE dbo.Grades ADD Quiz1 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_Quiz1 DEFAULT 0 WITH VALUES;
    IF COL_LENGTH(N'dbo.Grades', N'Quiz2') IS NULL ALTER TABLE dbo.Grades ADD Quiz2 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_Quiz2 DEFAULT 0 WITH VALUES;
    IF COL_LENGTH(N'dbo.Grades', N'CourseWork') IS NULL ALTER TABLE dbo.Grades ADD CourseWork DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_CourseWork DEFAULT 0 WITH VALUES;
    IF COL_LENGTH(N'dbo.Grades', N'FinalExam') IS NULL ALTER TABLE dbo.Grades ADD FinalExam DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Runtime_FinalExam DEFAULT 0 WITH VALUES;
    IF COL_LENGTH(N'dbo.Grades', N'GradeLetter') IS NULL ALTER TABLE dbo.Grades ADD GradeLetter NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'ResultStatus') IS NULL ALTER TABLE dbo.Grades ADD ResultStatus NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'Notes') IS NULL ALTER TABLE dbo.Grades ADD Notes NVARCHAR(MAX) NULL;
    IF COL_LENGTH(N'dbo.Grades', N'UpdatedAt') IS NULL ALTER TABLE dbo.Grades ADD UpdatedAt DATETIME NULL;
END;

/* Student attendance compatibility for older databases. */
IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.StudentAttendance', N'DepartureTime') IS NULL ALTER TABLE dbo.StudentAttendance ADD DepartureTime TIME NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'AbsenceReason') IS NULL ALTER TABLE dbo.StudentAttendance ADD AbsenceReason NVARCHAR(500) NULL;
END;

/* Timetable table already exists in the reported installation, but lacks time columns. */
IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.SchoolTimetable', N'StartTime') IS NULL ALTER TABLE dbo.SchoolTimetable ADD StartTime TIME NULL;
    IF COL_LENGTH(N'dbo.SchoolTimetable', N'EndTime') IS NULL ALTER TABLE dbo.SchoolTimetable ADD EndTime TIME NULL;
    IF COL_LENGTH(N'dbo.SchoolTimetable', N'CreatedAt') IS NULL ALTER TABLE dbo.SchoolTimetable ADD CreatedAt DATETIME NOT NULL CONSTRAINT DF_SchoolTimetable_Runtime_CreatedAt DEFAULT GETDATE() WITH VALUES;
    IF COL_LENGTH(N'dbo.SchoolTimetable', N'UpdatedAt') IS NULL ALTER TABLE dbo.SchoolTimetable ADD UpdatedAt DATETIME NULL;

    /* Repair legacy timetable rows created before time columns existed. */
    UPDATE dbo.SchoolTimetable
       SET StartTime = DATEADD(MINUTE, (ISNULL(PeriodNo, 1) - 1) * 45, CAST('08:00' AS TIME)),
           EndTime = DATEADD(MINUTE, ISNULL(PeriodNo, 1) * 45, CAST('08:00' AS TIME))
     WHERE StartTime IS NULL OR EndTime IS NULL;

    UPDATE dbo.SchoolTimetable
       SET DayName = N'الاثنين'
     WHERE DayName = N'الإثنين';
END
ELSE
BEGIN
    THROW 51001, N'جدول SchoolTimetable غير موجود. شغّل Migration_MissingApplicationTables.sql أولاً.', 1;
END;

IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.TeacherAttendance', N'CheckInTime') IS NULL ALTER TABLE dbo.TeacherAttendance ADD CheckInTime TIME NULL;
    IF COL_LENGTH(N'dbo.TeacherAttendance', N'CheckOutTime') IS NULL ALTER TABLE dbo.TeacherAttendance ADD CheckOutTime TIME NULL;
END
ELSE
BEGIN
    THROW 51002, N'جدول TeacherAttendance غير موجود. شغّل Migration_MissingApplicationTables.sql أولاً.', 1;
END;

IF OBJECT_ID(N'dbo.BookBorrowings', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.BookBorrowings', N'CreatedAt') IS NULL ALTER TABLE dbo.BookBorrowings ADD CreatedAt DATETIME NOT NULL CONSTRAINT DF_BookBorrowings_Runtime_CreatedAt DEFAULT GETDATE() WITH VALUES;
    IF COL_LENGTH(N'dbo.BookBorrowings', N'UpdatedAt') IS NULL ALTER TABLE dbo.BookBorrowings ADD UpdatedAt DATETIME NULL;
END;

/* Enrollments is absent in the reported installation. */
IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Enrollments
    (
        EnrollmentID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Enrollments_Runtime PRIMARY KEY,
        StudentID INT NOT NULL,
        ApplicationDate DATE NOT NULL CONSTRAINT DF_Enrollments_Runtime_ApplicationDate DEFAULT CONVERT(date, GETDATE()),
        ApplicationType NVARCHAR(50) NULL,
        AcademicYear NVARCHAR(20) NOT NULL CONSTRAINT DF_Enrollments_Runtime_AcademicYear DEFAULT N'',
        ClassID INT NULL,
        Section NVARCHAR(50) NULL,
        SeatNumber NVARCHAR(20) NULL,
        Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Enrollments_Runtime_Status DEFAULT N'جديد',
        PreviousSchool NVARCHAR(200) NULL,
        PreviousClass NVARCHAR(50) NULL,
        TransferReason NVARCHAR(MAX) NULL,
        RegistrationFee DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_Runtime_RegistrationFee DEFAULT 0,
        PaidAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_Runtime_PaidAmount DEFAULT 0,
        PaymentMethod NVARCHAR(50) NULL,
        ReceiptNo NVARCHAR(50) NULL,
        HasBirthCertificate BIT NOT NULL CONSTRAINT DF_Enrollments_Runtime_HasBirthCertificate DEFAULT 0,
        HasGuardianId BIT NOT NULL CONSTRAINT DF_Enrollments_Runtime_HasGuardianId DEFAULT 0,
        HasPhoto BIT NOT NULL CONSTRAINT DF_Enrollments_Runtime_HasPhoto DEFAULT 0,
        HasLastCertificate BIT NOT NULL CONSTRAINT DF_Enrollments_Runtime_HasLastCertificate DEFAULT 0,
        HasMedicalReport BIT NOT NULL CONSTRAINT DF_Enrollments_Runtime_HasMedicalReport DEFAULT 0,
        GeneralNotes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Enrollments_Runtime_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;

/* Fees query uses Students.ClassID; no ClassID is stored in Fees itself. */
IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Fees', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Fees ADD UpdatedAt DATETIME NULL;

/* Validation report: zero rows means all reported runtime fields exist. */
SELECT DB_NAME() AS DatabaseName, @@SERVERNAME AS ServerName;
SELECT v.ObjectName, v.ColumnName
FROM (VALUES
    (N'SchoolTimetable', N'StartTime'), (N'SchoolTimetable', N'EndTime'), (N'SchoolTimetable', N'CreatedAt'), (N'SchoolTimetable', N'UpdatedAt'),
    (N'Enrollments', N'EnrollmentID'), (N'TeacherAttendance', N'CheckInTime'),
    (N'TeacherAttendance', N'CheckOutTime'), (N'Subjects', N'ClassID'),
    (N'Subjects', N'SubjectCode'), (N'Subjects', N'MaxDegree'),
    (N'Subjects', N'PassDegree'), (N'Subjects', N'IsActive'),
    (N'Subjects', N'Notes'), (N'Subjects', N'CreatedAt'),
    (N'Subjects', N'UpdatedAt'), (N'BookBorrowings', N'CreatedAt'),
    (N'BookBorrowings', N'UpdatedAt'),     (N'Students', N'ClassID'),
    (N'Grades', N'Quiz1'), (N'Grades', N'Quiz2'), (N'Grades', N'CourseWork'),
    (N'Grades', N'FinalExam'), (N'Grades', N'GradeLetter'), (N'Grades', N'ResultStatus'),
    (N'Grades', N'Notes'), (N'StudentAttendance', N'DepartureTime'),
    (N'StudentAttendance', N'AbsenceReason')
) v(ObjectName, ColumnName)
WHERE OBJECT_ID(N'dbo.' + v.ObjectName, N'U') IS NULL
   OR COL_LENGTH(N'dbo.' + v.ObjectName, v.ColumnName) IS NULL;

PRINT N'اكتمل إصلاح مخطط التشغيل. أعد تشغيل التطبيق بعد إغلاقه بالكامل.';
GO
