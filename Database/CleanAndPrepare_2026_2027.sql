IF DB_ID(N'SchoolDB') IS NULL
BEGIN
    THROW 50000, N'قاعدة البيانات SchoolDB غير موجودة. هذا السكربت مخصّص للتنظيف بعد التثبيت فقط.', 1;
END
GO

USE [SchoolDB];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @CurrentAcademicYear NVARCHAR(20) = N'2026/2027';

    /* -----------------------------------------------------------------
       1) تنظيف البيانات التشغيلية والتجريبية
       ----------------------------------------------------------------- */
    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL DELETE FROM dbo.StudentAttendance;
    IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NOT NULL DELETE FROM dbo.TeacherAttendance;
    IF OBJECT_ID(N'dbo.Grades', N'U') IS NOT NULL DELETE FROM dbo.Grades;
    IF OBJECT_ID(N'dbo.Marks', N'U') IS NOT NULL DELETE FROM dbo.Marks;
    IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL DELETE FROM dbo.SchoolTimetable;
    IF OBJECT_ID(N'dbo.Receipts', N'U') IS NOT NULL DELETE FROM dbo.Receipts;
    IF OBJECT_ID(N'dbo.Fees', N'U') IS NOT NULL DELETE FROM dbo.Fees;
    IF OBJECT_ID(N'dbo.StudentFees', N'U') IS NOT NULL DELETE FROM dbo.StudentFees;
    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL DELETE FROM dbo.Enrollments;
    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL DELETE FROM dbo.StudentClasses;
    IF OBJECT_ID(N'dbo.TeacherContracts', N'U') IS NOT NULL DELETE FROM dbo.TeacherContracts;
    IF OBJECT_ID(N'dbo.Payroll', N'U') IS NOT NULL DELETE FROM dbo.Payroll;
    IF OBJECT_ID(N'dbo.BookBorrowings', N'U') IS NOT NULL DELETE FROM dbo.BookBorrowings;
    IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NOT NULL DELETE FROM dbo.Vouchers;
    IF OBJECT_ID(N'dbo.Expenses', N'U') IS NOT NULL DELETE FROM dbo.Expenses;
    IF OBJECT_ID(N'dbo.AnnualMigrationLog', N'U') IS NOT NULL DELETE FROM dbo.AnnualMigrationLog;
    IF OBJECT_ID(N'dbo.AnnualClosings', N'U') IS NOT NULL DELETE FROM dbo.AnnualClosings;
    IF OBJECT_ID(N'dbo.DatabaseBackupHistory', N'U') IS NOT NULL DELETE FROM dbo.DatabaseBackupHistory;
    IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NOT NULL DELETE FROM dbo.AuditLogs;

    IF OBJECT_ID(N'dbo.ClassSubjectTerms', N'U') IS NOT NULL DELETE FROM dbo.ClassSubjectTerms;
    IF OBJECT_ID(N'dbo.ClassSubjects', N'U') IS NOT NULL DELETE FROM dbo.ClassSubjects;
    IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NOT NULL DELETE FROM dbo.FeePlans;
    IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NOT NULL DELETE FROM dbo.SchoolSections;
    IF OBJECT_ID(N'dbo.Subjects', N'U') IS NOT NULL DELETE FROM dbo.Subjects;
    IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL DELETE FROM dbo.Classes;
    IF OBJECT_ID(N'dbo.Stages', N'U') IS NOT NULL DELETE FROM dbo.Stages;
    IF OBJECT_ID(N'dbo.AcademicTerms', N'U') IS NOT NULL DELETE FROM dbo.AcademicTerms;
    IF OBJECT_ID(N'dbo.AcademicYearCurrent', N'U') IS NOT NULL DELETE FROM dbo.AcademicYearCurrent;
    IF OBJECT_ID(N'dbo.AcademicYears', N'U') IS NOT NULL DELETE FROM dbo.AcademicYears;

    /* -----------------------------------------------------------------
       2) إعادة تثبيت العام الدراسي الحالي فقط
       ----------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicYears WHERE AcademicYear = @CurrentAcademicYear)
    BEGIN
        INSERT dbo.AcademicYears (AcademicYear, StartDate, EndDate, IsActive)
        VALUES (@CurrentAcademicYear, CONVERT(date, '2026-09-01'), CONVERT(date, '2027-06-30'), 1);
    END
    ELSE
    BEGIN
        UPDATE dbo.AcademicYears
        SET StartDate = CONVERT(date, '2026-09-01'),
            EndDate = CONVERT(date, '2027-06-30'),
            IsActive = 1
        WHERE AcademicYear = @CurrentAcademicYear;
    END

    DELETE FROM dbo.AcademicYears WHERE AcademicYear <> @CurrentAcademicYear;

    INSERT dbo.AcademicYearCurrent (AcademicYear, StartDate, EndDate, IsCurrent, Notes, CreatedAt, UpdatedAt)
    VALUES (@CurrentAcademicYear, CONVERT(date, '2026-09-01'), CONVERT(date, '2027-06-30'), 1, N'العام التشغيلي الحالي', GETDATE(), GETDATE());

    /* -----------------------------------------------------------------
       3) تثبيت المراحل والصفوف المرجعية
       ----------------------------------------------------------------- */
    INSERT dbo.Stages (StageCode, StageName, StageOrder, IsActive)
    SELECT v.StageCode, v.StageName, v.StageOrder, 1
    FROM (VALUES
        (N'BASIC', N'المرحلة الأساسية', 1),
        (N'PREP',  N'المرحلة الإعدادية', 2),
        (N'SEC',   N'المرحلة الثانوية', 3)
    ) v(StageCode, StageName, StageOrder)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Stages s WHERE s.StageCode = v.StageCode);

    INSERT dbo.Classes (ClassCode, ClassName, StageName, StageID, GradeOrder, IsActive, Notes)
    SELECT v.ClassCode, v.ClassName, s.StageName, s.StageID, v.GradeOrder, 1, N'صف مرجعي أساسي للنظام'
    FROM (VALUES
        (N'BASIC-01',N'الأول الأساسي',1,N'BASIC'),
        (N'BASIC-02',N'الثاني الأساسي',2,N'BASIC'),
        (N'BASIC-03',N'الثالث الأساسي',3,N'BASIC'),
        (N'BASIC-04',N'الرابع الأساسي',4,N'BASIC'),
        (N'BASIC-05',N'الخامس الأساسي',5,N'BASIC'),
        (N'BASIC-06',N'السادس الأساسي',6,N'BASIC'),
        (N'PREP-01',N'الأول الإعدادي',7,N'PREP'),
        (N'PREP-02',N'الثاني الإعدادي',8,N'PREP'),
        (N'PREP-03',N'الثالث الإعدادي',9,N'PREP'),
        (N'SEC-01',N'الأول الثانوي',10,N'SEC'),
        (N'SEC-02',N'الثاني الثانوي',11,N'SEC'),
        (N'SEC-03',N'الثالث الثانوي',12,N'SEC')
    ) v(ClassCode, ClassName, GradeOrder, StageCode)
    INNER JOIN dbo.Stages s ON s.StageCode = v.StageCode
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Classes c
        WHERE c.ClassCode = v.ClassCode OR c.ClassName = v.ClassName
    );

    INSERT dbo.AcademicTerms (TermCode, TermName, TermOrder, IsActive)
    SELECT v.TermCode, v.TermName, v.TermOrder, 1
    FROM (VALUES
        (N'TERM-1', N'الفصل الدراسي الأول', 1),
        (N'TERM-2', N'الفصل الدراسي الثاني', 2)
    ) v(TermCode, TermName, TermOrder)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.AcademicTerms t WHERE t.TermCode = v.TermCode);

    /* -----------------------------------------------------------------
       4) تثبيت المواد الرسمية وربطها بالصفوف والفصول
       ----------------------------------------------------------------- */
    DECLARE @SubjectRows TABLE
    (
        Suffix NVARCHAR(10) NOT NULL,
        SubjectName NVARCHAR(100) NOT NULL,
        StageCode NVARCHAR(30) NOT NULL
    );

    INSERT @SubjectRows (Suffix, SubjectName, StageCode)
    VALUES
      (N'QUR',N'القرآن الكريم',N'BASIC'),(N'ISL',N'التربية الإسلامية',N'BASIC'),
      (N'AR',N'اللغة العربية',N'BASIC'),(N'MA',N'الرياضيات',N'BASIC'),(N'SCI',N'العلوم',N'BASIC'),
      (N'SOC',N'التربية الاجتماعية',N'BASIC'),(N'EN',N'اللغة الإنجليزية',N'BASIC'),
      (N'CS',N'الحاسوب',N'BASIC'),(N'ART',N'التربية الفنية',N'BASIC'),(N'PE',N'التربية البدنية',N'BASIC'),
      (N'QUR',N'القرآن الكريم',N'PREP'),(N'ISL',N'التربية الإسلامية',N'PREP'),
      (N'AR',N'اللغة العربية',N'PREP'),(N'MA',N'الرياضيات',N'PREP'),(N'SCI',N'العلوم',N'PREP'),
      (N'SOC',N'الدراسات الاجتماعية',N'PREP'),(N'EN',N'اللغة الإنجليزية',N'PREP'),
      (N'CS',N'الحاسوب',N'PREP'),(N'ART',N'التربية الفنية',N'PREP'),(N'PE',N'التربية البدنية',N'PREP'),
      (N'ISL',N'التربية الإسلامية',N'SEC'),(N'AR',N'اللغة العربية',N'SEC'),
      (N'EN',N'اللغة الإنجليزية',N'SEC'),(N'MA',N'الرياضيات',N'SEC'),(N'PH',N'الفيزياء',N'SEC'),
      (N'CH',N'الكيمياء',N'SEC'),(N'BI',N'الأحياء',N'SEC'),(N'HIS',N'التاريخ',N'SEC'),
      (N'GEO',N'الجغرافيا',N'SEC'),(N'CS',N'الحاسوب',N'SEC'),(N'PE',N'التربية البدنية',N'SEC');

    INSERT dbo.Subjects (SubjectCode, SubjectName, ClassID, MaxDegree, PassDegree, IsActive, Notes)
    SELECT c.ClassCode + N'-' + x.Suffix, x.SubjectName, c.ClassID, 100, 50, 1, N'مادة مرجعية مرتبطة بالصف'
    FROM dbo.Classes c
    INNER JOIN dbo.Stages st ON st.StageID = c.StageID
    INNER JOIN @SubjectRows x ON x.StageCode = st.StageCode
    WHERE c.IsActive = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.Subjects s
          WHERE s.ClassID = c.ClassID
            AND (s.SubjectCode = c.ClassCode + N'-' + x.Suffix OR s.SubjectName = x.SubjectName)
      );

    INSERT dbo.ClassSubjects (ClassID, SubjectID, IsActive, CreatedAt)
    SELECT s.ClassID, s.SubjectID, 1, GETDATE()
    FROM dbo.Subjects s
    WHERE s.ClassID IS NOT NULL
      AND ISNULL(s.IsActive, 1) = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.ClassSubjects cs
          WHERE cs.ClassID = s.ClassID AND cs.SubjectID = s.SubjectID
      );

    INSERT dbo.ClassSubjectTerms (ClassSubjectID, TermID, IsActive, CreatedAt)
    SELECT cs.ClassSubjectID, t.TermID, 1, GETDATE()
    FROM dbo.ClassSubjects cs
    CROSS JOIN dbo.AcademicTerms t
    WHERE ISNULL(cs.IsActive, 1) = 1
      AND ISNULL(t.IsActive, 1) = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.ClassSubjectTerms cst
          WHERE cst.ClassSubjectID = cs.ClassSubjectID AND cst.TermID = t.TermID
      );

    /* -----------------------------------------------------------------
       5) تثبيت خطط الرسوم الصفية للعام الحالي فقط
       ----------------------------------------------------------------- */
    DECLARE @AcademicStartDate DATE = CONVERT(date, '2026-09-01');

    DECLARE @FeeCatalog TABLE
    (
        StageCode NVARCHAR(30) NOT NULL,
        FeeType NVARCHAR(100) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        IsRequired BIT NOT NULL,
        Notes NVARCHAR(500) NULL,
        DueOffsetDays INT NOT NULL
    );

    INSERT @FeeCatalog (StageCode, FeeType, Amount, IsRequired, Notes, DueOffsetDays)
    VALUES
        (N'BASIC', N'رسوم تسجيل',    10000, 1, N'رسوم التسجيل للطالب الجديد', 14),
        (N'BASIC', N'رسوم دراسية',   80000, 1, N'الرسوم الدراسية الأساسية', 30),
        (N'BASIC', N'رسوم كتب',      12000, 1, N'كتب ومقررات المرحلة الأساسية', 30),
        (N'BASIC', N'رسوم زي مدرسي',  15000, 0, N'الزي المدرسي', 30),
        (N'BASIC', N'رسوم مواصلات',  30000, 0, N'خدمة النقل المدرسي', 30),
        (N'BASIC', N'رسوم امتحانات',  8000, 1, N'رسوم التقييم والاختبارات', 120),
        (N'BASIC', N'رسوم أنشطة',     5000, 0, N'الأنشطة المدرسية', 60),
        (N'BASIC', N'أخرى',            1000, 0, N'رسوم إضافية عند الحاجة', 30),

        (N'PREP',  N'رسوم تسجيل',    10000, 1, N'رسوم التسجيل للطالب الجديد', 14),
        (N'PREP',  N'رسوم دراسية',  120000, 1, N'الرسوم الدراسية للمرحلة الإعدادية', 30),
        (N'PREP',  N'رسوم كتب',      15000, 1, N'كتب ومقررات المرحلة الإعدادية', 30),
        (N'PREP',  N'رسوم زي مدرسي',  15000, 0, N'الزي المدرسي', 30),
        (N'PREP',  N'رسوم مواصلات',  30000, 0, N'خدمة النقل المدرسي', 30),
        (N'PREP',  N'رسوم امتحانات', 10000, 1, N'رسوم التقييم والاختبارات', 120),
        (N'PREP',  N'رسوم أنشطة',     5000, 0, N'الأنشطة المدرسية', 60),
        (N'PREP',  N'أخرى',            1000, 0, N'رسوم إضافية عند الحاجة', 30),

        (N'SEC',   N'رسوم تسجيل',    10000, 1, N'رسوم التسجيل للطالب الجديد', 14),
        (N'SEC',   N'رسوم دراسية',  150000, 1, N'الرسوم الدراسية للمرحلة الثانوية', 30),
        (N'SEC',   N'رسوم كتب',      15000, 1, N'كتب ومقررات المرحلة الثانوية', 30),
        (N'SEC',   N'رسوم زي مدرسي',  15000, 0, N'الزي المدرسي', 30),
        (N'SEC',   N'رسوم مواصلات',  30000, 0, N'خدمة النقل المدرسي', 30),
        (N'SEC',   N'رسوم امتحانات', 12000, 1, N'رسوم التقييم والاختبارات', 120),
        (N'SEC',   N'رسوم أنشطة',     7000, 0, N'الأنشطة المدرسية', 60),
        (N'SEC',   N'أخرى',            1000, 0, N'رسوم إضافية عند الحاجة', 30);

    INSERT dbo.FeePlans (AcademicYear, ClassID, FeeType, Amount, DueDate, IsRequired, Notes, CreatedAt)
    SELECT
        @CurrentAcademicYear,
        c.ClassID,
        fc.FeeType,
        fc.Amount,
        DATEADD(DAY, fc.DueOffsetDays, @AcademicStartDate),
        fc.IsRequired,
        fc.Notes,
        GETDATE()
    FROM dbo.Classes c
    INNER JOIN dbo.Stages st ON st.StageID = c.StageID
    INNER JOIN @FeeCatalog fc ON fc.StageCode = st.StageCode
    WHERE c.IsActive = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.FeePlans fp
          WHERE REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@CurrentAcademicYear, N'-', N'/')
            AND fp.ClassID = c.ClassID
            AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = LTRIM(RTRIM(fc.FeeType))
      );

    /* -----------------------------------------------------------------
       6) إنشاء الشعب الدراسية الحالية فقط
       ----------------------------------------------------------------- */
    INSERT dbo.SchoolSections (ClassID, SectionName, AcademicYear, IsActive, Capacity, AllowedGender, CreatedAt)
    SELECT c.ClassID, v.SectionName, @CurrentAcademicYear, 1, 30, N'مختلط', GETDATE()
    FROM dbo.Classes c
    CROSS JOIN (VALUES (N'Section A'), (N'Section B'), (N'Section C'), (N'Section D')) v(SectionName)
    WHERE c.IsActive = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.SchoolSections ss
          WHERE ss.ClassID = c.ClassID
            AND ss.AcademicYear = @CurrentAcademicYear
            AND ss.SectionName = v.SectionName
      );

    /* -----------------------------------------------------------------
         7) فرض قيود منع التكرار على الجداول المرجعية/التشغيلية الرئيسية
       ----------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_AcademicYears_AcademicYear' AND object_id = OBJECT_ID(N'dbo.AcademicYears'))
        CREATE UNIQUE INDEX UX_AcademicYears_AcademicYear ON dbo.AcademicYears(AcademicYear);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_AcademicYearCurrent_OneCurrent' AND object_id = OBJECT_ID(N'dbo.AcademicYearCurrent'))
        CREATE UNIQUE INDEX UX_AcademicYearCurrent_OneCurrent ON dbo.AcademicYearCurrent(IsCurrent) WHERE IsCurrent = 1;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Stages_StageCode' AND object_id = OBJECT_ID(N'dbo.Stages'))
        CREATE UNIQUE INDEX UX_Stages_StageCode ON dbo.Stages(StageCode);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Classes_ClassCode' AND object_id = OBJECT_ID(N'dbo.Classes'))
        CREATE UNIQUE INDEX UX_Classes_ClassCode ON dbo.Classes(ClassCode);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Subjects_SubjectCode' AND object_id = OBJECT_ID(N'dbo.Subjects'))
        CREATE UNIQUE INDEX UX_Subjects_SubjectCode ON dbo.Subjects(SubjectCode);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_ClassSubjects_Class_Subject' AND object_id = OBJECT_ID(N'dbo.ClassSubjects'))
        CREATE UNIQUE INDEX UX_ClassSubjects_Class_Subject ON dbo.ClassSubjects(ClassID, SubjectID);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_ClassSubjectTerms_ClassSubject_Term' AND object_id = OBJECT_ID(N'dbo.ClassSubjectTerms'))
        CREATE UNIQUE INDEX UX_ClassSubjectTerms_ClassSubject_Term ON dbo.ClassSubjectTerms(ClassSubjectID, TermID);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_SchoolSections_Class_Year_Section' AND object_id = OBJECT_ID(N'dbo.SchoolSections'))
        CREATE UNIQUE INDEX UX_SchoolSections_Class_Year_Section ON dbo.SchoolSections(ClassID, AcademicYear, SectionName);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_FeePlans_Year_Class_Type' AND object_id = OBJECT_ID(N'dbo.FeePlans'))
        CREATE UNIQUE INDEX UX_FeePlans_Year_Class_Type ON dbo.FeePlans(AcademicYear, ClassID, FeeType);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Fees_Student_Year_Plan' AND object_id = OBJECT_ID(N'dbo.Fees'))
        CREATE UNIQUE INDEX UX_Fees_Student_Year_Plan ON dbo.Fees(StudentID, AcademicYear, FeePlanID) WHERE FeePlanID IS NOT NULL;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Fees_Student_Year_Type' AND object_id = OBJECT_ID(N'dbo.Fees'))
        CREATE UNIQUE INDEX UX_Fees_Student_Year_Type ON dbo.Fees(StudentID, AcademicYear, FeeType) WHERE FeePlanID IS NULL;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Receipts_ReceiptNumber' AND object_id = OBJECT_ID(N'dbo.Receipts'))
        CREATE UNIQUE INDEX UX_Receipts_ReceiptNumber ON dbo.Receipts(ReceiptNumber) WHERE ReceiptNumber IS NOT NULL;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Vouchers_VoucherNumber' AND object_id = OBJECT_ID(N'dbo.Vouchers'))
        CREATE UNIQUE INDEX UX_Vouchers_VoucherNumber ON dbo.Vouchers(VoucherNumber) WHERE VoucherNumber IS NOT NULL;

    /* -----------------------------------------------------------------
       8) فحوصات أخيرة بسيطة
       ----------------------------------------------------------------- */
    IF EXISTS (
        SELECT 1
        FROM dbo.AcademicYearCurrent
        WHERE IsCurrent = 1
        GROUP BY IsCurrent
        HAVING COUNT(*) > 1
    )
    BEGIN
        THROW 50001, N'تم العثور على أكثر من سجل Current في AcademicYearCurrent.', 1;
    END

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO