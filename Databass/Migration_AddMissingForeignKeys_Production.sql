/*
   SchoolSystem - Production foreign-key hardening
   الغرض: استكمال العلاقات المرجعية المؤكدة بين الجداول التشغيلية.
   آمن للتشغيل المتكرر: لا يحذف بيانات ولا ينشئ علاقة فوق بيانات يتيمة.
   إذا وُجدت بيانات يتيمة، تُطبع الحالة لتُعالج بعد مراجعة البيانات ثم يُعاد تشغيل الملف.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'SchoolDB') IS NULL
    THROW 50020, N'قاعدة SchoolDB غير موجودة. اختر قاعدة البيانات الصحيحة أولاً.', 1;
GO
USE SchoolDB;
GO

IF OBJECT_ID(N'tempdb..#ForeignKeys') IS NOT NULL DROP TABLE #ForeignKeys;
CREATE TABLE #ForeignKeys
(
    RowNo INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ConstraintName SYSNAME NOT NULL,
    ChildTable SYSNAME NOT NULL,
    ChildColumn SYSNAME NOT NULL,
    ParentTable SYSNAME NOT NULL,
    ParentColumn SYSNAME NOT NULL,
    DeleteAction NVARCHAR(30) NOT NULL
);

/* العلاقات غير متعددة الأشكال؛ لا تُضاف علاقة لـ EntityID/ReferenceID/BorrowerID. */
INSERT INTO #ForeignKeys (ConstraintName, ChildTable, ChildColumn, ParentTable, ParentColumn, DeleteAction)
VALUES
(N'FK_AuditLogs_Users', N'AuditLogs', N'UserID', N'Users', N'UserID', N'SET NULL'),
(N'FK_FeePlans_Classes', N'FeePlans', N'ClassID', N'Classes', N'ClassID', N'NO ACTION'),
(N'FK_Fees_Students', N'Fees', N'StudentID', N'Students', N'StudentID', N'NO ACTION'),
(N'FK_Fees_FeePlans', N'Fees', N'FeePlanID', N'FeePlans', N'FeePlanID', N'NO ACTION'),
(N'FK_Grades_Students', N'Grades', N'StudentID', N'Students', N'StudentID', N'NO ACTION'),
(N'FK_Grades_Subjects', N'Grades', N'SubjectID', N'Subjects', N'SubjectID', N'NO ACTION'),
(N'FK_Grades_Classes', N'Grades', N'ClassID', N'Classes', N'ClassID', N'NO ACTION'),
(N'FK_Payroll_Teachers', N'Payroll', N'TeacherID', N'Teachers', N'TeacherID', N'NO ACTION'),
(N'FK_Receipts_Students', N'Receipts', N'StudentID', N'Students', N'StudentID', N'NO ACTION'),
(N'FK_StudentFees_Students', N'StudentFees', N'StudentID', N'Students', N'StudentID', N'NO ACTION'),
(N'FK_TeacherAttendance_Teachers', N'TeacherAttendance', N'TeacherID', N'Teachers', N'TeacherID', N'NO ACTION'),
(N'FK_TeacherContracts_Teachers', N'TeacherContracts', N'TeacherID', N'Teachers', N'TeacherID', N'NO ACTION'),
(N'FK_SchoolTimetable_Classes', N'SchoolTimetable', N'ClassID', N'Classes', N'ClassID', N'NO ACTION'),
(N'FK_SchoolTimetable_Subjects', N'SchoolTimetable', N'SubjectID', N'Subjects', N'SubjectID', N'NO ACTION'),
(N'FK_SchoolTimetable_Teachers', N'SchoolTimetable', N'TeacherID', N'Teachers', N'TeacherID', N'NO ACTION');

DECLARE
    @RowNo INT = 1,
    @MaxRow INT = (SELECT MAX(RowNo) FROM #ForeignKeys),
    @ConstraintName SYSNAME,
    @ChildTable SYSNAME,
    @ChildColumn SYSNAME,
    @ParentTable SYSNAME,
    @ParentColumn SYSNAME,
    @DeleteAction NVARCHAR(30),
    @Sql NVARCHAR(MAX);

WHILE @RowNo <= ISNULL(@MaxRow, 0)
BEGIN
    SELECT
        @ConstraintName = ConstraintName,
        @ChildTable = ChildTable,
        @ChildColumn = ChildColumn,
        @ParentTable = ParentTable,
        @ParentColumn = ParentColumn,
        @DeleteAction = DeleteAction
    FROM #ForeignKeys
    WHERE RowNo = @RowNo;

    IF OBJECT_ID(N'dbo.' + @ChildTable, N'U') IS NULL
       OR OBJECT_ID(N'dbo.' + @ParentTable, N'U') IS NULL
       OR COL_LENGTH(N'dbo.' + @ChildTable, @ChildColumn) IS NULL
       OR COL_LENGTH(N'dbo.' + @ParentTable, @ParentColumn) IS NULL
    BEGIN
        PRINT N'تم التخطي لعدم اكتمال الجداول/الأعمدة: ' + @ConstraintName;
    END
    ELSE IF EXISTS
    (
        SELECT 1
        FROM sys.foreign_keys AS FK
        INNER JOIN sys.foreign_key_columns AS FKC
            ON FKC.constraint_object_id = FK.object_id
        WHERE FK.parent_object_id = OBJECT_ID(N'dbo.' + @ChildTable)
          AND FK.referenced_object_id = OBJECT_ID(N'dbo.' + @ParentTable)
          AND COL_NAME(FKC.parent_object_id, FKC.parent_column_id) = @ChildColumn
          AND COL_NAME(FKC.referenced_object_id, FKC.referenced_column_id) = @ParentColumn
    )
    BEGIN
        PRINT N'العلاقة موجودة مسبقاً لنفس الأعمدة: ' + @ChildTable + N'.' + @ChildColumn + N' -> ' + @ParentTable + N'.' + @ParentColumn;
    END
    ELSE
    BEGIN
        SET @Sql = N'
IF EXISTS
(
    SELECT 1
    FROM dbo.' + QUOTENAME(@ChildTable) + N' AS C
    LEFT JOIN dbo.' + QUOTENAME(@ParentTable) + N' AS P
        ON C.' + QUOTENAME(@ChildColumn) + N' = P.' + QUOTENAME(@ParentColumn) + N'
    WHERE C.' + QUOTENAME(@ChildColumn) + N' IS NOT NULL
      AND P.' + QUOTENAME(@ParentColumn) + N' IS NULL
)
BEGIN
    PRINT N''تم التخطي لوجود بيانات يتيمة للعلاقة: ' + REPLACE(@ConstraintName, '''', '''''') + N''';
END
ELSE
BEGIN
    ALTER TABLE dbo.' + QUOTENAME(@ChildTable) + N' WITH CHECK ADD CONSTRAINT ' + QUOTENAME(@ConstraintName) + N'
        FOREIGN KEY (' + QUOTENAME(@ChildColumn) + N') REFERENCES dbo.' + QUOTENAME(@ParentTable) + N' (' + QUOTENAME(@ParentColumn) + N')' +
        CASE
            WHEN @DeleteAction = N'CASCADE' THEN N' ON DELETE CASCADE'
            WHEN @DeleteAction = N'SET NULL' THEN N' ON DELETE SET NULL'
            ELSE N''
        END + N';';
    ALTER TABLE dbo.' + QUOTENAME(@ChildTable) + N' CHECK CONSTRAINT ' + QUOTENAME(@ConstraintName) + N';
    PRINT N''تمت إضافة العلاقة: ' + REPLACE(@ConstraintName, '''', '''''') + N''';
END;';
        EXEC sys.sp_executesql @Sql;
    END;

    SET @RowNo += 1;
END;

/* فهارس مساعدة على أعمدة العلاقات التي تُستخدم في الربط والتقارير. */
DECLARE @Indexes TABLE
(
    IndexName SYSNAME NOT NULL,
    TableName SYSNAME NOT NULL,
    ColumnName SYSNAME NOT NULL
);
INSERT INTO @Indexes (IndexName, TableName, ColumnName)
VALUES
(N'IX_FeePlans_ClassID', N'FeePlans', N'ClassID'),
(N'IX_Fees_StudentID', N'Fees', N'StudentID'),
(N'IX_Fees_FeePlanID', N'Fees', N'FeePlanID'),
(N'IX_Grades_StudentID', N'Grades', N'StudentID'),
(N'IX_Grades_SubjectID', N'Grades', N'SubjectID'),
(N'IX_Grades_ClassID', N'Grades', N'ClassID'),
(N'IX_Payroll_TeacherID', N'Payroll', N'TeacherID'),
(N'IX_Receipts_StudentID', N'Receipts', N'StudentID'),
(N'IX_StudentFees_StudentID', N'StudentFees', N'StudentID'),
(N'IX_TeacherAttendance_TeacherID', N'TeacherAttendance', N'TeacherID'),
(N'IX_TeacherContracts_TeacherID', N'TeacherContracts', N'TeacherID'),
(N'IX_SchoolTimetable_ClassID', N'SchoolTimetable', N'ClassID'),
(N'IX_SchoolTimetable_SubjectID', N'SchoolTimetable', N'SubjectID'),
(N'IX_SchoolTimetable_TeacherID', N'SchoolTimetable', N'TeacherID');

DECLARE @IndexName SYSNAME, @IndexTable SYSNAME, @IndexColumn SYSNAME;
DECLARE IndexCursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT IndexName, TableName, ColumnName FROM @Indexes;
OPEN IndexCursor;
FETCH NEXT FROM IndexCursor INTO @IndexName, @IndexTable, @IndexColumn;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF OBJECT_ID(N'dbo.' + @IndexTable, N'U') IS NOT NULL
       AND COL_LENGTH(N'dbo.' + @IndexTable, @IndexColumn) IS NOT NULL
       AND NOT EXISTS
       (
           SELECT 1 FROM sys.indexes
           WHERE object_id = OBJECT_ID(N'dbo.' + @IndexTable)
             AND name = @IndexName
       )
    BEGIN
        SET @Sql = N'CREATE INDEX ' + QUOTENAME(@IndexName) + N' ON dbo.' + QUOTENAME(@IndexTable) + N' (' + QUOTENAME(@IndexColumn) + N');';
        EXEC sys.sp_executesql @Sql;
    END;
    FETCH NEXT FROM IndexCursor INTO @IndexName, @IndexTable, @IndexColumn;
END;
CLOSE IndexCursor;
DEALLOCATE IndexCursor;

DROP TABLE #ForeignKeys;
PRINT N'اكتملت ترقية العلاقات المرجعية والفهارس بأمان.';
GO
