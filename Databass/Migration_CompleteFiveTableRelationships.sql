/*
    SchoolSystem - Complete relationships for Enrollments, Rooms, AuditLogs,
    Expenses and Vouchers.
    Safe and repeatable migration for SQL Server.
    It never deletes data and skips a foreign key when orphan rows exist.
*/

IF DB_ID(N'SchoolDB') IS NULL
    THROW 51000, N'قاعدة البيانات SchoolDB غير موجودة. نفّذ السكربت داخل قاعدة البيانات الصحيحة.', 1;
GO

USE SchoolDB;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Ensure the five target tables exist before changing them. */
IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL
    THROW 51001, N'جدول Enrollments غير موجود. نفّذ ترحيل الجداول الأساسية أولاً.', 1;
IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
    THROW 51002, N'جدول Rooms غير موجود. نفّذ ترحيل الجداول الأساسية أولاً.', 1;
IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
    THROW 51003, N'جدول AuditLogs غير موجود. نفّذ ترحيل سجل التدقيق أولاً.', 1;
IF OBJECT_ID(N'dbo.Expenses', N'U') IS NULL
    THROW 51004, N'جدول Expenses غير موجود. نفّذ ترحيل الجداول الأساسية أولاً.', 1;
IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NULL
    THROW 51005, N'جدول Vouchers غير موجود. نفّذ ترحيل الجداول الأساسية أولاً.', 1;
GO

/*
   Rooms is a master table. CreatedByUserID gives it an auditable owner,
   while Classes.RoomID represents the normal school relationship:
   one class may use one room; a room may serve many classes over time.
*/
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Rooms', N'CreatedByUserID') IS NULL
    ALTER TABLE dbo.Rooms ADD CreatedByUserID INT NULL;
GO

IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Classes', N'RoomID') IS NULL
    ALTER TABLE dbo.Classes ADD RoomID INT NULL;
GO

/* Expenses and Vouchers are financial transactions created by a user. */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Expenses', N'CreatedByUserID') IS NULL
    ALTER TABLE dbo.Expenses ADD CreatedByUserID INT NULL;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Vouchers', N'CreatedByUserID') IS NULL
    ALTER TABLE dbo.Vouchers ADD CreatedByUserID INT NULL;
GO

/*
   Helper rule used below: an FK is considered present by its actual columns,
   not by its constraint name. This prevents duplicate relationships after
   previous migrations used another constraint name.
*/

/* Enrollments -> Students */
IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Enrollments', N'StudentID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Enrollments')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Enrollments'), N'StudentID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Students')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Students'), N'StudentID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Enrollments e
        LEFT JOIN dbo.Students s ON s.StudentID = e.StudentID
        WHERE e.StudentID IS NOT NULL AND s.StudentID IS NULL
    )
        PRINT N'REVIEW: Enrollments تحتوي StudentID غير موجود؛ تم تجاوز FK إلى Students.';
    ELSE
        ALTER TABLE dbo.Enrollments WITH CHECK
            ADD CONSTRAINT FK_Enrollments_Students_Complete
            FOREIGN KEY (StudentID) REFERENCES dbo.Students(StudentID);
END;
GO

/* Enrollments -> Classes */
IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Enrollments', N'ClassID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Enrollments')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Enrollments'), N'ClassID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Classes')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Classes'), N'ClassID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Enrollments e
        LEFT JOIN dbo.Classes c ON c.ClassID = e.ClassID
        WHERE e.ClassID IS NOT NULL AND c.ClassID IS NULL
    )
        PRINT N'REVIEW: Enrollments تحتوي ClassID غير موجود؛ تم تجاوز FK إلى Classes.';
    ELSE
        ALTER TABLE dbo.Enrollments WITH CHECK
            ADD CONSTRAINT FK_Enrollments_Classes_Complete
            FOREIGN KEY (ClassID) REFERENCES dbo.Classes(ClassID);
END;
GO

/* Classes -> Rooms */
IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL
AND OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Classes', N'RoomID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Classes')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Classes'), N'RoomID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Rooms')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Rooms'), N'RoomID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Classes c
        LEFT JOIN dbo.Rooms r ON r.RoomID = c.RoomID
        WHERE c.RoomID IS NOT NULL AND r.RoomID IS NULL
    )
        PRINT N'REVIEW: Classes تحتوي RoomID غير موجود؛ تم تجاوز FK إلى Rooms.';
    ELSE
        ALTER TABLE dbo.Classes WITH CHECK
            ADD CONSTRAINT FK_Classes_Rooms_Complete
            FOREIGN KEY (RoomID) REFERENCES dbo.Rooms(RoomID) ON DELETE SET NULL;
END;
GO

