# مخطط ERD الكامل لقاعدة بيانات SchoolSystem

> هذا المستند مولّد من ملفات SQL الموجودة في `Databass/` و`DatabaseBackup/`. عند اختلاف نسخة احتياطية قديمة عن ترحيل أحدث، تُذكر الخصائص المضافة في الترحيلات ضمن الجدول الموحد.

**عدد الجداول المكتشفة:** 32  
**عدد العلاقات المكتشفة:** 23

## مفتاح القراءة

| الرمز | المعنى |
|---|---|
| PK | مفتاح أساسي |
| FK | مفتاح خارجي |
| 1:N | واحد إلى متعدد |
| 1:1 | واحد إلى واحد |
| N:M | متعدد إلى متعدد عبر جدول وسيط |
| SET NULL | تبقى السجلات التابعة وتصبح الإشارة فارغة |
| CASCADE | حذف التابع مع الأصل، ويُستخدم فقط حيث هو معرف في المخطط |

## الجداول والخصائص

### `UserPermissions`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `UserPermissionID` | `INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_UserPermissions PRIMARY KEY` | PK |
| `UserID` | `INT NOT NULL` | FK |
| `PermissionID` | `INT NOT NULL` | FK |
| `GrantedAt` | `DATETIME2 NOT NULL CONSTRAINT DF_UserPermissions_GrantedAt DEFAULT GETDATE()` | بيانات |
| `GrantedBy` | `INT NULL` | بيانات |

### `Users`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `FullName` | `NVARCHAR(200) NULL` | بيانات |
| `PasswordHash` | `NVARCHAR(MAX) NULL` | بيانات |
| `PasswordSalt` | `NVARCHAR(MAX) NULL` | بيانات |
| `RoleName` | `NVARCHAR(50) NULL` | بيانات |
| `Permissions` | `NVARCHAR(MAX) NULL` | بيانات |
| `Email` | `NVARCHAR(150) NULL` | بيانات |
| `Phone` | `NVARCHAR(30) NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1 WITH VALUES` | بيانات |
| `MustChangePassword` | `BIT NOT NULL CONSTRAINT DF_Users_MustChangePassword DEFAULT 0 WITH VALUES` | بيانات |
| `FailedLoginAttempts` | `INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT 0 WITH VALUES` | بيانات |
| `LockedAt` | `DATETIME NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT GETDATE() WITH VALUES` | بيانات |
| `LastLoginAt` | `DATETIME NULL` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `UserID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `UserName` | `NVARCHAR(50) NOT NULL` | بيانات |
| `Password` | `NVARCHAR(100) NOT NULL` | بيانات |
| `Role` | `NVARCHAR(20) NOT NULL` | بيانات |

### `Permissions`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `PermissionID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Permissions PRIMARY KEY` | PK |
| `PermissionKey` | `NVARCHAR(150) NOT NULL` | بيانات |
| `DisplayName` | `NVARCHAR(250) NULL` | بيانات |
| `ModuleName` | `NVARCHAR(100) NULL` | بيانات |
| `ActionName` | `NVARCHAR(100) NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Permissions_IsActive DEFAULT 1` | بيانات |
| `CreatedAt` | `DATETIME2(0) NOT NULL CONSTRAINT DF_Permissions_CreatedAt DEFAULT SYSUTCDATETIME()` | بيانات |

### `StudentClasses`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `AcademicYearKey` | `AS` | بيانات |
| `Section` | `NVARCHAR(50) NULL` | بيانات |
| `AssignedDate` | `DATETIME NOT NULL CONSTRAINT DF_StudentClasses_AssignedDate_Compat DEFAULT GETDATE() WITH VALUES` | بيانات |
| `AssignedBy` | `INT NULL` | بيانات |
| `StudentClassID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | FK |
| `ClassID` | `INT NOT NULL` | FK |
| `AcademicYear` | `NVARCHAR(20) NULL` | بيانات |

### `StudentAttendance`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `AttendanceDay` | `AS (CONVERT(date, AttendanceDate)) PERSISTED` | بيانات |
| `DepartureTime` | `TIME NULL` | بيانات |
| `AbsenceReason` | `NVARCHAR(500) NULL` | بيانات |
| `AttendanceID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentAttendance PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | FK |
| `ClassID` | `INT NOT NULL` | FK |
| `Section` | `NVARCHAR(50) NOT NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `AttendanceDate` | `DATE NOT NULL` | بيانات |
| `Status` | `NVARCHAR(30) NOT NULL CONSTRAINT DF_StudentAttendance_Status DEFAULT (N'حاضر')` | بيانات |
| `ArrivalTime` | `TIME NULL` | بيانات |
| `ExcuseStatus` | `NVARCHAR(50) NULL` | بيانات |
| `Notes` | `NVARCHAR(500) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_StudentAttendance_CreatedAt DEFAULT (GETDATE())` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `TeacherAttendance`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `AttendanceDay` | `AS (CONVERT(date, AttendanceDate)) PERSISTED` | بيانات |
| `AttendanceID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherAttendance PRIMARY KEY` | PK |
| `TeacherID` | `INT NOT NULL` | بيانات |
| `AttendanceDate` | `DATE NOT NULL` | بيانات |
| `Status` | `NVARCHAR(30) NOT NULL` | بيانات |
| `LateMinutes` | `INT NOT NULL CONSTRAINT DF_TeacherAttendance_LateMinutes DEFAULT 0` | بيانات |
| `EarlyLeaveMinutes` | `INT NOT NULL CONSTRAINT DF_TeacherAttendance_EarlyLeaveMinutes DEFAULT 0` | بيانات |
| `WorkHours` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_TeacherAttendance_WorkHours DEFAULT 0` | بيانات |
| `AbsenceReason` | `NVARCHAR(300) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `RecordedAt` | `DATETIME NOT NULL CONSTRAINT DF_TeacherAttendance_RecordedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `CheckInTime` | `TIME NULL` | بيانات |
| `CheckOutTime` | `TIME NULL` | بيانات |

### `SchoolSections`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `Capacity` | `INT NULL` | بيانات |
| `AllowedGender` | `NVARCHAR(20) NULL` | بيانات |
| `CONSTRAINT` | `CK_SchoolSections_Capacity` | بيانات |
| `SectionID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolSections PRIMARY KEY` | PK |
| `ClassID` | `INT NOT NULL` | FK |
| `SectionName` | `NVARCHAR(50) NOT NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_SchoolSections_IsActive DEFAULT (1)` | بيانات |
| `CreatedAt` | `DATETIME2(0) NOT NULL CONSTRAINT DF_SchoolSections_CreatedAt DEFAULT (SYSDATETIME())` | بيانات |

### `AuditLogs`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `AuditLogID` | `BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY` | PK |
| `UserID` | `INT NULL` | FK |
| `UserName` | `NVARCHAR(150) NULL` | بيانات |
| `ActionName` | `NVARCHAR(100) NOT NULL` | بيانات |
| `EntityName` | `NVARCHAR(100) NULL` | بيانات |
| `EntityID` | `NVARCHAR(100) NULL` | بيانات |
| `Details` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())` | بيانات |
| `Module` | `NVARCHAR(100) NULL` | بيانات |
| `MachineName` | `NVARCHAR(150) NULL` | بيانات |
| `IpAddress` | `NVARCHAR(64) NULL` | بيانات |

