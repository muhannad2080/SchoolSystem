/*
    SchoolSystem - Operational tables hardening
    Creates the tables required by TransportForm and DailyAttendanceForm.
    Safe to run repeatedly on an existing SQL Server database.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
BEGIN
    THROW 50001, N'قاعدة SchoolDB غير موجودة. شغّل SchoolDB.SQL للإنشاء من الصفر أولاً.', 1;
END;
GO

USE SchoolDB;
GO

BEGIN TRANSACTION;

/* =========================
   Buses
   ========================= */
IF OBJECT_ID(N'dbo.Buses', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Buses
    (
        BusID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Buses PRIMARY KEY,
        BusNumber NVARCHAR(50) NOT NULL,
        DriverName NVARCHAR(150) NULL,
        DriverPhone NVARCHAR(50) NULL,
        Capacity INT NOT NULL CONSTRAINT DF_Buses_Capacity DEFAULT (0),
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_Buses_CreatedAt DEFAULT (GETDATE()),
        UpdatedAt DATETIME NULL
    );
END;
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.Buses', N'BusNumber') IS NULL
        ALTER TABLE dbo.Buses ADD BusNumber NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Buses', N'DriverName') IS NULL
        ALTER TABLE dbo.Buses ADD DriverName NVARCHAR(150) NULL;
    IF COL_LENGTH(N'dbo.Buses', N'DriverPhone') IS NULL
        ALTER TABLE dbo.Buses ADD DriverPhone NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.Buses', N'Capacity') IS NULL
        ALTER TABLE dbo.Buses ADD Capacity INT NULL;
    IF COL_LENGTH(N'dbo.Buses', N'Notes') IS NULL
        ALTER TABLE dbo.Buses ADD Notes NVARCHAR(500) NULL;
    IF COL_LENGTH(N'dbo.Buses', N'CreatedAt') IS NULL
        ALTER TABLE dbo.Buses ADD CreatedAt DATETIME NULL;
    IF COL_LENGTH(N'dbo.Buses', N'UpdatedAt') IS NULL
        ALTER TABLE dbo.Buses ADD UpdatedAt DATETIME NULL;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Buses')
      AND name = N'UX_Buses_BusNumber'
)
BEGIN
    CREATE UNIQUE INDEX UX_Buses_BusNumber ON dbo.Buses(BusNumber);
END;

/* =========================
   BusRoutes
   ========================= */
IF OBJECT_ID(N'dbo.BusRoutes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BusRoutes
    (
        RouteID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BusRoutes PRIMARY KEY,
        RouteName NVARCHAR(150) NOT NULL,
        BusID INT NOT NULL,
        StartPoint NVARCHAR(200) NULL,
        EndPoint NVARCHAR(200) NULL,
        DepartureTime TIME NULL,
        ArrivalTime TIME NULL,
        Fee DECIMAL(18,2) NULL,
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_BusRoutes_CreatedAt DEFAULT (GETDATE()),
        UpdatedAt DATETIME NULL
    );
END;
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.BusRoutes', N'RouteName') IS NULL
        ALTER TABLE dbo.BusRoutes ADD RouteName NVARCHAR(150) NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'BusID') IS NULL
        ALTER TABLE dbo.BusRoutes ADD BusID INT NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'StartPoint') IS NULL
        ALTER TABLE dbo.BusRoutes ADD StartPoint NVARCHAR(200) NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'EndPoint') IS NULL
        ALTER TABLE dbo.BusRoutes ADD EndPoint NVARCHAR(200) NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'DepartureTime') IS NULL
        ALTER TABLE dbo.BusRoutes ADD DepartureTime TIME NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'ArrivalTime') IS NULL
        ALTER TABLE dbo.BusRoutes ADD ArrivalTime TIME NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'Fee') IS NULL
        ALTER TABLE dbo.BusRoutes ADD Fee DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'Notes') IS NULL
        ALTER TABLE dbo.BusRoutes ADD Notes NVARCHAR(500) NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'CreatedAt') IS NULL
        ALTER TABLE dbo.BusRoutes ADD CreatedAt DATETIME NULL;
    IF COL_LENGTH(N'dbo.BusRoutes', N'UpdatedAt') IS NULL
        ALTER TABLE dbo.BusRoutes ADD UpdatedAt DATETIME NULL;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_BusRoutes_Buses'
      AND parent_object_id = OBJECT_ID(N'dbo.BusRoutes')
)
AND OBJECT_ID(N'dbo.Buses', N'U') IS NOT NULL
BEGIN
    ALTER TABLE dbo.BusRoutes WITH NOCHECK
        ADD CONSTRAINT FK_BusRoutes_Buses
        FOREIGN KEY (BusID) REFERENCES dbo.Buses(BusID);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.BusRoutes')
      AND name = N'IX_BusRoutes_BusID'
)
BEGIN
    CREATE INDEX IX_BusRoutes_BusID ON dbo.BusRoutes(BusID);
END;

/* =========================
   StudentAttendance
   ========================= */
IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentAttendance
    (
        AttendanceID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentAttendance PRIMARY KEY,
        StudentID INT NOT NULL,
        ClassID INT NOT NULL,
        Section NVARCHAR(50) NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        AttendanceDate DATE NOT NULL,
        Status NVARCHAR(30) NOT NULL CONSTRAINT DF_StudentAttendance_Status DEFAULT (N'حاضر'),
        ArrivalTime TIME NULL,
        ExcuseStatus NVARCHAR(50) NULL,
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_StudentAttendance_CreatedAt DEFAULT (GETDATE()),
        UpdatedAt DATETIME NULL
    );
END;
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.StudentAttendance', N'StudentID') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD StudentID INT NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'ClassID') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD ClassID INT NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'Section') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD Section NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'AcademicYear') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD AcademicYear NVARCHAR(20) NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'AttendanceDate') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD AttendanceDate DATE NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'Status') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD Status NVARCHAR(30) NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'ArrivalTime') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD ArrivalTime TIME NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'ExcuseStatus') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD ExcuseStatus NVARCHAR(50) NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'Notes') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD Notes NVARCHAR(500) NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'CreatedAt') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD CreatedAt DATETIME NULL;
    IF COL_LENGTH(N'dbo.StudentAttendance', N'UpdatedAt') IS NULL
        ALTER TABLE dbo.StudentAttendance ADD UpdatedAt DATETIME NULL;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.StudentAttendance')
      AND name = N'UX_StudentAttendance_StudentDate'
)
BEGIN
    CREATE UNIQUE INDEX UX_StudentAttendance_StudentDate
        ON dbo.StudentAttendance(StudentID, AttendanceDate);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_StudentAttendance_Students'
      AND parent_object_id = OBJECT_ID(N'dbo.StudentAttendance')
)
AND OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
BEGIN
    ALTER TABLE dbo.StudentAttendance WITH NOCHECK
        ADD CONSTRAINT FK_StudentAttendance_Students
        FOREIGN KEY (StudentID) REFERENCES dbo.Students(StudentID);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_StudentAttendance_Classes'
      AND parent_object_id = OBJECT_ID(N'dbo.StudentAttendance')
)
AND OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL
BEGIN
    ALTER TABLE dbo.StudentAttendance WITH NOCHECK
        ADD CONSTRAINT FK_StudentAttendance_Classes
        FOREIGN KEY (ClassID) REFERENCES dbo.Classes(ClassID);
END;

COMMIT TRANSACTION;
