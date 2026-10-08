IF DB_ID(N'SchoolDB') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE [SchoolDB]');
END
GO
USE [SchoolDB]
GO
/****** Object:  UserDefinedFunction [dbo].[GetActiveAcademicYear]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[GetActiveAcademicYear]()
RETURNS NVARCHAR(20)
AS
BEGIN
    DECLARE @result NVARCHAR(20);
    SELECT TOP 1 @result = AcademicYear FROM dbo.AcademicYearCurrent WHERE IsCurrent = 1;
    RETURN @result;
END;

GO
/****** Object:  Table [dbo].[AcademicTerms]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicTerms](
	[TermID] [int] IDENTITY(1,1) NOT NULL,
	[TermCode] [nvarchar](30) NOT NULL,
	[TermName] [nvarchar](100) NOT NULL,
	[TermOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_AcademicTerms] PRIMARY KEY CLUSTERED 
(
	[TermID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AcademicYearCurrent]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicYearCurrent](
	[AcademicYear] [nvarchar](20) NOT NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[IsCurrent] [bit] NOT NULL,
	[Notes] [nvarchar](200) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AcademicYear] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AcademicYears]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicYears](
	[AcademicYearID] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_AcademicYears] PRIMARY KEY CLUSTERED 
(
	[AcademicYearID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnnualClosings]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnnualClosings](
	[ClosingID] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[ClosingStatus] [nvarchar](20) NOT NULL,
	[VerifiedAt] [datetime] NULL,
	[ClosedAt] [datetime] NULL,
	[ClosedByUserID] [int] NULL,
	[NextAcademicYear] [nvarchar](20) NULL,
	[Notes] [nvarchar](1000) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_AnnualClosings] PRIMARY KEY CLUSTERED 
(
	[ClosingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnnualMigrationLog]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnnualMigrationLog](
	[MigrationID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[FromAcademicYear] [nvarchar](20) NOT NULL,
	[ToAcademicYear] [nvarchar](20) NOT NULL,
	[FromClassID] [int] NULL,
	[FromSection] [nvarchar](50) NULL,
	[ToClassID] [int] NULL,
	[ToSection] [nvarchar](50) NULL,
	[MigrationStatus] [nvarchar](20) NOT NULL,
	[CreatedByUserID] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[Notes] [nvarchar](1000) NULL,
 CONSTRAINT [PK_AnnualMigrationLog] PRIMARY KEY CLUSTERED 
(
	[MigrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[AuditLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[UserName] [nvarchar](150) NULL,
	[ActionName] [nvarchar](100) NOT NULL,
	[EntityName] [nvarchar](100) NULL,
	[EntityID] [nvarchar](100) NULL,
	[Details] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[Module] [nvarchar](100) NULL,
	[MachineName] [nvarchar](150) NULL,
	[IpAddress] [nvarchar](64) NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[AuditLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookBorrowings]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookBorrowings](
	[BorrowingID] [int] IDENTITY(1,1) NOT NULL,
	[BookID] [int] NOT NULL,
	[BorrowerType] [nvarchar](20) NOT NULL,
	[BorrowerID] [int] NOT NULL,
	[BorrowDate] [date] NOT NULL,
	[DueDate] [date] NOT NULL,
	[ReturnDate] [date] NULL,
	[Status] [nvarchar](30) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_BookBorrowings] PRIMARY KEY CLUSTERED 
(
	[BorrowingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentTransportAssignments](
	[AssignmentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[RouteID] [int] NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[PickupPoint] [nvarchar](200) NOT NULL,
	[DropoffPoint] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_StudentTransportAssignments] PRIMARY KEY CLUSTERED
(
	[AssignmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Books](
	[BookID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Author] [nvarchar](200) NULL,
	[ISBN] [nvarchar](50) NULL,
	[Category] [nvarchar](100) NULL,
	[Publisher] [nvarchar](200) NULL,
	[PublicationYear] [int] NULL,
	[Copies] [int] NOT NULL,
	[AvailableCopies] [int] NOT NULL,
	[ShelfLocation] [nvarchar](100) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buses]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buses](
	[BusID] [int] IDENTITY(1,1) NOT NULL,
	[BusNumber] [nvarchar](50) NOT NULL,
	[DriverName] [nvarchar](150) NULL,
	[DriverPhone] [nvarchar](50) NULL,
	[Capacity] [int] NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[BusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusRoutes]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusRoutes](
	[RouteID] [int] IDENTITY(1,1) NOT NULL,
	[RouteName] [nvarchar](150) NOT NULL,
	[BusID] [int] NOT NULL,
	[StartPoint] [nvarchar](200) NULL,
	[EndPoint] [nvarchar](200) NULL,
	[DepartureTime] [time](7) NULL,
	[ArrivalTime] [time](7) NULL,
	[Fee] [decimal](18, 2) NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RouteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[ClassID] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) NOT NULL,
	[ClassCode] [nvarchar](30) NULL,
	[StageName] [nvarchar](100) NULL,
	[GradeOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[StageID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassSubjects]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassSubjects](
	[ClassSubjectID] [int] IDENTITY(1,1) NOT NULL,
	[ClassID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_ClassSubjects] PRIMARY KEY CLUSTERED 
(
	[ClassSubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassSubjectTerms]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassSubjectTerms](
	[ClassSubjectTermID] [int] IDENTITY(1,1) NOT NULL,
	[ClassSubjectID] [int] NOT NULL,
	[TermID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_ClassSubjectTerms] PRIMARY KEY CLUSTERED 
(
	[ClassSubjectTermID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatabaseBackupHistory]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatabaseBackupHistory](
	[BackupHistoryID] [bigint] IDENTITY(1,1) NOT NULL,
	[BackupFile] [nvarchar](500) NOT NULL,
	[BackupType] [nvarchar](50) NOT NULL,
	[AcademicYear] [nvarchar](20) NULL,
	[CreatedByUserID] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsVerified] [bit] NOT NULL,
 CONSTRAINT [PK_DatabaseBackupHistory] PRIMARY KEY CLUSTERED 
(
	[BackupHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments](
	[EnrollmentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ApplicationDate] [date] NOT NULL,
	[ApplicationType] [nvarchar](50) NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[ClassID] [int] NULL,
	[Section] [nvarchar](50) NULL,
	[SeatNumber] [nvarchar](20) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[PreviousSchool] [nvarchar](200) NULL,
	[PreviousClass] [nvarchar](50) NULL,
	[TransferReason] [nvarchar](max) NULL,
	[RegistrationFee] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[ReceiptNo] [nvarchar](50) NULL,
	[HasBirthCertificate] [bit] NOT NULL,
	[HasGuardianId] [bit] NOT NULL,
	[HasPhoto] [bit] NOT NULL,
	[HasLastCertificate] [bit] NOT NULL,
	[HasMedicalReport] [bit] NOT NULL,
	[GeneralNotes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Enrollments_Runtime] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[ExpenseID] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseNumber] [nvarchar](50) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[ExpenseDate] [date] NOT NULL,
	[Category] [nvarchar](100) NULL,
	[PayeeName] [nvarchar](200) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeTypes]    Script Date: current ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeTypes](
    [FeeTypeID] [int] IDENTITY(1,1) NOT NULL,
    [FeeTypeCode] [nvarchar](30) NOT NULL,
    [FeeTypeName] [nvarchar](100) NOT NULL,
    [IsTotal] [bit] NOT NULL,
    [IsRequiredDefault] [bit] NOT NULL,
    [DistributionWeight] [decimal](9,4) NOT NULL,
    [SortOrder] [int] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [Notes] [nvarchar](500) NULL,
    [CreatedAt] [datetime2](0) NOT NULL,
    [UpdatedAt] [datetime2](0) NULL,
 CONSTRAINT [PK_FeeTypes] PRIMARY KEY CLUSTERED 
(
    [FeeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_FeeTypes_Code] UNIQUE NONCLUSTERED ([FeeTypeCode] ASC),
 CONSTRAINT [UQ_FeeTypes_Name] UNIQUE NONCLUSTERED ([FeeTypeName] ASC)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeePlans]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeePlans](
	[FeePlanID] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[ClassID] [int] NOT NULL,
	[FeeType] [nvarchar](100) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[DueDate] [date] NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_FeePlans] PRIMARY KEY CLUSTERED 
(
	[FeePlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fees]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fees](
	[FeeID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[FeePlanID] [int] NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[FeeType] [nvarchar](100) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[NetAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[RemainingAmount] [decimal](18, 2) NOT NULL,
	[DueDate] [date] NOT NULL,
	[PaymentDate] [date] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[ReceiptNumber] [nvarchar](50) NULL,
	[Status] [nvarchar](30) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Fees] PRIMARY KEY CLUSTERED 
(
	[FeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grades]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
	[GradeID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NULL,
	[SubjectID] [int] NULL,
	[ClassID] [int] NULL,
	[AcademicYear] [nvarchar](20) NULL,
	[TermName] [nvarchar](50) NULL,
	[GradeValue] [decimal](10, 2) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[Section] [nvarchar](100) NULL,
	[Quiz1] [decimal](10, 2) NOT NULL,
	[Quiz2] [decimal](10, 2) NOT NULL,
	[CourseWork] [decimal](10, 2) NOT NULL,
	[FinalExam] [decimal](10, 2) NOT NULL,
	[GradeLetter] [nvarchar](50) NULL,
	[ResultStatus] [nvarchar](50) NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[GradeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marks]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marks](
	[MarkID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[TeacherID] [int] NULL,
	[Mark] [decimal](5, 2) NOT NULL,
	[ExamType] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MarkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payroll]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payroll](
	[PayrollID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NOT NULL,
	[SalaryMonth] [int] NOT NULL,
	[SalaryYear] [int] NOT NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[Allowances] [decimal](18, 2) NOT NULL,
	[Deductions] [decimal](18, 2) NOT NULL,
	[NetSalary]  AS (([BasicSalary]+[Allowances])-[Deductions]),
	[PaymentDate] [date] NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Payroll] PRIMARY KEY CLUSTERED 
(
	[PayrollID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionKey] [nvarchar](150) NOT NULL,
	[DisplayName] [nvarchar](250) NULL,
	[ModuleName] [nvarchar](100) NULL,
	[ActionName] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receipts]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipts](
	[ReceiptID] [int] IDENTITY(1,1) NOT NULL,
	[ReceiptNumber] [nvarchar](50) NULL,
	[StudentID] [int] NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[ReceiptDate] [date] NOT NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Receipts] PRIMARY KEY CLUSTERED 
(
	[ReceiptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[RoleID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[GrantedAt] [datetime2](0) NOT NULL,
	[GrantedBy] [int] NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC,
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsSystemRole] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
	[UpdatedAt] [datetime2](0) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[RoomCode] [nvarchar](30) NULL,
	[RoomName] [nvarchar](100) NOT NULL,
	[RoomType] [nvarchar](50) NULL,
	[Capacity] [int] NOT NULL,
	[Location] [nvarchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemaMigrations]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemaMigrations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[AppliedAtUtc] [datetime] NOT NULL,
	[Success] [bit] NOT NULL,
 CONSTRAINT [PK_SchemaMigrations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolSections]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolSections](
	[SectionID] [int] IDENTITY(1,1) NOT NULL,
	[ClassID] [int] NOT NULL,
	[SectionName] [nvarchar](50) NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
	[Capacity] [int] NULL,
	[AllowedGender] [nvarchar](20) NULL,
 CONSTRAINT [PK_SchoolSections] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolTimetable]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolTimetable](
	[TimetableID] [int] IDENTITY(1,1) NOT NULL,
	[ClassID] [int] NOT NULL,
	[Section] [nvarchar](50) NULL,
	[SubjectID] [int] NOT NULL,
	[TeacherID] [int] NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[TermName] [nvarchar](50) NULL,
	[DayName] [nvarchar](30) NOT NULL,
	[PeriodNo] [int] NOT NULL,
	[RoomName] [nvarchar](100) NULL,
	[Notes] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_SchoolTimetable] PRIMARY KEY CLUSTERED 
(
	[TimetableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stages]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stages](
	[StageID] [int] IDENTITY(1,1) NOT NULL,
	[StageCode] [nvarchar](30) NOT NULL,
	[StageName] [nvarchar](100) NOT NULL,
	[StageOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_Stages] PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentAttendance]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAttendance](
	[AttendanceID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[Section] [nvarchar](50) NOT NULL,
	[AcademicYear] [nvarchar](20) NOT NULL,
	[AttendanceDate] [date] NOT NULL,
	[Status] [nvarchar](30) NOT NULL,
	[ArrivalTime] [time](7) NULL,
	[ExcuseStatus] [nvarchar](50) NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DepartureTime] [time](7) NULL,
	[AbsenceReason] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClasses]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClasses](
	[StudentClassID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[AcademicYear] [nvarchar](20) NULL,
	[Section] [nvarchar](50) NULL,
	[AssignedDate] [datetime] NOT NULL,
	[AssignedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentFees]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentFees](
	[StudentFeeID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[FeeType] [nvarchar](100) NULL,
	[AcademicYear] [nvarchar](20) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](30) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StudentFees] PRIMARY KEY CLUSTERED 
(
	[StudentFeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentNumber] [nvarchar](30) NULL,
	[FullName] [nvarchar](200) NOT NULL,
	[Gender] [nvarchar](20) NULL,
	[BirthDate] [date] NULL,
	[BirthPlace] [nvarchar](100) NULL,
	[Nationality] [nvarchar](100) NULL,
	[NationalId] [nvarchar](50) NULL,
	[StudentPhone] [nvarchar](30) NULL,
	[Status] [nvarchar](30) NOT NULL,
	[GuardianName] [nvarchar](200) NULL,
	[GuardianRelation] [nvarchar](50) NULL,
	[GuardianPhone] [nvarchar](30) NULL,
	[GuardianEmail] [nvarchar](150) NULL,
	[GuardianJob] [nvarchar](100) NULL,
	[Governorate] [nvarchar](100) NULL,
	[District] [nvarchar](100) NULL,
	[Address] [nvarchar](300) NULL,
	[Photo] [varbinary](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[ClassID] [int] NULL,
	[Section] [nvarchar](50) NULL,
	[AcademicYear] [nvarchar](20) NULL,
	[Phone] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[SubjectID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectName] [nvarchar](100) NOT NULL,
	[SubjectCode] [nvarchar](30) NULL,
	[ClassID] [int] NULL,
	[MaxDegree] [decimal](10, 2) NOT NULL,
	[PassDegree] [decimal](10, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherAttendance]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherAttendance](
	[AttendanceID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NOT NULL,
	[AttendanceDate] [date] NOT NULL,
	[Status] [nvarchar](30) NOT NULL,
	[LateMinutes] [int] NOT NULL,
	[EarlyLeaveMinutes] [int] NOT NULL,
	[WorkHours] [decimal](10, 2) NOT NULL,
	[AbsenceReason] [nvarchar](300) NULL,
	[Notes] [nvarchar](max) NULL,
	[RecordedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CheckInTime] [time](7) NULL,
	[CheckOutTime] [time](7) NULL,
 CONSTRAINT [PK_TeacherAttendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherContracts]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherContracts](
	[ContractID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NOT NULL,
	[ContractNumber] [nvarchar](50) NULL,
	[ContractType] [nvarchar](50) NULL,
	[ContractStatus] [nvarchar](30) NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[HousingAllowance] [decimal](18, 2) NOT NULL,
	[TransportAllowance] [decimal](18, 2) NOT NULL,
	[OtherAllowances] [decimal](18, 2) NOT NULL,
	[Deductions] [decimal](18, 2) NOT NULL,
	[TotalSalary]  AS ((([BasicSalary]+[HousingAllowance])+[TransportAllowance])+[OtherAllowances]),
	[NetSalary]  AS (((([BasicSalary]+[HousingAllowance])+[TransportAllowance])+[OtherAllowances])-[Deductions]),
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_TeacherContracts] PRIMARY KEY CLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers](
	[TeacherID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNumber] [nvarchar](50) NULL,
	[FullName] [nvarchar](200) NOT NULL,
	[Gender] [nvarchar](10) NULL,
	[BirthDate] [date] NULL,
	[BirthPlace] [nvarchar](100) NULL,
	[Nationality] [nvarchar](100) NULL,
	[NationalID] [nvarchar](50) NULL,
	[Phone] [nvarchar](30) NULL,
	[Email] [nvarchar](150) NULL,
	[Address] [nvarchar](300) NULL,
	[Qualification] [nvarchar](100) NULL,
	[Specialization] [nvarchar](100) NULL,
	[HireDate] [date] NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[TransportAllowance] [decimal](18, 2) NOT NULL,
	[HousingAllowance] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TeacherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPermissions]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermissions](
	[UserPermissionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[GrantedAt] [datetime2](7) NOT NULL,
	[GrantedBy] [int] NULL,
 CONSTRAINT [PK_UserPermissions] PRIMARY KEY CLUSTERED 
(
	[UserPermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[AssignedAt] [datetime2](0) NOT NULL,
	[AssignedBy] [int] NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](200) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[PasswordSalt] [nvarchar](max) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Permissions] [nvarchar](max) NULL,
	[Email] [nvarchar](150) NULL,
	[Phone] [nvarchar](30) NULL,
	[IsActive] [bit] NOT NULL,
	[MustChangePassword] [bit] NOT NULL,
	[LastLoginAt] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[LockedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/* ================================================================
   جدول إعدادات المدرسة
   يتم تنفيذه ضمن مخطط قاعدة البيانات SchoolDB
   ================================================================ */