### `Rooms`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `CreatedByUserID` | `INT NULL` | FK |
| `RoomID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY` | PK |
| `RoomCode` | `NVARCHAR(30) NULL` | بيانات |
| `RoomName` | `NVARCHAR(100) NOT NULL` | بيانات |
| `RoomType` | `NVARCHAR(50) NULL` | بيانات |
| `Capacity` | `INT NOT NULL CONSTRAINT DF_Rooms_Capacity DEFAULT 0` | بيانات |
| `Location` | `NVARCHAR(200) NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT 1` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Rooms_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Classes`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `RoomID` | `INT NULL` | FK |
| `ClassID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Classes PRIMARY KEY` | PK |
| `ClassCode` | `NVARCHAR(30) NULL` | بيانات |
| `ClassName` | `NVARCHAR(100) NOT NULL` | بيانات |
| `StageName` | `NVARCHAR(100) NULL` | بيانات |
| `GradeOrder` | `INT NOT NULL CONSTRAINT DF_Classes_GradeOrder DEFAULT 0` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Classes_IsActive DEFAULT 1` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Classes_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Expenses`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `CreatedByUserID` | `INT NULL` | FK |
| `ExpenseID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Expenses PRIMARY KEY` | PK |
| `ExpenseNumber` | `NVARCHAR(50) NULL` | بيانات |
| `Amount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Expenses_Amount DEFAULT 0` | بيانات |
| `ExpenseDate` | `DATE NOT NULL` | بيانات |
| `Category` | `NVARCHAR(100) NULL` | بيانات |
| `PayeeName` | `NVARCHAR(200) NULL` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `Description` | `NVARCHAR(500) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Expenses_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Vouchers`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `CreatedByUserID` | `INT NULL` | FK |
| `VoucherID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Vouchers PRIMARY KEY` | PK |
| `VoucherNumber` | `NVARCHAR(50) NULL` | بيانات |
| `VoucherType` | `NVARCHAR(30) NOT NULL` | بيانات |
| `Amount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Vouchers_Amount DEFAULT 0` | بيانات |
| `VoucherDate` | `DATE NOT NULL` | بيانات |
| `PartyName` | `NVARCHAR(200) NULL` | بيانات |
| `Description` | `NVARCHAR(500) NULL` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `ReferenceType` | `NVARCHAR(50) NULL` | بيانات |
| `ReferenceID` | `INT NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `IsAutoGenerated` | `BIT NOT NULL CONSTRAINT DF_Vouchers_IsAutoGenerated DEFAULT 0` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Vouchers_CreatedAt DEFAULT GETDATE()` | بيانات |

### `Enrollments`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `EnrollmentID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Enrollments PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | FK |
| `ApplicationDate` | `DATE NOT NULL CONSTRAINT DF_Enrollments_ApplicationDate DEFAULT CONVERT(date, GETDATE())` | بيانات |
| `ApplicationType` | `NVARCHAR(50) NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `ClassID` | `INT NOT NULL` | FK |
| `Section` | `NVARCHAR(50) NULL` | بيانات |
| `SeatNumber` | `NVARCHAR(20) NULL` | بيانات |
| `Status` | `NVARCHAR(50) NOT NULL CONSTRAINT DF_Enrollments_Status DEFAULT N'جديد'` | بيانات |
| `PreviousSchool` | `NVARCHAR(200) NULL` | بيانات |
| `PreviousClass` | `NVARCHAR(50) NULL` | بيانات |
| `TransferReason` | `NVARCHAR(MAX) NULL` | بيانات |
| `RegistrationFee` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_RegistrationFee DEFAULT 0` | بيانات |
| `PaidAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_PaidAmount DEFAULT 0` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `ReceiptNo` | `NVARCHAR(50) NULL` | بيانات |
| `HasBirthCertificate` | `BIT NOT NULL CONSTRAINT DF_Enrollments_HasBirthCertificate DEFAULT 0` | بيانات |
| `HasGuardianId` | `BIT NOT NULL CONSTRAINT DF_Enrollments_HasGuardianId DEFAULT 0` | بيانات |
| `HasPhoto` | `BIT NOT NULL CONSTRAINT DF_Enrollments_HasPhoto DEFAULT 0` | بيانات |
| `HasLastCertificate` | `BIT NOT NULL CONSTRAINT DF_Enrollments_HasLastCertificate DEFAULT 0` | بيانات |
| `HasMedicalReport` | `BIT NOT NULL CONSTRAINT DF_Enrollments_HasMedicalReport DEFAULT 0` | بيانات |
| `GeneralNotes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Enrollments_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Students`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `ClassID` | `INT NULL` | بيانات |
| `Section` | `NVARCHAR(50) NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NULL` | بيانات |
| `Phone` | `NVARCHAR(30) NULL` | بيانات |
| `StudentNumber` | `NVARCHAR(30) NULL` | بيانات |
| `FullName` | `NVARCHAR(200) NULL` | بيانات |
| `BirthPlace` | `NVARCHAR(100) NULL` | بيانات |
| `Nationality` | `NVARCHAR(100) NULL` | بيانات |
| `NationalId` | `NVARCHAR(50) NULL` | بيانات |
| `StudentPhone` | `NVARCHAR(30) NULL` | بيانات |
| `Status` | `NVARCHAR(30) NOT NULL CONSTRAINT DF_Students_Status DEFAULT N'نشط' WITH VALUES` | بيانات |
| `GuardianName` | `NVARCHAR(200) NULL` | بيانات |
| `GuardianRelation` | `NVARCHAR(50) NULL` | بيانات |
| `GuardianPhone` | `NVARCHAR(30) NULL` | بيانات |
| `GuardianEmail` | `NVARCHAR(150) NULL` | بيانات |
| `GuardianJob` | `NVARCHAR(100) NULL` | بيانات |
| `Governorate` | `NVARCHAR(100) NULL` | بيانات |
| `District` | `NVARCHAR(100) NULL` | بيانات |
| `Address` | `NVARCHAR(300) NULL` | بيانات |
| `Photo` | `VARBINARY(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Students_CreatedAt DEFAULT GETDATE() WITH VALUES` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `StudentID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `Gender` | `NVARCHAR(20) NULL` | بيانات |
| `BirthDate` | `DATE NULL` | بيانات |
| `StudentName` | `NVARCHAR(100) NOT NULL` | بيانات |

### `Grades`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `GradeID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades PRIMARY KEY` | PK |
| `StudentID` | `INT NULL` | بيانات |
| `SubjectID` | `INT NULL` | بيانات |
| `ClassID` | `INT NULL` | بيانات |
| `Section` | `NVARCHAR(100) NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NULL` | بيانات |
| `TermName` | `NVARCHAR(50) NULL` | بيانات |
| `Quiz1` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz1 DEFAULT (0)` | بيانات |
| `Quiz2` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz2 DEFAULT (0)` | بيانات |
| `CourseWork` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_CourseWork DEFAULT (0)` | بيانات |
| `FinalExam` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_FinalExam DEFAULT (0)` | بيانات |
| `GradeValue` | `DECIMAL(10,2) NULL` | بيانات |
| `GradeLetter` | `NVARCHAR(50) NULL` | بيانات |
| `ResultStatus` | `NVARCHAR(50) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Grades_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Books`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `BookID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Books PRIMARY KEY` | PK |
| `Title` | `NVARCHAR(200) NOT NULL` | بيانات |
| `Author` | `NVARCHAR(200) NULL` | بيانات |
| `ISBN` | `NVARCHAR(50) NULL` | بيانات |
| `Category` | `NVARCHAR(100) NULL` | بيانات |
| `Publisher` | `NVARCHAR(200) NULL` | بيانات |
| `PublicationYear` | `INT NULL` | بيانات |
| `Copies` | `INT NOT NULL CONSTRAINT DF_Books_Copies DEFAULT 0` | بيانات |
| `AvailableCopies` | `INT NOT NULL CONSTRAINT DF_Books_AvailableCopies DEFAULT 0` | بيانات |
| `ShelfLocation` | `NVARCHAR(100) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Books_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Books_IsActive DEFAULT 1 WITH VALUES` | بيانات |

### `BookBorrowings`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `BorrowingID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BookBorrowings PRIMARY KEY` | PK |
| `BookID` | `INT NOT NULL` | FK |
| `BorrowerType` | `NVARCHAR(20) NOT NULL` | بيانات |
| `BorrowerID` | `INT NOT NULL` | بيانات |
| `BorrowDate` | `DATE NOT NULL` | بيانات |
| `DueDate` | `DATE NOT NULL` | بيانات |
| `ReturnDate` | `DATE NULL` | بيانات |
| `Status` | `NVARCHAR(30) NOT NULL CONSTRAINT DF_BookBorrowings_Status DEFAULT N'معار'` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_BookBorrowings_CreatedAt DEFAULT GETDATE() WITH VALUES` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `SchoolTimetable`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `TimetableID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolTimetable PRIMARY KEY` | PK |
| `ClassID` | `INT NOT NULL` | بيانات |
| `Section` | `NVARCHAR(50) NULL` | بيانات |
| `SubjectID` | `INT NOT NULL` | بيانات |
| `TeacherID` | `INT NOT NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `TermName` | `NVARCHAR(50) NULL` | بيانات |
| `DayName` | `NVARCHAR(30) NOT NULL` | بيانات |
| `PeriodNo` | `INT NOT NULL` | بيانات |
| `StartTime` | `TIME NOT NULL CONSTRAINT DF_SchoolTimetable_StartTime DEFAULT '08:00'` | بيانات |
| `EndTime` | `TIME NOT NULL CONSTRAINT DF_SchoolTimetable_EndTime DEFAULT '08:45'` | بيانات |
| `RoomName` | `NVARCHAR(100) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_SchoolTimetable_IsActive DEFAULT 1` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_SchoolTimetable_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `TeacherContracts`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `ContractID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherContracts PRIMARY KEY` | PK |
| `TeacherID` | `INT NOT NULL` | بيانات |
| `ContractNumber` | `NVARCHAR(50) NULL` | بيانات |
| `ContractType` | `NVARCHAR(50) NULL` | بيانات |
| `ContractStatus` | `NVARCHAR(30) NULL` | بيانات |
| `BasicSalary` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_BasicSalary DEFAULT 0` | بيانات |
| `HousingAllowance` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_HousingAllowance DEFAULT 0` | بيانات |
| `TransportAllowance` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_TransportAllowance DEFAULT 0` | بيانات |
| `OtherAllowances` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_OtherAllowances DEFAULT 0` | بيانات |
| `Deductions` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_Deductions DEFAULT 0` | بيانات |
| `TotalSalary` | `AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances)` | بيانات |
| `NetSalary` | `AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances - Deductions)` | بيانات |
| `StartDate` | `DATE NOT NULL CONSTRAINT DF_TeacherContracts_StartDate DEFAULT CONVERT(date, GETDATE())` | بيانات |
| `EndDate` | `DATE NULL` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_TeacherContracts_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `FeePlans`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `FeePlanID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeePlans PRIMARY KEY` | PK |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `ClassID` | `INT NOT NULL` | بيانات |
| `FeeType` | `NVARCHAR(100) NOT NULL` | بيانات |
| `Amount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeePlans_Amount DEFAULT 0` | بيانات |
| `DueDate` | `DATE NOT NULL` | بيانات |
| `IsRequired` | `BIT NOT NULL CONSTRAINT DF_FeePlans_IsRequired DEFAULT 1` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_FeePlans_CreatedAt DEFAULT GETDATE()` | بيانات |

### `Fees`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `FeeID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Fees PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | بيانات |
| `FeePlanID` | `INT NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NOT NULL` | بيانات |
| `FeeType` | `NVARCHAR(100) NOT NULL` | بيانات |
| `TotalAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_TotalAmount DEFAULT 0` | بيانات |
| `DiscountAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_DiscountAmount DEFAULT 0` | بيانات |
| `NetAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_NetAmount DEFAULT 0` | بيانات |
| `PaidAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_PaidAmount DEFAULT 0` | بيانات |
| `RemainingAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_RemainingAmount DEFAULT 0` | بيانات |
| `DueDate` | `DATE NOT NULL` | بيانات |
| `PaymentDate` | `DATE NULL` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `ReceiptNumber` | `NVARCHAR(50) NULL` | بيانات |
| `Status` | `NVARCHAR(30) NOT NULL CONSTRAINT DF_Fees_Status DEFAULT N'غير مدفوع'` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Fees_CreatedAt DEFAULT GETDATE()` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Payroll`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `PayrollID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payroll PRIMARY KEY` | PK |
| `TeacherID` | `INT NOT NULL` | بيانات |
| `SalaryMonth` | `INT NOT NULL` | بيانات |
| `SalaryYear` | `INT NOT NULL` | بيانات |
| `BasicSalary` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_BasicSalary DEFAULT 0` | بيانات |
| `Allowances` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Allowances DEFAULT 0` | بيانات |
| `Deductions` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Deductions DEFAULT 0` | بيانات |
| `NetSalary` | `AS (BasicSalary + Allowances - Deductions)` | بيانات |
| `PaymentDate` | `DATE NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Payroll_CreatedAt DEFAULT GETDATE()` | بيانات |

### `Receipts`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `ReceiptID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Receipts PRIMARY KEY` | PK |
| `ReceiptNumber` | `NVARCHAR(50) NULL` | بيانات |
| `StudentID` | `INT NULL` | بيانات |
| `Amount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_Receipts_Amount DEFAULT 0` | بيانات |
| `ReceiptDate` | `DATE NOT NULL` | بيانات |
| `PaymentMethod` | `NVARCHAR(50) NULL` | بيانات |
| `Description` | `NVARCHAR(500) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Receipts_CreatedAt DEFAULT GETDATE()` | بيانات |

### `StudentFees`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `StudentFeeID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentFees PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | بيانات |
| `FeeType` | `NVARCHAR(100) NULL` | بيانات |
| `AcademicYear` | `NVARCHAR(20) NULL` | بيانات |
| `Amount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_Amount DEFAULT 0` | بيانات |
| `PaidAmount` | `DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_PaidAmount DEFAULT 0` | بيانات |
| `Status` | `NVARCHAR(30) NULL` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_StudentFees_CreatedAt DEFAULT GETDATE()` | بيانات |

### `Subjects`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `SubjectCode` | `NVARCHAR(30) NULL` | بيانات |
| `ClassID` | `INT NULL` | بيانات |
| `MaxDegree` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_MaxDegree DEFAULT 100 WITH VALUES` | بيانات |
| `PassDegree` | `DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_PassDegree DEFAULT 50 WITH VALUES` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Subjects_IsActive DEFAULT 1 WITH VALUES` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Subjects_CreatedAt DEFAULT GETDATE() WITH VALUES` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `SubjectID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `SubjectName` | `NVARCHAR(100) NOT NULL` | بيانات |

### `Buses`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `BusID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Buses PRIMARY KEY` | PK |
| `BusNumber` | `NVARCHAR(50) NOT NULL` | بيانات |
| `DriverName` | `NVARCHAR(150) NULL` | بيانات |
| `DriverPhone` | `NVARCHAR(50) NULL` | بيانات |
| `Capacity` | `INT NOT NULL CONSTRAINT DF_Buses_Capacity DEFAULT (0)` | بيانات |
| `Notes` | `NVARCHAR(500) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Buses_CreatedAt DEFAULT (GETDATE())` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `BusRoutes`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `RouteID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BusRoutes PRIMARY KEY` | PK |
| `RouteName` | `NVARCHAR(150) NOT NULL` | بيانات |
| `BusID` | `INT NOT NULL` | FK |
| `StartPoint` | `NVARCHAR(200) NULL` | بيانات |
| `EndPoint` | `NVARCHAR(200) NULL` | بيانات |
| `DepartureTime` | `TIME NULL` | بيانات |
| `ArrivalTime` | `TIME NULL` | بيانات |
| `Fee` | `DECIMAL(18,2) NULL` | بيانات |
| `Notes` | `NVARCHAR(500) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_BusRoutes_CreatedAt DEFAULT (GETDATE())` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |

### `Roles`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `RoleID` | `INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY` | PK |
| `RoleName` | `NVARCHAR(100) NOT NULL` | بيانات |
| `Description` | `NVARCHAR(500) NULL` | بيانات |
| `IsSystemRole` | `BIT NOT NULL CONSTRAINT DF_Roles_IsSystemRole DEFAULT 0` | بيانات |
| `IsActive` | `BIT NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT 1` | بيانات |
| `CreatedAt` | `DATETIME2(0) NOT NULL CONSTRAINT DF_Roles_CreatedAt DEFAULT SYSUTCDATETIME()` | بيانات |
| `UpdatedAt` | `DATETIME2(0) NULL` | بيانات |

### `UserRoles`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `UserID` | `INT NOT NULL` | FK |
| `RoleID` | `INT NOT NULL` | FK |
| `AssignedAt` | `DATETIME2(0) NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT SYSUTCDATETIME()` | بيانات |
| `AssignedBy` | `INT NULL` | بيانات |

### `RolePermissions`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `RoleID` | `INT NOT NULL` | FK |
| `PermissionID` | `INT NOT NULL` | FK |
| `GrantedAt` | `DATETIME2(0) NOT NULL CONSTRAINT DF_RolePermissions_GrantedAt DEFAULT SYSUTCDATETIME()` | بيانات |
| `GrantedBy` | `INT NULL` | بيانات |

### `Teachers`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `FullName` | `NVARCHAR(200) NULL` | بيانات |
| `EmployeeNumber` | `NVARCHAR(50) NULL` | بيانات |
| `Gender` | `NVARCHAR(10) NULL` | بيانات |
| `BirthDate` | `DATE NULL` | بيانات |
| `BirthPlace` | `NVARCHAR(100) NULL` | بيانات |
| `Nationality` | `NVARCHAR(100) NULL` | بيانات |
| `Phone` | `NVARCHAR(30) NULL` | بيانات |
| `NationalID` | `NVARCHAR(50) NULL` | بيانات |
| `Email` | `NVARCHAR(150) NULL` | بيانات |
| `Qualification` | `NVARCHAR(100) NULL` | بيانات |
| `Specialization` | `NVARCHAR(100) NULL` | بيانات |
| `Address` | `NVARCHAR(300) NULL` | بيانات |
| `HireDate` | `DATE NULL` | بيانات |
| `BasicSalary` | `DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_BasicSalary DEFAULT 0 WITH VALUES` | بيانات |
| `TransportAllowance` | `DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_TransportAllowance DEFAULT 0 WITH VALUES` | بيانات |
| `HousingAllowance` | `DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_HousingAllowance DEFAULT 0 WITH VALUES` | بيانات |
| `Status` | `NVARCHAR(50) NOT NULL CONSTRAINT DF_Teachers_Status DEFAULT N'نشط' WITH VALUES` | بيانات |
| `Notes` | `NVARCHAR(MAX) NULL` | بيانات |
| `CreatedAt` | `DATETIME NOT NULL CONSTRAINT DF_Teachers_CreatedAt DEFAULT GETDATE() WITH VALUES` | بيانات |
| `UpdatedAt` | `DATETIME NULL` | بيانات |
| `TeacherID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `TeacherName` | `NVARCHAR(100) NOT NULL` | بيانات |

