-- ========================================================
-- المرحلة 1: تحديث هيكل قاعدة البيانات لتتوافق مع الكود الحالي
-- ========================================================

USE SchoolDB;
GO

-- 1. تحديث جدول الطلاب (Students)
-- إضافة الأعمدة الناقصة وتعديل المسميات لتتوافق مع StudentRepository
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'StudentNumber')
BEGIN
    ALTER TABLE Students ADD StudentNumber NVARCHAR(30) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'FullName')
BEGIN
    -- إذا كان العمود StudentName موجوداً، نقوم بنقل البيانات ثم حذفه أو استخدامه كـ FullName
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'StudentName')
    BEGIN
        EXEC sp_rename 'Students.StudentName', 'FullName', 'COLUMN';
    END
    ELSE
    BEGIN
        ALTER TABLE Students ADD FullName NVARCHAR(200) NULL;
    END
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'BirthPlace')
    ALTER TABLE Students ADD BirthPlace NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'Nationality')
    ALTER TABLE Students ADD Nationality NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'NationalId')
    ALTER TABLE Students ADD NationalId NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'StudentPhone')
BEGIN
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'Phone')
        EXEC sp_rename 'Students.Phone', 'StudentPhone', 'COLUMN';
    ELSE
        ALTER TABLE Students ADD StudentPhone NVARCHAR(30) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'Status')
    ALTER TABLE Students ADD Status NVARCHAR(30) DEFAULT N'نشط';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'GuardianName')
    ALTER TABLE Students ADD GuardianName NVARCHAR(200) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'GuardianRelation')
    ALTER TABLE Students ADD GuardianRelation NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'GuardianPhone')
    ALTER TABLE Students ADD GuardianPhone NVARCHAR(30) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'GuardianEmail')
    ALTER TABLE Students ADD GuardianEmail NVARCHAR(150) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'GuardianJob')
    ALTER TABLE Students ADD GuardianJob NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'Governorate')
    ALTER TABLE Students ADD Governorate NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'District')
    ALTER TABLE Students ADD District NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'Photo')
    ALTER TABLE Students ADD Photo VARBINARY(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = N'UpdatedAt')
    ALTER TABLE Students ADD UpdatedAt DATETIME NULL;

-- 2. تحديث جدول المعلمين (Teachers)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'FullName')
BEGIN
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'TeacherName')
        EXEC sp_rename 'Teachers.TeacherName', 'FullName', 'COLUMN';
    ELSE
        ALTER TABLE Teachers ADD FullName NVARCHAR(200) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'EmployeeNumber')
    ALTER TABLE Teachers ADD EmployeeNumber NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Gender')
    ALTER TABLE Teachers ADD Gender NVARCHAR(10) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'BirthDate')
    ALTER TABLE Teachers ADD BirthDate DATE NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'BirthPlace')
    ALTER TABLE Teachers ADD BirthPlace NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Nationality')
    ALTER TABLE Teachers ADD Nationality NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'NationalID')
    ALTER TABLE Teachers ADD NationalID NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Email')
    ALTER TABLE Teachers ADD Email NVARCHAR(150) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Qualification')
    ALTER TABLE Teachers ADD Qualification NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Specialization')
    ALTER TABLE Teachers ADD Specialization NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'HireDate')
    ALTER TABLE Teachers ADD HireDate DATE NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'BasicSalary')
    ALTER TABLE Teachers ADD BasicSalary DECIMAL(18, 2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'TransportAllowance')
    ALTER TABLE Teachers ADD TransportAllowance DECIMAL(18, 2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'HousingAllowance')
    ALTER TABLE Teachers ADD HousingAllowance DECIMAL(18, 2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Status')
    ALTER TABLE Teachers ADD Status NVARCHAR(50) DEFAULT N'نشط';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'Notes')
    ALTER TABLE Teachers ADD Notes NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND name = N'UpdatedAt')
    ALTER TABLE Teachers ADD UpdatedAt DATETIME NULL;

-- 3. تحديث جدول المستخدمين (Users)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'FullName')
    ALTER TABLE Users ADD FullName NVARCHAR(200) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'PasswordHash')
BEGIN
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'Password')
        EXEC sp_rename 'Users.Password', 'PasswordHash', 'COLUMN';
    ELSE
        ALTER TABLE Users ADD PasswordHash NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'PasswordSalt')
    ALTER TABLE Users ADD PasswordSalt NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'RoleName')
BEGIN
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'Role')
        EXEC sp_rename 'Users.Role', 'RoleName', 'COLUMN';
    ELSE
        ALTER TABLE Users ADD RoleName NVARCHAR(50) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'Permissions')
    ALTER TABLE Users ADD Permissions NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'Email')
    ALTER TABLE Users ADD Email NVARCHAR(150) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'Phone')
    ALTER TABLE Users ADD Phone NVARCHAR(30) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'MustChangePassword')
    ALTER TABLE Users ADD MustChangePassword BIT DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'FailedLoginAttempts')
    ALTER TABLE Users ADD FailedLoginAttempts INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts_Backup DEFAULT 0 WITH VALUES;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'LockedAt')
    ALTER TABLE Users ADD LockedAt DATETIME NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'LastLoginAt')
    ALTER TABLE Users ADD LastLoginAt DATETIME NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'UpdatedAt')
    ALTER TABLE Users ADD UpdatedAt DATETIME NULL;

-- 4. إنشاء جدول القبول والتسجيل (Enrollments)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND type in (N'U'))
BEGIN
    CREATE TABLE Enrollments (
        EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
        StudentID INT NOT NULL,
        ApplicationDate DATE NOT NULL,
        ApplicationType NVARCHAR(50) NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        ClassID INT NOT NULL,
        Section NVARCHAR(50) NULL,
        SeatNumber NVARCHAR(20) NULL,
        Status NVARCHAR(50) NOT NULL,
        PreviousSchool NVARCHAR(200) NULL,
        PreviousClass NVARCHAR(50) NULL,
        TransferReason NVARCHAR(MAX) NULL,
        RegistrationFee DECIMAL(18, 2) DEFAULT 0,
        PaidAmount DECIMAL(18, 2) DEFAULT 0,
        PaymentMethod NVARCHAR(50) NULL,
        ReceiptNo NVARCHAR(50) NULL,
        HasBirthCertificate BIT DEFAULT 0,
        HasGuardianId BIT DEFAULT 0,
        HasPhoto BIT DEFAULT 0,
        HasLastCertificate BIT DEFAULT 0,
        HasMedicalReport BIT DEFAULT 0,
        GeneralNotes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL,
        CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE,
        CONSTRAINT FK_Enrollments_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
    );
END

-- 5. تحديث جدول توزيع الفصول (StudentClasses)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudentClasses]') AND name = N'Section')
    ALTER TABLE StudentClasses ADD Section NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudentClasses]') AND name = N'AssignedDate')
    ALTER TABLE StudentClasses ADD AssignedDate DATETIME DEFAULT GETDATE();

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudentClasses]') AND name = N'AssignedBy')
    ALTER TABLE StudentClasses ADD AssignedBy INT NULL;

GO
