/*
  SchoolSystem - Verify Demo Data 2026/2027
  يعرض أعداد البيانات التجريبية ويتحقق من العلاقات والمجاميع المالية.
*/
SET NOCOUNT ON;
USE SchoolDB;

DECLARE @Year nvarchar(20)=N'2026/2027';
DECLARE @Demo nvarchar(100)=N'DEMO-SEED-2026-2027';

SELECT N'الحالة العامة' AS Section,
       CASE WHEN EXISTS (SELECT 1 FROM dbo.AcademicYears WHERE AcademicYear=@Year AND IsActive=1)
            THEN N'OK' ELSE N'خطأ: العام غير موجود' END AS Status;

SELECT
    (SELECT COUNT(*) FROM dbo.Students WHERE StudentNumber IN (N'10001',N'10002',N'20001',N'20002',N'30001',N'30002')) AS DemoStudents,
    (SELECT COUNT(*) FROM dbo.Teachers WHERE EmployeeNumber IN (N'30001',N'30002',N'30003')) AS DemoTeachers,
    (SELECT COUNT(*) FROM dbo.Enrollments WHERE GeneralNotes=@Demo) AS DemoEnrollments,
    (SELECT COUNT(*) FROM dbo.StudentClasses sc INNER JOIN dbo.Students s ON s.StudentID=sc.StudentID WHERE sc.AcademicYear=@Year AND s.StudentNumber IN (N'10001',N'10002',N'20001',N'20002',N'30001',N'30002')) AS DemoStudentClasses,
    (SELECT COUNT(*) FROM dbo.FeePlans WHERE Notes=@Demo) AS DemoFeePlans,
    (SELECT COUNT(*) FROM dbo.Fees WHERE Notes=@Demo) AS DemoFees,
    (SELECT COUNT(*) FROM dbo.Vouchers WHERE Notes LIKE @Demo+N'%') AS DemoVouchers,
    (SELECT COUNT(*) FROM dbo.TeacherContracts WHERE Notes=@Demo) AS DemoContracts,
    (SELECT COUNT(*) FROM dbo.Payroll WHERE Notes=@Demo) AS DemoPayroll,
    (SELECT COUNT(*) FROM dbo.StudentAttendance WHERE Notes=@Demo) AS DemoStudentAttendance,
    (SELECT COUNT(*) FROM dbo.TeacherAttendance WHERE Notes=@Demo) AS DemoTeacherAttendance,
    (SELECT COUNT(*) FROM dbo.Grades WHERE Notes=@Demo) AS DemoGrades,
    (SELECT COUNT(*) FROM dbo.SchoolTimetable WHERE Notes=@Demo) AS DemoTimetable,
    (SELECT COUNT(*) FROM dbo.StudentTransportAssignments WHERE Notes=@Demo) AS DemoTransport,
    (SELECT COUNT(*) FROM dbo.Books WHERE Notes=@Demo) AS DemoBooks,
    (SELECT COUNT(*) FROM dbo.BookBorrowings WHERE Notes=@Demo) AS DemoBorrowings,
    (SELECT COUNT(*) FROM dbo.Expenses WHERE Notes LIKE @Demo+N'%') AS DemoExpenses,
    (SELECT COUNT(*) FROM dbo.AuditLogs WHERE Details LIKE @Demo+N'%') AS DemoAuditLogs;

SELECT
    f.FeeType,
    COUNT(*) AS RowsCount,
    SUM(f.TotalAmount) AS TotalAmount,
    SUM(f.DiscountAmount) AS DiscountAmount,
    SUM(f.NetAmount) AS NetAmount,
    SUM(f.PaidAmount) AS PaidAmount,
    SUM(f.RemainingAmount) AS RemainingAmount
FROM dbo.Fees f
WHERE f.Notes=@Demo
GROUP BY f.FeeType
ORDER BY f.FeeType;

SELECT
    s.StudentNumber,
    s.FullName,
    c.ClassName,
    sc.Section,
    SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType,N''))) IN (N'الرسوم الكلية',N'رسوم كلية',N'الرسوم الكليه',N'رسوم كليه',N'Total Fee') THEN 0 ELSE f.NetAmount END) AS NetFees,
    SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType,N''))) IN (N'الرسوم الكلية',N'رسوم كلية',N'الرسوم الكليه',N'رسوم كليه',N'Total Fee') THEN 0 ELSE f.PaidAmount END) AS PaidFees,
    SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType,N''))) IN (N'الرسوم الكلية',N'رسوم كلية',N'الرسوم الكليه',N'رسوم كليه',N'Total Fee') THEN 0 ELSE f.RemainingAmount END) AS RemainingFees
FROM dbo.Students s
INNER JOIN dbo.StudentClasses sc ON sc.StudentID=s.StudentID
    AND sc.AcademicYear=@Year
LEFT JOIN dbo.Classes c ON c.ClassID=sc.ClassID
LEFT JOIN dbo.Fees f ON f.StudentID=s.StudentID AND f.AcademicYear=@Year AND f.Notes=@Demo
WHERE s.StudentNumber IN (N'10001',N'10002',N'20001',N'20002',N'30001',N'30002')
GROUP BY s.StudentNumber,s.FullName,c.ClassName,sc.Section
ORDER BY s.StudentNumber;

SELECT
    CASE
      WHEN EXISTS (
        SELECT 1 FROM dbo.Fees f
        LEFT JOIN dbo.FeePlans fp ON fp.FeePlanID=f.FeePlanID
        WHERE f.Notes=@Demo
          AND f.FeePlanID IS NOT NULL
          AND fp.FeePlanID IS NULL
      ) THEN N'FAIL: رسوم تجريبية مرتبطة بخطة غير موجودة'
      WHEN EXISTS (
        SELECT 1 FROM dbo.Fees f
        WHERE f.Notes=@Demo
          AND LTRIM(RTRIM(ISNULL(f.FeeType,N''))) NOT IN (N'الرسوم الكلية',N'رسوم كلية',N'الرسوم الكليه',N'رسوم كليه',N'Total Fee')
          AND (f.TotalAmount<0 OR f.DiscountAmount<0 OR f.PaidAmount<0 OR f.NetAmount<0)
      ) THEN N'FAIL: قيم مالية سالبة'
      WHEN EXISTS (
        SELECT 1 FROM dbo.StudentClasses sc
        INNER JOIN dbo.Students s ON s.StudentID=sc.StudentID
        WHERE sc.AcademicYear=@Year
          AND s.StudentNumber IN (N'10001',N'10002',N'20001',N'20002',N'30001',N'30002')
        GROUP BY sc.StudentID,sc.AcademicYear
        HAVING COUNT(*)>1
      ) THEN N'FAIL: أكثر من توزيع للطالب في نفس العام'
      ELSE N'PASS: العلاقات والقيم الأساسية سليمة'
    END AS DemoIntegrityCheck;