### `Marks`

| الخاصية | نوع البيانات والقيود | الدور |
|---|---|---|
| `MarkID` | `INT IDENTITY(1,1) PRIMARY KEY` | PK |
| `StudentID` | `INT NOT NULL` | FK |
| `SubjectID` | `INT NOT NULL` | FK |
| `TeacherID` | `INT NULL` | FK |
| `Mark` | `DECIMAL(5,2) NOT NULL` | بيانات |
| `ExamType` | `NVARCHAR(50) NULL` | بيانات |
| `CreatedAt` | `DATETIME DEFAULT GETDATE()` | بيانات |

## العلاقات والكاردينالية

| الجدول الابن | الحقل | الجدول الأصل | الحقل الأصل | الكاردينالية | الحذف | القيد |
|---|---|---|---|---|---|---|
| `UserPermissions` | `UserID` | `Users` | `UserID` | 1:N | NO ACTION / غير محدد | `FK_UserPermissions_Users` |
| `UserPermissions` | `PermissionID` | `Permissions` | `PermissionID` | 1:N | NO ACTION / غير محدد | `FK_UserPermissions_Permissions` |
| `Enrollments` | `StudentID` | `Students` | `StudentID` | 1:N | NO ACTION / غير محدد | `FK_Enrollments_Students_Complete` |
| `Enrollments` | `ClassID` | `Classes` | `ClassID` | 1:N | NO ACTION / غير محدد | `FK_Enrollments_Classes_Complete` |
| `Classes` | `RoomID` | `Rooms` | `RoomID` | 1:N | SET NULL | `FK_Classes_Rooms_Complete` |
| `AuditLogs` | `UserID` | `Users` | `UserID` | 1:N | SET NULL | `FK_AuditLogs_Users_Complete` |
| `Rooms` | `CreatedByUserID` | `Users` | `UserID` | 1:N | SET NULL | `FK_Rooms_CreatedByUser_Complete` |
| `Expenses` | `CreatedByUserID` | `Users` | `UserID` | 1:N | SET NULL | `FK_Expenses_CreatedByUser_Complete` |
| `Vouchers` | `CreatedByUserID` | `Users` | `UserID` | 1:N | SET NULL | `FK_Vouchers_CreatedByUser_Complete` |
| `SchoolSections` | `ClassID` | `Classes` | `ClassID` | 1:N | NO ACTION / غير محدد | `FK_SchoolSections_Classes` |
| `BookBorrowings` | `BookID` | `Books` | `BookID` | 1:N | NO ACTION / غير محدد | `FK_BookBorrowings_Books` |
| `BusRoutes` | `BusID` | `Buses` | `BusID` | 1:N | NO ACTION / غير محدد | `FK_BusRoutes_Buses` |
| `StudentAttendance` | `StudentID` | `Students` | `StudentID` | 1:N | NO ACTION / غير محدد | `FK_StudentAttendance_Students` |
| `StudentAttendance` | `ClassID` | `Classes` | `ClassID` | 1:N | NO ACTION / غير محدد | `FK_StudentAttendance_Classes` |
| `UserRoles` | `UserID` | `Users` | `UserID` | 1:N | NO ACTION / غير محدد | `FK_UserRoles_Users` |
| `UserRoles` | `RoleID` | `Roles` | `RoleID` | 1:N | NO ACTION / غير محدد | `FK_UserRoles_Roles` |
| `RolePermissions` | `RoleID` | `Roles` | `RoleID` | 1:N | NO ACTION / غير محدد | `FK_RolePermissions_Roles` |
| `RolePermissions` | `PermissionID` | `Permissions` | `PermissionID` | 1:N | NO ACTION / غير محدد | `FK_RolePermissions_Permissions` |
| `StudentClasses` | `StudentID` | `Students` | `StudentID` | 1:N | NO ACTION / غير محدد | `FK_StudentClasses_Students` |
| `StudentClasses` | `ClassID` | `Classes` | `ClassID` | 1:N | NO ACTION / غير محدد | `FK_StudentClasses_Classes` |
| `Marks` | `StudentID` | `Students` | `StudentID` | 1:N | NO ACTION / غير محدد | `FK_Marks_Students` |
| `Marks` | `SubjectID` | `Subjects` | `SubjectID` | 1:N | NO ACTION / غير محدد | `FK_Marks_Subjects` |
| `Marks` | `TeacherID` | `Teachers` | `TeacherID` | 1:N | NO ACTION / غير محدد | `FK_Marks_Teachers` |