IF OBJECT_ID(N'dbo.SchoolSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSettings
    (
        SchoolSettingsID INT NOT NULL
            CONSTRAINT PK_SchoolSettings
            PRIMARY KEY,

        SchoolNameAr NVARCHAR(200) NOT NULL
            CONSTRAINT CK_SchoolSettings_NameAr
            CHECK (LEN(LTRIM(RTRIM(SchoolNameAr))) > 0),

        SchoolNameEn NVARCHAR(200) NOT NULL
            CONSTRAINT CK_SchoolSettings_NameEn
            CHECK (LEN(LTRIM(RTRIM(SchoolNameEn))) > 0),

        OrganizationNameAr NVARCHAR(200) NULL,
        OrganizationNameEn NVARCHAR(200) NULL,

        AddressAr NVARCHAR(500) NULL,
        AddressEn NVARCHAR(500) NULL,

        Phone NVARCHAR(40) NULL,
        AdditionalPhone NVARCHAR(40) NULL,

        Email NVARCHAR(254) NULL,
        Website NVARCHAR(300) NULL,

        PrincipalNameAr NVARCHAR(200) NULL,
        PrincipalNameEn NVARCHAR(200) NULL,

        LicenseNumber NVARCHAR(100) NULL,

        Logo VARBINARY(MAX) NULL,
        LogoFileName NVARCHAR(260) NULL,

        UpdatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_SchoolSettings_UpdatedAt
            DEFAULT SYSUTCDATETIME(),

        UpdatedByUserID INT NULL
    );
END;
GO


/* ================================================================
   إنشاء السجل الأساسي الوحيد لإعدادات المدرسة
   ================================================================ */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.SchoolSettings
    WHERE SchoolSettingsID = 1
)
BEGIN
    INSERT INTO dbo.SchoolSettings
    (
        SchoolSettingsID,
        SchoolNameAr,
        SchoolNameEn,
        OrganizationNameAr,
        OrganizationNameEn,
        AddressAr,
        AddressEn,
        Phone,
        AdditionalPhone,
        Email,
        Website,
        PrincipalNameAr,
        PrincipalNameEn,
        LicenseNumber,
        Logo,
        LogoFileName,
        UpdatedAt,
        UpdatedByUserID
    )
    VALUES
    (
        1,
        N'اسم المدرسة',
        N'اسم المدرسة',
        N'اسم المؤسسة التعليمية',
        N'اسم المؤسسة التعليمية',
        N'عنوان المدرسة',
        N'عنوان المدرسة',
        NULL,
        NULL,
        NULL,
        NULL,
        N'اسم مدير المدرسة',
        N'اسم مدير المدرسة',
        NULL,
        NULL,
        NULL,
        SYSUTCDATETIME(),
        NULL
    );
