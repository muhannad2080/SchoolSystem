/*
    SchoolSystem - Missing application tables hardening
    Run this file in SQL Server on an existing SchoolDB database.
    It is safe to run repeatedly and does not recreate existing tables.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 50002, N'قاعدة SchoolDB غير موجودة. أنشئها أولاً عبر SchoolDB.SQL.', 1;
GO
USE SchoolDB;
GO

/* Library */
IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books
    (
        BookID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Books PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Author NVARCHAR(200) NULL,
        ISBN NVARCHAR(50) NULL,
        Category NVARCHAR(100) NULL,
        Publisher NVARCHAR(200) NULL,
        PublicationYear INT NULL,
        Copies INT NOT NULL CONSTRAINT DF_Books_Copies DEFAULT 0,
        AvailableCopies INT NOT NULL CONSTRAINT DF_Books_AvailableCopies DEFAULT 0,
        ShelfLocation NVARCHAR(100) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Books_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;
IF OBJECT_ID(N'dbo.BookBorrowings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookBorrowings
    (
        BorrowingID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BookBorrowings PRIMARY KEY,
        BookID INT NOT NULL,
        BorrowerType NVARCHAR(20) NOT NULL,
        BorrowerID INT NOT NULL,
        BorrowDate DATE NOT NULL,
        DueDate DATE NOT NULL,
        ReturnDate DATE NULL,
        Status NVARCHAR(30) NOT NULL CONSTRAINT DF_BookBorrowings_Status DEFAULT N'معار',
        Notes NVARCHAR(MAX) NULL,
        CONSTRAINT FK_BookBorrowings_Books FOREIGN KEY (BookID) REFERENCES dbo.Books(BookID)
    );
END;

/* Rooms and timetable */
IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Rooms
    (
        RoomID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY,
        RoomCode NVARCHAR(30) NULL,
        RoomName NVARCHAR(100) NOT NULL,
        RoomType NVARCHAR(50) NULL,
        Capacity INT NOT NULL CONSTRAINT DF_Rooms_Capacity DEFAULT 0,
        Location NVARCHAR(200) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT 1,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Rooms_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;
IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolTimetable
    (
        TimetableID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolTimetable PRIMARY KEY,
        ClassID INT NOT NULL,
        Section NVARCHAR(50) NULL,
        SubjectID INT NOT NULL,
        TeacherID INT NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        TermName NVARCHAR(50) NULL,
        DayName NVARCHAR(30) NOT NULL,
        PeriodNo INT NOT NULL,
        RoomName NVARCHAR(100) NULL,
        Notes NVARCHAR(MAX) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_SchoolTimetable_IsActive DEFAULT 1
    );
END;

/* Staff attendance and contracts */
IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TeacherAttendance
    (
        AttendanceID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherAttendance PRIMARY KEY,
        TeacherID INT NOT NULL,
        AttendanceDate DATE NOT NULL,
        Status NVARCHAR(30) NOT NULL,
        LateMinutes INT NOT NULL CONSTRAINT DF_TeacherAttendance_LateMinutes DEFAULT 0,
        EarlyLeaveMinutes INT NOT NULL CONSTRAINT DF_TeacherAttendance_EarlyLeaveMinutes DEFAULT 0,
        WorkHours DECIMAL(10,2) NOT NULL CONSTRAINT DF_TeacherAttendance_WorkHours DEFAULT 0,
        AbsenceReason NVARCHAR(300) NULL,
        Notes NVARCHAR(MAX) NULL,
        RecordedAt DATETIME NOT NULL CONSTRAINT DF_TeacherAttendance_RecordedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;
IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TeacherContracts
    (
        ContractID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherContracts PRIMARY KEY,
        TeacherID INT NOT NULL,
        ContractNumber NVARCHAR(50) NULL,
        ContractType NVARCHAR(50) NULL,
        ContractStatus NVARCHAR(30) NULL,
        BasicSalary DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_BasicSalary DEFAULT 0,
        HousingAllowance DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_HousingAllowance DEFAULT 0,
        TransportAllowance DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_TransportAllowance DEFAULT 0,
        OtherAllowances DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_OtherAllowances DEFAULT 0,
        Deductions DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_Deductions DEFAULT 0,
        TotalSalary AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances),
        NetSalary AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances - Deductions),
        StartDate DATE NOT NULL CONSTRAINT DF_TeacherContracts_StartDate DEFAULT CONVERT(date, GETDATE()),
        EndDate DATE NULL,
        PaymentMethod NVARCHAR(50) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_TeacherContracts_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;

/* Finance */
IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.FeePlans
    (
        FeePlanID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeePlans PRIMARY KEY,
        AcademicYear NVARCHAR(20) NOT NULL,
        ClassID INT NOT NULL,
        FeeType NVARCHAR(100) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeePlans_Amount DEFAULT 0,
        DueDate DATE NOT NULL,
        IsRequired BIT NOT NULL CONSTRAINT DF_FeePlans_IsRequired DEFAULT 1,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_FeePlans_CreatedAt DEFAULT GETDATE()
    );
END;
IF OBJECT_ID(N'dbo.Fees', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Fees
    (
        FeeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Fees PRIMARY KEY,
        StudentID INT NOT NULL,
        FeePlanID INT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        FeeType NVARCHAR(100) NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_TotalAmount DEFAULT 0,
        DiscountAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_DiscountAmount DEFAULT 0,
        NetAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_NetAmount DEFAULT 0,
        PaidAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_PaidAmount DEFAULT 0,
        RemainingAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_RemainingAmount DEFAULT 0,
        DueDate DATE NOT NULL,
        PaymentDate DATE NULL,
        PaymentMethod NVARCHAR(50) NULL,
        ReceiptNumber NVARCHAR(50) NULL,
        Status NVARCHAR(30) NOT NULL CONSTRAINT DF_Fees_Status DEFAULT N'غير مدفوع',
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Fees_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;
IF OBJECT_ID(N'dbo.Expenses', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Expenses
    (
        ExpenseID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Expenses PRIMARY KEY,
        ExpenseNumber NVARCHAR(50) NULL,
        Amount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Expenses_Amount DEFAULT 0,
        ExpenseDate DATE NOT NULL,
        Category NVARCHAR(100) NULL,
        PayeeName NVARCHAR(200) NULL,
        PaymentMethod NVARCHAR(50) NULL,
        Description NVARCHAR(500) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Expenses_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;
IF OBJECT_ID(N'dbo.Payroll', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Payroll
    (
        PayrollID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payroll PRIMARY KEY,
        TeacherID INT NOT NULL,
        SalaryMonth INT NOT NULL,
        SalaryYear INT NOT NULL,
        BasicSalary DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_BasicSalary DEFAULT 0,
        Allowances DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Allowances DEFAULT 0,
        Deductions DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Deductions DEFAULT 0,
        NetSalary AS (BasicSalary + Allowances - Deductions),
        PaymentDate DATE NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Payroll_CreatedAt DEFAULT GETDATE()
    );
END;
IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vouchers
    (
        VoucherID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Vouchers PRIMARY KEY,
        VoucherNumber NVARCHAR(50) NULL,
        VoucherType NVARCHAR(30) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Vouchers_Amount DEFAULT 0,
        VoucherDate DATE NOT NULL,
        PartyName NVARCHAR(200) NULL,
        Description NVARCHAR(500) NULL,
        PaymentMethod NVARCHAR(50) NULL,
        ReferenceType NVARCHAR(50) NULL,
        ReferenceID INT NULL,
        Notes NVARCHAR(MAX) NULL,
        IsAutoGenerated BIT NOT NULL CONSTRAINT DF_Vouchers_IsAutoGenerated DEFAULT 0,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Vouchers_CreatedAt DEFAULT GETDATE()
    );
END;
IF OBJECT_ID(N'dbo.Receipts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Receipts
    (
        ReceiptID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Receipts PRIMARY KEY,
        ReceiptNumber NVARCHAR(50) NULL,
        StudentID INT NULL,
        Amount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Receipts_Amount DEFAULT 0,
        ReceiptDate DATE NOT NULL,
        PaymentMethod NVARCHAR(50) NULL,
        Description NVARCHAR(500) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Receipts_CreatedAt DEFAULT GETDATE()
    );
END;
IF OBJECT_ID(N'dbo.StudentFees', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentFees
    (
        StudentFeeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentFees PRIMARY KEY,
        StudentID INT NOT NULL,
        FeeType NVARCHAR(100) NULL,
        AcademicYear NVARCHAR(20) NULL,
        Amount DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_Amount DEFAULT 0,
        PaidAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_PaidAmount DEFAULT 0,
        Status NVARCHAR(30) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_StudentFees_CreatedAt DEFAULT GETDATE()
    );
END;
IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grades
    (
        GradeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades PRIMARY KEY,
        StudentID INT NULL,
        SubjectID INT NULL,
        ClassID INT NULL,
        AcademicYear NVARCHAR(20) NULL,
        TermName NVARCHAR(50) NULL,
        GradeValue DECIMAL(10,2) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Grades_CreatedAt DEFAULT GETDATE()
    );
END;

/* Helpful indexes, all idempotent */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BookBorrowings_BookID' AND object_id = OBJECT_ID(N'dbo.BookBorrowings')) CREATE INDEX IX_BookBorrowings_BookID ON dbo.BookBorrowings(BookID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_TeacherAttendance_Date' AND object_id = OBJECT_ID(N'dbo.TeacherAttendance')) CREATE INDEX IX_TeacherAttendance_Date ON dbo.TeacherAttendance(AttendanceDate);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_TeacherContracts_TeacherID' AND object_id = OBJECT_ID(N'dbo.TeacherContracts')) CREATE INDEX IX_TeacherContracts_TeacherID ON dbo.TeacherContracts(TeacherID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Fees_StudentID' AND object_id = OBJECT_ID(N'dbo.Fees')) CREATE INDEX IX_Fees_StudentID ON dbo.Fees(StudentID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Payroll_TeacherPeriod' AND object_id = OBJECT_ID(N'dbo.Payroll')) CREATE INDEX IX_Payroll_TeacherPeriod ON dbo.Payroll(TeacherID, SalaryYear, SalaryMonth);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Vouchers_Date' AND object_id = OBJECT_ID(N'dbo.Vouchers')) CREATE INDEX IX_Vouchers_Date ON dbo.Vouchers(VoucherDate);


/* Classes compatibility hardening
   Required by ClassRepository, ClassService, EnrollmentForm and ClassesForm. */
IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Classes
    (
        ClassID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Classes PRIMARY KEY,
        ClassCode NVARCHAR(30) NULL,
        ClassName NVARCHAR(100) NOT NULL,
        StageName NVARCHAR(100) NULL,
        GradeOrder INT NOT NULL CONSTRAINT DF_Classes_GradeOrder DEFAULT 0,
        IsActive BIT NOT NULL CONSTRAINT DF_Classes_IsActive DEFAULT 1,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Classes_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.Classes', N'ClassCode') IS NULL
        ALTER TABLE dbo.Classes ADD ClassCode NVARCHAR(30) NULL;
    IF COL_LENGTH(N'dbo.Classes', N'StageName') IS NULL
        ALTER TABLE dbo.Classes ADD StageName NVARCHAR(100) NULL;
    IF COL_LENGTH(N'dbo.Classes', N'GradeOrder') IS NULL
        ALTER TABLE dbo.Classes ADD GradeOrder INT NOT NULL CONSTRAINT DF_Classes_GradeOrder DEFAULT 0 WITH VALUES;
    IF COL_LENGTH(N'dbo.Classes', N'IsActive') IS NULL
        ALTER TABLE dbo.Classes ADD IsActive BIT NOT NULL CONSTRAINT DF_Classes_IsActive DEFAULT 1 WITH VALUES;
    IF COL_LENGTH(N'dbo.Classes', N'Notes') IS NULL
        ALTER TABLE dbo.Classes ADD Notes NVARCHAR(MAX) NULL;
    IF COL_LENGTH(N'dbo.Classes', N'CreatedAt') IS NULL
        ALTER TABLE dbo.Classes ADD CreatedAt DATETIME NOT NULL CONSTRAINT DF_Classes_CreatedAt DEFAULT GETDATE() WITH VALUES;
    IF COL_LENGTH(N'dbo.Classes', N'UpdatedAt') IS NULL
        ALTER TABLE dbo.Classes ADD UpdatedAt DATETIME NULL;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Classes')
      AND name = N'IX_Classes_GradeOrder'
)
BEGIN
    CREATE INDEX IX_Classes_GradeOrder ON dbo.Classes(IsActive, GradeOrder, ClassID);
END;

PRINT N'تمت ترقية جدول Classes بما يتوافق مع شاشات التسجيل وإدارة الفصول.';
GO
