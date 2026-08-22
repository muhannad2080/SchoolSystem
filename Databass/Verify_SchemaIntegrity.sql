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
(N'FK_Enrollments_Students_Complete', N'Enrollments', N'StudentID', N'Students', N'StudentID'),
(N'FK_Enrollments_Classes_Complete', N'Enrollments', N'ClassID', N'Classes', N'ClassID'),
(N'FK_Classes_Rooms_Complete', N'Classes', N'RoomID', N'Rooms', N'RoomID'),
(N'FK_Rooms_CreatedByUser_Complete', N'Rooms', N'CreatedByUserID', N'Users', N'UserID'),
(N'FK_Expenses_CreatedByUser_Complete', N'Expenses', N'CreatedByUserID', N'Users', N'UserID'),
(N'FK_Vouchers_CreatedByUser_Complete', N'Vouchers', N'CreatedByUserID', N'Users', N'UserID'),
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

/* Static, non-destructive orphan checks. Optional columns are evaluated dynamically. */
DECLARE @Orphans TABLE (RelationName SYSNAME NOT NULL, OrphanRows INT NOT NULL);

IF OBJECT_ID(N'dbo.Fees', N'U') IS NULL OR OBJECT_ID(N'dbo.Students', N'U') IS NULL OR COL_LENGTH(N'dbo.Fees', N'StudentID') IS NULL
    INSERT INTO @Orphans VALUES (N'Fees.StudentID -> Students', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Fees.StudentID -> Students'', COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.Students s ON s.StudentID=f.StudentID WHERE f.StudentID IS NOT NULL AND s.StudentID IS NULL';

IF OBJECT_ID(N'dbo.Fees', N'U') IS NULL OR OBJECT_ID(N'dbo.FeePlans', N'U') IS NULL OR COL_LENGTH(N'dbo.Fees', N'FeePlanID') IS NULL
    INSERT INTO @Orphans VALUES (N'Fees.FeePlanID -> FeePlans', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Fees.FeePlanID -> FeePlans'', COUNT(*) FROM dbo.Fees f LEFT JOIN dbo.FeePlans p ON p.FeePlanID=f.FeePlanID WHERE f.FeePlanID IS NOT NULL AND p.FeePlanID IS NULL';

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL OR OBJECT_ID(N'dbo.Students', N'U') IS NULL OR COL_LENGTH(N'dbo.Grades', N'StudentID') IS NULL
    INSERT INTO @Orphans VALUES (N'Grades.StudentID -> Students', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Grades.StudentID -> Students'', COUNT(*) FROM dbo.Grades g LEFT JOIN dbo.Students s ON s.StudentID=g.StudentID WHERE g.StudentID IS NOT NULL AND s.StudentID IS NULL';

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL OR OBJECT_ID(N'dbo.Subjects', N'U') IS NULL OR COL_LENGTH(N'dbo.Grades', N'SubjectID') IS NULL
    INSERT INTO @Orphans VALUES (N'Grades.SubjectID -> Subjects', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Grades.SubjectID -> Subjects'', COUNT(*) FROM dbo.Grades g LEFT JOIN dbo.Subjects s ON s.SubjectID=g.SubjectID WHERE g.SubjectID IS NOT NULL AND s.SubjectID IS NULL';

IF OBJECT_ID(N'dbo.Payroll', N'U') IS NULL OR OBJECT_ID(N'dbo.Teachers', N'U') IS NULL OR COL_LENGTH(N'dbo.Payroll', N'TeacherID') IS NULL
    INSERT INTO @Orphans VALUES (N'Payroll.TeacherID -> Teachers', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Payroll.TeacherID -> Teachers'', COUNT(*) FROM dbo.Payroll p LEFT JOIN dbo.Teachers t ON t.TeacherID=p.TeacherID WHERE p.TeacherID IS NOT NULL AND t.TeacherID IS NULL';

IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL OR OBJECT_ID(N'dbo.Students', N'U') IS NULL OR COL_LENGTH(N'dbo.Enrollments', N'StudentID') IS NULL
    INSERT INTO @Orphans VALUES (N'Enrollments.StudentID -> Students', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Enrollments.StudentID -> Students'', COUNT(*) FROM dbo.Enrollments e LEFT JOIN dbo.Students s ON s.StudentID=e.StudentID WHERE e.StudentID IS NOT NULL AND s.StudentID IS NULL';

IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL OR OBJECT_ID(N'dbo.Classes', N'U') IS NULL OR COL_LENGTH(N'dbo.Enrollments', N'ClassID') IS NULL
    INSERT INTO @Orphans VALUES (N'Enrollments.ClassID -> Classes', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Enrollments.ClassID -> Classes'', COUNT(*) FROM dbo.Enrollments e LEFT JOIN dbo.Classes c ON c.ClassID=e.ClassID WHERE e.ClassID IS NOT NULL AND c.ClassID IS NULL';

IF OBJECT_ID(N'dbo.Classes', N'U') IS NULL OR OBJECT_ID(N'dbo.Rooms', N'U') IS NULL OR COL_LENGTH(N'dbo.Classes', N'RoomID') IS NULL
    INSERT INTO @Orphans VALUES (N'Classes.RoomID -> Rooms', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Classes.RoomID -> Rooms'', COUNT(*) FROM dbo.Classes c LEFT JOIN dbo.Rooms r ON r.RoomID=c.RoomID WHERE c.RoomID IS NOT NULL AND r.RoomID IS NULL';

IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL OR OBJECT_ID(N'dbo.Users', N'U') IS NULL OR COL_LENGTH(N'dbo.AuditLogs', N'UserID') IS NULL
    INSERT INTO @Orphans VALUES (N'AuditLogs.UserID -> Users', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''AuditLogs.UserID -> Users'', COUNT(*) FROM dbo.AuditLogs a LEFT JOIN dbo.Users u ON u.UserID=a.UserID WHERE a.UserID IS NOT NULL AND u.UserID IS NULL';

IF OBJECT_ID(N'dbo.Expenses', N'U') IS NULL OR OBJECT_ID(N'dbo.Users', N'U') IS NULL OR COL_LENGTH(N'dbo.Expenses', N'CreatedByUserID') IS NULL
    INSERT INTO @Orphans VALUES (N'Expenses.CreatedByUserID -> Users', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Expenses.CreatedByUserID -> Users'', COUNT(*) FROM dbo.Expenses e LEFT JOIN dbo.Users u ON u.UserID=e.CreatedByUserID WHERE e.CreatedByUserID IS NOT NULL AND u.UserID IS NULL';

IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NULL OR OBJECT_ID(N'dbo.Users', N'U') IS NULL OR COL_LENGTH(N'dbo.Vouchers', N'CreatedByUserID') IS NULL
    INSERT INTO @Orphans VALUES (N'Vouchers.CreatedByUserID -> Users', -1);
ELSE
    INSERT INTO @Orphans EXEC sys.sp_executesql N'SELECT N''Vouchers.CreatedByUserID -> Users'', COUNT(*) FROM dbo.Vouchers v LEFT JOIN dbo.Users u ON u.UserID=v.CreatedByUserID WHERE v.CreatedByUserID IS NOT NULL AND u.UserID IS NULL';

SELECT RelationName, OrphanRows, CASE WHEN OrphanRows = 0 THEN N'PASS' WHEN OrphanRows = -1 THEN N'NOT_CHECKED_MISSING_TABLE_OR_COLUMN' ELSE N'REVIEW' END AS Status
FROM @Orphans ORDER BY RelationName;

SELECT t.name AS TableName, c.name AS ColumnName, i.name AS IndexName
FROM sys.tables t
JOIN sys.columns c ON c.object_id=t.object_id
LEFT JOIN sys.index_columns ic ON ic.object_id=t.object_id AND ic.column_id=c.column_id
LEFT JOIN sys.indexes i ON i.object_id=ic.object_id AND i.index_id=ic.index_id
WHERE c.name IN (N'StudentID',N'TeacherID',N'ClassID',N'SubjectID',N'FeePlanID',N'RoomID',N'UserID',N'CreatedByUserID')
ORDER BY t.name, c.name;
GO