## العلاقات غير المباشرة N:M

العلاقة **متعدد إلى متعدد** لا تُخزن عادةً مباشرة؛ تُنفذ بجدول وسيط. في هذا النظام أهم الأمثلة هي `StudentClasses` بين الطلاب والفصول، و`UserRoles` بين المستخدمين والأدوار، و`RolePermissions` بين الأدوار والصلاحيات، و`UserPermissions` بين المستخدمين والصلاحيات. كل صف في الجدول الوسيط يربط سجلاً واحداً من الطرف الأول بسجل واحد من الطرف الثاني، فتتكون علاقة N:M من مجموع علاقتي 1:N.

## ملاحظات تصميمية مهمة

1. `AuditLogs.UserID` وحقول `CreatedByUserID` مصممة للحفاظ على السجل التاريخي عند حذف المستخدم، ولذلك يفضل `ON DELETE SET NULL`.
2. `Vouchers.ReferenceID` حقل مرجعي متعدد الاستخدامات يعتمد على `ReferenceType`، ولا يصح ربطه بمفتاح خارجي واحد إلى جدول محدد.
3. `Classes.RoomID` علاقة اختيارية؛ الصف قد يُنشأ قبل تخصيص القاعة.
4. `Enrollments.ClassID` قد يكون اختيارياً في مرحلة القبول الأولى ثم يُملأ عند التوزيع.
5. يجب تشغيل `Databass/Verify_SchemaIntegrity.sql` على قاعدة `SchoolDB` الفعلية للتحقق من العلاقات والبيانات اليتيمة، لأن ملفات SQL تصف المخطط ولا تعرض حالة البيانات الحالية.

