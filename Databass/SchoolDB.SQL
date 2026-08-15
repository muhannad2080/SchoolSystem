-- =========================
-- 1. إنشاء قاعدة البيانات
-- =========================
-- هذا الملف مخصص للإنشاء من الصفر فقط. لقاعدة موجودة استخدم ملفات Migration_*.sql.
IF DB_ID(N'SchoolDB') IS NOT NULL
BEGIN
    PRINT N'قاعدة SchoolDB موجودة مسبقًا. لم يتم تنفيذ سكربت الإنشاء؛ استخدم ملفات Migration_*.sql.';
    RETURN;
END;
GO

CREATE DATABASE SchoolDB;
GO

USE SchoolDB;
GO

-- =========================
-- 2. جدول الطلاب
-- =========================
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentNumber NVARCHAR(30) NULL,
    FullName NVARCHAR(200) NOT NULL,
    Gender NVARCHAR(20) NULL,
    BirthDate DATE NULL,
    BirthPlace NVARCHAR(100) NULL,
    Nationality NVARCHAR(100) NULL,
    NationalId NVARCHAR(50) NULL,
    StudentPhone NVARCHAR(30) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'نشط',
    GuardianName NVARCHAR(200) NULL,
    GuardianRelation NVARCHAR(50) NULL,
    GuardianPhone NVARCHAR(30) NULL,
    GuardianEmail NVARCHAR(150) NULL,
    GuardianJob NVARCHAR(100) NULL,
    Governorate NVARCHAR(100) NULL,
    District NVARCHAR(100) NULL,
    Address NVARCHAR(300) NULL,
    Photo VARBINARY(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

CREATE UNIQUE INDEX UX_Students_StudentNumber ON Students(StudentNumber) WHERE StudentNumber IS NOT NULL;

-- =========================
-- 3. جدول المعلمين
-- =========================
CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeNumber NVARCHAR(50) NULL,
    FullName NVARCHAR(200) NOT NULL,
    Gender NVARCHAR(10) NULL,
    BirthDate DATE NULL,
    BirthPlace NVARCHAR(100) NULL,
    Nationality NVARCHAR(100) NULL,
    NationalID NVARCHAR(50) NULL,
    Phone NVARCHAR(30) NULL,
    Email NVARCHAR(150) NULL,
    Address NVARCHAR(300) NULL,
    Qualification NVARCHAR(100) NULL,
    Specialization NVARCHAR(100) NULL,
    HireDate DATE NULL,
    BasicSalary DECIMAL(18, 2) NOT NULL DEFAULT 0,
    TransportAllowance DECIMAL(18, 2) NOT NULL DEFAULT 0,
    HousingAllowance DECIMAL(18, 2) NOT NULL DEFAULT 0,
    Status NVARCHAR(50) NOT NULL DEFAULT N'نشط',
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

CREATE UNIQUE INDEX UX_Teachers_EmployeeNumber ON Teachers(EmployeeNumber) WHERE EmployeeNumber IS NOT NULL;

-- =========================
-- 4. جدول الصفوف
-- =========================
CREATE TABLE Classes (
    ClassID INT IDENTITY(1,1) PRIMARY KEY,
    ClassCode NVARCHAR(30) NULL,
    ClassName NVARCHAR(100) NOT NULL,
    StageName NVARCHAR(100) NULL,
    GradeOrder INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

-- =========================
-- 5. جدول المواد
-- =========================
CREATE TABLE Subjects (
    SubjectID INT IDENTITY(1,1) PRIMARY KEY,
    SubjectName NVARCHAR(100) NOT NULL
);
GO

-- =========================
-- 6. جدول ربط الطلاب بالصفوف
-- =========================
CREATE TABLE StudentClasses (
    StudentClassID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    ClassID INT NOT NULL,
    AcademicYear NVARCHAR(20) NULL,

    CONSTRAINT FK_StudentClasses_Students
        FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
        ON DELETE CASCADE,

    CONSTRAINT FK_StudentClasses_Classes
        FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
        ON DELETE CASCADE
);
GO

-- =========================
-- 7. جدول الدرجات
-- =========================
CREATE TABLE Marks (
    MarkID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    SubjectID INT NOT NULL,
    TeacherID INT NULL,
    Mark DECIMAL(5,2) NOT NULL,
    ExamType NVARCHAR(50) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Marks_Students
        FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
        ON DELETE CASCADE,

    CONSTRAINT FK_Marks_Subjects
        FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),

    CONSTRAINT FK_Marks_Teachers
        FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID)
        ON DELETE SET NULL
);
GO

-- =========================
-- 8. جدول المستخدمين (تسجيل الدخول)
-- =========================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    PasswordSalt NVARCHAR(MAX) NOT NULL,
    RoleName NVARCHAR(50) NOT NULL,
    Permissions NVARCHAR(MAX) NULL,
    Email NVARCHAR(150) NULL,
    Phone NVARCHAR(30) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    MustChangePassword BIT NOT NULL DEFAULT 0,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LockedAt DATETIME NULL,
    LastLoginAt DATETIME NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

CREATE UNIQUE INDEX UX_Users_UserName ON Users(UserName);

-- =========================
-- 9. تحسين الأداء (Indexes)
-- =========================

CREATE INDEX IX_Students_Name ON Students(FullName);
CREATE INDEX IX_Marks_StudentID ON Marks(StudentID);
CREATE INDEX IX_Marks_SubjectID ON Marks(SubjectID);
GO

-- =========================
-- 10. جداول النقل المدرسي والتحضير اليومي
-- =========================
CREATE TABLE Buses (
    BusID INT IDENTITY(1,1) PRIMARY KEY,
    BusNumber NVARCHAR(50) NOT NULL,
    DriverName NVARCHAR(150) NULL,
    DriverPhone NVARCHAR(50) NULL,
    Capacity INT NOT NULL DEFAULT 0,
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

CREATE UNIQUE INDEX UX_Buses_BusNumber ON Buses(BusNumber);
GO

CREATE TABLE BusRoutes (
    RouteID INT IDENTITY(1,1) PRIMARY KEY,
    RouteName NVARCHAR(150) NOT NULL,
    BusID INT NOT NULL,
    StartPoint NVARCHAR(200) NULL,
    EndPoint NVARCHAR(200) NULL,
    DepartureTime TIME NULL,
    ArrivalTime TIME NULL,
    Fee DECIMAL(18,2) NULL,
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    CONSTRAINT FK_BusRoutes_Buses FOREIGN KEY (BusID) REFERENCES Buses(BusID)
);
GO

CREATE INDEX IX_BusRoutes_BusID ON BusRoutes(BusID);
GO

CREATE TABLE StudentAttendance (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    ClassID INT NOT NULL,
    Section NVARCHAR(50) NOT NULL,
    AcademicYear NVARCHAR(20) NOT NULL,
    AttendanceDate DATE NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'حاضر',
    ArrivalTime TIME NULL,
    ExcuseStatus NVARCHAR(50) NULL,
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    CONSTRAINT FK_StudentAttendance_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_StudentAttendance_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
);
GO

CREATE UNIQUE INDEX UX_StudentAttendance_StudentDate
    ON StudentAttendance(StudentID, AttendanceDate);
GO

CREATE INDEX IX_StudentAttendance_ClassDate
    ON StudentAttendance(ClassID, AttendanceDate);
GO

CREATE INDEX IX_StudentAttendance_Date
    ON StudentAttendance(AttendanceDate);
GO
