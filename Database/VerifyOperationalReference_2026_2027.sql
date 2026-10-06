/*
    SchoolSystem - Verify 2026/2027 operational reference data
    -----------------------------------------------------------
    يعرض حالة المرجع الأكاديمي ويتأكد من عدم وجود بيانات أشخاص/مالية
    أو سجلات تشغيلية قديمة بعد التنظيف.
*/

USE [SchoolDB];
SET NOCOUNT ON;

SELECT N'المرحلة' AS SectionName, StageID AS ID, StageCode AS Code, StageName AS Name, StageOrder AS SortOrder
FROM dbo.Stages
ORDER BY StageOrder, StageID;

SELECT N'الصف' AS SectionName, c.ClassID AS ID, c.ClassCode AS Code, c.ClassName AS Name,
       s.StageName, c.GradeOrder, c.IsActive
FROM dbo.Classes c
LEFT JOIN dbo.Stages s ON s.StageID = c.StageID
ORDER BY c.GradeOrder, c.ClassID;

SELECT
    s.SubjectID,
    s.SubjectCode,
    s.SubjectName,
    COUNT(DISTINCT cs.ClassID) AS ClassesCount,
    COUNT(DISTINCT CASE WHEN t.TermOrder = 1 THEN t.TermID END) AS Term1Count,
    COUNT(DISTINCT CASE WHEN t.TermOrder = 2 THEN t.TermID END) AS Term2Count
FROM dbo.Subjects s
LEFT JOIN dbo.ClassSubjects cs
    ON cs.SubjectID = s.SubjectID
   AND ISNULL(cs.IsActive, 1) = 1
LEFT JOIN dbo.ClassSubjectTerms cst
    ON cst.ClassSubjectID = cs.ClassSubjectID
   AND ISNULL(cst.IsActive, 1) = 1
LEFT JOIN dbo.AcademicTerms t
    ON t.TermID = cst.TermID
   AND ISNULL(t.IsActive, 1) = 1
GROUP BY s.SubjectID, s.SubjectCode, s.SubjectName
ORDER BY s.SubjectName;

SELECT
    c.GradeOrder,
    c.ClassName,
    t.TermName,
    COUNT(DISTINCT cs.SubjectID) AS SubjectCount
FROM dbo.Classes c
CROSS JOIN dbo.AcademicTerms t
LEFT JOIN dbo.ClassSubjects cs
    ON cs.ClassID = c.ClassID
   AND ISNULL(cs.IsActive, 1) = 1
LEFT JOIN dbo.ClassSubjectTerms cst
    ON cst.ClassSubjectID = cs.ClassSubjectID
   AND cst.TermID = t.TermID
   AND ISNULL(cst.IsActive, 1) = 1
WHERE ISNULL(c.IsActive, 1) = 1
  AND ISNULL(t.IsActive, 1) = 1
GROUP BY c.GradeOrder, c.ClassName, t.TermOrder, t.TermName
ORDER BY c.GradeOrder, t.TermOrder;

SELECT
    N'Users' AS TableName, COUNT(*) AS RowCount FROM dbo.Users
UNION ALL SELECT N'Roles', COUNT(*) FROM dbo.Roles
UNION ALL SELECT N'Permissions', COUNT(*) FROM dbo.Permissions
UNION ALL SELECT N'Rooms', COUNT(*) FROM dbo.Rooms
UNION ALL SELECT N'AcademicYears', COUNT(*) FROM dbo.AcademicYears
UNION ALL SELECT N'AcademicTerms', COUNT(*) FROM dbo.AcademicTerms
UNION ALL SELECT N'Stages', COUNT(*) FROM dbo.Stages
UNION ALL SELECT N'Classes', COUNT(*) FROM dbo.Classes
UNION ALL SELECT N'Subjects', COUNT(*) FROM dbo.Subjects
UNION ALL SELECT N'ClassSubjects', COUNT(*) FROM dbo.ClassSubjects
UNION ALL SELECT N'ClassSubjectTerms', COUNT(*) FROM dbo.ClassSubjectTerms
UNION ALL SELECT N'SchoolSections', COUNT(*) FROM dbo.SchoolSections
UNION ALL SELECT N'Students', COUNT(*) FROM dbo.Students
UNION ALL SELECT N'Teachers', COUNT(*) FROM dbo.Teachers
UNION ALL SELECT N'Fees', COUNT(*) FROM dbo.Fees
UNION ALL SELECT N'Vouchers', COUNT(*) FROM dbo.Vouchers
UNION ALL SELECT N'StudentFees', COUNT(*) FROM dbo.StudentFees
UNION ALL SELECT N'Payroll', COUNT(*) FROM dbo.Payroll
UNION ALL SELECT N'TeacherContracts', COUNT(*) FROM dbo.TeacherContracts
UNION ALL SELECT N'Grades', COUNT(*) FROM dbo.Grades
UNION ALL SELECT N'Marks', COUNT(*) FROM dbo.Marks
UNION ALL SELECT N'StudentAttendance', COUNT(*) FROM dbo.StudentAttendance
UNION ALL SELECT N'TeacherAttendance', COUNT(*) FROM dbo.TeacherAttendance
UNION ALL SELECT N'Expenses', COUNT(*) FROM dbo.Expenses
UNION ALL SELECT N'Enrollments', COUNT(*) FROM dbo.Enrollments
UNION ALL SELECT N'SchoolTimetable', COUNT(*) FROM dbo.SchoolTimetable
UNION ALL SELECT N'AnnualClosings', COUNT(*) FROM dbo.AnnualClosings
UNION ALL SELECT N'AnnualMigrationLog', COUNT(*) FROM dbo.AnnualMigrationLog
UNION ALL SELECT N'AuditLogs', COUNT(*) FROM dbo.AuditLogs
ORDER BY TableName;
