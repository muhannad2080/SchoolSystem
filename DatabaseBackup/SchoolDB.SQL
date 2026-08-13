-- =========================
-- 1. إنشاء قاعدة البيانات
-- =========================
CREATE DATABASE SchoolDB;
GO

USE SchoolDB;
GO

-- =========================
-- 2. جدول الطلاب
-- =========================
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    BirthDate DATE NULL,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(200) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT CK_Students_Gender CHECK (Gender IN (N'ذكر', N'أنثى'))
);
GO

-- =========================
-- 3. جدول المعلمين
-- =========================
CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
    TeacherName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(200) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =========================
-- 4. جدول الصفوف
-- =========================
CREATE TABLE Classes (
    ClassID INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(50) NOT NULL
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
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT CK_Users_Role CHECK (Role IN (N'Admin', N'Teacher', N'Student'))
);
GO

-- =========================
-- 9. تحسين الأداء (Indexes)
-- =========================

CREATE INDEX IX_Students_Name ON Students(StudentName);
CREATE INDEX IX_Marks_StudentID ON Marks(StudentID);
CREATE INDEX IX_Marks_SubjectID ON Marks(SubjectID);
GO