/* AuditLogs -> Users. Historical logs survive user deletion. */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.AuditLogs', N'UserID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.AuditLogs')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.AuditLogs'), N'UserID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Users')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Users'), N'UserID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.AuditLogs a
        LEFT JOIN dbo.Users u ON u.UserID = a.UserID
        WHERE a.UserID IS NOT NULL AND u.UserID IS NULL
    )
        PRINT N'REVIEW: AuditLogs تحتوي UserID غير موجود؛ تم تجاوز FK إلى Users للحفاظ على السجل.';
    ELSE
        ALTER TABLE dbo.AuditLogs WITH CHECK
            ADD CONSTRAINT FK_AuditLogs_Users_Complete
            FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID) ON DELETE SET NULL;
END;
GO

/* Rooms -> Users */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Rooms', N'CreatedByUserID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Rooms')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Rooms'), N'CreatedByUserID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Users')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Users'), N'UserID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Rooms r
        LEFT JOIN dbo.Users u ON u.UserID = r.CreatedByUserID
        WHERE r.CreatedByUserID IS NOT NULL AND u.UserID IS NULL
    )
        PRINT N'REVIEW: Rooms تحتوي CreatedByUserID غير موجود؛ تم تجاوز FK إلى Users.';
    ELSE
        ALTER TABLE dbo.Rooms WITH CHECK
            ADD CONSTRAINT FK_Rooms_CreatedByUser_Complete
            FOREIGN KEY (CreatedByUserID) REFERENCES dbo.Users(UserID) ON DELETE SET NULL;
END;
GO

/* Expenses -> Users */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Expenses', N'CreatedByUserID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Expenses')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Expenses'), N'CreatedByUserID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Users')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Users'), N'UserID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Expenses e
        LEFT JOIN dbo.Users u ON u.UserID = e.CreatedByUserID
        WHERE e.CreatedByUserID IS NOT NULL AND u.UserID IS NULL
    )
        PRINT N'REVIEW: Expenses تحتوي CreatedByUserID غير موجود؛ تم تجاوز FK إلى Users.';
    ELSE
        ALTER TABLE dbo.Expenses WITH CHECK
            ADD CONSTRAINT FK_Expenses_CreatedByUser_Complete
            FOREIGN KEY (CreatedByUserID) REFERENCES dbo.Users(UserID) ON DELETE SET NULL;
END;
GO

/* Vouchers -> Users */
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Vouchers', N'CreatedByUserID') IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_key_columns fkc
    WHERE fkc.parent_object_id = OBJECT_ID(N'dbo.Vouchers')
      AND fkc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Vouchers'), N'CreatedByUserID', 'ColumnId')
      AND fkc.referenced_object_id = OBJECT_ID(N'dbo.Users')
      AND fkc.referenced_column_id = COLUMNPROPERTY(OBJECT_ID(N'dbo.Users'), N'UserID', 'ColumnId')
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Vouchers v
        LEFT JOIN dbo.Users u ON u.UserID = v.CreatedByUserID
        WHERE v.CreatedByUserID IS NOT NULL AND u.UserID IS NULL
    )
        PRINT N'REVIEW: Vouchers تحتوي CreatedByUserID غير موجود؛ تم تجاوز FK إلى Users.';
    ELSE
        ALTER TABLE dbo.Vouchers WITH CHECK
            ADD CONSTRAINT FK_Vouchers_CreatedByUser_Complete
            FOREIGN KEY (CreatedByUserID) REFERENCES dbo.Users(UserID) ON DELETE SET NULL;
END;
GO

/* Indexes supporting the new relationships and common reports. */
IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL
AND COL_LENGTH(N'dbo.Classes', N'RoomID') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Classes_RoomID' AND object_id = OBJECT_ID(N'dbo.Classes'))
    CREATE INDEX IX_Classes_RoomID ON dbo.Classes(RoomID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Enrollments_Student_Class' AND object_id = OBJECT_ID(N'dbo.Enrollments'))
    CREATE INDEX IX_Enrollments_Student_Class ON dbo.Enrollments(StudentID, ClassID, AcademicYear);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Expenses_CreatedByUserID' AND object_id = OBJECT_ID(N'dbo.Expenses'))
    CREATE INDEX IX_Expenses_CreatedByUserID ON dbo.Expenses(CreatedByUserID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Vouchers_CreatedByUserID' AND object_id = OBJECT_ID(N'dbo.Vouchers'))
    CREATE INDEX IX_Vouchers_CreatedByUserID ON dbo.Vouchers(CreatedByUserID);
GO

PRINT N'اكتملت ترقية العلاقات للجداول Enrollments وRooms وAuditLogs وExpenses وVouchers.';
GO

/* Important design note:
   Vouchers.ReferenceID is polymorphic because ReferenceType identifies whether
   it points to an expense, fee, enrollment, or another business document.
   SQL Server cannot enforce one FK for multiple target tables. It is therefore
   intentionally validated by VoucherService and must not receive a false FK.
*/