END;
GO
/****** Object:  Table [dbo].[Vouchers]    Script Date: 8/31/2026 7:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vouchers](
	[VoucherID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherNumber] [nvarchar](50) NULL,
	[VoucherType] [nvarchar](30) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[VoucherDate] [date] NOT NULL,
	[PartyName] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[ReferenceType] [nvarchar](50) NULL,
	[ReferenceID] [int] NULL,
	[Notes] [nvarchar](max) NULL,
	[IsAutoGenerated] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Vouchers] PRIMARY KEY CLUSTERED 
(
	[VoucherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[AcademicYearCurrent] ([AcademicYear], [StartDate], [EndDate], [IsCurrent], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'2026/2027', NULL, NULL, 1, N'عام دراسي текущий', CAST(N'2026-08-22T21:48:02.173' AS DateTime), CAST(N'2026-08-22T22:56:41.077' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (1, N'Dashboard.View', N'عرض لوحة التحكم', N'Dashboard', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (2, N'Students.View', N'الطلاب - عرض', N'Students', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (3, N'Enrollment.View', N'القبول والتسجيل - عرض', N'Enrollment', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (4, N'ClassAssignment.View', N'توزيع الطلاب - عرض', N'ClassAssignment', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (5, N'Teachers.View', N'المعلمون - عرض', N'Teachers', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (6, N'TeacherAttendance.View', N'حضور المعلمين - عرض', N'TeacherAttendance', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (7, N'StaffAttendance.View', N'حضور الموظفين - عرض', N'StaffAttendance', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (8, N'TeacherContracts.View', N'عقود المعلمين - عرض', N'TeacherContracts', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (9, N'Payroll.View', N'الرواتب - عرض', N'Payroll', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (10, N'Subjects.View', N'المواد - عرض', N'Subjects', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (11, N'Classes.View', N'الفصول الدراسية - عرض', N'Classes', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (12, N'Rooms.View', N'القاعات - عرض', N'Rooms', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (13, N'Timetable.View', N'الجدول الدراسي - عرض', N'Timetable', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (14, N'Grades.View', N'الدرجات - عرض', N'Grades', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (15, N'Attendance.View', N'حضور الطلاب - عرض', N'Attendance', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (16, N'Fees.View', N'الرسوم - عرض', N'Fees', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (17, N'FeePlans.View', N'خطط الرسوم - عرض', N'FeePlans', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (18, N'Vouchers.View', N'السندات - عرض', N'Vouchers', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (19, N'Expenses.View', N'المصروفات - عرض', N'Expenses', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (20, N'Transport.View', N'النقل - عرض', N'Transport', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (21, N'Library.View', N'المكتبة - عرض', N'Library', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (22, N'Reports.View', N'التقارير - عرض', N'Reports', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (23, N'AuditLogs.View', N'سجل التدقيق - عرض', N'AuditLogs', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (24, N'Settings.View', N'الإعدادات - عرض', N'Settings', N'View', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (25, N'Dashboard.Add', N'لوحة التحكم - إضافة', N'Dashboard', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (26, N'Students.Add', N'الطلاب - إضافة', N'Students', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (27, N'Enrollment.Add', N'القبول والتسجيل - إضافة', N'Enrollment', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (28, N'ClassAssignment.Add', N'توزيع الطلاب - إضافة', N'ClassAssignment', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (29, N'Teachers.Add', N'المعلمون - إضافة', N'Teachers', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (30, N'TeacherAttendance.Add', N'حضور المعلمين - إضافة', N'TeacherAttendance', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (31, N'StaffAttendance.Add', N'حضور الموظفين - إضافة', N'StaffAttendance', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (32, N'TeacherContracts.Add', N'عقود المعلمين - إضافة', N'TeacherContracts', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (33, N'Payroll.Add', N'الرواتب - إضافة', N'Payroll', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (34, N'Subjects.Add', N'المواد - إضافة', N'Subjects', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (35, N'Classes.Add', N'الفصول الدراسية - إضافة', N'Classes', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (36, N'Rooms.Add', N'القاعات - إضافة', N'Rooms', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (37, N'Timetable.Add', N'الجدول الدراسي - إضافة', N'Timetable', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (38, N'Grades.Add', N'الدرجات - إضافة', N'Grades', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (39, N'Attendance.Add', N'حضور الطلاب - إضافة', N'Attendance', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (40, N'Fees.Add', N'الرسوم - إضافة', N'Fees', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (41, N'FeePlans.Add', N'خطط الرسوم - إضافة', N'FeePlans', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (42, N'Vouchers.Add', N'السندات - إضافة', N'Vouchers', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (43, N'Expenses.Add', N'المصروفات - إضافة', N'Expenses', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (44, N'Transport.Add', N'النقل - إضافة', N'Transport', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (45, N'Library.Add', N'المكتبة - إضافة', N'Library', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (46, N'Reports.Add', N'التقارير - إضافة', N'Reports', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (47, N'AuditLogs.Add', N'سجل التدقيق - إضافة', N'AuditLogs', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (48, N'Settings.Add', N'الإعدادات - إضافة', N'Settings', N'Add', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (49, N'Dashboard.Edit', N'لوحة التحكم - تعديل', N'Dashboard', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (50, N'Students.Edit', N'الطلاب - تعديل', N'Students', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (51, N'Enrollment.Edit', N'القبول والتسجيل - تعديل', N'Enrollment', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (52, N'ClassAssignment.Edit', N'توزيع الطلاب - تعديل', N'ClassAssignment', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (53, N'Teachers.Edit', N'المعلمون - تعديل', N'Teachers', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (54, N'TeacherAttendance.Edit', N'حضور المعلمين - تعديل', N'TeacherAttendance', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (55, N'StaffAttendance.Edit', N'حضور الموظفين - تعديل', N'StaffAttendance', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (56, N'TeacherContracts.Edit', N'عقود المعلمين - تعديل', N'TeacherContracts', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (57, N'Payroll.Edit', N'الرواتب - تعديل', N'Payroll', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (58, N'Subjects.Edit', N'المواد - تعديل', N'Subjects', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (59, N'Classes.Edit', N'الفصول الدراسية - تعديل', N'Classes', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (60, N'Rooms.Edit', N'القاعات - تعديل', N'Rooms', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (61, N'Timetable.Edit', N'الجدول الدراسي - تعديل', N'Timetable', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (62, N'Grades.Edit', N'الدرجات - تعديل', N'Grades', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (63, N'Attendance.Edit', N'حضور الطلاب - تعديل', N'Attendance', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (64, N'Fees.Edit', N'الرسوم - تعديل', N'Fees', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (65, N'FeePlans.Edit', N'خطط الرسوم - تعديل', N'FeePlans', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (66, N'Vouchers.Edit', N'السندات - تعديل', N'Vouchers', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (67, N'Expenses.Edit', N'المصروفات - تعديل', N'Expenses', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (68, N'Transport.Edit', N'النقل - تعديل', N'Transport', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (69, N'Library.Edit', N'المكتبة - تعديل', N'Library', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (70, N'Reports.Edit', N'التقارير - تعديل', N'Reports', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (71, N'AuditLogs.Edit', N'سجل التدقيق - تعديل', N'AuditLogs', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (72, N'Settings.Edit', N'الإعدادات - تعديل', N'Settings', N'Edit', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (73, N'Dashboard.Delete', N'لوحة التحكم - حذف', N'Dashboard', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (74, N'Students.Delete', N'الطلاب - حذف', N'Students', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (75, N'Enrollment.Delete', N'القبول والتسجيل - حذف', N'Enrollment', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (76, N'ClassAssignment.Delete', N'توزيع الطلاب - حذف', N'ClassAssignment', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (77, N'Teachers.Delete', N'المعلمون - حذف', N'Teachers', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (78, N'TeacherAttendance.Delete', N'حضور المعلمين - حذف', N'TeacherAttendance', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (79, N'StaffAttendance.Delete', N'حضور الموظفين - حذف', N'StaffAttendance', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (80, N'TeacherContracts.Delete', N'عقود المعلمين - حذف', N'TeacherContracts', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (81, N'Payroll.Delete', N'الرواتب - حذف', N'Payroll', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (82, N'Subjects.Delete', N'المواد - حذف', N'Subjects', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (83, N'Classes.Delete', N'الفصول الدراسية - حذف', N'Classes', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (84, N'Rooms.Delete', N'القاعات - حذف', N'Rooms', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (85, N'Timetable.Delete', N'الجدول الدراسي - حذف', N'Timetable', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (86, N'Grades.Delete', N'الدرجات - حذف', N'Grades', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (87, N'Attendance.Delete', N'حضور الطلاب - حذف', N'Attendance', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (88, N'Fees.Delete', N'الرسوم - حذف', N'Fees', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (89, N'FeePlans.Delete', N'خطط الرسوم - حذف', N'FeePlans', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (90, N'Vouchers.Delete', N'السندات - حذف', N'Vouchers', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (91, N'Expenses.Delete', N'المصروفات - حذف', N'Expenses', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (92, N'Transport.Delete', N'النقل - حذف', N'Transport', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (93, N'Library.Delete', N'المكتبة - حذف', N'Library', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (94, N'Reports.Delete', N'التقارير - حذف', N'Reports', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (95, N'AuditLogs.Delete', N'سجل التدقيق - حذف', N'AuditLogs', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (96, N'Settings.Delete', N'الإعدادات - حذف', N'Settings', N'Delete', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (97, N'Dashboard.Search', N'لوحة التحكم - بحث', N'Dashboard', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (98, N'Students.Search', N'الطلاب - بحث', N'Students', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (99, N'Enrollment.Search', N'القبول والتسجيل - بحث', N'Enrollment', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (100, N'ClassAssignment.Search', N'توزيع الطلاب - بحث', N'ClassAssignment', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (101, N'Teachers.Search', N'المعلمون - بحث', N'Teachers', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (102, N'TeacherAttendance.Search', N'حضور المعلمين - بحث', N'TeacherAttendance', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (103, N'StaffAttendance.Search', N'حضور الموظفين - بحث', N'StaffAttendance', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (104, N'TeacherContracts.Search', N'عقود المعلمين - بحث', N'TeacherContracts', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (105, N'Payroll.Search', N'الرواتب - بحث', N'Payroll', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (106, N'Subjects.Search', N'المواد - بحث', N'Subjects', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (107, N'Classes.Search', N'الفصول الدراسية - بحث', N'Classes', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (108, N'Rooms.Search', N'القاعات - بحث', N'Rooms', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (109, N'Timetable.Search', N'الجدول الدراسي - بحث', N'Timetable', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (110, N'Grades.Search', N'الدرجات - بحث', N'Grades', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (111, N'Attendance.Search', N'حضور الطلاب - بحث', N'Attendance', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (112, N'Fees.Search', N'الرسوم - بحث', N'Fees', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (113, N'FeePlans.Search', N'خطط الرسوم - بحث', N'FeePlans', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (114, N'Vouchers.Search', N'السندات - بحث', N'Vouchers', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (115, N'Expenses.Search', N'المصروفات - بحث', N'Expenses', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (116, N'Transport.Search', N'النقل - بحث', N'Transport', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (117, N'Library.Search', N'المكتبة - بحث', N'Library', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (118, N'Reports.Search', N'التقارير - بحث', N'Reports', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (119, N'AuditLogs.Search', N'سجل التدقيق - بحث', N'AuditLogs', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (120, N'Settings.Search', N'الإعدادات - بحث', N'Settings', N'Search', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (121, N'Dashboard.Print', N'لوحة التحكم - طباعة', N'Dashboard', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (122, N'Students.Print', N'الطلاب - طباعة', N'Students', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (123, N'Enrollment.Print', N'القبول والتسجيل - طباعة', N'Enrollment', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (124, N'ClassAssignment.Print', N'توزيع الطلاب - طباعة', N'ClassAssignment', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (125, N'Teachers.Print', N'المعلمون - طباعة', N'Teachers', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (126, N'TeacherAttendance.Print', N'حضور المعلمين - طباعة', N'TeacherAttendance', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (127, N'StaffAttendance.Print', N'حضور الموظفين - طباعة', N'StaffAttendance', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (128, N'TeacherContracts.Print', N'عقود المعلمين - طباعة', N'TeacherContracts', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (129, N'Payroll.Print', N'الرواتب - طباعة', N'Payroll', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (130, N'Subjects.Print', N'المواد - طباعة', N'Subjects', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (131, N'Classes.Print', N'الفصول الدراسية - طباعة', N'Classes', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (132, N'Rooms.Print', N'القاعات - طباعة', N'Rooms', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (133, N'Timetable.Print', N'الجدول الدراسي - طباعة', N'Timetable', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (134, N'Grades.Print', N'الدرجات - طباعة', N'Grades', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (135, N'Attendance.Print', N'حضور الطلاب - طباعة', N'Attendance', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (136, N'Fees.Print', N'الرسوم - طباعة', N'Fees', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (137, N'FeePlans.Print', N'خطط الرسوم - طباعة', N'FeePlans', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (138, N'Vouchers.Print', N'السندات - طباعة', N'Vouchers', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (139, N'Expenses.Print', N'المصروفات - طباعة', N'Expenses', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (140, N'Transport.Print', N'النقل - طباعة', N'Transport', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (141, N'Library.Print', N'المكتبة - طباعة', N'Library', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (142, N'Reports.Print', N'التقارير - طباعة', N'Reports', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (143, N'AuditLogs.Print', N'سجل التدقيق - طباعة', N'AuditLogs', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (144, N'Settings.Print', N'الإعدادات - طباعة', N'Settings', N'Print', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (145, N'Dashboard.ExportExcel', N'لوحة التحكم - تصدير Excel', N'Dashboard', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (146, N'Students.ExportExcel', N'الطلاب - تصدير Excel', N'Students', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (147, N'Enrollment.ExportExcel', N'القبول والتسجيل - تصدير Excel', N'Enrollment', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (148, N'ClassAssignment.ExportExcel', N'توزيع الطلاب - تصدير Excel', N'ClassAssignment', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (149, N'Teachers.ExportExcel', N'المعلمون - تصدير Excel', N'Teachers', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (150, N'TeacherAttendance.ExportExcel', N'حضور المعلمين - تصدير Excel', N'TeacherAttendance', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (151, N'StaffAttendance.ExportExcel', N'حضور الموظفين - تصدير Excel', N'StaffAttendance', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (152, N'TeacherContracts.ExportExcel', N'عقود المعلمين - تصدير Excel', N'TeacherContracts', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (153, N'Payroll.ExportExcel', N'الرواتب - تصدير Excel', N'Payroll', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (154, N'Subjects.ExportExcel', N'المواد - تصدير Excel', N'Subjects', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (155, N'Classes.ExportExcel', N'الفصول الدراسية - تصدير Excel', N'Classes', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (156, N'Rooms.ExportExcel', N'القاعات - تصدير Excel', N'Rooms', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (157, N'Timetable.ExportExcel', N'الجدول الدراسي - تصدير Excel', N'Timetable', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (158, N'Grades.ExportExcel', N'الدرجات - تصدير Excel', N'Grades', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (159, N'Attendance.ExportExcel', N'حضور الطلاب - تصدير Excel', N'Attendance', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (160, N'Fees.ExportExcel', N'الرسوم - تصدير Excel', N'Fees', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (161, N'FeePlans.ExportExcel', N'خطط الرسوم - تصدير Excel', N'FeePlans', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (162, N'Vouchers.ExportExcel', N'السندات - تصدير Excel', N'Vouchers', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (163, N'Expenses.ExportExcel', N'المصروفات - تصدير Excel', N'Expenses', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (164, N'Transport.ExportExcel', N'النقل - تصدير Excel', N'Transport', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (165, N'Library.ExportExcel', N'المكتبة - تصدير Excel', N'Library', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (166, N'Reports.ExportExcel', N'التقارير - تصدير Excel', N'Reports', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (167, N'AuditLogs.ExportExcel', N'سجل التدقيق - تصدير Excel', N'AuditLogs', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (168, N'Settings.ExportExcel', N'الإعدادات - تصدير Excel', N'Settings', N'ExportExcel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (169, N'Dashboard.ExportPDF', N'لوحة التحكم - تصدير PDF', N'Dashboard', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (170, N'Students.ExportPDF', N'الطلاب - تصدير PDF', N'Students', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (171, N'Enrollment.ExportPDF', N'القبول والتسجيل - تصدير PDF', N'Enrollment', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (172, N'ClassAssignment.ExportPDF', N'توزيع الطلاب - تصدير PDF', N'ClassAssignment', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (173, N'Teachers.ExportPDF', N'المعلمون - تصدير PDF', N'Teachers', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (174, N'TeacherAttendance.ExportPDF', N'حضور المعلمين - تصدير PDF', N'TeacherAttendance', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (175, N'StaffAttendance.ExportPDF', N'حضور الموظفين - تصدير PDF', N'StaffAttendance', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (176, N'TeacherContracts.ExportPDF', N'عقود المعلمين - تصدير PDF', N'TeacherContracts', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (177, N'Payroll.ExportPDF', N'الرواتب - تصدير PDF', N'Payroll', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (178, N'Subjects.ExportPDF', N'المواد - تصدير PDF', N'Subjects', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (179, N'Classes.ExportPDF', N'الفصول الدراسية - تصدير PDF', N'Classes', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (180, N'Rooms.ExportPDF', N'القاعات - تصدير PDF', N'Rooms', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (181, N'Timetable.ExportPDF', N'الجدول الدراسي - تصدير PDF', N'Timetable', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (182, N'Grades.ExportPDF', N'الدرجات - تصدير PDF', N'Grades', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (183, N'Attendance.ExportPDF', N'حضور الطلاب - تصدير PDF', N'Attendance', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (184, N'Fees.ExportPDF', N'الرسوم - تصدير PDF', N'Fees', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (185, N'FeePlans.ExportPDF', N'خطط الرسوم - تصدير PDF', N'FeePlans', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (186, N'Vouchers.ExportPDF', N'السندات - تصدير PDF', N'Vouchers', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (187, N'Expenses.ExportPDF', N'المصروفات - تصدير PDF', N'Expenses', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (188, N'Transport.ExportPDF', N'النقل - تصدير PDF', N'Transport', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (189, N'Library.ExportPDF', N'المكتبة - تصدير PDF', N'Library', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (190, N'Reports.ExportPDF', N'التقارير - تصدير PDF', N'Reports', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (191, N'AuditLogs.ExportPDF', N'سجل التدقيق - تصدير PDF', N'AuditLogs', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (192, N'Settings.ExportPDF', N'الإعدادات - تصدير PDF', N'Settings', N'ExportPDF', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (193, N'Dashboard.Approve', N'لوحة التحكم - اعتماد', N'Dashboard', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (194, N'Students.Approve', N'الطلاب - اعتماد', N'Students', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (195, N'Enrollment.Approve', N'القبول والتسجيل - اعتماد', N'Enrollment', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (196, N'ClassAssignment.Approve', N'توزيع الطلاب - اعتماد', N'ClassAssignment', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (197, N'Teachers.Approve', N'المعلمون - اعتماد', N'Teachers', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (198, N'TeacherAttendance.Approve', N'حضور المعلمين - اعتماد', N'TeacherAttendance', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (199, N'StaffAttendance.Approve', N'حضور الموظفين - اعتماد', N'StaffAttendance', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (200, N'TeacherContracts.Approve', N'عقود المعلمين - اعتماد', N'TeacherContracts', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (201, N'Payroll.Approve', N'الرواتب - اعتماد', N'Payroll', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (202, N'Subjects.Approve', N'المواد - اعتماد', N'Subjects', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (203, N'Classes.Approve', N'الفصول الدراسية - اعتماد', N'Classes', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (204, N'Rooms.Approve', N'القاعات - اعتماد', N'Rooms', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (205, N'Timetable.Approve', N'الجدول الدراسي - اعتماد', N'Timetable', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (206, N'Grades.Approve', N'الدرجات - اعتماد', N'Grades', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (207, N'Attendance.Approve', N'حضور الطلاب - اعتماد', N'Attendance', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (208, N'Fees.Approve', N'الرسوم - اعتماد', N'Fees', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (209, N'FeePlans.Approve', N'خطط الرسوم - اعتماد', N'FeePlans', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (210, N'Vouchers.Approve', N'السندات - اعتماد', N'Vouchers', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (211, N'Expenses.Approve', N'المصروفات - اعتماد', N'Expenses', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (212, N'Transport.Approve', N'النقل - اعتماد', N'Transport', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (213, N'Library.Approve', N'المكتبة - اعتماد', N'Library', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (214, N'Reports.Approve', N'التقارير - اعتماد', N'Reports', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (215, N'AuditLogs.Approve', N'سجل التدقيق - اعتماد', N'AuditLogs', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (216, N'Settings.Approve', N'الإعدادات - اعتماد', N'Settings', N'Approve', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (217, N'Dashboard.Cancel', N'لوحة التحكم - إلغاء', N'Dashboard', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (218, N'Students.Cancel', N'الطلاب - إلغاء', N'Students', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (219, N'Enrollment.Cancel', N'القبول والتسجيل - إلغاء', N'Enrollment', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (220, N'ClassAssignment.Cancel', N'توزيع الطلاب - إلغاء', N'ClassAssignment', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (221, N'Teachers.Cancel', N'المعلمون - إلغاء', N'Teachers', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (222, N'TeacherAttendance.Cancel', N'حضور المعلمين - إلغاء', N'TeacherAttendance', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (223, N'StaffAttendance.Cancel', N'حضور الموظفين - إلغاء', N'StaffAttendance', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (224, N'TeacherContracts.Cancel', N'عقود المعلمين - إلغاء', N'TeacherContracts', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (225, N'Payroll.Cancel', N'الرواتب - إلغاء', N'Payroll', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (226, N'Subjects.Cancel', N'المواد - إلغاء', N'Subjects', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (227, N'Classes.Cancel', N'الفصول الدراسية - إلغاء', N'Classes', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (228, N'Rooms.Cancel', N'القاعات - إلغاء', N'Rooms', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (229, N'Timetable.Cancel', N'الجدول الدراسي - إلغاء', N'Timetable', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (230, N'Grades.Cancel', N'الدرجات - إلغاء', N'Grades', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (231, N'Attendance.Cancel', N'حضور الطلاب - إلغاء', N'Attendance', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (232, N'Fees.Cancel', N'الرسوم - إلغاء', N'Fees', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (233, N'FeePlans.Cancel', N'خطط الرسوم - إلغاء', N'FeePlans', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (234, N'Vouchers.Cancel', N'السندات - إلغاء', N'Vouchers', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (235, N'Expenses.Cancel', N'المصروفات - إلغاء', N'Expenses', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (236, N'Transport.Cancel', N'النقل - إلغاء', N'Transport', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (237, N'Library.Cancel', N'المكتبة - إلغاء', N'Library', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (238, N'Reports.Cancel', N'التقارير - إلغاء', N'Reports', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (239, N'AuditLogs.Cancel', N'سجل التدقيق - إلغاء', N'AuditLogs', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (240, N'Settings.Cancel', N'الإعدادات - إلغاء', N'Settings', N'Cancel', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (241, N'Students.Manage', N'الطلاب - إدارة', N'Students', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (242, N'Enrollment.Manage', N'القبول والتسجيل - إدارة', N'Enrollment', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (243, N'ClassAssignment.Manage', N'توزيع الطلاب - إدارة', N'ClassAssignment', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (244, N'Teachers.Manage', N'المعلمون - إدارة', N'Teachers', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (245, N'Payroll.Manage', N'الرواتب - إدارة', N'Payroll', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (246, N'Subjects.Manage', N'المواد - إدارة', N'Subjects', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (247, N'Classes.Manage', N'الفصول الدراسية - إدارة', N'Classes', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (248, N'Timetable.Manage', N'الجدول الدراسي - إدارة', N'Timetable', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (249, N'Attendance.Manage', N'حضور الطلاب - إدارة', N'Attendance', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (250, N'Grades.Manage', N'الدرجات - إدارة', N'Grades', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (251, N'Fees.Manage', N'الرسوم - إدارة', N'Fees', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (252, N'Vouchers.Manage', N'السندات - إدارة', N'Vouchers', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (253, N'Expenses.Manage', N'المصروفات - إدارة', N'Expenses', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (254, N'Library.Manage', N'المكتبة - إدارة', N'Library', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (255, N'Transport.Manage', N'النقل - إدارة', N'Transport', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (256, N'Users.Manage', N'إدارة المستخدمين (شاملة)', N'Users', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (257, N'Settings.Manage', N'الإعدادات - إدارة', N'Settings', N'Manage', 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (258, N'Attendance.ExportCsv', N'حضور الطلاب - تصدير CSV', N'Attendance', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (259, N'AuditLogs.ExportCsv', N'سجل التدقيق - تصدير CSV', N'AuditLogs', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (260, N'ClassAssignment.ExportCsv', N'توزيع الطلاب - تصدير CSV', N'ClassAssignment', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (261, N'Classes.ExportCsv', N'الفصول الدراسية - تصدير CSV', N'Classes', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (262, N'Dashboard.ExportCsv', N'لوحة التحكم - تصدير CSV', N'Dashboard', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (263, N'Enrollment.ExportCsv', N'القبول والتسجيل - تصدير CSV', N'Enrollment', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (264, N'Expenses.ExportCsv', N'المصروفات - تصدير CSV', N'Expenses', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (265, N'FeePlans.ExportCsv', N'خطط الرسوم - تصدير CSV', N'FeePlans', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (266, N'Fees.ExportCsv', N'الرسوم - تصدير CSV', N'Fees', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (267, N'Grades.ExportCsv', N'الدرجات - تصدير CSV', N'Grades', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (268, N'Library.ExportCsv', N'المكتبة - تصدير CSV', N'Library', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (269, N'Payroll.ExportCsv', N'الرواتب - تصدير CSV', N'Payroll', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (270, N'Permissions.Manage', N'إدارة الصلاحيات', N'Permissions', N'Manage', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (271, N'Reports.ExportCsv', N'التقارير - تصدير CSV', N'Reports', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (272, N'Roles.Add', N'الأدوار - إضافة', N'Roles', N'Add', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (273, N'Roles.Delete', N'الأدوار - حذف', N'Roles', N'Delete', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (274, N'Roles.Edit', N'الأدوار - تعديل', N'Roles', N'Edit', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (275, N'Roles.Manage', N'إدارة الأدوار', N'Roles', N'Manage', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (276, N'Roles.View', N'الأدوار - عرض', N'Roles', N'View', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (277, N'Rooms.ExportCsv', N'القاعات - تصدير CSV', N'Rooms', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (278, N'Settings.ExportCsv', N'الإعدادات - تصدير CSV', N'Settings', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (279, N'StaffAttendance.ExportCsv', N'حضور الموظفين - تصدير CSV', N'StaffAttendance', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (280, N'StaffAttendance.Manage', N'حضور الموظفين - إدارة', N'StaffAttendance', N'Manage', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (281, N'Students.ExportCsv', N'الطلاب - تصدير CSV', N'Students', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (282, N'Subjects.ExportCsv', N'المواد - تصدير CSV', N'Subjects', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (283, N'TeacherAttendance.ExportCsv', N'حضور المعلمين - تصدير CSV', N'TeacherAttendance', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (284, N'TeacherContracts.ExportCsv', N'عقود المعلمين - تصدير CSV', N'TeacherContracts', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (285, N'Teachers.ExportCsv', N'المعلمون - تصدير CSV', N'Teachers', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (286, N'Timetable.ExportCsv', N'الجدول الدراسي - تصدير CSV', N'Timetable', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (287, N'Transport.ExportCsv', N'النقل - تصدير CSV', N'Transport', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (288, N'Users.Add', N'المستخدمون - إضافة', N'Users', N'Add', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (289, N'Users.Delete', N'المستخدمون - حذف', N'Users', N'Delete', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (290, N'Users.Edit', N'المستخدمون - تعديل', N'Users', N'Edit', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (291, N'Users.ManageRoles', N'إدارة أدوار المستخدمين', N'Users', N'ManageRoles', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (292, N'Users.View', N'المستخدمون - عرض', N'Users', N'View', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (293, N'Vouchers.ExportCsv', N'السندات - تصدير CSV', N'Vouchers', N'ExportCsv', 1, CAST(N'2026-08-18T15:27:43.0000000' AS DateTime2))
INSERT [dbo].[Permissions] ([PermissionID], [PermissionKey], [DisplayName], [ModuleName], [ActionName], [IsActive], [CreatedAt]) VALUES (294, N'Permissions.View', N'الصلاحيات - عرض', N'Permissions', N'View', 1, CAST(N'2026-08-20T21:32:49.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 4, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 5, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 6, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 7, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 8, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 9, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 10, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 11, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 12, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 13, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 16, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 17, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 18, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 19, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 20, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 21, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 23, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 24, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 276, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 292, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1, 294, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 4, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 5, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 10, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 11, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 12, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 13, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (2, 23, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 4, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (3, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 13, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (4, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 9, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 16, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 17, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 18, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 19, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (5, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (6, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (6, 21, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (6, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (7, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (7, 20, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (7, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (8, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (8, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 4, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 5, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 10, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 11, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 12, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 13, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (9, 23, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 4, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 5, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 10, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 11, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 12, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 13, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 14, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 15, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (10, 23, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 5, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 7, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 8, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 9, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (11, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
GO
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (12, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (12, 21, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (12, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (13, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (13, 20, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (13, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (14, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (14, 2, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (14, 3, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (14, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (15, 1, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (15, 22, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
INSERT [dbo].[RolePermissions] ([RoleID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (15, 23, CAST(N'2026-08-20T22:33:07.0000000' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (1, N'مدير النظام', N'صلاحيات كاملة مع حماية الحساب الأخير', 1, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (2, N'الإدارة', N'إدارة أكاديمية واسعة وتقارير', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (3, N'شؤون الطلاب', N'الطلاب والقبول والتوزيع', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (4, N'المعلمون', N'الحضور والدرجات والطلاب المرتبطون', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (5, N'المالية', N'الرسوم والسندات والمصروفات والرواتب', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (6, N'المكتبة', N'إدارة الكتب والإعارات', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (7, N'النقل', N'إدارة الحافلات والطرق', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (8, N'التقارير', N'عرض وطباعة وتصدير التقارير فقط', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (9, N'مدير المدرسة', N'دور إداري مخصص', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (10, N'وكيل المدرسة', N'دور أكاديمي مخصص', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (11, N'شؤون الموظفين', N'الموظفون والعقود والحضور', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (12, N'أمين المكتبة', N'إدارة المكتبة', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (13, N'مسؤول النقل', N'إدارة النقل', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (14, N'موظف الاستقبال', N'بحث وعرض محدود', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (15, N'مدقق', N'عرض التقارير وسجل الأنشطة دون تعديل', 0, 1, CAST(N'2026-08-17T20:16:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description], [IsSystemRole], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (16, N'مستخدم محدود', NULL, 0, 1, CAST(N'2026-08-20T22:13:28.0000000' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([RoomID], [RoomCode], [RoomName], [RoomType], [Capacity], [Location], [IsActive], [Notes], [CreatedAt], [UpdatedAt]) VALUES (3, N'R-101', N'قاعة 101', N'فصل دراسي', 30, N'المبنى الرئيسي', 1, N'قاعة تجريبية', CAST(N'2026-08-17T23:15:44.823' AS DateTime), NULL)
INSERT [dbo].[Rooms] ([RoomID], [RoomCode], [RoomName], [RoomType], [Capacity], [Location], [IsActive], [Notes], [CreatedAt], [UpdatedAt]) VALUES (4, N'LAB-01', N'مختبر العلوم', N'مختبر', 20, N'المبنى الرئيسي', 1, N'قاعة تجريبية', CAST(N'2026-08-17T23:15:44.823' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Rooms] OFF
GO
SET IDENTITY_INSERT [dbo].[SchemaMigrations] ON 

INSERT [dbo].[SchemaMigrations] ([Id], [Version], [Name], [AppliedAtUtc], [Success]) VALUES (1, 1, N'baseline-adopted-existing-schema', CAST(N'2026-08-31T15:41:47.757' AS DateTime), 1)
INSERT [dbo].[SchemaMigrations] ([Id], [Version], [Name], [AppliedAtUtc], [Success]) VALUES (3, 3, N'EnsureAcademicReference', CAST(N'2026-08-31T15:41:47.900' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[SchemaMigrations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserPermissions] ON 

INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (21, 3, 2, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (22, 3, 3, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (23, 3, 4, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (24, 3, 5, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (25, 3, 6, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (26, 3, 7, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (27, 3, 8, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (28, 3, 10, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (29, 3, 11, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (30, 3, 12, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (31, 3, 13, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (32, 3, 14, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (33, 3, 15, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (34, 3, 20, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (35, 3, 21, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (36, 3, 23, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (37, 3, 24, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (38, 3, 276, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (39, 3, 292, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (40, 4, 6, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (41, 4, 7, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (42, 4, 8, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (43, 4, 9, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (44, 4, 16, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (45, 4, 17, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (46, 4, 18, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (47, 4, 19, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (48, 4, 20, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (49, 4, 21, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (50, 4, 24, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (51, 4, 276, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (52, 4, 292, CAST(N'2026-08-20T21:32:48.8100000' AS DateTime2), NULL)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1626, 2, 2, CAST(N'2026-08-21T20:12:22.3066667' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1627, 2, 3, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1628, 2, 4, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1629, 2, 5, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1630, 2, 6, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1631, 2, 7, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1632, 2, 8, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1633, 2, 9, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1634, 2, 10, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1635, 2, 13, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1636, 2, 14, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1637, 2, 15, CAST(N'2026-08-21T20:12:22.3100000' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1638, 2, 19, CAST(N'2026-08-21T20:12:22.3133333' AS DateTime2), 1)
INSERT [dbo].[UserPermissions] ([UserPermissionID], [UserID], [PermissionID], [GrantedAt], [GrantedBy]) VALUES (1639, 2, 21, CAST(N'2026-08-21T20:12:22.3133333' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[UserPermissions] OFF
GO
INSERT [dbo].[UserRoles] ([UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (1, 1, CAST(N'2026-08-20T20:45:32.0000000' AS DateTime2), NULL)
INSERT [dbo].[UserRoles] ([UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (2, 3, CAST(N'2026-08-21T20:12:22.0000000' AS DateTime2), NULL)
INSERT [dbo].[UserRoles] ([UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (3, 5, CAST(N'2026-08-20T20:45:32.0000000' AS DateTime2), NULL)
INSERT [dbo].[UserRoles] ([UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (4, 9, CAST(N'2026-08-20T20:45:32.0000000' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [UserName], [PasswordHash], [PasswordSalt], [RoleName], [Permissions], [Email], [Phone], [IsActive], [MustChangePassword], [LastLoginAt], [CreatedAt], [UpdatedAt], [FailedLoginAttempts], [LockedAt]) VALUES (1, N'مدير النظام', N'admin', N'GBp0tj8dcDVmm0iSsSzj8y/TvLYLd7Dk8JejNg4TwCc=', N'KO1MeNPp+YwgREUxbe5k5g==', N'مدير النظام', N'Students.View,Enrollment.View,ClassAssignment.View,Teachers.View,TeacherAttendance.View,StaffAttendance.View,TeacherContracts.View,Payroll.View,Subjects.View,Classes.View,Rooms.View,Timetable.View,Grades.View,Attendance.View,Fees.View,FeePlans.View,Vouchers.View,Expenses.View,Transport.View,Library.View,Reports.View,Dashboard.View,AuditLogs.View,Settings.View,Users.View,Roles.View,Permissions.View', NULL, NULL, 1, 0, CAST(N'2026-08-31T19:16:14.680' AS DateTime), CAST(N'2026-08-14T17:15:55.737' AS DateTime), CAST(N'2026-08-31T19:16:14.680' AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [UserName], [PasswordHash], [PasswordSalt], [RoleName], [Permissions], [Email], [Phone], [IsActive], [MustChangePassword], [LastLoginAt], [CreatedAt], [UpdatedAt], [FailedLoginAttempts], [LockedAt]) VALUES (2, N'ali', N'all', N'Yzj8WVIOEXlIjZ4QvWwEfiUqNDcJn+Xwih0Z1XigJWQ=', N'YLy7UgI8Fi5V7G/FZZe6EA==', N'شؤون الطلاب', N'Attendance.View,ClassAssignment.View,Enrollment.View,Expenses.View,Grades.View,Library.View,Payroll.View,StaffAttendance.View,Students.View,Subjects.View,TeacherAttendance.View,TeacherContracts.View,Teachers.View,Timetable.View', NULL, NULL, 1, 0, CAST(N'2026-08-21T20:12:34.403' AS DateTime), CAST(N'2026-08-14T18:09:45.580' AS DateTime), CAST(N'2026-08-21T20:12:34.403' AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [UserName], [PasswordHash], [PasswordSalt], [RoleName], [Permissions], [Email], [Phone], [IsActive], [MustChangePassword], [LastLoginAt], [CreatedAt], [UpdatedAt], [FailedLoginAttempts], [LockedAt]) VALUES (3, N'ahmaddd', N'ahm', N'S4CjCz9AsBiGrqKF7cXa09Fc7Z42QPFKbdT4PEbwXys=', N'F2bQLLuawbYua7OmszpRIg==', N'المالية', N'Attendance.View,AuditLogs.View,ClassAssignment.View,Classes.View,Dashboard.View,Enrollment.View,Expenses.View,FeePlans.View,Fees.View,Grades.View,Library.View,Payroll.View,Reports.View,Roles.View,Rooms.View,Settings.View,StaffAttendance.View,Students.View,Subjects.View,TeacherAttendance.View,TeacherContracts.View,Teachers.View,Timetable.View,Transport.View,Users.View,Vouchers.View', NULL, NULL, 1, 0, CAST(N'2026-08-20T20:56:19.507' AS DateTime), CAST(N'2026-08-17T21:36:46.757' AS DateTime), CAST(N'2026-08-20T22:33:07.167' AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [UserName], [PasswordHash], [PasswordSalt], [RoleName], [Permissions], [Email], [Phone], [IsActive], [MustChangePassword], [LastLoginAt], [CreatedAt], [UpdatedAt], [FailedLoginAttempts], [LockedAt]) VALUES (4, N'muhannad ali', N'muhannad', N'Q49dyhmql6o5cy42hQhdcLH/bz1gQZCh+vkB2kzQeBs=', N'IqYTGhpGSVkFP/OQB386xA==', N'مدير المدرسة', N'Attendance.View,AuditLogs.View,ClassAssignment.View,Classes.View,Dashboard.View,Enrollment.View,Expenses.View,FeePlans.View,Fees.View,Grades.View,Library.View,Payroll.View,Reports.View,Roles.View,Rooms.View,Settings.View,StaffAttendance.View,Students.View,Subjects.View,TeacherAttendance.View,TeacherContracts.View,Teachers.View,Timetable.View,Transport.View,Users.View,Vouchers.View', NULL, NULL, 1, 0, CAST(N'2026-08-18T18:58:51.673' AS DateTime), CAST(N'2026-08-18T17:35:57.063' AS DateTime), CAST(N'2026-08-20T22:33:07.167' AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Permissions_PermissionKey]    Script Date: 8/31/2026 7:21:21 PM ******/
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [UX_Permissions_PermissionKey] UNIQUE NONCLUSTERED 
(
	[PermissionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Roles_RoleName]    Script Date: 8/31/2026 7:21:21 PM ******/
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [UX_Roles_RoleName] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_SchemaMigrations_Version]    Script Date: 8/31/2026 7:21:21 PM ******/
ALTER TABLE [dbo].[SchemaMigrations] ADD  CONSTRAINT [UQ_SchemaMigrations_Version] UNIQUE NONCLUSTERED 
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_UserPermissions_UserPermission]    Script Date: 8/31/2026 7:21:21 PM ******/
ALTER TABLE [dbo].[UserPermissions] ADD  CONSTRAINT [UQ_UserPermissions_UserPermission] UNIQUE NONCLUSTERED 
(
	[UserID] ASC,
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AcademicTerms] ADD  CONSTRAINT [DF_AcademicTerms_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AcademicTerms] ADD  CONSTRAINT [DF_AcademicTerms_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AcademicYearCurrent] ADD  DEFAULT ((0)) FOR [IsCurrent]
GO
ALTER TABLE [dbo].[AcademicYearCurrent] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AcademicYears] ADD  CONSTRAINT [DF_AcademicYears_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AcademicYears] ADD  CONSTRAINT [DF_AcademicYears_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AnnualClosings] ADD  CONSTRAINT [DF_AnnualClosings_Status]  DEFAULT (N'مفتوح') FOR [ClosingStatus]
GO
ALTER TABLE [dbo].[AnnualClosings] ADD  CONSTRAINT [DF_AnnualClosings_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AnnualMigrationLog] ADD  CONSTRAINT [DF_AnnualMigrationLog_Status]  DEFAULT (N'مخطط') FOR [MigrationStatus]
GO
ALTER TABLE [dbo].[AnnualMigrationLog] ADD  CONSTRAINT [DF_AnnualMigrationLog_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AuditLogs] ADD  CONSTRAINT [DF_AuditLogs_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[BookBorrowings] ADD  CONSTRAINT [DF_BookBorrowings_Status]  DEFAULT (N'معار') FOR [Status]
GO
ALTER TABLE [dbo].[BookBorrowings] ADD  CONSTRAINT [DF_BookBorrowings_Runtime_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_Copies]  DEFAULT ((0)) FOR [Copies]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_AvailableCopies]  DEFAULT ((0)) FOR [AvailableCopies]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Buses] ADD  DEFAULT ((0)) FOR [Capacity]
GO
ALTER TABLE [dbo].[Buses] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[BusRoutes] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Classes] ADD  CONSTRAINT [DF_Classes_GradeOrder]  DEFAULT ((0)) FOR [GradeOrder]
GO
ALTER TABLE [dbo].[Classes] ADD  CONSTRAINT [DF_Classes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Classes] ADD  CONSTRAINT [DF_Classes_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ClassSubjects] ADD  CONSTRAINT [DF_ClassSubjects_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ClassSubjects] ADD  CONSTRAINT [DF_ClassSubjects_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ClassSubjectTerms] ADD  CONSTRAINT [DF_ClassSubjectTerms_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ClassSubjectTerms] ADD  CONSTRAINT [DF_ClassSubjectTerms_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[DatabaseBackupHistory] ADD  CONSTRAINT [DF_DatabaseBackupHistory_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[DatabaseBackupHistory] ADD  CONSTRAINT [DF_DatabaseBackupHistory_IsVerified]  DEFAULT ((0)) FOR [IsVerified]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_ApplicationDate]  DEFAULT (CONVERT([date],getdate())) FOR [ApplicationDate]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_AcademicYear]  DEFAULT (N'') FOR [AcademicYear]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_Status]  DEFAULT (N'جديد') FOR [Status]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_RegistrationFee]  DEFAULT ((0)) FOR [RegistrationFee]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_PaidAmount]  DEFAULT ((0)) FOR [PaidAmount]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_HasBirthCertificate]  DEFAULT ((0)) FOR [HasBirthCertificate]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_HasGuardianId]  DEFAULT ((0)) FOR [HasGuardianId]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_HasPhoto]  DEFAULT ((0)) FOR [HasPhoto]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_HasLastCertificate]  DEFAULT ((0)) FOR [HasLastCertificate]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_HasMedicalReport]  DEFAULT ((0)) FOR [HasMedicalReport]
GO
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [DF_Enrollments_Runtime_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF_Expenses_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF_Expenses_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FeePlans] ADD  CONSTRAINT [DF_FeePlans_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[FeePlans] ADD  CONSTRAINT [DF_FeePlans_IsRequired]  DEFAULT ((1)) FOR [IsRequired]
GO
ALTER TABLE [dbo].[FeePlans] ADD  CONSTRAINT [DF_FeePlans_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_NetAmount]  DEFAULT ((0)) FOR [NetAmount]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_PaidAmount]  DEFAULT ((0)) FOR [PaidAmount]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_RemainingAmount]  DEFAULT ((0)) FOR [RemainingAmount]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_Status]  DEFAULT (N'غير مدفوع') FOR [Status]
GO
ALTER TABLE [dbo].[Fees] ADD  CONSTRAINT [DF_Fees_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_Runtime_Quiz1]  DEFAULT ((0)) FOR [Quiz1]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_Runtime_Quiz2]  DEFAULT ((0)) FOR [Quiz2]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_Runtime_CourseWork]  DEFAULT ((0)) FOR [CourseWork]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_Runtime_FinalExam]  DEFAULT ((0)) FOR [FinalExam]
GO
ALTER TABLE [dbo].[Marks] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_BasicSalary]  DEFAULT ((0)) FOR [BasicSalary]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_Allowances]  DEFAULT ((0)) FOR [Allowances]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_Deductions]  DEFAULT ((0)) FOR [Deductions]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Receipts] ADD  CONSTRAINT [DF_Receipts_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Receipts] ADD  CONSTRAINT [DF_Receipts_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_GrantedAt]  DEFAULT (sysutcdatetime()) FOR [GrantedAt]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsSystemRole]  DEFAULT ((0)) FOR [IsSystemRole]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Rooms] ADD  CONSTRAINT [DF_Rooms_Capacity]  DEFAULT ((0)) FOR [Capacity]
GO
ALTER TABLE [dbo].[Rooms] ADD  CONSTRAINT [DF_Rooms_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Rooms] ADD  CONSTRAINT [DF_Rooms_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SchemaMigrations] ADD  CONSTRAINT [DF_SchemaMigrations_AppliedAtUtc]  DEFAULT (getutcdate()) FOR [AppliedAtUtc]
GO
ALTER TABLE [dbo].[SchemaMigrations] ADD  CONSTRAINT [DF_SchemaMigrations_Success]  DEFAULT ((1)) FOR [Success]
GO
ALTER TABLE [dbo].[SchoolSections] ADD  CONSTRAINT [DF_SchoolSections_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SchoolSections] ADD  CONSTRAINT [DF_SchoolSections_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SchoolTimetable] ADD  CONSTRAINT [DF_SchoolTimetable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SchoolTimetable] ADD  CONSTRAINT [DF_SchoolTimetable_Runtime_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Stages] ADD  CONSTRAINT [DF_Stages_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Stages] ADD  CONSTRAINT [DF_Stages_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[StudentAttendance] ADD  DEFAULT (N'حاضر') FOR [Status]
GO
ALTER TABLE [dbo].[StudentAttendance] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[StudentClasses] ADD  CONSTRAINT [DF_StudentClasses_Runtime_AssignedDate]  DEFAULT (getdate()) FOR [AssignedDate]
GO
ALTER TABLE [dbo].[StudentFees] ADD  CONSTRAINT [DF_StudentFees_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[StudentFees] ADD  CONSTRAINT [DF_StudentFees_PaidAmount]  DEFAULT ((0)) FOR [PaidAmount]
GO
ALTER TABLE [dbo].[StudentFees] ADD  CONSTRAINT [DF_StudentFees_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (N'نشط') FOR [Status]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_Runtime_MaxDegree]  DEFAULT ((100)) FOR [MaxDegree]
GO
ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_Runtime_PassDegree]  DEFAULT ((50)) FOR [PassDegree]
GO
ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_Runtime_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_Runtime_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TeacherAttendance] ADD  CONSTRAINT [DF_TeacherAttendance_LateMinutes]  DEFAULT ((0)) FOR [LateMinutes]
GO
ALTER TABLE [dbo].[TeacherAttendance] ADD  CONSTRAINT [DF_TeacherAttendance_EarlyLeaveMinutes]  DEFAULT ((0)) FOR [EarlyLeaveMinutes]
GO
ALTER TABLE [dbo].[TeacherAttendance] ADD  CONSTRAINT [DF_TeacherAttendance_WorkHours]  DEFAULT ((0)) FOR [WorkHours]
GO
ALTER TABLE [dbo].[TeacherAttendance] ADD  CONSTRAINT [DF_TeacherAttendance_RecordedAt]  DEFAULT (getdate()) FOR [RecordedAt]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_BasicSalary]  DEFAULT ((0)) FOR [BasicSalary]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_HousingAllowance]  DEFAULT ((0)) FOR [HousingAllowance]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_TransportAllowance]  DEFAULT ((0)) FOR [TransportAllowance]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_OtherAllowances]  DEFAULT ((0)) FOR [OtherAllowances]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_Deductions]  DEFAULT ((0)) FOR [Deductions]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_StartDate]  DEFAULT (CONVERT([date],getdate())) FOR [StartDate]
GO
ALTER TABLE [dbo].[TeacherContracts] ADD  CONSTRAINT [DF_TeacherContracts_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT ((0)) FOR [BasicSalary]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT ((0)) FOR [TransportAllowance]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT ((0)) FOR [HousingAllowance]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT (N'نشط') FOR [Status]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserPermissions] ADD  CONSTRAINT [DF_UserPermissions_GrantedAt]  DEFAULT (getdate()) FOR [GrantedAt]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_AssignedAt]  DEFAULT (sysutcdatetime()) FOR [AssignedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [MustChangePassword]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FailedLoginAttempts]  DEFAULT ((0)) FOR [FailedLoginAttempts]
GO
ALTER TABLE [dbo].[Vouchers] ADD  CONSTRAINT [DF_Vouchers_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Vouchers] ADD  CONSTRAINT [DF_Vouchers_IsAutoGenerated]  DEFAULT ((0)) FOR [IsAutoGenerated]
GO
ALTER TABLE [dbo].[Vouchers] ADD  CONSTRAINT [DF_Vouchers_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[BookBorrowings]  WITH CHECK ADD  CONSTRAINT [FK_BookBorrowings_Books] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[BookBorrowings] CHECK CONSTRAINT [FK_BookBorrowings_Books]
GO
ALTER TABLE [dbo].[StudentTransportAssignments]  WITH CHECK ADD CONSTRAINT [FK_StudentTransportAssignments_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[StudentTransportAssignments] CHECK CONSTRAINT [FK_StudentTransportAssignments_Students]
GO
ALTER TABLE [dbo].[StudentTransportAssignments]  WITH CHECK ADD CONSTRAINT [FK_StudentTransportAssignments_BusRoutes] FOREIGN KEY([RouteID])
REFERENCES [dbo].[BusRoutes] ([RouteID])
GO
ALTER TABLE [dbo].[StudentTransportAssignments] CHECK CONSTRAINT [FK_StudentTransportAssignments_BusRoutes]
GO
ALTER TABLE [dbo].[BusRoutes]  WITH CHECK ADD  CONSTRAINT [FK_BusRoutes_Buses] FOREIGN KEY([BusID])
REFERENCES [dbo].[Buses] ([BusID])
GO
ALTER TABLE [dbo].[BusRoutes] CHECK CONSTRAINT [FK_BusRoutes_Buses]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Stages] FOREIGN KEY([StageID])
REFERENCES [dbo].[Stages] ([StageID])
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Stages]
GO
ALTER TABLE [dbo].[ClassSubjects]  WITH CHECK ADD  CONSTRAINT [FK_ClassSubjects_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[ClassSubjects] CHECK CONSTRAINT [FK_ClassSubjects_Classes]
GO
ALTER TABLE [dbo].[ClassSubjects]  WITH CHECK ADD  CONSTRAINT [FK_ClassSubjects_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[ClassSubjects] CHECK CONSTRAINT [FK_ClassSubjects_Subjects]
GO
ALTER TABLE [dbo].[ClassSubjectTerms]  WITH CHECK ADD  CONSTRAINT [FK_ClassSubjectTerms_ClassSubjects] FOREIGN KEY([ClassSubjectID])
REFERENCES [dbo].[ClassSubjects] ([ClassSubjectID])
GO
ALTER TABLE [dbo].[ClassSubjectTerms] CHECK CONSTRAINT [FK_ClassSubjectTerms_ClassSubjects]
GO
ALTER TABLE [dbo].[ClassSubjectTerms]  WITH CHECK ADD  CONSTRAINT [FK_ClassSubjectTerms_Terms] FOREIGN KEY([TermID])
REFERENCES [dbo].[AcademicTerms] ([TermID])
GO
ALTER TABLE [dbo].[ClassSubjectTerms] CHECK CONSTRAINT [FK_ClassSubjectTerms_Terms]
GO
ALTER TABLE [dbo].[FeePlans]  WITH CHECK ADD  CONSTRAINT [FK_FeePlans_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[FeePlans] CHECK CONSTRAINT [FK_FeePlans_Classes]
GO
ALTER TABLE [dbo].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Fees_FeePlans] FOREIGN KEY([FeePlanID])
REFERENCES [dbo].[FeePlans] ([FeePlanID])
GO
ALTER TABLE [dbo].[Fees] CHECK CONSTRAINT [FK_Fees_FeePlans]
GO
ALTER TABLE [dbo].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Fees_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[Fees] CHECK CONSTRAINT [FK_Fees_Students]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Classes]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Students]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Subjects]
GO
ALTER TABLE [dbo].[Marks]  WITH CHECK ADD  CONSTRAINT [FK_Marks_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Marks] CHECK CONSTRAINT [FK_Marks_Students]
GO
ALTER TABLE [dbo].[Marks]  WITH CHECK ADD  CONSTRAINT [FK_Marks_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[Marks] CHECK CONSTRAINT [FK_Marks_Subjects]
GO
ALTER TABLE [dbo].[Marks]  WITH CHECK ADD  CONSTRAINT [FK_Marks_Teachers] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teachers] ([TeacherID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Marks] CHECK CONSTRAINT [FK_Marks_Teachers]
GO
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [FK_Payroll_Teachers] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teachers] ([TeacherID])
GO
ALTER TABLE [dbo].[Payroll] CHECK CONSTRAINT [FK_Payroll_Teachers]
GO
ALTER TABLE [dbo].[Receipts]  WITH CHECK ADD  CONSTRAINT [FK_Receipts_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[Receipts] CHECK CONSTRAINT [FK_Receipts_Students]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionID])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles]
GO
ALTER TABLE [dbo].[SchoolSections]  WITH CHECK ADD  CONSTRAINT [FK_SchoolSections_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[SchoolSections] CHECK CONSTRAINT [FK_SchoolSections_Classes]
GO
ALTER TABLE [dbo].[SchoolTimetable]  WITH CHECK ADD  CONSTRAINT [FK_SchoolTimetable_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[SchoolTimetable] CHECK CONSTRAINT [FK_SchoolTimetable_Classes]
GO
ALTER TABLE [dbo].[SchoolTimetable]  WITH CHECK ADD  CONSTRAINT [FK_SchoolTimetable_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[SchoolTimetable] CHECK CONSTRAINT [FK_SchoolTimetable_Subjects]
GO
ALTER TABLE [dbo].[SchoolTimetable]  WITH CHECK ADD  CONSTRAINT [FK_SchoolTimetable_Teachers] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teachers] ([TeacherID])
GO
ALTER TABLE [dbo].[SchoolTimetable] CHECK CONSTRAINT [FK_SchoolTimetable_Teachers]
GO
ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
GO
ALTER TABLE [dbo].[StudentAttendance] CHECK CONSTRAINT [FK_StudentAttendance_Classes]
GO
ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[StudentAttendance] CHECK CONSTRAINT [FK_StudentAttendance_Students]
GO
ALTER TABLE [dbo].[StudentClasses]  WITH CHECK ADD  CONSTRAINT [FK_StudentClasses_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentClasses] CHECK CONSTRAINT [FK_StudentClasses_Classes]
GO
ALTER TABLE [dbo].[StudentClasses]  WITH CHECK ADD  CONSTRAINT [FK_StudentClasses_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentClasses] CHECK CONSTRAINT [FK_StudentClasses_Students]
GO
ALTER TABLE [dbo].[StudentFees]  WITH CHECK ADD  CONSTRAINT [FK_StudentFees_Students] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[StudentFees] CHECK CONSTRAINT [FK_StudentFees_Students]
GO
ALTER TABLE [dbo].[TeacherAttendance]  WITH CHECK ADD  CONSTRAINT [FK_TeacherAttendance_Teachers] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teachers] ([TeacherID])
GO
ALTER TABLE [dbo].[TeacherAttendance] CHECK CONSTRAINT [FK_TeacherAttendance_Teachers]
GO
ALTER TABLE [dbo].[TeacherContracts]  WITH CHECK ADD  CONSTRAINT [FK_TeacherContracts_Teachers] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teachers] ([TeacherID])
GO
ALTER TABLE [dbo].[TeacherContracts] CHECK CONSTRAINT [FK_TeacherContracts_Teachers]
GO
ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Permissions] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionID])
GO
ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Permissions]
GO
ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Users]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
ALTER TABLE [dbo].[AcademicYears]  WITH CHECK ADD  CONSTRAINT [CK_AcademicYears_Dates] CHECK  (([EndDate]>[StartDate]))
GO
ALTER TABLE [dbo].[AcademicYears] CHECK CONSTRAINT [CK_AcademicYears_Dates]
GO
ALTER TABLE [dbo].[AnnualClosings]  WITH CHECK ADD  CONSTRAINT [CK_AnnualClosings_Status] CHECK  (([ClosingStatus]=N'مؤرشف' OR [ClosingStatus]=N'مغلق' OR [ClosingStatus]=N'مفتوح'))
GO
ALTER TABLE [dbo].[AnnualClosings] CHECK CONSTRAINT [CK_AnnualClosings_Status]
GO
ALTER TABLE [dbo].[AnnualClosings]  WITH CHECK ADD  CONSTRAINT [CK_AnnualClosings_Year] CHECK  ((nullif(ltrim(rtrim([AcademicYear])),N'') IS NOT NULL))
GO
ALTER TABLE [dbo].[AnnualClosings] CHECK CONSTRAINT [CK_AnnualClosings_Year]
GO
ALTER TABLE [dbo].[AnnualMigrationLog]  WITH CHECK ADD  CONSTRAINT [CK_AnnualMigrationLog_Status] CHECK  (([MigrationStatus]=N'فشل' OR [MigrationStatus]=N'مستبعد' OR [MigrationStatus]=N'منفذ' OR [MigrationStatus]=N'مخطط'))
GO
ALTER TABLE [dbo].[AnnualMigrationLog] CHECK CONSTRAINT [CK_AnnualMigrationLog_Status]
GO
ALTER TABLE [dbo].[DatabaseBackupHistory]  WITH CHECK ADD  CONSTRAINT [CK_DatabaseBackupHistory_Type] CHECK  (([BackupType]=N'قبل الاستعادة' OR [BackupType]=N'قبل الإغلاق' OR [BackupType]=N'يدوي'))
GO
ALTER TABLE [dbo].[DatabaseBackupHistory] CHECK CONSTRAINT [CK_DatabaseBackupHistory_Type]
GO
ALTER TABLE [dbo].[SchoolSections]  WITH CHECK ADD  CONSTRAINT [CK_SchoolSections_AcademicYear_NotEmpty] CHECK  ((nullif(ltrim(rtrim([AcademicYear])),N'') IS NOT NULL))
GO
ALTER TABLE [dbo].[SchoolSections] CHECK CONSTRAINT [CK_SchoolSections_AcademicYear_NotEmpty]
GO
ALTER TABLE [dbo].[SchoolSections]  WITH CHECK ADD  CONSTRAINT [CK_SchoolSections_Capacity] CHECK  (([Capacity] IS NULL OR [Capacity]>(0)))
GO
ALTER TABLE [dbo].[SchoolSections] CHECK CONSTRAINT [CK_SchoolSections_Capacity]
GO
ALTER TABLE [dbo].[SchoolSections]  WITH CHECK ADD  CONSTRAINT [CK_SchoolSections_SectionName_NotEmpty] CHECK  ((nullif(ltrim(rtrim([SectionName])),N'') IS NOT NULL))
GO
ALTER TABLE [dbo].[SchoolSections] CHECK CONSTRAINT [CK_SchoolSections_SectionName_NotEmpty]
GO
/****** Object:  StoredProcedure [dbo].[ApproveStudentYearMigration]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* --------------------------------------------------------------------------------------------------
   6) dbo.ApproveStudentYearMigration — اعتماد وتنفيذ ترحيل طالب بين عامين دراسيين.
      معالجة معاملاتية آمنة مع تحقق كامل من الشعبة والسعة وحالة العام.
   -------------------------------------------------------------------------------------------------- */
