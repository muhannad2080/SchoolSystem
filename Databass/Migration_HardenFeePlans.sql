/* تشغيل هذا الملف على قاعدة SchoolDB فقط */
IF DB_NAME() <> N'SchoolDB'
    THROW 51030, N'يجب تشغيل ترحيل خطط الرسوم على قاعدة SchoolDB فقط.', 1;

IF OBJECT_ID(N'dbo.FeePlans', N'U') IS NULL
    THROW 51031, N'جدول FeePlans غير موجود. شغّل الترحيل الأساسي أولاً.', 1;

IF EXISTS
(
    SELECT AcademicYear, ClassID, FeeType
    FROM dbo.FeePlans
    GROUP BY AcademicYear, ClassID, FeeType
    HAVING COUNT(*) > 1
)
    THROW 51032, N'توجد خطط رسوم مكررة. عالج التكرارات قبل إنشاء القيد الفريد.', 1;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.FeePlans')
      AND name = N'UX_FeePlans_AcademicYear_Class_FeeType'
)
BEGIN
    CREATE UNIQUE INDEX UX_FeePlans_AcademicYear_Class_FeeType
        ON dbo.FeePlans(AcademicYear, ClassID, FeeType);
END;

PRINT N'تم تفعيل حماية تكرار خطط الرسوم بنجاح.';
