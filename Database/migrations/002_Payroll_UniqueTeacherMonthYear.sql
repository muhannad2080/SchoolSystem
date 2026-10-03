/*
  002_Payroll_UniqueTeacherMonthYear.sql
  يمنع تسجيل أكثر من راتب للمعلم في الشهر والسنة نفسها.
*/
IF OBJECT_ID(N'dbo.Payroll', N'U') IS NULL
    THROW 51020, N'جدول Payroll غير موجود؛ تعذر تطبيق حماية تكرار الرواتب.', 1;

IF EXISTS
(
    SELECT 1
    FROM dbo.Payroll
    GROUP BY TeacherID, SalaryMonth, SalaryYear
    HAVING COUNT(*) > 1
)
    THROW 51021, N'توجد سجلات رواتب مكررة لنفس المعلم والشهر والسنة؛ عالج التكرار قبل تطبيق القيد.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_Payroll_Teacher_Month_Year'
      AND object_id = OBJECT_ID(N'dbo.Payroll')
)
BEGIN
    CREATE UNIQUE INDEX UX_Payroll_Teacher_Month_Year
        ON dbo.Payroll(TeacherID, SalaryMonth, SalaryYear);
END;