CREATE   PROCEDURE [dbo].[ApproveStudentYearMigration]
    @MigrationID INT,
    @ToClassID INT,
    @ToSection NVARCHAR(50),
    @ApprovedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET XACT_ABORT ON;

    SET @ToSection = LTRIM(RTRIM(ISNULL(@ToSection, N'')));

    IF @MigrationID IS NULL OR @ToClassID IS NULL OR NULLIF(@ToSection, N'') IS NULL
        THROW 51030, N'يجب تحديد سجل الترحيل والصف والشعبة الجديدة.', 1;

    BEGIN TRANSACTION;

    DECLARE @StudentID INT, @FromYear NVARCHAR(20), @ToYear NVARCHAR(20),
            @FromClassID INT, @FromSection NVARCHAR(50), @Status NVARCHAR(20);

    SELECT @StudentID = StudentID, @FromYear = FromAcademicYear, @ToYear = ToAcademicYear,
           @FromClassID = FromClassID, @FromSection = FromSection, @Status = MigrationStatus
    FROM dbo.AnnualMigrationLog WITH (UPDLOCK, HOLDLOCK)
    WHERE MigrationID = @MigrationID;

    IF @StudentID IS NULL
        THROW 51031, N'سجل الترحيل غير موجود.', 1;

    IF @Status <> N'مخطط'
        THROW 51032, N'لا يمكن اعتماد سجل غير مخطط أو سبق تنفيذه.', 1;

    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear = @ToYear AND ClosingStatus IN (N'مغلق', N'مؤرشف'))
        THROW 51033, N'لا يمكن الترحيل إلى عام مغلق أو مؤرشف.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Classes WHERE ClassID = @ToClassID)
        THROW 51034, N'الصف الجديد غير موجود.', 1;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Enrollments e
        WHERE e.StudentID = @StudentID
          AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@ToYear, N'-', N'/')
          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted')
    )
        THROW 51035, N'لا يمكن اعتماد الترحيل قبل قبول تسجيل الطالب في العام الجديد.', 1;

    IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.SchoolSections
                       WHERE ClassID = @ToClassID AND SectionName = @ToSection
                         AND AcademicYear = @ToYear AND ISNULL(IsActive, 1) = 1)
        THROW 51036, N'الشعبة الجديدة غير موجودة أو غير فعالة في العام الجديد.', 1;

    IF EXISTS (SELECT 1 FROM dbo.StudentClasses
               WHERE StudentID = @StudentID
                 AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@ToYear, N'-', N'/'))
        THROW 51037, N'للطالب توزيع مسجل مسبقاً في العام الجديد.', 1;

    IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.SchoolSections ss
                   WHERE ss.ClassID = @ToClassID AND ss.SectionName = @ToSection
                     AND ss.AcademicYear = @ToYear AND ss.Capacity IS NOT NULL
                     AND (SELECT COUNT(*) FROM dbo.StudentClasses sc
                          WHERE sc.ClassID = @ToClassID AND sc.Section = @ToSection
                            AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@ToYear, N'-', N'/'))
                         >= ss.Capacity)
        THROW 51038, N'لا توجد مقاعد شاغرة في الشعبة الجديدة.', 1;

    INSERT dbo.StudentClasses (StudentID, ClassID, Section, AcademicYear, AssignedDate, AssignedBy)
    VALUES (@StudentID, @ToClassID, @ToSection, @ToYear, GETDATE(), @ApprovedByUserID);

    UPDATE dbo.Students
       SET ClassID = @ToClassID, Section = @ToSection, AcademicYear = @ToYear
     WHERE StudentID = @StudentID;

    UPDATE dbo.AnnualMigrationLog
       SET ToClassID = @ToClassID,
           ToSection = @ToSection,
           MigrationStatus = N'منفذ',
           CreatedByUserID = COALESCE(@ApprovedByUserID, CreatedByUserID),
           Notes = COALESCE(Notes, N'') + CASE WHEN LEN(ISNULL(Notes, N'')) > 0 THEN N' ' ELSE N'' END + N'تم الاعتماد والتنفيذ.'
     WHERE MigrationID = @MigrationID;

    COMMIT TRANSACTION;

    SELECT @MigrationID AS MigrationID, @StudentID AS StudentID, @ToYear AS AcademicYear, N'منفذ' AS MigrationStatus;
END;

GO
/****** Object:  StoredProcedure [dbo].[CloseAcademicYear]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CloseAcademicYear]
    @AcademicYear NVARCHAR(20),
    @NextAcademicYear NVARCHAR(20) = NULL,
    @ClosedByUserID INT = NULL,
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @AcademicYear=REPLACE(LTRIM(RTRIM(@AcademicYear)),N'-',N'/');
    SET @NextAcademicYear=REPLACE(NULLIF(LTRIM(RTRIM(@NextAcademicYear)),N''),N'-',N'/');
    IF NULLIF(@AcademicYear,N'') IS NULL THROW 51021,N'العام الدراسي غير صالح.',1;
    IF @NextAcademicYear=@AcademicYear THROW 51022,N'لا يمكن أن يكون العام التالي مطابقاً للعام المغلق.',1;

    DECLARE @Critical INT=0;
    DECLARE @Issues TABLE (CheckCode NVARCHAR(80), CheckName NVARCHAR(200), IssueCount INT, Severity NVARCHAR(20), Details NVARCHAR(500), Result NVARCHAR(20));
    INSERT @Issues EXEC dbo.VerifyAnnualClosing @AcademicYear;
    SELECT @Critical=COUNT(*) FROM @Issues WHERE Severity=N'حرج' AND IssueCount>0;
    IF @Critical>0 THROW 51023,N'لا يمكن إغلاق العام لوجود أخطاء حرجة. أصلح نتائج الفحص أولاً.',1;

    BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@AcademicYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51024,N'العام الدراسي مغلق مسبقاً.',1;
    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@AcademicYear)
        UPDATE dbo.AnnualClosings SET ClosingStatus=N'مغلق',VerifiedAt=GETDATE(),ClosedAt=GETDATE(),ClosedByUserID=@ClosedByUserID,NextAcademicYear=@NextAcademicYear,Notes=@Notes,UpdatedAt=GETDATE() WHERE AcademicYear=@AcademicYear;
    ELSE
        INSERT dbo.AnnualClosings(AcademicYear,ClosingStatus,VerifiedAt,ClosedAt,ClosedByUserID,NextAcademicYear,Notes) VALUES(@AcademicYear,N'مغلق',GETDATE(),GETDATE(),@ClosedByUserID,@NextAcademicYear,@Notes);
    COMMIT TRANSACTION;
    SELECT CAST(1 AS BIT) AS Success, @AcademicYear AS AcademicYear, @NextAcademicYear AS NextAcademicYear;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetDashboardOperationalStatus]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* --------------------------------------------------------------------------------------------------
   3) dbo.GetDashboardOperationalStatus — حالة التشغيل للوحة المعلومات.
      ملاحظة عقد: العام النشط يُقرأ من الدالة الموحدة (وليس من جدول إعدادات ثانٍ).
   -------------------------------------------------------------------------------------------------- */
