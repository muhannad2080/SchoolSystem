/*
    Migration 013
    إدارة ربط الطلاب بمسارات النقل والحافلات.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51410, N'جدول Students غير موجود؛ لا يمكن تطبيق Migration 013.', 1;

IF OBJECT_ID(N'dbo.BusRoutes', N'U') IS NULL
    THROW 51411, N'جدول BusRoutes غير موجود؛ لا يمكن تطبيق Migration 013.', 1;

IF OBJECT_ID(N'dbo.StudentTransportAssignments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentTransportAssignments
    (
        AssignmentID INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_StudentTransportAssignments PRIMARY KEY,
        StudentID INT NOT NULL,
        RouteID INT NOT NULL,
        AcademicYear NVARCHAR(20) NOT NULL,
        PickupPoint NVARCHAR(200) NOT NULL,
        DropoffPoint NVARCHAR(200) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_StudentTransportAssignments_IsActive DEFAULT (1),
        StartDate DATE NOT NULL CONSTRAINT DF_StudentTransportAssignments_StartDate DEFAULT (GETDATE()),
        EndDate DATE NULL,
        Notes NVARCHAR(1000) NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_StudentTransportAssignments_CreatedAt DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL
    );
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_StudentTransportAssignments_Students'
)
BEGIN
    ALTER TABLE dbo.StudentTransportAssignments
    ADD CONSTRAINT FK_StudentTransportAssignments_Students
        FOREIGN KEY (StudentID) REFERENCES dbo.Students(StudentID);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_StudentTransportAssignments_BusRoutes'
)
BEGIN
    ALTER TABLE dbo.StudentTransportAssignments
    ADD CONSTRAINT FK_StudentTransportAssignments_BusRoutes
        FOREIGN KEY (RouteID) REFERENCES dbo.BusRoutes(RouteID);
END;

IF EXISTS
(
    SELECT StudentID, AcademicYear
    FROM dbo.StudentTransportAssignments
    WHERE IsActive = 1
    GROUP BY StudentID, AcademicYear
    HAVING COUNT(*) > 1
)
    THROW 51412, N'توجد اشتراكات نقل نشطة مكررة للطالب في نفس العام الدراسي.', 1;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.StudentTransportAssignments')
      AND name = N'UX_StudentTransportAssignments_StudentYear_Active'
)
BEGIN
    CREATE UNIQUE INDEX UX_StudentTransportAssignments_StudentYear_Active
        ON dbo.StudentTransportAssignments(StudentID, AcademicYear)
        WHERE IsActive = 1;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.StudentTransportAssignments')
      AND name = N'IX_StudentTransportAssignments_RouteYear'
)
BEGIN
    CREATE INDEX IX_StudentTransportAssignments_RouteYear
        ON dbo.StudentTransportAssignments(RouteID, AcademicYear, IsActive);
END;
