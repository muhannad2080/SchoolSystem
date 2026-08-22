/*
   SchoolSystem - Database integrity verification
   يعرض حالة العلاقات المطلوبة والبيانات اليتيمة دون تعديل البيانات.
*/
SET NOCOUNT ON;
IF DB_ID(N'SchoolDB') IS NULL
    THROW 50021, N'قاعدة SchoolDB غير موجودة.', 1;
GO
USE SchoolDB;
GO

DECLARE @Expected TABLE
(
    ConstraintName SYSNAME NOT NULL,
    ChildTable SYSNAME NOT NULL,
    ChildColumn SYSNAME NOT NULL,
    ParentTable SYSNAME NOT NULL,
    ParentColumn SYSNAME NOT NULL
);
INSERT INTO @Expected VALUES
(N'FK_AuditLogs_Users', N'AuditLogs', N'UserID', N'Users', N'UserID'),
(N'FK_FeePlans_Classes', N'FeePlans', N'ClassID', N'Classes', N'ClassID'),
(N'FK_Fees_Students', N'Fees', N'StudentID', N'Students', N'StudentID'),
(N'FK_Fees_FeePlans', N'Fees', N'FeePlanID', N'FeePlans', N'FeePlanID'),
(N'FK_Grades_Students', N'Grades', N'StudentID', N'Students', N'StudentID'),
(N'FK_Grades_Subjects', N'Grades', N'SubjectID', N'Subjects', N'SubjectID'),
(N'FK_Grades_Classes', N'Grades', N'ClassID', N'Classes', N'ClassID'),
(N'FK_Payroll_Teachers', N'Payroll', N'TeacherID', N'Teachers', N'TeacherID'),
(N'FK_Receipts_Students', N'Receipts', N'StudentID', N'Students', N'StudentID'),
(N'FK_StudentFees_Students', N'StudentFees', N'StudentID', N'Students', N'StudentID'),
(N'FK_TeacherAttendance_Teachers', N'TeacherAttendance', N'TeacherID', N'Teachers', N'TeacherID'),
(N'FK_TeacherContracts_Teachers', N'TeacherContracts', N'TeacherID', N'Teachers', N'TeacherID'),
(N'FK_SchoolTimetable_Classes', N'SchoolTimetable', N'ClassID', N'Classes', N'ClassID'),
(N'FK_SchoolTimetable_Subjects', N'SchoolTimetable', N'SubjectID', N'Subjects', N'SubjectID'),
(N'FK_SchoolTimetable_Teachers', N'SchoolTimetable', N'TeacherID', N'Teachers', N'TeacherID');

SELECT e.ConstraintName, e.ChildTable, e.ChildColumn, e.ParentTable, e.ParentColumn,
       CASE WHEN EXISTS
       (
           SELECT 1
           FROM sys.foreign_keys fk
           INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
           WHERE fk.parent_object_id = OBJECT_ID(N'dbo.' + e.ChildTable)
             AND fk.referenced_object_id = OBJECT_ID(N'dbo.' + e.ParentTable)
             AND COL_NAME(fkc.parent_object_id, fkc.parent_column_id) = e.ChildColumn
             AND COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) = e.ParentColumn
       ) THEN N'PRESENT' ELSE N'MISSING' END AS Status
FROM @Expected e
ORDER BY Status DESC, e.ChildTable, e.ChildColumn;

/* Static, non-destructive orphan checks for nullable and required references. */
DECLARE @Orphans TABLE (RelationName SYSNAME, OrphanRows INT);
INSERT INTO @Orphans VALUES
(N'Fees.StudentID -> Students', CASE WHEN OBJECT_ID(N'dbo.Fees') IS NULL OR OBJECT_ID(N'dbo.Students') IS NULL THEN -1 ELSE (SELECT COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.Students s ON s.StudentID=f.StudentID WHERE f.StudentID IS NOT NULL AND s.StudentID IS NULL) END),
(N'Fees.FeePlanID -> FeePlans', CASE WHEN OBJECT_ID(N'dbo.Fees') IS NULL OR OBJECT_ID(N'dbo.FeePlans') IS NULL THEN -1 ELSE (SELECT COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.FeePlans p ON p.FeePlanID=f.FeePlanID WHERE f.FeePlanID IS NOT NULL AND p.FeePlanID IS NULL) END),
(N'Grades.StudentID -> Students', CASE WHEN OBJECT_ID(N'dbo.Grades') IS NULL OR OBJECT_ID(N'dbo.Students') IS NULL THEN -1 ELSE (SELECT COUNT(*) FROM dbo.Grades g LEFT JOIN dbo.Students s ON s.StudentID=g.StudentID WHERE g.StudentID IS NOT NULL AND s.StudentID IS NULL) END),
(N'Grades.SubjectID -> Subjects', CASE WHEN OBJECT_ID(N'dbo.Grades') IS NULL OR OBJECT_ID(N'dbo.Subjects') IS NULL THEN -1 ELSE (SELECT COUNT(*) FROM dbo.Grades g LEFT JOIN dbo.Subjects s ON s.SubjectID=g.SubjectID WHERE g.SubjectID IS NOT NULL AND s.SubjectID IS NULL) END),
(N'Payroll.TeacherID -> Teachers', CASE WHEN OBJECT_ID(N'dbo.Payroll') IS NULL OR OBJECT_ID(N'dbo.Teachers') IS NULL THEN -1 ELSE (SELECT COUNT(*) FROM dbo.Payroll p LEFT JOIN dbo.Teachers t ON t.TeacherID=p.TeacherID WHERE t.TeacherID IS NULL) END);
SELECT RelationName, OrphanRows, CASE WHEN OrphanRows = 0 THEN N'PASS' WHEN OrphanRows = -1 THEN N'NOT_CHECKED_MISSING_TABLE' ELSE N'REVIEW' END AS Status
FROM @Orphans ORDER BY RelationName;

SELECT t.name AS TableName, c.name AS ColumnName, i.name AS IndexName
FROM sys.tables t
JOIN sys.columns c ON c.object_id=t.object_id
LEFT JOIN sys.index_columns ic ON ic.object_id=t.object_id AND ic.column_id=c.column_id
LEFT JOIN sys.indexes i ON i.object_id=ic.object_id AND i.index_id=ic.index_id
WHERE c.name IN (N'StudentID',N'TeacherID',N'ClassID',N'SubjectID',N'FeePlanID')
ORDER BY t.name, c.name;
GO