CREATE   PROCEDURE [dbo].[GetDashboardOperationalStatus]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dbo.GetActiveAcademicYear()                                                              AS ActiveAcademicYear,
        (SELECT TOP (1) BackupFile    FROM dbo.DatabaseBackupHistory ORDER BY BackupHistoryID DESC) AS LastBackupFile,
        (SELECT TOP (1) CreatedAt     FROM dbo.DatabaseBackupHistory ORDER BY BackupHistoryID DESC) AS LastBackupDate,
        (SELECT TOP (1) AcademicYear  FROM dbo.AnnualClosings        ORDER BY ClosingID DESC) AS LastClosingYear,
        (SELECT TOP (1) ClosingStatus FROM dbo.AnnualClosings        ORDER BY ClosingID DESC) AS LastClosingStatus,
        (SELECT TOP (1) VerifiedAt    FROM dbo.AnnualClosings        ORDER BY ClosingID DESC) AS LastClosingVerifiedAt;
END;

GO
/****** Object:  StoredProcedure [dbo].[GetStudentMigrationReport]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentMigrationReport]
    @FromAcademicYear NVARCHAR(20),
    @ToAcademicYear NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET @FromAcademicYear=REPLACE(LTRIM(RTRIM(@FromAcademicYear)),N'-',N'/');
    SET @ToAcademicYear=REPLACE(LTRIM(RTRIM(@ToAcademicYear)),N'-',N'/');
    IF NULLIF(@FromAcademicYear,N'') IS NULL OR NULLIF(@ToAcademicYear,N'') IS NULL OR @FromAcademicYear=@ToAcademicYear
        THROW 51028,N'يجب تحديد عامين مختلفين لتقرير الترحيل.',1;
    IF OBJECT_ID(N'dbo.StudentClasses',N'U') IS NULL OR OBJECT_ID(N'dbo.Students',N'U') IS NULL
        THROW 51029,N'جداول الطلاب والتوزيع غير موجودة.',1;

    ;WITH SourceAssignments AS
    (
        SELECT sc.StudentID, sc.ClassID, sc.Section,
               COUNT(*) OVER (PARTITION BY sc.StudentID) AS AssignmentCount
        FROM dbo.StudentClasses sc
        WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@FromAcademicYear
    )
    SELECT sa.StudentID,
           s.FullName AS StudentName,
           sa.ClassID AS FromClassID,
           sa.Section AS FromSection,
           sa.AssignmentCount,
           ml.MigrationID,
           ml.MigrationStatus,
           CASE
             WHEN sa.AssignmentCount > 1 THEN N'مستبعد - توزيع مكرر'
             WHEN ml.MigrationID IS NOT NULL AND ml.MigrationStatus=N'منفذ' THEN N'منقول'
             WHEN ml.MigrationID IS NOT NULL AND ml.MigrationStatus=N'مستبعد' THEN N'مستبعد'
             WHEN ml.MigrationID IS NOT NULL THEN N'مخطط'
             ELSE N'مرشح - يحتاج اعتماداً'
           END AS MigrationResult
    FROM SourceAssignments sa
    INNER JOIN dbo.Students s ON s.StudentID=sa.StudentID
    OUTER APPLY
    (
        SELECT TOP (1) m.MigrationID, m.MigrationStatus
        FROM dbo.AnnualMigrationLog m
        WHERE m.StudentID=sa.StudentID AND m.FromAcademicYear=@FromAcademicYear AND m.ToAcademicYear=@ToAcademicYear
        ORDER BY m.MigrationID DESC
    ) ml
    ORDER BY MigrationResult, s.FullName, sa.StudentID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PlanStudentYearMigration]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PlanStudentYearMigration]
    @FromAcademicYear NVARCHAR(20),
    @ToAcademicYear NVARCHAR(20),
    @CreatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @FromAcademicYear=REPLACE(LTRIM(RTRIM(@FromAcademicYear)),N'-',N'/');
    SET @ToAcademicYear=REPLACE(LTRIM(RTRIM(@ToAcademicYear)),N'-',N'/');
    IF NULLIF(@FromAcademicYear,N'') IS NULL OR NULLIF(@ToAcademicYear,N'') IS NULL OR @FromAcademicYear=@ToAcademicYear
        THROW 51025,N'يجب تحديد عامين مختلفين للترحيل.',1;
    IF NOT EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear=@FromAcademicYear AND ClosingStatus IN (N'مغلق',N'مؤرشف'))
        THROW 51026,N'يجب إغلاق العام السابق قبل تخطيط الترحيل.',1;
    IF OBJECT_ID(N'dbo.StudentClasses',N'U') IS NULL THROW 51027,N'جدول توزيع الطلاب غير موجود.',1;

    INSERT dbo.AnnualMigrationLog(StudentID,FromAcademicYear,ToAcademicYear,FromClassID,FromSection,MigrationStatus,CreatedByUserID)
    SELECT sc.StudentID,@FromAcademicYear,@ToAcademicYear,sc.ClassID,sc.Section,N'مخطط',@CreatedByUserID
    FROM dbo.StudentClasses sc
    INNER JOIN dbo.Students s ON s.StudentID=sc.StudentID
    WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@FromAcademicYear
      /* يعتمد الترحيل على وجود توزيع للطالب في العام المغلق؛
         لا نستخدم Students.IsActive لأنه غير موجود في المخطط الفعلي. */
      AND NOT EXISTS (SELECT 1 FROM dbo.AnnualMigrationLog ml WHERE ml.StudentID=sc.StudentID AND ml.FromAcademicYear=@FromAcademicYear AND ml.ToAcademicYear=@ToAcademicYear);

    SELECT MigrationID,StudentID,FromAcademicYear,ToAcademicYear,FromClassID,FromSection,MigrationStatus FROM dbo.AnnualMigrationLog WHERE FromAcademicYear=@FromAcademicYear AND ToAcademicYear=@ToAcademicYear ORDER BY MigrationID;
END;
GO
/****** Object:  StoredProcedure [dbo].[RegisterDatabaseBackup]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* --------------------------------------------------------------------------------------------------
   5) dbo.RegisterDatabaseBackup — تسجيل النسخ الاحتياطية قبل الإغلاق السنوي.
   -------------------------------------------------------------------------------------------------- */
CREATE   PROCEDURE [dbo].[RegisterDatabaseBackup]
    @BackupFile  NVARCHAR(500),
    @BackupType  NVARCHAR(50),
    @AcademicYear NVARCHAR(20) = NULL,
    @UserID       INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NULLIF(LTRIM(RTRIM(@BackupFile)), N'') IS NULL
        THROW 51103, N'ملف النسخة الاحتياطية غير صالح.', 1;

    INSERT dbo.DatabaseBackupHistory (BackupFile, BackupType, AcademicYear, CreatedByUserID, IsVerified)
    VALUES (@BackupFile, @BackupType, NULLIF(LTRIM(RTRIM(@AcademicYear)), N''), @UserID, 1);

    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS BackupHistoryID;
END;

GO
/****** Object:  StoredProcedure [dbo].[SetActiveAcademicYear]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* --------------------------------------------------------------------------------------------------
   4) dbo.SetActiveAcademicYear — تعيين العام الدراسي النشط.
   -------------------------------------------------------------------------------------------------- */
CREATE   PROCEDURE [dbo].[SetActiveAcademicYear]
    @AcademicYear NVARCHAR(20),
    @UserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @AcademicYear = REPLACE(NULLIF(LTRIM(RTRIM(@AcademicYear)), N''), N'-', N'/');

    IF @AcademicYear IS NULL
        THROW 51101, N'العام الدراسي النشط غير صالح.', 1;

    IF EXISTS (SELECT 1 FROM dbo.AnnualClosings WHERE AcademicYear = @AcademicYear AND ClosingStatus IN (N'مغلق', N'مؤرشف'))
        THROW 51102, N'لا يمكن جعل عام مغلق هو العام النشط.', 1;

    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM dbo.SystemAcademicSettings)
        UPDATE dbo.SystemAcademicSettings
           SET ActiveAcademicYear = @AcademicYear,
               UpdatedByUserID = @UserID,
               UpdatedAt = GETDATE();
    ELSE
        INSERT dbo.SystemAcademicSettings (ActiveAcademicYear, UpdatedByUserID)
        VALUES (@AcademicYear, @UserID);

    COMMIT TRANSACTION;

    SELECT @AcademicYear AS ActiveAcademicYear;
END;

GO
/****** Object:  StoredProcedure [dbo].[VerifyAnnualClosing]    Script Date: 8/31/2026 7:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VerifyAnnualClosing]
    @AcademicYear NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET @AcademicYear = REPLACE(LTRIM(RTRIM(@AcademicYear)), N'-', N'/');
    IF NULLIF(@AcademicYear, N'') IS NULL
        THROW 51020, N'يجب تحديد العام الدراسي قبل الفحص.', 1;

    DECLARE @Issues TABLE
    (
        CheckCode NVARCHAR(80) NOT NULL,
        CheckName NVARCHAR(200) NOT NULL,
        IssueCount INT NOT NULL,
        Severity NVARCHAR(20) NOT NULL,
        Details NVARCHAR(500) NULL
    );

    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'STUDENT_ASSIGNMENT_DUPLICATE', N'توزيع الطالب المكرر في العام',
         ISNULL((SELECT COUNT(*) FROM (SELECT StudentID FROM dbo.StudentClasses WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear GROUP BY StudentID HAVING COUNT(*)>1) x),0), N'حرج', N'يجب أن يكون لكل طالب توزيع فعّال واحد في العام.'),
        (N'STUDENT_ASSIGNMENT_ORPHAN', N'توزيع يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.StudentClasses sc LEFT JOIN dbo.Students s ON s.StudentID=sc.StudentID WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'سجل توزيع بلا طالب.'),
        (N'STUDENT_ASSIGNMENT_ORPHAN_CLASS', N'توزيع يشير إلى صف غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.StudentClasses sc LEFT JOIN dbo.Classes c ON c.ClassID=sc.ClassID WHERE REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=@AcademicYear AND c.ClassID IS NULL),0), N'حرج', N'سجل توزيع بلا صف.');
    END;

    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'ENROLLMENT_ORPHAN', N'تسجيل يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.Enrollments e LEFT JOIN dbo.Students s ON s.StudentID=e.StudentID WHERE REPLACE(ISNULL(e.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'سجل تسجيل بلا طالب.'),
        (N'ENROLLMENT_DUPLICATE', N'التسجيل المكرر للطالب في العام',
         ISNULL((SELECT COUNT(*) FROM (SELECT StudentID FROM dbo.Enrollments WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear GROUP BY StudentID HAVING COUNT(*)>1) x),0), N'حرج', N'يجب مراجعة التسجيلات المكررة قبل الإغلاق.');
    END;

    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'FEE_ORPHAN', N'رسم يشير إلى طالب غير موجود',
         ISNULL((SELECT COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.Students s ON s.StudentID=f.StudentID WHERE REPLACE(ISNULL(f.AcademicYear,N''),N'-',N'/')=@AcademicYear AND s.StudentID IS NULL),0), N'حرج', N'الرسم بلا طالب.'),
        (N'FEE_NEGATIVE', N'مبلغ رسم سالب أو صافي غير صحيح',
         ISNULL((SELECT COUNT(*) FROM dbo.Fees WHERE REPLACE(ISNULL(AcademicYear,N''),N'-',N'/')=@AcademicYear AND (ISNULL(TotalAmount,0)<0 OR ISNULL(PaidAmount,0)<0 OR ISNULL(RemainingAmount,0)<0 OR ISNULL(PaidAmount,0)>ISNULL(NetAmount,ISNULL(TotalAmount,0)))),0), N'حرج', N'مراجعة المبالغ والدفعات.');
    END;

    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL
    BEGIN
        INSERT @Issues VALUES
        (N'VOUCHER_DUPLICATE_NUMBER', N'رقم السند المكرر',
         ISNULL((SELECT COUNT(*) FROM (SELECT VoucherNumber FROM dbo.Vouchers WHERE NULLIF(LTRIM(RTRIM(VoucherNumber)),N'') IS NOT NULL GROUP BY VoucherNumber HAVING COUNT(*)>1) x),0), N'حرج', N'رقم السند يجب أن يكون فريداً.');
    END;

    SELECT CheckCode, CheckName, IssueCount, Severity, Details,
           CASE WHEN IssueCount=0 THEN N'سليم' ELSE N'يحتاج معالجة' END AS Result
    FROM @Issues ORDER BY CASE Severity WHEN N'حرج' THEN 1 ELSE 2 END, CheckCode;
END;
GO

/*
   Production reference seeding
   ----------------------------
   Only reference/setup data is seeded here:
     - active academic year and academic terms
     - global subject catalog (not tied to a class)
     - school settings
     - users, roles and permissions

   Classes, sections, fee plans, students, teachers and all operational
   transactions are intentionally NOT seeded. They are entered from the UI.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT dbo.AcademicYears (AcademicYear, StartDate, EndDate, IsActive)
    SELECT N'2026/2027',
           CONVERT(date, '2026-09-01'),
           CONVERT(date, '2027-06-30'),
           CAST(1 AS bit)
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicYears
        WHERE REPLACE(AcademicYear, N'-', N'/') = N'2026/2027'
    );

    INSERT dbo.AcademicTerms (TermCode, TermName, TermOrder, IsActive)
    SELECT v.TermCode, v.TermName, v.TermOrder, CAST(1 AS bit)
    FROM
    (
        VALUES
            (N'TERM-1', N'الفصل الدراسي الأول', 1),
            (N'TERM-2', N'الفصل الدراسي الثاني', 2)
    ) v(TermCode, TermName, TermOrder)
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.AcademicTerms t
        WHERE t.TermCode = v.TermCode
    );

    /* كتالوج مواد عام؛ الربط بالصف يتم لاحقًا من واجهة إدارة المواد. */
    INSERT dbo.Subjects
    (
        SubjectName,
        SubjectCode,
        ClassID,
        MaxDegree,
        PassDegree,
        IsActive,
        Notes,
        CreatedAt
    )
    SELECT v.SubjectName,
           N'MAT-' +
           RIGHT(
               N'000000' +
               CONVERT(
                   NVARCHAR(6),
                   ROW_NUMBER() OVER (ORDER BY v.SubjectCode)),
               6),
           NULL,
           CAST(100 AS decimal(10,2)),
           CAST(50 AS decimal(10,2)),
           CAST(1 AS bit),
           N'مادة مرجعية عامة؛ يتم ربطها بالصف من الواجهة.',
           GETDATE()
    FROM
    (
        VALUES
            (N'AR',  N'اللغة العربية'),
            (N'EN',  N'اللغة الإنجليزية'),
            (N'MA',  N'الرياضيات'),
            (N'IS',  N'التربية الإسلامية'),
            (N'QUR', N'القرآن الكريم'),
            (N'SCI', N'العلوم'),
            (N'SOC', N'الدراسات الاجتماعية'),
            (N'CS',  N'الحاسوب وتقنية المعلومات'),
            (N'PH',  N'الفيزياء'),
            (N'CH',  N'الكيمياء'),
            (N'BI',  N'الأحياء'),
            (N'HIS', N'التاريخ'),
            (N'GEO', N'الجغرافيا'),
            (N'PE',  N'التربية البدنية والصحية'),
            (N'ART', N'التربية الفنية')
    ) v(SubjectCode, SubjectName)
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Subjects s
        WHERE LTRIM(RTRIM(s.SubjectName)) = v.SubjectName
          AND s.ClassID IS NULL
    );

    INSERT dbo.SchoolSettings (SchoolSettingsID, SchoolNameAr, SchoolNameEn)
    SELECT 1, N'اسم المدرسة', N'School Name'
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.SchoolSettings
        WHERE SchoolSettingsID = 1
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* ======================================================================
   Add-only RBAC baseline aligned with Security/PermissionKeys.cs
   ====================================================================== */

DECLARE @PermissionRows TABLE
(
    PermissionKey NVARCHAR(150) NOT NULL,
    DisplayName NVARCHAR(250) NOT NULL,
    ModuleName NVARCHAR(100) NOT NULL,
    ActionName NVARCHAR(100) NOT NULL
);
INSERT @PermissionRows(PermissionKey, DisplayName, ModuleName, ActionName)
VALUES
(N'Dashboard.View',N'عرض لوحة التحكم',N'Dashboard',N'View'),
(N'Students.View',N'عرض الطلاب',N'Students',N'View'),(N'Students.Manage',N'إدارة الطلاب',N'Students',N'Manage'),
(N'Enrollment.Manage',N'إدارة القبول والتسجيل',N'Enrollment',N'Manage'),
(N'ClassAssignment.View',N'عرض توزيع الطلاب',N'ClassAssignment',N'View'),(N'ClassAssignment.Manage',N'إدارة توزيع الطلاب',N'ClassAssignment',N'Manage'),
(N'Teachers.Manage',N'إدارة المعلمين',N'Teachers',N'Manage'),(N'StaffAttendance.Manage',N'إدارة حضور الموظفين',N'StaffAttendance',N'Manage'),
(N'Payroll.Manage',N'إدارة الرواتب',N'Payroll',N'Manage'),(N'Subjects.Manage',N'إدارة المواد',N'Subjects',N'Manage'),
(N'Classes.Manage',N'إدارة الصفوف',N'Classes',N'Manage'),(N'Timetable.Manage',N'إدارة الجدول',N'Timetable',N'Manage'),
(N'Attendance.Manage',N'إدارة الحضور',N'Attendance',N'Manage'),(N'Grades.Manage',N'إدارة الدرجات',N'Grades',N'Manage'),
(N'Fees.Manage',N'إدارة الرسوم',N'Fees',N'Manage'),(N'FeePlans.Manage',N'إدارة خطط الرسوم',N'FeePlans',N'Manage'),
(N'Vouchers.Manage',N'إدارة السندات',N'Vouchers',N'Manage'),(N'Expenses.Manage',N'إدارة المصروفات',N'Expenses',N'Manage'),
(N'Library.View',N'عرض المكتبة',N'Library',N'View'),(N'Library.Manage',N'إدارة المكتبة',N'Library',N'Manage'),
(N'Transport.Manage',N'إدارة النقل',N'Transport',N'Manage'),
(N'Reports.View',N'عرض التقارير',N'Reports',N'View'),(N'Users.Manage',N'إدارة المستخدمين',N'Users',N'Manage'),
(N'AuditLogs.View',N'عرض سجل التدقيق',N'AuditLogs',N'View'),(N'Settings.Manage',N'إدارة الإعدادات',N'Settings',N'Manage'),
(N'Users.View',N'عرض المستخدمين',N'Users',N'View'),(N'Users.Add',N'إضافة المستخدمين',N'Users',N'Add'),
(N'Users.Edit',N'تعديل المستخدمين',N'Users',N'Edit'),(N'Users.Delete',N'حذف المستخدمين',N'Users',N'Delete'),
(N'Users.ManageRoles',N'إدارة أدوار المستخدمين',N'Users',N'ManageRoles'),
(N'Roles.View',N'عرض الأدوار',N'Roles',N'View'),(N'Roles.Add',N'إضافة الأدوار',N'Roles',N'Add'),
(N'Roles.Edit',N'تعديل الأدوار',N'Roles',N'Edit'),(N'Roles.Delete',N'حذف الأدوار',N'Roles',N'Delete'),
(N'Roles.Manage',N'إدارة الأدوار',N'Roles',N'Manage'),(N'Permissions.Manage',N'إدارة الصلاحيات',N'Permissions',N'Manage'),
(N'AuditLogs.ExportExcel',N'تصدير سجل التدقيق إلى Excel',N'AuditLogs',N'ExportExcel'),
(N'AuditLogs.ExportPDF',N'تصدير سجل التدقيق إلى PDF',N'AuditLogs',N'ExportPDF'),
(N'AuditLogs.Print',N'طباعة سجل التدقيق',N'AuditLogs',N'Print'),
(N'Settings.View',N'عرض الإعدادات',N'Settings',N'View'),(N'Settings.Edit',N'تعديل الإعدادات',N'Settings',N'Edit'),
(N'AnnualClosing.View',N'عرض الإغلاق السنوي',N'AnnualClosing',N'View'),
(N'AnnualClosing.Manage',N'إدارة الإغلاق السنوي',N'AnnualClosing',N'Manage'),
(N'AnnualClosing.Approve',N'اعتماد الإغلاق السنوي',N'AnnualClosing',N'Approve');
INSERT dbo.Permissions(PermissionKey, DisplayName, ModuleName, ActionName)
SELECT p.PermissionKey, p.DisplayName, p.ModuleName, p.ActionName
FROM @PermissionRows p
WHERE NOT EXISTS (SELECT 1 FROM dbo.Permissions x WHERE x.PermissionKey = p.PermissionKey);
INSERT dbo.Roles(RoleName, Description, IsSystemRole)
SELECT N'مدير النظام', N'الدور الافتراضي لإدارة النظام', 1
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'مدير النظام');
INSERT dbo.Roles(RoleName, Description, IsSystemRole)
SELECT N'مستخدم', N'دور استخدام محدود قابل للتخصيص', 0
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'مستخدم');
INSERT dbo.RolePermissions(RoleID, PermissionID)
SELECT r.RoleID, p.PermissionID
FROM dbo.Roles r CROSS JOIN dbo.Permissions p
WHERE r.RoleName = N'مدير النظام'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolePermissions rp WHERE rp.RoleID = r.RoleID AND rp.PermissionID = p.PermissionID);
GO