## مخطط Mermaid

```mermaid
erDiagram
    UserPermissions {
        INT UserPermissionID PK 'INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_UserPermissions PRIMARY KEY'
        INT UserID FK 'INT NOT NULL'
        INT PermissionID FK 'INT NOT NULL'
        DATETIME GrantedAt  'DATETIME2 NOT NULL CONSTRAINT DF_UserPermissions_GrantedAt DEFAULT GETDATE()'
        INT GrantedBy  'INT NULL'
    }
    Users {
        NVARCHAR FullName  'NVARCHAR(200) NULL'
        NVARCHAR PasswordHash  'NVARCHAR(MAX) NULL'
        NVARCHAR PasswordSalt  'NVARCHAR(MAX) NULL'
        NVARCHAR RoleName  'NVARCHAR(50) NULL'
        NVARCHAR Permissions  'NVARCHAR(MAX) NULL'
        NVARCHAR Email  'NVARCHAR(150) NULL'
        NVARCHAR Phone  'NVARCHAR(30) NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1 WITH VALUES'
        BIT MustChangePassword  'BIT NOT NULL CONSTRAINT DF_Users_MustChangePassword DEFAULT 0 WITH VALUES'
        INT FailedLoginAttempts  'INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT 0 WITH VALUES'
        DATETIME LockedAt  'DATETIME NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT GETDATE() WITH VALUES'
        DATETIME LastLoginAt  'DATETIME NULL'
        DATETIME UpdatedAt  'DATETIME NULL'
        INT UserID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        NVARCHAR UserName  'NVARCHAR(50) NOT NULL'
        NVARCHAR Password  'NVARCHAR(100) NOT NULL'
        NVARCHAR Role  'NVARCHAR(20) NOT NULL'
    }
    Permissions {
        INT PermissionID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Permissions PRIMARY KEY'
        NVARCHAR PermissionKey  'NVARCHAR(150) NOT NULL'
        NVARCHAR DisplayName  'NVARCHAR(250) NULL'
        NVARCHAR ModuleName  'NVARCHAR(100) NULL'
        NVARCHAR ActionName  'NVARCHAR(100) NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Permissions_IsActive DEFAULT 1'
        DATETIME CreatedAt  'DATETIME2(0) NOT NULL CONSTRAINT DF_Permissions_CreatedAt DEFAULT SYSUTCDATETIME'
    }
    StudentClasses {
        AS AcademicYearKey  'AS'
        NVARCHAR Section  'NVARCHAR(50) NULL'
        DATETIME AssignedDate  'DATETIME NOT NULL CONSTRAINT DF_StudentClasses_AssignedDate_Compat DEFAULT GETDA'
        INT AssignedBy  'INT NULL'
        INT StudentClassID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        INT StudentID FK 'INT NOT NULL'
        INT ClassID FK 'INT NOT NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NULL'
    }
    StudentAttendance {
        AS AttendanceDay  'AS (CONVERT(date, AttendanceDate)) PERSISTED'
        TIME DepartureTime  'TIME NULL'
        NVARCHAR AbsenceReason  'NVARCHAR(500) NULL'
        INT AttendanceID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentAttendance PRIMARY KEY'
        INT StudentID FK 'INT NOT NULL'
        INT ClassID FK 'INT NOT NULL'
        NVARCHAR Section  'NVARCHAR(50) NOT NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        DATE AttendanceDate  'DATE NOT NULL'
        NVARCHAR Status  'NVARCHAR(30) NOT NULL CONSTRAINT DF_StudentAttendance_Status DEFAULT (N'حاضر')'
        TIME ArrivalTime  'TIME NULL'
        NVARCHAR ExcuseStatus  'NVARCHAR(50) NULL'
        NVARCHAR Notes  'NVARCHAR(500) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_StudentAttendance_CreatedAt DEFAULT (GETDATE())'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    TeacherAttendance {
        AS AttendanceDay  'AS (CONVERT(date, AttendanceDate)) PERSISTED'
        INT AttendanceID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherAttendance PRIMARY KEY'
        INT TeacherID  'INT NOT NULL'
        DATE AttendanceDate  'DATE NOT NULL'
        NVARCHAR Status  'NVARCHAR(30) NOT NULL'
        INT LateMinutes  'INT NOT NULL CONSTRAINT DF_TeacherAttendance_LateMinutes DEFAULT 0'
        INT EarlyLeaveMinutes  'INT NOT NULL CONSTRAINT DF_TeacherAttendance_EarlyLeaveMinutes DEFAULT 0'
        DECIMAL WorkHours  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_TeacherAttendance_WorkHours DEFAULT 0'
        NVARCHAR AbsenceReason  'NVARCHAR(300) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME RecordedAt  'DATETIME NOT NULL CONSTRAINT DF_TeacherAttendance_RecordedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
        TIME CheckInTime  'TIME NULL'
        TIME CheckOutTime  'TIME NULL'
    }
    SchoolSections {
        INT Capacity  'INT NULL'
        NVARCHAR AllowedGender  'NVARCHAR(20) NULL'
        CK CONSTRAINT  'CK_SchoolSections_Capacity'
        INT SectionID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolSections PRIMARY KEY'
        INT ClassID FK 'INT NOT NULL'
        NVARCHAR SectionName  'NVARCHAR(50) NOT NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_SchoolSections_IsActive DEFAULT (1)'
        DATETIME CreatedAt  'DATETIME2(0) NOT NULL CONSTRAINT DF_SchoolSections_CreatedAt DEFAULT (SYSDATETIM'
    }
    AuditLogs {
        BIGINT AuditLogID PK 'BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY'
        INT UserID FK 'INT NULL'
        NVARCHAR UserName  'NVARCHAR(150) NULL'
        NVARCHAR ActionName  'NVARCHAR(100) NOT NULL'
        NVARCHAR EntityName  'NVARCHAR(100) NULL'
        NVARCHAR EntityID  'NVARCHAR(100) NULL'
        NVARCHAR Details  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_AuditLogs_CreatedAt DEFAULT(GETDATE())'
        NVARCHAR Module  'NVARCHAR(100) NULL'
        NVARCHAR MachineName  'NVARCHAR(150) NULL'
        NVARCHAR IpAddress  'NVARCHAR(64) NULL'
    }
    Rooms {
        INT CreatedByUserID FK 'INT NULL'
        INT RoomID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY'
        NVARCHAR RoomCode  'NVARCHAR(30) NULL'
        NVARCHAR RoomName  'NVARCHAR(100) NOT NULL'
        NVARCHAR RoomType  'NVARCHAR(50) NULL'
        INT Capacity  'INT NOT NULL CONSTRAINT DF_Rooms_Capacity DEFAULT 0'
        NVARCHAR Location  'NVARCHAR(200) NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT 1'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Rooms_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Classes {
        INT RoomID FK 'INT NULL'
        INT ClassID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Classes PRIMARY KEY'
        NVARCHAR ClassCode  'NVARCHAR(30) NULL'
        NVARCHAR ClassName  'NVARCHAR(100) NOT NULL'
        NVARCHAR StageName  'NVARCHAR(100) NULL'
        INT GradeOrder  'INT NOT NULL CONSTRAINT DF_Classes_GradeOrder DEFAULT 0'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Classes_IsActive DEFAULT 1'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Classes_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Expenses {
        INT CreatedByUserID FK 'INT NULL'
        INT ExpenseID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Expenses PRIMARY KEY'
        NVARCHAR ExpenseNumber  'NVARCHAR(50) NULL'
        DECIMAL Amount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Expenses_Amount DEFAULT 0'
        DATE ExpenseDate  'DATE NOT NULL'
        NVARCHAR Category  'NVARCHAR(100) NULL'
        NVARCHAR PayeeName  'NVARCHAR(200) NULL'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR Description  'NVARCHAR(500) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Expenses_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Vouchers {
        INT CreatedByUserID FK 'INT NULL'
        INT VoucherID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Vouchers PRIMARY KEY'
        NVARCHAR VoucherNumber  'NVARCHAR(50) NULL'
        NVARCHAR VoucherType  'NVARCHAR(30) NOT NULL'
        DECIMAL Amount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Vouchers_Amount DEFAULT 0'
        DATE VoucherDate  'DATE NOT NULL'
        NVARCHAR PartyName  'NVARCHAR(200) NULL'
        NVARCHAR Description  'NVARCHAR(500) NULL'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR ReferenceType  'NVARCHAR(50) NULL'
        INT ReferenceID  'INT NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        BIT IsAutoGenerated  'BIT NOT NULL CONSTRAINT DF_Vouchers_IsAutoGenerated DEFAULT 0'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Vouchers_CreatedAt DEFAULT GETDATE()'
    }
    Enrollments {
        INT EnrollmentID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Enrollments PRIMARY KEY'
        INT StudentID FK 'INT NOT NULL'
        DATE ApplicationDate  'DATE NOT NULL CONSTRAINT DF_Enrollments_ApplicationDate DEFAULT CONVERT(date, GE'
        NVARCHAR ApplicationType  'NVARCHAR(50) NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        INT ClassID FK 'INT NOT NULL'
        NVARCHAR Section  'NVARCHAR(50) NULL'
        NVARCHAR SeatNumber  'NVARCHAR(20) NULL'
        NVARCHAR Status  'NVARCHAR(50) NOT NULL CONSTRAINT DF_Enrollments_Status DEFAULT N'جديد''
        NVARCHAR PreviousSchool  'NVARCHAR(200) NULL'
        NVARCHAR PreviousClass  'NVARCHAR(50) NULL'
        NVARCHAR TransferReason  'NVARCHAR(MAX) NULL'
        DECIMAL RegistrationFee  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_RegistrationFee DEFAULT 0'
        DECIMAL PaidAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Enrollments_PaidAmount DEFAULT 0'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR ReceiptNo  'NVARCHAR(50) NULL'
        BIT HasBirthCertificate  'BIT NOT NULL CONSTRAINT DF_Enrollments_HasBirthCertificate DEFAULT 0'
        BIT HasGuardianId  'BIT NOT NULL CONSTRAINT DF_Enrollments_HasGuardianId DEFAULT 0'
        BIT HasPhoto  'BIT NOT NULL CONSTRAINT DF_Enrollments_HasPhoto DEFAULT 0'
        BIT HasLastCertificate  'BIT NOT NULL CONSTRAINT DF_Enrollments_HasLastCertificate DEFAULT 0'
        BIT HasMedicalReport  'BIT NOT NULL CONSTRAINT DF_Enrollments_HasMedicalReport DEFAULT 0'
        NVARCHAR GeneralNotes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Enrollments_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Students {
        INT ClassID  'INT NULL'
        NVARCHAR Section  'NVARCHAR(50) NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NULL'
        NVARCHAR Phone  'NVARCHAR(30) NULL'
        NVARCHAR StudentNumber  'NVARCHAR(30) NULL'
        NVARCHAR FullName  'NVARCHAR(200) NULL'
        NVARCHAR BirthPlace  'NVARCHAR(100) NULL'
        NVARCHAR Nationality  'NVARCHAR(100) NULL'
        NVARCHAR NationalId  'NVARCHAR(50) NULL'
        NVARCHAR StudentPhone  'NVARCHAR(30) NULL'
        NVARCHAR Status  'NVARCHAR(30) NOT NULL CONSTRAINT DF_Students_Status DEFAULT N'نشط' WITH VALUES'
        NVARCHAR GuardianName  'NVARCHAR(200) NULL'
        NVARCHAR GuardianRelation  'NVARCHAR(50) NULL'
        NVARCHAR GuardianPhone  'NVARCHAR(30) NULL'
        NVARCHAR GuardianEmail  'NVARCHAR(150) NULL'
        NVARCHAR GuardianJob  'NVARCHAR(100) NULL'
        NVARCHAR Governorate  'NVARCHAR(100) NULL'
        NVARCHAR District  'NVARCHAR(100) NULL'
        NVARCHAR Address  'NVARCHAR(300) NULL'
        VARBINARY Photo  'VARBINARY(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Students_CreatedAt DEFAULT GETDATE() WITH VALUES'
        DATETIME UpdatedAt  'DATETIME NULL'
        INT StudentID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        NVARCHAR Gender  'NVARCHAR(20) NULL'
        DATE BirthDate  'DATE NULL'
        NVARCHAR StudentName  'NVARCHAR(100) NOT NULL'
    }
    Grades {
        INT GradeID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades PRIMARY KEY'
        INT StudentID  'INT NULL'
        INT SubjectID  'INT NULL'
        INT ClassID  'INT NULL'
        NVARCHAR Section  'NVARCHAR(100) NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NULL'
        NVARCHAR TermName  'NVARCHAR(50) NULL'
        DECIMAL Quiz1  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz1 DEFAULT (0)'
        DECIMAL Quiz2  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Quiz2 DEFAULT (0)'
        DECIMAL CourseWork  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_CourseWork DEFAULT (0)'
        DECIMAL FinalExam  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_FinalExam DEFAULT (0)'
        DECIMAL GradeValue  'DECIMAL(10,2) NULL'
        NVARCHAR GradeLetter  'NVARCHAR(50) NULL'
        NVARCHAR ResultStatus  'NVARCHAR(50) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Grades_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Books {
        INT BookID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Books PRIMARY KEY'
        NVARCHAR Title  'NVARCHAR(200) NOT NULL'
        NVARCHAR Author  'NVARCHAR(200) NULL'
        NVARCHAR ISBN  'NVARCHAR(50) NULL'
        NVARCHAR Category  'NVARCHAR(100) NULL'
        NVARCHAR Publisher  'NVARCHAR(200) NULL'
        INT PublicationYear  'INT NULL'
        INT Copies  'INT NOT NULL CONSTRAINT DF_Books_Copies DEFAULT 0'
        INT AvailableCopies  'INT NOT NULL CONSTRAINT DF_Books_AvailableCopies DEFAULT 0'
        NVARCHAR ShelfLocation  'NVARCHAR(100) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Books_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Books_IsActive DEFAULT 1 WITH VALUES'
    }
    BookBorrowings {
        INT BorrowingID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BookBorrowings PRIMARY KEY'
        INT BookID FK 'INT NOT NULL'
        NVARCHAR BorrowerType  'NVARCHAR(20) NOT NULL'
        INT BorrowerID  'INT NOT NULL'
        DATE BorrowDate  'DATE NOT NULL'
        DATE DueDate  'DATE NOT NULL'
        DATE ReturnDate  'DATE NULL'
        NVARCHAR Status  'NVARCHAR(30) NOT NULL CONSTRAINT DF_BookBorrowings_Status DEFAULT N'معار''
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_BookBorrowings_CreatedAt DEFAULT GETDATE() WITH '
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    SchoolTimetable {
        INT TimetableID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SchoolTimetable PRIMARY KEY'
        INT ClassID  'INT NOT NULL'
        NVARCHAR Section  'NVARCHAR(50) NULL'
        INT SubjectID  'INT NOT NULL'
        INT TeacherID  'INT NOT NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        NVARCHAR TermName  'NVARCHAR(50) NULL'
        NVARCHAR DayName  'NVARCHAR(30) NOT NULL'
        INT PeriodNo  'INT NOT NULL'
        TIME StartTime  'TIME NOT NULL CONSTRAINT DF_SchoolTimetable_StartTime DEFAULT '08:00''
        TIME EndTime  'TIME NOT NULL CONSTRAINT DF_SchoolTimetable_EndTime DEFAULT '08:45''
        NVARCHAR RoomName  'NVARCHAR(100) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_SchoolTimetable_IsActive DEFAULT 1'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_SchoolTimetable_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    TeacherContracts {
        INT ContractID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TeacherContracts PRIMARY KEY'
        INT TeacherID  'INT NOT NULL'
        NVARCHAR ContractNumber  'NVARCHAR(50) NULL'
        NVARCHAR ContractType  'NVARCHAR(50) NULL'
        NVARCHAR ContractStatus  'NVARCHAR(30) NULL'
        DECIMAL BasicSalary  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_BasicSalary DEFAULT 0'
        DECIMAL HousingAllowance  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_HousingAllowance DEFAULT 0'
        DECIMAL TransportAllowance  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_TransportAllowance DEFAULT'
        DECIMAL OtherAllowances  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_OtherAllowances DEFAULT 0'
        DECIMAL Deductions  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_TeacherContracts_Deductions DEFAULT 0'
        AS TotalSalary  'AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances)'
        AS NetSalary  'AS (BasicSalary + HousingAllowance + TransportAllowance + OtherAllowances - Dedu'
        DATE StartDate  'DATE NOT NULL CONSTRAINT DF_TeacherContracts_StartDate DEFAULT CONVERT(date, GET'
        DATE EndDate  'DATE NULL'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_TeacherContracts_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    FeePlans {
        INT FeePlanID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeePlans PRIMARY KEY'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        INT ClassID  'INT NOT NULL'
        NVARCHAR FeeType  'NVARCHAR(100) NOT NULL'
        DECIMAL Amount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeePlans_Amount DEFAULT 0'
        DATE DueDate  'DATE NOT NULL'
        BIT IsRequired  'BIT NOT NULL CONSTRAINT DF_FeePlans_IsRequired DEFAULT 1'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_FeePlans_CreatedAt DEFAULT GETDATE()'
    }
    Fees {
        INT FeeID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Fees PRIMARY KEY'
        INT StudentID  'INT NOT NULL'
        INT FeePlanID  'INT NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NOT NULL'
        NVARCHAR FeeType  'NVARCHAR(100) NOT NULL'
        DECIMAL TotalAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_TotalAmount DEFAULT 0'
        DECIMAL DiscountAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_DiscountAmount DEFAULT 0'
        DECIMAL NetAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_NetAmount DEFAULT 0'
        DECIMAL PaidAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_PaidAmount DEFAULT 0'
        DECIMAL RemainingAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Fees_RemainingAmount DEFAULT 0'
        DATE DueDate  'DATE NOT NULL'
        DATE PaymentDate  'DATE NULL'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR ReceiptNumber  'NVARCHAR(50) NULL'
        NVARCHAR Status  'NVARCHAR(30) NOT NULL CONSTRAINT DF_Fees_Status DEFAULT N'غير مدفوع''
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Fees_CreatedAt DEFAULT GETDATE()'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Payroll {
        INT PayrollID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payroll PRIMARY KEY'
        INT TeacherID  'INT NOT NULL'
        INT SalaryMonth  'INT NOT NULL'
        INT SalaryYear  'INT NOT NULL'
        DECIMAL BasicSalary  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_BasicSalary DEFAULT 0'
        DECIMAL Allowances  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Allowances DEFAULT 0'
        DECIMAL Deductions  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Payroll_Deductions DEFAULT 0'
        AS NetSalary  'AS (BasicSalary + Allowances - Deductions)'
        DATE PaymentDate  'DATE NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Payroll_CreatedAt DEFAULT GETDATE()'
    }
    Receipts {
        INT ReceiptID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Receipts PRIMARY KEY'
        NVARCHAR ReceiptNumber  'NVARCHAR(50) NULL'
        INT StudentID  'INT NULL'
        DECIMAL Amount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_Receipts_Amount DEFAULT 0'
        DATE ReceiptDate  'DATE NOT NULL'
        NVARCHAR PaymentMethod  'NVARCHAR(50) NULL'
        NVARCHAR Description  'NVARCHAR(500) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Receipts_CreatedAt DEFAULT GETDATE()'
    }
    StudentFees {
        INT StudentFeeID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentFees PRIMARY KEY'
        INT StudentID  'INT NOT NULL'
        NVARCHAR FeeType  'NVARCHAR(100) NULL'
        NVARCHAR AcademicYear  'NVARCHAR(20) NULL'
        DECIMAL Amount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_Amount DEFAULT 0'
        DECIMAL PaidAmount  'DECIMAL(18,2) NOT NULL CONSTRAINT DF_StudentFees_PaidAmount DEFAULT 0'
        NVARCHAR Status  'NVARCHAR(30) NULL'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_StudentFees_CreatedAt DEFAULT GETDATE()'
    }
    Subjects {
        NVARCHAR SubjectCode  'NVARCHAR(30) NULL'
        INT ClassID  'INT NULL'
        DECIMAL MaxDegree  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_MaxDegree DEFAULT 100 WITH VALUES'
        DECIMAL PassDegree  'DECIMAL(10,2) NOT NULL CONSTRAINT DF_Subjects_PassDegree DEFAULT 50 WITH VALUES'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Subjects_IsActive DEFAULT 1 WITH VALUES'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Subjects_CreatedAt DEFAULT GETDATE() WITH VALUES'
        DATETIME UpdatedAt  'DATETIME NULL'
        INT SubjectID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        NVARCHAR SubjectName  'NVARCHAR(100) NOT NULL'
    }
    Buses {
        INT BusID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Buses PRIMARY KEY'
        NVARCHAR BusNumber  'NVARCHAR(50) NOT NULL'
        NVARCHAR DriverName  'NVARCHAR(150) NULL'
        NVARCHAR DriverPhone  'NVARCHAR(50) NULL'
        INT Capacity  'INT NOT NULL CONSTRAINT DF_Buses_Capacity DEFAULT (0)'
        NVARCHAR Notes  'NVARCHAR(500) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Buses_CreatedAt DEFAULT (GETDATE())'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    BusRoutes {
        INT RouteID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BusRoutes PRIMARY KEY'
        NVARCHAR RouteName  'NVARCHAR(150) NOT NULL'
        INT BusID FK 'INT NOT NULL'
        NVARCHAR StartPoint  'NVARCHAR(200) NULL'
        NVARCHAR EndPoint  'NVARCHAR(200) NULL'
        TIME DepartureTime  'TIME NULL'
        TIME ArrivalTime  'TIME NULL'
        DECIMAL Fee  'DECIMAL(18,2) NULL'
        NVARCHAR Notes  'NVARCHAR(500) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_BusRoutes_CreatedAt DEFAULT (GETDATE())'
        DATETIME UpdatedAt  'DATETIME NULL'
    }
    Roles {
        INT RoleID PK 'INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY'
        NVARCHAR RoleName  'NVARCHAR(100) NOT NULL'
        NVARCHAR Description  'NVARCHAR(500) NULL'
        BIT IsSystemRole  'BIT NOT NULL CONSTRAINT DF_Roles_IsSystemRole DEFAULT 0'
        BIT IsActive  'BIT NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT 1'
        DATETIME CreatedAt  'DATETIME2(0) NOT NULL CONSTRAINT DF_Roles_CreatedAt DEFAULT SYSUTCDATETIME()'
        DATETIME UpdatedAt  'DATETIME2(0) NULL'
    }
    UserRoles {
        INT UserID FK 'INT NOT NULL'
        INT RoleID FK 'INT NOT NULL'
        DATETIME AssignedAt  'DATETIME2(0) NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT SYSUTCDATETIME('
        INT AssignedBy  'INT NULL'
    }
    RolePermissions {
        INT RoleID FK 'INT NOT NULL'
        INT PermissionID FK 'INT NOT NULL'
        DATETIME GrantedAt  'DATETIME2(0) NOT NULL CONSTRAINT DF_RolePermissions_GrantedAt DEFAULT SYSUTCDATE'
        INT GrantedBy  'INT NULL'
    }
    Teachers {
        NVARCHAR FullName  'NVARCHAR(200) NULL'
        NVARCHAR EmployeeNumber  'NVARCHAR(50) NULL'
        NVARCHAR Gender  'NVARCHAR(10) NULL'
        DATE BirthDate  'DATE NULL'
        NVARCHAR BirthPlace  'NVARCHAR(100) NULL'
        NVARCHAR Nationality  'NVARCHAR(100) NULL'
        NVARCHAR Phone  'NVARCHAR(30) NULL'
        NVARCHAR NationalID  'NVARCHAR(50) NULL'
        NVARCHAR Email  'NVARCHAR(150) NULL'
        NVARCHAR Qualification  'NVARCHAR(100) NULL'
        NVARCHAR Specialization  'NVARCHAR(100) NULL'
        NVARCHAR Address  'NVARCHAR(300) NULL'
        DATE HireDate  'DATE NULL'
        DECIMAL BasicSalary  'DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_BasicSalary DEFAULT 0 WITH VALUES'
        DECIMAL TransportAllowance  'DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_TransportAllowance DEFAULT 0 WITH'
        DECIMAL HousingAllowance  'DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Teachers_HousingAllowance DEFAULT 0 WITH V'
        NVARCHAR Status  'NVARCHAR(50) NOT NULL CONSTRAINT DF_Teachers_Status DEFAULT N'نشط' WITH VALUES'
        NVARCHAR Notes  'NVARCHAR(MAX) NULL'
        DATETIME CreatedAt  'DATETIME NOT NULL CONSTRAINT DF_Teachers_CreatedAt DEFAULT GETDATE() WITH VALUES'
        DATETIME UpdatedAt  'DATETIME NULL'
        INT TeacherID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        NVARCHAR TeacherName  'NVARCHAR(100) NOT NULL'
    }
    Marks {
        INT MarkID PK 'INT IDENTITY(1,1) PRIMARY KEY'
        INT StudentID FK 'INT NOT NULL'
        INT SubjectID FK 'INT NOT NULL'
        INT TeacherID FK 'INT NULL'
        DECIMAL Mark  'DECIMAL(5,2) NOT NULL'
        NVARCHAR ExamType  'NVARCHAR(50) NULL'
        DATETIME CreatedAt  'DATETIME DEFAULT GETDATE()'
    }
    Users ||--o{ UserPermissions : "UserID -> UserID"
    Permissions ||--o{ UserPermissions : "PermissionID -> PermissionID"
    Students ||--o{ Enrollments : "StudentID -> StudentID"
    Classes ||--o{ Enrollments : "ClassID -> ClassID"
    Rooms ||--o{ Classes : "RoomID -> RoomID"
    Users ||--o{ AuditLogs : "UserID -> UserID"
    Users ||--o{ Rooms : "UserID -> CreatedByUserID"
    Users ||--o{ Expenses : "UserID -> CreatedByUserID"
    Users ||--o{ Vouchers : "UserID -> CreatedByUserID"
    Classes ||--o{ SchoolSections : "ClassID -> ClassID"
    Books ||--o{ BookBorrowings : "BookID -> BookID"
    Buses ||--o{ BusRoutes : "BusID -> BusID"
    Students ||--o{ StudentAttendance : "StudentID -> StudentID"
    Classes ||--o{ StudentAttendance : "ClassID -> ClassID"
    Users ||--o{ UserRoles : "UserID -> UserID"
    Roles ||--o{ UserRoles : "RoleID -> RoleID"
    Roles ||--o{ RolePermissions : "RoleID -> RoleID"
    Permissions ||--o{ RolePermissions : "PermissionID -> PermissionID"
    Students ||--o{ StudentClasses : "StudentID -> StudentID"
    Classes ||--o{ StudentClasses : "ClassID -> ClassID"
    Students ||--o{ Marks : "StudentID -> StudentID"
    Subjects ||--o{ Marks : "SubjectID -> SubjectID"
    Teachers ||--o{ Marks : "TeacherID -> TeacherID"
```
