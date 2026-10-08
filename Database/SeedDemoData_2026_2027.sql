/*
  SchoolSystem - Demo Data 2026/2027
  ----------------------------------
  بيانات تشغيلية تجريبية مترابطة للعرض والاختبار.
  لا تحذف بيانات موجودة؛ وتمنع التكرار باستخدام مفاتيح طبيعية وملاحظات DEMO.
  يجب أن يكون المرجع الأكاديمي 2026/2027 موجودًا قبل التشغيل.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 56000, N'قاعدة SchoolDB غير موجودة. شغّل النظام/تهيئة قاعدة البيانات أولاً.', 1;

USE SchoolDB;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Year nvarchar(20)=N'2026/2027';
    DECLARE @Demo nvarchar(100)=N'DEMO-SEED-2026-2027';
    DECLARE @Today date=CONVERT(date,GETDATE());
    DECLARE @AdminUserID int=NULL;

    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicYears WHERE AcademicYear=@Year AND IsActive=1)
        THROW 56001,N'العام 2026/2027 غير موجود أو غير نشط.',1;
    IF NOT EXISTS (SELECT 1 FROM dbo.AcademicTerms WHERE TermName=N'الفصل الدراسي الأول' AND IsActive=1)
        THROW 56002,N'الفصل الدراسي الأول غير موجود أو غير نشط.',1;

    SELECT TOP(1) @AdminUserID=UserID
    FROM dbo.Users
    WHERE IsActive=1 AND LTRIM(RTRIM(ISNULL(RoleName,N''))) IN (N'مدير النظام',N'Admin',N'Administrator')
    ORDER BY UserID;

    DECLARE @C1 int,@C9 int,@C10 int;
    SELECT @C1=ClassID FROM dbo.Classes WHERE ClassName=N'الأول الأساسي' AND IsActive=1;
    SELECT @C9=ClassID FROM dbo.Classes WHERE ClassName=N'التاسع الإعدادي' AND IsActive=1;
    SELECT @C10=ClassID FROM dbo.Classes WHERE ClassName=N'الأول الثانوي' AND IsActive=1;

    IF ISNULL(@C1,0)=0 OR ISNULL(@C9,0)=0 OR ISNULL(@C10,0)=0
        THROW 56003,N'الصفوف المطلوبة غير موجودة. شغّل SeedAcademicStructure_2026_2027.sql.',1;

    /* أنواع الرسوم المستخدمة في الشاشة؛ مصدر القائمة هو dbo.FeeTypes. */
    IF OBJECT_ID(N'dbo.FeeTypes', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.FeeTypes
        (
            FeeTypeID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            FeeTypeCode NVARCHAR(30) NOT NULL,
            FeeTypeName NVARCHAR(100) NOT NULL,
            IsTotal BIT NOT NULL DEFAULT(0),
            IsRequiredDefault BIT NOT NULL DEFAULT(1),
            DistributionWeight DECIMAL(9,4) NOT NULL DEFAULT(1),
            SortOrder INT NOT NULL DEFAULT(100),
            IsActive BIT NOT NULL DEFAULT(1),
            Notes NVARCHAR(500) NULL,
            CreatedAt DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
            UpdatedAt DATETIME2(0) NULL
        );
    END;

    INSERT dbo.FeeTypes
    (FeeTypeCode,FeeTypeName,IsTotal,IsRequiredDefault,DistributionWeight,SortOrder,IsActive,Notes,CreatedAt)
    SELECT v.FeeTypeCode,v.FeeTypeName,v.IsTotal,v.IsRequiredDefault,v.DistributionWeight,v.SortOrder,1,@Demo,SYSUTCDATETIME()
    FROM
    (
        VALUES
        (N'TUITION',N'رسوم دراسية',0,1,1.0000,10),
        (N'TRANSPORT',N'مواصلات',0,0,1.0000,20),
        (N'BOOKS',N'كتب',0,0,1.0000,30),
        (N'TOTAL',N'الرسوم الكلية',1,1,1.0000,1),
        (N'REGISTRATION',N'رسوم تسجيل',0,1,1.0000,5)
    ) v(FeeTypeCode,FeeTypeName,IsTotal,IsRequiredDefault,DistributionWeight,SortOrder)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.FeeTypes ft
        WHERE ft.FeeTypeCode=v.FeeTypeCode OR ft.FeeTypeName=v.FeeTypeName
    );

    /* ---------------------------------------------------------------
       الشعب والقاعات
       --------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolSections WHERE ClassID=@C1 AND SectionName=N'أ' AND AcademicYear=@Year)
        INSERT dbo.SchoolSections(ClassID,SectionName,AcademicYear,IsActive,Capacity,AllowedGender,CreatedAt)
        VALUES(@C1,N'أ',@Year,1,30,N'مختلط',GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolSections WHERE ClassID=@C9 AND SectionName=N'أ' AND AcademicYear=@Year)
        INSERT dbo.SchoolSections(ClassID,SectionName,AcademicYear,IsActive,Capacity,AllowedGender,CreatedAt)
        VALUES(@C9,N'أ',@Year,1,30,N'مختلط',GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolSections WHERE ClassID=@C10 AND SectionName=N'أ' AND AcademicYear=@Year)
        INSERT dbo.SchoolSections(ClassID,SectionName,AcademicYear,IsActive,Capacity,AllowedGender,CreatedAt)
        VALUES(@C10,N'أ',@Year,1,30,N'مختلط',GETDATE());

    IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomCode=N'R-102')
        INSERT dbo.Rooms(RoomCode,RoomName,RoomType,Capacity,Location,IsActive,Notes,CreatedAt)
        VALUES(N'R-102',N'قاعة 102',N'فصل دراسي',30,N'المبنى الرئيسي',1,@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomCode=N'LAB-02')
        INSERT dbo.Rooms(RoomCode,RoomName,RoomType,Capacity,Location,IsActive,Notes,CreatedAt)
        VALUES(N'LAB-02',N'مختبر الفيزياء',N'مختبر',25,N'المبنى العلمي',1,@Demo,GETDATE());

    /* ---------------------------------------------------------------
       الطلاب
       --------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'10001')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'10001',N'أحمد محمد علي',N'ذكر','2018-03-14',N'صنعاء',N'يمني',N'100000001',N'نشط',
            N'عبدالله محمد',N'والد',N'770000001',N'test.student1@example.invalid',N'موظف',
            N'أمانة العاصمة',N'الوحدة',N'صنعاء',GETDATE(),@C1,N'أ',@Year);

    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'10002')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'10002',N'محمد عبدالله حسن',N'ذكر','2018-07-22',N'صنعاء',N'يمني',N'100000002',N'نشط',
            N'عبدالله محمد',N'والد',N'770000002',N'test.student2@example.invalid',N'موظف',
            N'أمانة العاصمة',N'الوحدة',N'صنعاء',GETDATE(),@C1,N'أ',@Year);

    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'20001')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'20001',N'عبدالرحمن صالح أحمد',N'ذكر','2013-05-09',N'صنعاء',N'يمني',N'100000003',N'نشط',
            N'صالح أحمد',N'والد',N'770000003',N'test.student3@example.invalid',N'مهندس',
            N'أمانة العاصمة',N'السبعين',N'صنعاء',GETDATE(),@C9,N'أ',@Year);

    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'20002')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'20002',N'ياسر علي محمد',N'ذكر','2013-11-18',N'صنعاء',N'يمني',N'100000004',N'نشط',
            N'علي محمد',N'والد',N'770000004',N'test.student4@example.invalid',N'تاجر',
            N'أمانة العاصمة',N'معين',N'صنعاء',GETDATE(),@C9,N'أ',@Year);

    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'30001')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'30001',N'سامي خالد حسن',N'ذكر','2009-02-27',N'صنعاء',N'يمني',N'100000005',N'نشط',
            N'خالد حسن',N'والد',N'770000005',N'test.student5@example.invalid',N'مدرس',
            N'أمانة العاصمة',N'الصافية',N'صنعاء',GETDATE(),@C10,N'أ',@Year);

    IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentNumber=N'30002')
        INSERT dbo.Students(StudentNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalId,Status,
            GuardianName,GuardianRelation,GuardianPhone,GuardianEmail,GuardianJob,Governorate,District,Address,
            CreatedAt,ClassID,Section,AcademicYear)
        VALUES(N'30002',N'عمار فؤاد علي',N'ذكر','2009-08-13',N'صنعاء',N'يمني',N'100000006',N'نشط',
            N'فؤاد علي',N'والد',N'770000006',N'test.student6@example.invalid',N'موظف',
            N'أمانة العاصمة',N'حدة',N'صنعاء',GETDATE(),@C10,N'أ',@Year);

    DECLARE @S1 int,@S2 int,@S3 int,@S4 int,@S5 int,@S6 int;
    SELECT @S1=StudentID FROM dbo.Students WHERE StudentNumber=N'10001';
    SELECT @S2=StudentID FROM dbo.Students WHERE StudentNumber=N'10002';
    SELECT @S3=StudentID FROM dbo.Students WHERE StudentNumber=N'20001';
    SELECT @S4=StudentID FROM dbo.Students WHERE StudentNumber=N'20002';
    SELECT @S5=StudentID FROM dbo.Students WHERE StudentNumber=N'30001';
    SELECT @S6=StudentID FROM dbo.Students WHERE StudentNumber=N'30002';

    /* ---------------------------------------------------------------
       توزيع الطلاب + التسجيل
       --------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S1 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S1,@C1,N'أ',@Year,@Today,@AdminUserID);
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S2 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S2,@C1,N'أ',@Year,@Today,@AdminUserID);
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S3 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S3,@C9,N'أ',@Year,@Today,@AdminUserID);
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S4 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S4,@C9,N'أ',@Year,@Today,@AdminUserID);
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S5 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S5,@C10,N'أ',@Year,@Today,@AdminUserID);
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentClasses WHERE StudentID=@S6 AND AcademicYear=@Year)
        INSERT dbo.StudentClasses(StudentID,ClassID,Section,AcademicYear,AssignedDate,AssignedBy)
        VALUES(@S6,@C10,N'أ',@Year,@Today,@AdminUserID);

    DECLARE @Enroll table(StudentID int,ClassID int,Section nvarchar(50),Seat nvarchar(20),ApplicationDate date);
    INSERT @Enroll VALUES
      (@S1,@C1,N'أ',N'1','2026-09-03'),
      (@S2,@C1,N'أ',N'2','2026-09-04'),
      (@S3,@C9,N'أ',N'1','2026-09-05'),
      (@S4,@C9,N'أ',N'2','2026-09-06'),
      (@S5,@C10,N'أ',N'1','2026-09-07'),
      (@S6,@C10,N'أ',N'2','2026-09-08');

    INSERT dbo.Enrollments
    (StudentID,ApplicationDate,ApplicationType,AcademicYear,ClassID,Section,SeatNumber,Status,
     RegistrationFee,PaidAmount,PaymentMethod,HasBirthCertificate,HasGuardianId,HasPhoto,
     HasLastCertificate,HasMedicalReport,GeneralNotes,CreatedAt)
    SELECT e.StudentID,e.ApplicationDate,N'مستجد',@Year,e.ClassID,e.Section,e.Seat,N'مقبول',
           5000,CASE WHEN e.StudentID=@S1 THEN 5000 ELSE 0 END,
           CASE WHEN e.StudentID=@S1 THEN N'نقداً' ELSE NULL END,
           1,1,1,1,1,@Demo,GETDATE()
    FROM @Enroll e
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Enrollments x WHERE x.StudentID=e.StudentID AND x.AcademicYear=@Year);

    /* ---------------------------------------------------------------
       خطط الرسوم
       الأول الأساسي 140,000 | التاسع 165,000 | الأول الثانوي 205,000
       --------------------------------------------------------------- */
    DECLARE @Plans table(ClassID int,FeeType nvarchar(100),Amount decimal(18,2),PlanID int);

    INSERT dbo.FeePlans(AcademicYear,ClassID,FeeType,Amount,DueDate,IsRequired,Notes,CreatedAt)
    SELECT @Year,v.ClassID,v.FeeType,v.Amount,'2026-10-31',v.Required,@Demo,GETDATE()
    FROM (VALUES
      (@C1,N'رسوم دراسية',100000,1),(@C1,N'مواصلات',25000,0),(@C1,N'كتب',15000,0),(@C1,N'الرسوم الكلية',140000,1),
      (@C9,N'رسوم دراسية',120000,1),(@C9,N'مواصلات',25000,0),(@C9,N'كتب',20000,0),(@C9,N'الرسوم الكلية',165000,1),
      (@C10,N'رسوم دراسية',150000,1),(@C10,N'مواصلات',30000,0),(@C10,N'كتب',25000,0),(@C10,N'الرسوم الكلية',205000,1)
    ) v(ClassID,FeeType,Amount,Required)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.FeePlans p
        WHERE p.AcademicYear=@Year AND p.ClassID=v.ClassID AND LTRIM(RTRIM(p.FeeType))=v.FeeType
    );

    ;WITH RankedPlans AS
    (
        SELECT
            fp.ClassID,
            fp.FeeType,
            fp.Amount,
            fp.FeePlanID,
            ROW_NUMBER() OVER
            (
                PARTITION BY fp.ClassID, LTRIM(RTRIM(fp.FeeType))
                ORDER BY
                    CASE WHEN fp.Notes=@Demo THEN 0 ELSE 1 END,
                    fp.FeePlanID DESC
            ) AS rn
        FROM dbo.FeePlans fp
        WHERE REPLACE(ISNULL(fp.AcademicYear,N''),N'-',N'/')=REPLACE(@Year,N'-',N'/')
          AND fp.ClassID IN (@C1,@C9,@C10)
    )
    INSERT @Plans(ClassID,FeeType,Amount,PlanID)
    SELECT ClassID,FeeType,Amount,FeePlanID
    FROM RankedPlans
    WHERE rn=1;

    DECLARE @P1T int=(SELECT PlanID FROM @Plans WHERE ClassID=@C1 AND FeeType=N'رسوم دراسية');
    DECLARE @P1R int=(SELECT PlanID FROM @Plans WHERE ClassID=@C1 AND FeeType=N'مواصلات');
    DECLARE @P1B int=(SELECT PlanID FROM @Plans WHERE ClassID=@C1 AND FeeType=N'كتب');
    DECLARE @P1X int=(SELECT PlanID FROM @Plans WHERE ClassID=@C1 AND FeeType=N'الرسوم الكلية');
    DECLARE @P9T int=(SELECT PlanID FROM @Plans WHERE ClassID=@C9 AND FeeType=N'رسوم دراسية');
    DECLARE @P9R int=(SELECT PlanID FROM @Plans WHERE ClassID=@C9 AND FeeType=N'مواصلات');
    DECLARE @P9B int=(SELECT PlanID FROM @Plans WHERE ClassID=@C9 AND FeeType=N'كتب');
    DECLARE @P9X int=(SELECT PlanID FROM @Plans WHERE ClassID=@C9 AND FeeType=N'الرسوم الكلية');
    DECLARE @P10T int=(SELECT PlanID FROM @Plans WHERE ClassID=@C10 AND FeeType=N'رسوم دراسية');
    DECLARE @P10R int=(SELECT PlanID FROM @Plans WHERE ClassID=@C10 AND FeeType=N'مواصلات');
    DECLARE @P10B int=(SELECT PlanID FROM @Plans WHERE ClassID=@C10 AND FeeType=N'كتب');
    DECLARE @P10X int=(SELECT PlanID FROM @Plans WHERE ClassID=@C10 AND FeeType=N'الرسوم الكلية');

    IF @P1T IS NULL OR @P1X IS NULL OR @P9T IS NULL OR @P9X IS NULL OR @P10T IS NULL OR @P10X IS NULL
        THROW 56004,N'فشل تجهيز خطط الرسوم التجريبية.',1;

    /* ---------------------------------------------------------------
       رسوم الطلاب المرتبطة بخطط الرسوم
       --------------------------------------------------------------- */
    INSERT dbo.Fees
    (StudentID,FeePlanID,AcademicYear,FeeType,TotalAmount,DiscountAmount,NetAmount,PaidAmount,RemainingAmount,
     DueDate,PaymentDate,PaymentMethod,ReceiptNumber,Status,Notes,CreatedAt)
    SELECT z.StudentID,p.FeePlanID,@Year,p.FeeType,p.Amount,0,p.Amount,z.Paid,p.Amount-z.Paid,
           p.DueDate,CASE WHEN z.Paid>0 THEN z.PayDate END,
           CASE WHEN z.Paid>0 THEN N'نقداً' END,NULL,
           CASE WHEN z.Paid=0 THEN N'غير مسدد' WHEN z.Paid>=p.Amount THEN N'مسدد' ELSE N'مسدد جزئياً' END,
           @Demo,GETDATE()
    FROM (
      SELECT @S1 StudentID,@P1T FeePlanID,CAST(30000 AS decimal(18,2)) Paid,CONVERT(date,'2026-10-05') PayDate
      UNION ALL SELECT @S1,@P1R,0,NULL
      UNION ALL SELECT @S1,@P1B,0,NULL
      UNION ALL SELECT @S2,@P1T,100000,'2026-10-05'
      UNION ALL SELECT @S2,@P1R,25000,'2026-10-05'
      UNION ALL SELECT @S2,@P1B,15000,'2026-10-05'
      UNION ALL SELECT @S3,@P9T,0,NULL
      UNION ALL SELECT @S3,@P9R,0,NULL
      UNION ALL SELECT @S3,@P9B,0,NULL
      UNION ALL SELECT @S4,@P9T,60000,'2026-10-06'
      UNION ALL SELECT @S4,@P9R,25000,'2026-10-06'
      UNION ALL SELECT @S4,@P9B,20000,'2026-10-06'
      UNION ALL SELECT @S5,@P10T,50000,'2026-10-07'
      UNION ALL SELECT @S5,@P10R,30000,'2026-10-07'
      UNION ALL SELECT @S5,@P10B,0,NULL
      UNION ALL SELECT @S6,@P10T,0,NULL
      UNION ALL SELECT @S6,@P10R,0,NULL
      UNION ALL SELECT @S6,@P10B,0,NULL
    ) z
    INNER JOIN dbo.FeePlans p ON p.FeePlanID=z.FeePlanID
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Fees f WHERE f.StudentID=z.StudentID AND f.FeePlanID=z.FeePlanID AND f.AcademicYear=@Year);

    /* الرسوم الكلية */
    INSERT dbo.Fees
    (StudentID,FeePlanID,AcademicYear,FeeType,TotalAmount,DiscountAmount,NetAmount,PaidAmount,RemainingAmount,
     DueDate,PaymentDate,PaymentMethod,ReceiptNumber,Status,Notes,CreatedAt)
    SELECT z.StudentID,p.FeePlanID,@Year,p.FeeType,p.Amount,0,p.Amount,z.Paid,p.Amount-z.Paid,
           p.DueDate,CASE WHEN z.Paid>0 THEN z.PayDate END,
           CASE WHEN z.Paid>0 THEN N'نقداً' END,NULL,
           CASE WHEN z.Paid=0 THEN N'غير مسدد' WHEN z.Paid>=p.Amount THEN N'مسدد' ELSE N'مسدد جزئياً' END,
           @Demo,GETDATE()
    FROM (
      SELECT @S1 StudentID,@P1X FeePlanID,CAST(30000 AS decimal(18,2)) Paid,CONVERT(date,'2026-10-05') PayDate
      UNION ALL SELECT @S2,@P1X,140000,'2026-10-05'
      UNION ALL SELECT @S3,@P9X,0,NULL
      UNION ALL SELECT @S4,@P9X,105000,'2026-10-06'
      UNION ALL SELECT @S5,@P10X,80000,'2026-10-07'
      UNION ALL SELECT @S6,@P10X,0,NULL
    ) z
    INNER JOIN dbo.FeePlans p ON p.FeePlanID=z.FeePlanID
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Fees f WHERE f.StudentID=z.StudentID AND f.FeePlanID=z.FeePlanID AND f.AcademicYear=@Year);

    /* ---------------------------------------------------------------
       رسوم التسجيل — سطر لكل طالب مطابق لمسار التسجيل اليدوي
       --------------------------------------------------------------- */
    INSERT dbo.Fees
    (
        StudentID,FeePlanID,AcademicYear,FeeType,TotalAmount,DiscountAmount,
        NetAmount,PaidAmount,RemainingAmount,DueDate,PaymentDate,
        PaymentMethod,ReceiptNumber,Status,Notes,CreatedAt
    )
    SELECT e.StudentID,NULL,@Year,N'رسوم تسجيل',e.RegistrationFee,0,e.RegistrationFee,
           e.PaidAmount,e.RegistrationFee-e.PaidAmount,
           e.ApplicationDate,
           CASE WHEN e.PaidAmount>0 THEN e.ApplicationDate END,
           CASE WHEN e.PaidAmount>0 THEN e.PaymentMethod END,
           NULL,
           CASE
               WHEN e.PaidAmount>=e.RegistrationFee THEN N'مسدد'
               WHEN e.PaidAmount>0 THEN N'مسدد جزئياً'
               ELSE N'غير مسدد'
           END,
           @Demo,GETDATE()
    FROM dbo.Enrollments e
    WHERE e.AcademicYear=@Year
      AND e.GeneralNotes=@Demo
      AND e.RegistrationFee>0
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.Fees f
          WHERE f.StudentID=e.StudentID
            AND f.AcademicYear=@Year
            AND f.FeeType=N'رسوم تسجيل'
            AND f.Notes=@Demo
      );

    DECLARE @RegistrationFeeID int;
    DECLARE @ReceiptFee int,@V int,@VN nvarchar(50);

    SELECT TOP(1) @RegistrationFeeID=FeeID
    FROM dbo.Fees
    WHERE StudentID=@S1 AND AcademicYear=@Year AND FeeType=N'رسوم تسجيل' AND Notes=@Demo
    ORDER BY FeeID;

    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':REG')
    BEGIN
        INSERT dbo.Vouchers
        (VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,
         ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES
        (N'قبض',5000,'2026-09-03',N'أحمد محمد علي',N'رسوم تسجيل الطالب',
         N'نقداً',N'رسوم',@RegistrationFeeID,@Demo+N':REG',1);

        SET @V=CONVERT(int,SCOPE_IDENTITY());
        SET @VN=N'REC-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@V),5);

        UPDATE dbo.Vouchers
        SET VoucherNumber=@VN
        WHERE VoucherID=@V;

        UPDATE dbo.Fees
        SET ReceiptNumber=@VN
        WHERE FeeID=@RegistrationFeeID;

        UPDATE dbo.Enrollments
        SET ReceiptNo=@VN
        WHERE StudentID=@S1 AND AcademicYear=@Year;
    END;

    /* helper-like repeated receipts */
    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':P1')
    BEGIN
        SELECT @ReceiptFee=FeeID FROM dbo.Fees WHERE StudentID=@S1 AND FeePlanID=@P1T;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'قبض',30000,'2026-10-05',N'أحمد محمد علي',N'دفعة رسوم دراسية',N'نقداً',N'رسوم',@ReceiptFee,@Demo+N':P1',1);
        SET @V=CONVERT(int,SCOPE_IDENTITY()); SET @VN=N'REC-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@V),5);
        UPDATE dbo.Vouchers SET VoucherNumber=@VN WHERE VoucherID=@V;
        UPDATE dbo.Fees SET ReceiptNumber=@VN,PaymentDate='2026-10-05',PaymentMethod=N'نقداً' WHERE StudentID=@S1 AND FeePlanID=@P1T;
        UPDATE dbo.Fees SET PaidAmount=30000,RemainingAmount=110000,Status=N'مسدد جزئياً' WHERE StudentID=@S1 AND FeePlanID=@P1X;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':P2')
    BEGIN
        SELECT @ReceiptFee=FeeID FROM dbo.Fees WHERE StudentID=@S2 AND FeePlanID=@P1T;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'قبض',140000,'2026-10-05',N'محمد عبدالله حسن',N'سداد كامل رسوم الصف',N'نقداً',N'رسوم',@ReceiptFee,@Demo+N':P2',1);
        SET @V=CONVERT(int,SCOPE_IDENTITY()); SET @VN=N'REC-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@V),5);
        UPDATE dbo.Vouchers SET VoucherNumber=@VN WHERE VoucherID=@V;
        UPDATE dbo.Fees SET ReceiptNumber=@VN,PaymentDate='2026-10-05',PaymentMethod=N'نقداً',PaidAmount=TotalAmount,RemainingAmount=0,Status=N'مسدد'
        WHERE StudentID=@S2 AND FeePlanID IN (@P1T,@P1R,@P1B,@P1X);
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':P3')
    BEGIN
        SELECT @ReceiptFee=FeeID FROM dbo.Fees WHERE StudentID=@S4 AND FeePlanID=@P9T;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'قبض',105000,'2026-10-06',N'ياسر علي محمد',N'دفعة رسوم الصف التاسع',N'نقداً',N'رسوم',@ReceiptFee,@Demo+N':P3',1);
        SET @V=CONVERT(int,SCOPE_IDENTITY()); SET @VN=N'REC-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@V),5);
        UPDATE dbo.Vouchers SET VoucherNumber=@VN WHERE VoucherID=@V;
        UPDATE dbo.Fees SET ReceiptNumber=@VN,PaymentDate='2026-10-06',PaymentMethod=N'نقداً' WHERE StudentID=@S4 AND FeePlanID IN (@P9T,@P9R,@P9B,@P9X);
        UPDATE dbo.Fees SET PaidAmount=CASE FeePlanID WHEN @P9T THEN 60000 WHEN @P9R THEN 25000 WHEN @P9B THEN 20000 WHEN @P9X THEN 105000 END,
                             RemainingAmount=CASE FeePlanID WHEN @P9T THEN 60000 WHEN @P9R THEN 0 WHEN @P9B THEN 0 WHEN @P9X THEN 60000 END,
                             Status=CASE FeePlanID WHEN @P9T THEN N'مسدد جزئياً' ELSE CASE WHEN FeePlanID=@P9X THEN N'مسدد جزئياً' ELSE N'مسدد' END END
        WHERE StudentID=@S4 AND FeePlanID IN (@P9T,@P9R,@P9B,@P9X);
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':P4')
    BEGIN
        SELECT @ReceiptFee=FeeID FROM dbo.Fees WHERE StudentID=@S5 AND FeePlanID=@P10T;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'قبض',80000,'2026-10-07',N'سامي خالد حسن',N'دفعة رسوم الصف الأول الثانوي',N'نقداً',N'رسوم',@ReceiptFee,@Demo+N':P4',1);
        SET @V=CONVERT(int,SCOPE_IDENTITY()); SET @VN=N'REC-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@V),5);
        UPDATE dbo.Vouchers SET VoucherNumber=@VN WHERE VoucherID=@V;
        UPDATE dbo.Fees SET ReceiptNumber=@VN,PaymentDate='2026-10-07',PaymentMethod=N'نقداً' WHERE StudentID=@S5 AND FeePlanID IN (@P10T,@P10R,@P10X);
        UPDATE dbo.Fees SET PaidAmount=CASE FeePlanID WHEN @P10T THEN 50000 WHEN @P10R THEN 30000 WHEN @P10B THEN 0 WHEN @P10X THEN 80000 END,
                             RemainingAmount=CASE FeePlanID WHEN @P10T THEN 100000 WHEN @P10R THEN 0 WHEN @P10B THEN 25000 WHEN @P10X THEN 125000 END,
                             Status=CASE FeePlanID WHEN @P10T THEN N'مسدد جزئياً' WHEN @P10R THEN N'مسدد' WHEN @P10B THEN N'غير مسدد' ELSE N'مسدد جزئياً' END
        WHERE StudentID=@S5 AND FeePlanID IN (@P10T,@P10R,@P10B,@P10X);
    END;

    /* ---------------------------------------------------------------
       المعلمون + العقود + الرواتب + سندات الصرف
       --------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber=N'30001')
        INSERT dbo.Teachers(EmployeeNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalID,Phone,Email,Address,
            Qualification,Specialization,HireDate,BasicSalary,TransportAllowance,HousingAllowance,Status,Notes,CreatedAt)
        VALUES(N'30001',N'أحمد محمد صالح',N'ذكر','1988-04-12',N'صنعاء',N'يمني',N'200000001',N'771000001',
            N'test.teacher1@example.invalid',N'صنعاء',N'بكالوريوس',N'رياضيات','2015-09-01',70000,5000,8000,N'نشط',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber=N'30002')
        INSERT dbo.Teachers(EmployeeNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalID,Phone,Email,Address,
            Qualification,Specialization,HireDate,BasicSalary,TransportAllowance,HousingAllowance,Status,Notes,CreatedAt)
        VALUES(N'30002',N'عبدالرحمن علي حسن',N'ذكر','1990-01-25',N'تعز',N'يمني',N'200000002',N'771000002',
            N'test.teacher2@example.invalid',N'صنعاء',N'بكالوريوس',N'اللغة العربية','2017-09-01',65000,5000,7000,N'نشط',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.Teachers WHERE EmployeeNumber=N'30003')
        INSERT dbo.Teachers(EmployeeNumber,FullName,Gender,BirthDate,BirthPlace,Nationality,NationalID,Phone,Email,Address,
            Qualification,Specialization,HireDate,BasicSalary,TransportAllowance,HousingAllowance,Status,Notes,CreatedAt)
        VALUES(N'30003',N'سامي خالد محمد',N'ذكر','1987-10-03',N'الحديدة',N'يمني',N'200000003',N'771000003',
            N'test.teacher3@example.invalid',N'صنعاء',N'بكالوريوس',N'فيزياء','2014-09-01',75000,5000,8000,N'نشط',@Demo,GETDATE());

    DECLARE @T1 int,@T2 int,@T3 int;
    SELECT @T1=TeacherID FROM dbo.Teachers WHERE EmployeeNumber=N'30001';
    SELECT @T2=TeacherID FROM dbo.Teachers WHERE EmployeeNumber=N'30002';
    SELECT @T3=TeacherID FROM dbo.Teachers WHERE EmployeeNumber=N'30003';

    IF NOT EXISTS (SELECT 1 FROM dbo.TeacherContracts WHERE TeacherID=@T1 AND ContractStatus=N'ساري' AND StartDate<@Today AND (EndDate IS NULL OR EndDate>=@Today))
        INSERT dbo.TeacherContracts(TeacherID,ContractNumber,ContractType,ContractStatus,BasicSalary,HousingAllowance,TransportAllowance,OtherAllowances,Deductions,StartDate,PaymentMethod,Notes,CreatedAt)
        VALUES(@T1,N'CNT-30001-2026',N'دوام كامل',N'ساري',70000,8000,5000,0,3000,'2026-09-01',N'نقداً',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.TeacherContracts WHERE TeacherID=@T2 AND ContractStatus=N'ساري' AND StartDate<@Today AND (EndDate IS NULL OR EndDate>=@Today))
        INSERT dbo.TeacherContracts(TeacherID,ContractNumber,ContractType,ContractStatus,BasicSalary,HousingAllowance,TransportAllowance,OtherAllowances,Deductions,StartDate,PaymentMethod,Notes,CreatedAt)
        VALUES(@T2,N'CNT-30002-2026',N'دوام كامل',N'ساري',65000,7000,5000,0,2000,'2026-09-01',N'نقداً',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.TeacherContracts WHERE TeacherID=@T3 AND ContractStatus=N'ساري' AND StartDate<@Today AND (EndDate IS NULL OR EndDate>=@Today))
        INSERT dbo.TeacherContracts(TeacherID,ContractNumber,ContractType,ContractStatus,BasicSalary,HousingAllowance,TransportAllowance,OtherAllowances,Deductions,StartDate,PaymentMethod,Notes,CreatedAt)
        VALUES(@T3,N'CNT-30003-2026',N'دوام كامل',N'ساري',75000,8000,5000,0,4000,'2026-09-01',N'نقداً',@Demo,GETDATE());

    IF NOT EXISTS (SELECT 1 FROM dbo.Payroll WHERE TeacherID=@T1 AND SalaryMonth=9 AND SalaryYear=2026)
        INSERT dbo.Payroll(TeacherID,SalaryMonth,SalaryYear,BasicSalary,Allowances,Deductions,PaymentDate,Notes,CreatedAt)
        VALUES(@T1,9,2026,70000,13000,3000,'2026-10-05',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.Payroll WHERE TeacherID=@T2 AND SalaryMonth=9 AND SalaryYear=2026)
        INSERT dbo.Payroll(TeacherID,SalaryMonth,SalaryYear,BasicSalary,Allowances,Deductions,PaymentDate,Notes,CreatedAt)
        VALUES(@T2,9,2026,65000,12000,2000,'2026-10-05',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.Payroll WHERE TeacherID=@T3 AND SalaryMonth=9 AND SalaryYear=2026)
        INSERT dbo.Payroll(TeacherID,SalaryMonth,SalaryYear,BasicSalary,Allowances,Deductions,PaymentDate,Notes,CreatedAt)
        VALUES(@T3,9,2026,75000,13000,4000,'2026-10-05',@Demo,GETDATE());

    DECLARE @PR int,@PY nvarchar(50);
    DECLARE @PayrollID1 int=(SELECT PayrollID FROM dbo.Payroll WHERE TeacherID=@T1 AND SalaryMonth=9 AND SalaryYear=2026);
    DECLARE @PayrollID2 int=(SELECT PayrollID FROM dbo.Payroll WHERE TeacherID=@T2 AND SalaryMonth=9 AND SalaryYear=2026);
    DECLARE @PayrollID3 int=(SELECT PayrollID FROM dbo.Payroll WHERE TeacherID=@T3 AND SalaryMonth=9 AND SalaryYear=2026);

    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':SAL1')
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'صرف',80000,'2026-10-05',N'أحمد محمد صالح',N'راتب سبتمبر 2026',N'نقداً',N'رواتب',@PayrollID1,@Demo+N':SAL1',1);
    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':SAL2')
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'صرف',75000,'2026-10-05',N'عبدالرحمن علي حسن',N'راتب سبتمبر 2026',N'نقداً',N'رواتب',@PayrollID2,@Demo+N':SAL2',1);
    IF NOT EXISTS (SELECT 1 FROM dbo.Vouchers WHERE Notes=@Demo+N':SAL3')
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'صرف',84000,'2026-10-05',N'سامي خالد محمد',N'راتب سبتمبر 2026',N'نقداً',N'رواتب',@PayrollID3,@Demo+N':SAL3',1);

    UPDATE v SET VoucherNumber=N'PAY-'+CONVERT(nvarchar(4),YEAR(VoucherDate))+N'-'+RIGHT(N'00000'+CONVERT(nvarchar(10),VoucherID),5)
    FROM dbo.Vouchers v
    WHERE v.Notes LIKE @Demo+N':SAL%' AND (v.VoucherNumber IS NULL OR v.VoucherNumber=N'');

    /* ---------------------------------------------------------------
       الحضور والدرجات والجدول
       --------------------------------------------------------------- */
    DECLARE @Math int,@Arabic int,@Physics int;
    SELECT @Math=SubjectID FROM dbo.Subjects WHERE SubjectName=N'الرياضيات' AND IsActive=1;
    SELECT @Arabic=SubjectID FROM dbo.Subjects WHERE SubjectName=N'اللغة العربية' AND IsActive=1;
    SELECT @Physics=SubjectID FROM dbo.Subjects WHERE SubjectName=N'الفيزياء' AND IsActive=1;

    IF @Math IS NULL OR @Arabic IS NULL OR @Physics IS NULL
        THROW 56005,N'المواد المطلوبة لبيانات العرض غير موجودة أو غير نشطة.',1;

    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID=@S1 AND AttendanceDate='2026-10-07')
        INSERT dbo.StudentAttendance(StudentID,ClassID,Section,AcademicYear,AttendanceDate,Status,ArrivalTime,Notes,CreatedAt)
        VALUES(@S1,@C1,N'أ',@Year,'2026-10-07',N'حاضر','07:10',@Demo,GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentAttendance WHERE StudentID=@S2 AND AttendanceDate='2026-10-07')
        INSERT dbo.StudentAttendance(StudentID,ClassID,Section,AcademicYear,AttendanceDate,Status,ExcuseStatus,Notes,CreatedAt)
        VALUES(@S2,@C1,N'أ',@Year,'2026-10-07',N'غائب',N'مقبول',@Demo,GETDATE());

    IF NOT EXISTS (SELECT 1 FROM dbo.TeacherAttendance WHERE TeacherID=@T1 AND AttendanceDate='2026-10-07')
        INSERT dbo.TeacherAttendance(TeacherID,AttendanceDate,Status,LateMinutes,EarlyLeaveMinutes,WorkHours,Notes,RecordedAt,CheckInTime,CheckOutTime)
        VALUES(@T1,'2026-10-07',N'حاضر',0,0,7.5,@Demo,GETDATE(),'07:00','14:30');
    IF NOT EXISTS (SELECT 1 FROM dbo.TeacherAttendance WHERE TeacherID=@T2 AND AttendanceDate='2026-10-07')
        INSERT dbo.TeacherAttendance(TeacherID,AttendanceDate,Status,LateMinutes,EarlyLeaveMinutes,WorkHours,Notes,RecordedAt,CheckInTime,CheckOutTime)
        VALUES(@T2,'2026-10-07',N'متأخر',20,0,7,@Demo,GETDATE(),'07:20','14:20');

    IF @Math IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Grades WHERE StudentID=@S1 AND SubjectID=@Math AND ClassID=@C1 AND Section=N'أ' AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول')
        INSERT dbo.Grades(StudentID,SubjectID,ClassID,AcademicYear,TermName,GradeValue,Notes,CreatedAt,Section,Quiz1,Quiz2,CourseWork,FinalExam,GradeLetter,ResultStatus,UpdatedAt)
        VALUES(@S1,@Math,@C1,@Year,N'الفصل الدراسي الأول',90,@Demo,GETDATE(),N'أ',10,10,25,45,N'ممتاز',N'ناجح',GETDATE());
    IF @Arabic IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Grades WHERE StudentID=@S3 AND SubjectID=@Arabic AND ClassID=@C9 AND Section=N'أ' AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول')
        INSERT dbo.Grades(StudentID,SubjectID,ClassID,AcademicYear,TermName,GradeValue,Notes,CreatedAt,Section,Quiz1,Quiz2,CourseWork,FinalExam,GradeLetter,ResultStatus,UpdatedAt)
        VALUES(@S3,@Arabic,@C9,@Year,N'الفصل الدراسي الأول',78,@Demo,GETDATE(),N'أ',8,9,23,38,N'جيد',N'ناجح',GETDATE());
    IF @Physics IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Grades WHERE StudentID=@S5 AND SubjectID=@Physics AND ClassID=@C10 AND Section=N'أ' AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول')
        INSERT dbo.Grades(StudentID,SubjectID,ClassID,AcademicYear,TermName,GradeValue,Notes,CreatedAt,Section,Quiz1,Quiz2,CourseWork,FinalExam,GradeLetter,ResultStatus,UpdatedAt)
        VALUES(@S5,@Physics,@C10,@Year,N'الفصل الدراسي الأول',92,@Demo,GETDATE(),N'أ',10,10,27,45,N'ممتاز',N'ناجح',GETDATE());

    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolTimetable WHERE ClassID=@C1 AND Section=N'أ' AND SubjectID=@Math AND TeacherID=@T1 AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول' AND DayName=N'الأحد' AND PeriodNo=1)
        INSERT dbo.SchoolTimetable(ClassID,Section,SubjectID,TeacherID,AcademicYear,TermName,DayName,PeriodNo,RoomName,Notes,IsActive,StartTime,EndTime,CreatedAt)
        VALUES(@C1,N'أ',@Math,@T1,@Year,N'الفصل الدراسي الأول',N'الأحد',1,N'قاعة 101',@Demo,1,'07:30','08:15',GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolTimetable WHERE ClassID=@C9 AND Section=N'أ' AND SubjectID=@Arabic AND TeacherID=@T2 AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول' AND DayName=N'الاثنين' AND PeriodNo=1)
        INSERT dbo.SchoolTimetable(ClassID,Section,SubjectID,TeacherID,AcademicYear,TermName,DayName,PeriodNo,RoomName,Notes,IsActive,StartTime,EndTime,CreatedAt)
        VALUES(@C9,N'أ',@Arabic,@T2,@Year,N'الفصل الدراسي الأول',N'الاثنين',1,N'قاعة 102',@Demo,1,'07:30','08:15',GETDATE());
    IF NOT EXISTS (SELECT 1 FROM dbo.SchoolTimetable WHERE ClassID=@C10 AND Section=N'أ' AND SubjectID=@Physics AND TeacherID=@T3 AND AcademicYear=@Year AND TermName=N'الفصل الدراسي الأول' AND DayName=N'الثلاثاء' AND PeriodNo=1)
        INSERT dbo.SchoolTimetable(ClassID,Section,SubjectID,TeacherID,AcademicYear,TermName,DayName,PeriodNo,RoomName,Notes,IsActive,StartTime,EndTime,CreatedAt)
        VALUES(@C10,N'أ',@Physics,@T3,@Year,N'الفصل الدراسي الأول',N'الثلاثاء',1,N'مختبر الفيزياء',@Demo,1,'07:30','08:15',GETDATE());

    /* ---------------------------------------------------------------
       النقل والمكتبة والمصروفات
       --------------------------------------------------------------- */
    IF OBJECT_ID(N'dbo.StudentTransportAssignments',N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.StudentTransportAssignments
        (
            AssignmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            StudentID INT NOT NULL,
            RouteID INT NOT NULL,
            AcademicYear NVARCHAR(20) NOT NULL,
            PickupPoint NVARCHAR(200) NOT NULL,
            DropoffPoint NVARCHAR(200) NOT NULL,
            IsActive BIT NOT NULL,
            StartDate DATE NOT NULL,
            EndDate DATE NULL,
            Notes NVARCHAR(MAX) NULL,
            CreatedAt DATETIME NOT NULL,
            UpdatedAt DATETIME NULL
        );
    END;

    DECLARE @Bus int,@Route int;
    SELECT @Bus=BusID FROM dbo.Buses WHERE BusNumber=N'BUS-01';
    IF @Bus IS NULL
    BEGIN
        INSERT dbo.Buses(BusNumber,DriverName,DriverPhone,Capacity,Notes,CreatedAt)
        VALUES(N'BUS-01',N'عبدالله سالم',N'772000001',30,@Demo,GETDATE());
        SET @Bus=CONVERT(int,SCOPE_IDENTITY());
    END;
    SELECT @Route=RouteID FROM dbo.BusRoutes WHERE RouteName=N'خط صنعاء - الوحدة';
    IF @Route IS NULL
    BEGIN
        INSERT dbo.BusRoutes(RouteName,BusID,StartPoint,EndPoint,DepartureTime,ArrivalTime,Fee,Notes,CreatedAt)
        VALUES(N'خط صنعاء - الوحدة',@Bus,N'باب اليمن',N'مدرسة النور','06:45','07:20',25000,@Demo,GETDATE());
        SET @Route=CONVERT(int,SCOPE_IDENTITY());
    END;
    IF NOT EXISTS (SELECT 1 FROM dbo.StudentTransportAssignments WHERE StudentID=@S1 AND AcademicYear=@Year AND IsActive=1)
        INSERT dbo.StudentTransportAssignments(StudentID,RouteID,AcademicYear,PickupPoint,DropoffPoint,IsActive,StartDate,Notes,CreatedAt)
        VALUES(@S1,@Route,@Year,N'باب اليمن',N'مدرسة النور',1,'2026-09-10',@Demo,GETDATE());

    DECLARE @Book1 int;
    SELECT @Book1=BookID FROM dbo.Books WHERE ISBN=N'978-0000000011';
    IF @Book1 IS NULL
    BEGIN
        INSERT dbo.Books(Title,Author,ISBN,Category,Publisher,PublicationYear,Copies,AvailableCopies,ShelfLocation,Notes,CreatedAt,IsActive)
        VALUES(N'أساسيات الرياضيات المدرسية',N'قسم الرياضيات',N'978-0000000011',N'رياضيات',N'مكتبة النور',2025,2,2,N'رف A-01',@Demo,GETDATE(),1);
        SET @Book1=CONVERT(int,SCOPE_IDENTITY());
    END;
    DECLARE @Book2 int;
    SELECT @Book2=BookID FROM dbo.Books WHERE ISBN=N'978-0000000028';
    IF @Book2 IS NULL
    BEGIN
        INSERT dbo.Books(Title,Author,ISBN,Category,Publisher,PublicationYear,Copies,AvailableCopies,ShelfLocation,Notes,CreatedAt,IsActive)
        VALUES(N'مبادئ الفيزياء',N'قسم العلوم',N'978-0000000028',N'فيزياء',N'مكتبة النور',2025,2,2,N'رف B-02',@Demo,GETDATE(),1);
        SET @Book2=CONVERT(int,SCOPE_IDENTITY());
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.BookBorrowings WHERE BookID=@Book1 AND BorrowerType=N'طالب' AND BorrowerID=@S1 AND Status=N'معار')
    BEGIN
        INSERT dbo.BookBorrowings(BookID,BorrowerType,BorrowerID,BorrowDate,DueDate,Status,Notes,CreatedAt)
        VALUES(@Book1,N'طالب',@S1,'2026-10-06','2026-10-20',N'معار',@Demo,GETDATE());
        UPDATE dbo.Books SET AvailableCopies=CASE WHEN AvailableCopies>0 THEN AvailableCopies-1 ELSE 0 END,UpdatedAt=GETDATE() WHERE BookID=@Book1;
    END;

    DECLARE @E1 int;
    IF NOT EXISTS (SELECT 1 FROM dbo.Expenses WHERE Notes=@Demo+N':EXP1')
    BEGIN
        INSERT dbo.Expenses(Amount,ExpenseDate,Category,PayeeName,PaymentMethod,Description,Notes,CreatedAt)
        VALUES(15000,'2026-10-04',N'قرطاسية',N'مكتبة الإمداد',N'نقداً',N'شراء أدوات وقرطاسية للمدرسة',@Demo+N':EXP1',GETDATE());
        SET @E1=CONVERT(int,SCOPE_IDENTITY());
        UPDATE dbo.Expenses SET ExpenseNumber=N'EXP-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@E1),5) WHERE ExpenseID=@E1;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'صرف',15000,'2026-10-04',N'مكتبة الإمداد',N'سند صرف قرطاسية',N'نقداً',N'مصروفات',@E1,@Demo+N':EXP1',1);
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Expenses WHERE Notes=@Demo+N':EXP2')
    BEGIN
        DECLARE @E2 int;
        INSERT dbo.Expenses(Amount,ExpenseDate,Category,PayeeName,PaymentMethod,Description,Notes,CreatedAt)
        VALUES(25000,'2026-10-06',N'صيانة',N'مؤسسة الصيانة الحديثة',N'تحويل بنكي',
               N'صيانة أجهزة ومعمل العلوم',@Demo+N':EXP2',GETDATE());
        SET @E2=CONVERT(int,SCOPE_IDENTITY());
        UPDATE dbo.Expenses
        SET ExpenseNumber=N'EXP-2026-'+RIGHT(N'00000'+CONVERT(nvarchar(10),@E2),5)
        WHERE ExpenseID=@E2;
        INSERT dbo.Vouchers(VoucherType,Amount,VoucherDate,PartyName,Description,PaymentMethod,ReferenceType,ReferenceID,Notes,IsAutoGenerated)
        VALUES(N'صرف',25000,'2026-10-06',N'مؤسسة الصيانة الحديثة',N'سند صرف أعمال صيانة',
               N'تحويل بنكي',N'مصروفات',@E2,@Demo+N':EXP2',1);
    END;

    UPDATE v
    SET VoucherNumber=N'PAY-'+CONVERT(nvarchar(4),YEAR(VoucherDate))+N'-'+RIGHT(N'00000'+CONVERT(nvarchar(10),VoucherID),5)
    FROM dbo.Vouchers v
    WHERE v.Notes LIKE @Demo+N':EXP%'
      AND (v.VoucherNumber IS NULL OR v.VoucherNumber=N'');

    /* ---------------------------------------------------------------
       سجل تدقيق تجريبي
       --------------------------------------------------------------- */
    IF @AdminUserID IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.AuditLogs WHERE Details LIKE @Demo+N'%')
    BEGIN
        INSERT dbo.AuditLogs
        (UserID,UserName,ActionName,EntityName,EntityID,Details,CreatedAt,Module,MachineName,IpAddress)
        VALUES
        (@AdminUserID,N'مدير النظام',N'إنشاء',N'Student',CONVERT(nvarchar(30),@S1),
         @Demo+N' - إدخال طالب تجريبي',GETDATE(),N'الطلاب',HOST_NAME(),N'127.0.0.1'),
        (@AdminUserID,N'مدير النظام',N'إنشاء',N'FeePlan',CONVERT(nvarchar(30),@P1T),
         @Demo+N' - إنشاء خطة رسوم',GETDATE(),N'خطط الرسوم',HOST_NAME(),N'127.0.0.1'),
        (@AdminUserID,N'مدير النظام',N'إنشاء',N'Voucher',@Demo+N':P1',
         @Demo+N' - تسجيل سند قبض',GETDATE(),N'السندات',HOST_NAME(),N'127.0.0.1');
    END;

    /* ---------------------------------------------------------------
       تحقق نهائي
       --------------------------------------------------------------- */
    IF EXISTS
    (
        SELECT 1 FROM dbo.Fees f
        WHERE f.Notes=@Demo
          AND (f.TotalAmount<0 OR f.DiscountAmount<0 OR f.PaidAmount<0 OR f.PaidAmount>f.NetAmount)
          AND f.FeeType NOT IN (N'الرسوم الكلية',N'رسوم كلية',N'الرسوم الكليه',N'رسوم كليه',N'Total Fee')
    )
        THROW 56010,N'وجد خلل في مبالغ بيانات الرسوم التجريبية.',1;

    COMMIT TRANSACTION;

    SELECT
        N'تم تجهيز البيانات التجريبية بنجاح' AS Result,
        (SELECT COUNT(*) FROM dbo.Students WHERE StudentNumber IN (N'10001',N'10002',N'20001',N'20002',N'30001',N'30002')) AS Students,
        (SELECT COUNT(*) FROM dbo.Teachers WHERE EmployeeNumber IN (N'30001',N'30002',N'30003')) AS Teachers,
        (SELECT COUNT(*) FROM dbo.Enrollments WHERE GeneralNotes=@Demo) AS Enrollments,
        (SELECT COUNT(*) FROM dbo.FeePlans WHERE Notes=@Demo) AS FeePlans,
        (SELECT COUNT(*) FROM dbo.Fees WHERE Notes=@Demo) AS Fees,
        (SELECT COUNT(*) FROM dbo.Vouchers WHERE Notes LIKE @Demo+N'%') AS Vouchers,
        (SELECT COUNT(*) FROM dbo.Payroll WHERE Notes=@Demo) AS Payroll,
        (SELECT COUNT(*) FROM dbo.Grades WHERE Notes=@Demo) AS Grades,
        (SELECT COUNT(*) FROM dbo.StudentAttendance WHERE Notes=@Demo) AS StudentAttendance,
        (SELECT COUNT(*) FROM dbo.TeacherAttendance WHERE Notes=@Demo) AS TeacherAttendance,
        (SELECT COUNT(*) FROM dbo.SchoolTimetable WHERE Notes=@Demo) AS Timetable,
        (SELECT COUNT(*) FROM dbo.StudentTransportAssignments WHERE Notes=@Demo) AS Transport,
        (SELECT COUNT(*) FROM dbo.Books WHERE Notes=@Demo) AS Books,
        (SELECT COUNT(*) FROM dbo.BookBorrowings WHERE Notes=@Demo) AS Borrowings,
        (SELECT COUNT(*) FROM dbo.Expenses WHERE Notes LIKE @Demo+N'%') AS Expenses;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;