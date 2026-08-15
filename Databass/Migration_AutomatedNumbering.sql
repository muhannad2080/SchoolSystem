/*
    SchoolSystem - Automated Numbering Hardening
    الهدف: حماية أرقام الطلاب والموظفين وأرقام الجلوس على مستوى قاعدة البيانات.
    يمكن تشغيل هذا الملف أكثر من مرة بأمان.
*/
USE SchoolDB;
GO

SET NOCOUNT ON;

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
        PRINT N'تحذير: توجد أرقام طلاب مكررة؛ لم يتم إنشاء القيد الفريد حتى تتم معالجة التكرار.';
    ELSE IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE name = N'UX_Students_StudentNumber'
          AND object_id = OBJECT_ID(N'dbo.Students')
    )
        CREATE UNIQUE INDEX UX_Students_StudentNumber
            ON dbo.Students(StudentNumber)
            WHERE StudentNumber IS NOT NULL AND LTRIM(RTRIM(StudentNumber)) <> N'';
END;
GO

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
        PRINT N'تحذير: توجد أرقام موظفين مكررة؛ لم يتم إنشاء القيد الفريد حتى تتم معالجة التكرار.';
    ELSE IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE name = N'UX_Teachers_EmployeeNumber'
          AND object_id = OBJECT_ID(N'dbo.Teachers')
    )
        CREATE UNIQUE INDEX UX_Teachers_EmployeeNumber
            ON dbo.Teachers(EmployeeNumber)
            WHERE EmployeeNumber IS NOT NULL AND LTRIM(RTRIM(EmployeeNumber)) <> N'';
END;
GO

IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT AcademicYear, ClassID, ISNULL(Section, N''), SeatNumber
        FROM dbo.Enrollments
        WHERE NULLIF(LTRIM(RTRIM(SeatNumber)), N'') IS NOT NULL
          AND ISNULL(Status, N'') <> N'مرفوض'
        GROUP BY AcademicYear, ClassID, ISNULL(Section, N''), SeatNumber
        HAVING COUNT(*) > 1
    )
        PRINT N'تحذير: توجد أرقام جلوس مكررة ضمن نفس العام/الفصل/الشعبة؛ لم يتم إنشاء القيد حتى تتم المعالجة.';
    ELSE IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE name = N'UX_Enrollments_SeatNumberScope'
          AND object_id = OBJECT_ID(N'dbo.Enrollments')
    )
        CREATE UNIQUE INDEX UX_Enrollments_SeatNumberScope
            ON dbo.Enrollments(AcademicYear, ClassID, Section, SeatNumber)
            WHERE SeatNumber IS NOT NULL AND LTRIM(RTRIM(SeatNumber)) <> N'' AND (Status IS NULL OR Status <> N'مرفوض');
END;
GO

PRINT N'اكتملت ترقية حماية الترقيم التلقائي.';
GO
