/*
    إصلاح أعمدة الدرجات المطلوبة من GradeRepository وGradeEntryForm.
    آمن لإعادة التشغيل: لا يضيف العمود إذا كان موجودًا.
*/
SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    THROW 51020, N'جدول dbo.Grades غير موجود. شغّل ترحيلات إنشاء الجداول الأساسية أولاً.', 1;
END;

IF COL_LENGTH(N'dbo.Grades', N'Section') IS NULL
    ALTER TABLE dbo.Grades ADD Section NVARCHAR(100) NULL;

IF COL_LENGTH(N'dbo.Grades', N'Quiz1') IS NULL
    ALTER TABLE dbo.Grades ADD Quiz1 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Repair_Quiz1 DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.Grades', N'Quiz2') IS NULL
    ALTER TABLE dbo.Grades ADD Quiz2 DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Repair_Quiz2 DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.Grades', N'CourseWork') IS NULL
    ALTER TABLE dbo.Grades ADD CourseWork DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Repair_CourseWork DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.Grades', N'FinalExam') IS NULL
    ALTER TABLE dbo.Grades ADD FinalExam DECIMAL(10,2) NOT NULL CONSTRAINT DF_Grades_Repair_FinalExam DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.Grades', N'GradeLetter') IS NULL
    ALTER TABLE dbo.Grades ADD GradeLetter NVARCHAR(50) NULL;

IF COL_LENGTH(N'dbo.Grades', N'ResultStatus') IS NULL
    ALTER TABLE dbo.Grades ADD ResultStatus NVARCHAR(50) NULL;

IF COL_LENGTH(N'dbo.Grades', N'Notes') IS NULL
    ALTER TABLE dbo.Grades ADD Notes NVARCHAR(MAX) NULL;

IF COL_LENGTH(N'dbo.Grades', N'UpdatedAt') IS NULL
    ALTER TABLE dbo.Grades ADD UpdatedAt DATETIME NULL;

PRINT N'تم التحقق من أعمدة جدول الدرجات وإضافة الأعمدة المفقودة عند الحاجة.';
GO
