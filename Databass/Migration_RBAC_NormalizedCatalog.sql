/*
    SchoolSystem - Normalized RBAC catalog
    المرحلة 3: إنشاء كتالوج الأدوار والصلاحيات والعلاقات بصورة idempotent.
    لا تحذف هذه الهجرة Users.Permissions؛ فهي طبقة توافق مؤقتة إلى أن تكتمل
    قراءة العلاقات المعيارية في الخدمات.
*/

IF DB_ID(N'SchoolDB') IS NULL
    THROW 51000, N'قاعدة البيانات SchoolDB غير موجودة. نفّذ الهجرات الأساسية أولاً.', 1;
GO

USE SchoolDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
    THROW 51001, N'جدول Users غير موجود. نفّذ Migration_Step1.sql أولاً.', 1;
GO

IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permissions
    (
        PermissionID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Permissions PRIMARY KEY,
        PermissionKey NVARCHAR(150) NOT NULL,
        DisplayName NVARCHAR(250) NULL,
        ModuleName NVARCHAR(100) NULL,
        ActionName NVARCHAR(100) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Permissions_IsActive DEFAULT 1,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Permissions_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT UX_Permissions_PermissionKey UNIQUE (PermissionKey)
    );
END;
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        RoleID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
        RoleName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        IsSystemRole BIT NOT NULL CONSTRAINT DF_Roles_IsSystemRole DEFAULT 0,
        IsActive BIT NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT 1,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Roles_CreatedAt DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        CONSTRAINT UX_Roles_RoleName UNIQUE (RoleName)
    );
END;
GO

IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserRoles
    (
        UserID INT NOT NULL,
        RoleID INT NOT NULL,
        AssignedAt DATETIME2(0) NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT SYSUTCDATETIME(),
        AssignedBy INT NULL,
        CONSTRAINT PK_UserRoles PRIMARY KEY (UserID, RoleID),
        CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID),
        CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleID) REFERENCES dbo.Roles(RoleID)
    );
END;
GO

IF OBJECT_ID(N'dbo.RolePermissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolePermissions
    (
        RoleID INT NOT NULL,
        PermissionID INT NOT NULL,
        GrantedAt DATETIME2(0) NOT NULL CONSTRAINT DF_RolePermissions_GrantedAt DEFAULT SYSUTCDATETIME(),
        GrantedBy INT NULL,
        CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleID, PermissionID),
        CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleID) REFERENCES dbo.Roles(RoleID),
        CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionID) REFERENCES dbo.Permissions(PermissionID)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UserRoles_RoleID' AND object_id = OBJECT_ID(N'dbo.UserRoles'))
    CREATE INDEX IX_UserRoles_RoleID ON dbo.UserRoles(RoleID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RolePermissions_PermissionID' AND object_id = OBJECT_ID(N'dbo.RolePermissions'))
    CREATE INDEX IX_RolePermissions_PermissionID ON dbo.RolePermissions(PermissionID);
GO

;WITH Modules(ModuleName) AS
(
    SELECT ModuleName FROM (VALUES
        (N'Dashboard'), (N'Students'), (N'Enrollment'), (N'ClassAssignment'),
        (N'Teachers'), (N'TeacherAttendance'), (N'StaffAttendance'), (N'TeacherContracts'),
        (N'Payroll'), (N'Subjects'), (N'Classes'), (N'Rooms'), (N'Timetable'),
        (N'Grades'), (N'Attendance'), (N'Fees'), (N'FeePlans'), (N'Vouchers'),
        (N'Expenses'), (N'Transport'), (N'Library'), (N'Reports'), (N'AuditLogs'), (N'Settings')
    ) V(ModuleName)
), Actions(ActionName) AS
(
    SELECT ActionName FROM (VALUES
        (N'View'), (N'Add'), (N'Edit'), (N'Delete'), (N'Search'), (N'Print'),
        (N'ExportExcel'), (N'ExportPDF'), (N'Approve'), (N'Cancel')
    ) V(ActionName)
)
INSERT INTO dbo.Permissions(PermissionKey, DisplayName, ModuleName, ActionName)
SELECT M.ModuleName + N'.' + A.ActionName,
       M.ModuleName + N' - ' + A.ActionName,
       M.ModuleName,
       A.ActionName
FROM Modules M CROSS JOIN Actions A
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Permissions P
    WHERE P.PermissionKey = M.ModuleName + N'.' + A.ActionName
);
GO

INSERT INTO dbo.Permissions(PermissionKey, DisplayName, ModuleName, ActionName)
SELECT V.PermissionKey, V.DisplayName, V.ModuleName, V.ActionName
FROM (VALUES
    (N'Students.Manage', N'إدارة الطلاب القديمة', N'Students', N'Manage'),
    (N'Enrollment.Manage', N'إدارة القبول والتسجيل القديمة', N'Enrollment', N'Manage'),
    (N'ClassAssignment.Manage', N'إدارة توزيع الطلاب القديمة', N'ClassAssignment', N'Manage'),
    (N'Teachers.Manage', N'إدارة المعلمين القديمة', N'Teachers', N'Manage'),
    (N'Payroll.Manage', N'إدارة الرواتب القديمة', N'Payroll', N'Manage'),
    (N'Subjects.Manage', N'إدارة المواد القديمة', N'Subjects', N'Manage'),
    (N'Classes.Manage', N'إدارة الفصول القديمة', N'Classes', N'Manage'),
    (N'Timetable.Manage', N'إدارة الجداول القديمة', N'Timetable', N'Manage'),
    (N'Attendance.Manage', N'إدارة الحضور القديمة', N'Attendance', N'Manage'),
    (N'Grades.Manage', N'إدارة الدرجات القديمة', N'Grades', N'Manage'),
    (N'Fees.Manage', N'إدارة الرسوم القديمة', N'Fees', N'Manage'),
    (N'Vouchers.Manage', N'إدارة السندات القديمة', N'Vouchers', N'Manage'),
    (N'Expenses.Manage', N'إدارة المصروفات القديمة', N'Expenses', N'Manage'),
    (N'Library.Manage', N'إدارة المكتبة القديمة', N'Library', N'Manage'),
    (N'Transport.Manage', N'إدارة النقل القديمة', N'Transport', N'Manage'),
    (N'Reports.View', N'عرض التقارير', N'Reports', N'View'),
    (N'Users.Manage', N'إدارة المستخدمين القديمة', N'Users', N'Manage'),
    (N'AuditLogs.View', N'عرض سجل التدقيق', N'AuditLogs', N'View'),
    (N'Settings.Manage', N'إدارة الإعدادات القديمة', N'Settings', N'Manage')
) V(PermissionKey, DisplayName, ModuleName, ActionName)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Permissions P WHERE P.PermissionKey = V.PermissionKey
);
GO

INSERT INTO dbo.Roles(RoleName, Description, IsSystemRole)
SELECT V.RoleName, V.Description, V.IsSystemRole
FROM (VALUES
    (N'مدير النظام', N'صلاحيات كاملة مع حماية الحساب الأخير', CAST(1 AS BIT)),
    (N'الإدارة', N'إدارة أكاديمية واسعة وتقارير', CAST(0 AS BIT)),
    (N'شؤون الطلاب', N'الطلاب والقبول والتوزيع', CAST(0 AS BIT)),
    (N'المعلمون', N'الحضور والدرجات والطلاب المرتبطون', CAST(0 AS BIT)),
    (N'المالية', N'الرسوم والسندات والمصروفات والرواتب', CAST(0 AS BIT)),
    (N'المكتبة', N'إدارة الكتب والإعارات', CAST(0 AS BIT)),
    (N'النقل', N'إدارة الحافلات والطرق', CAST(0 AS BIT)),
    (N'التقارير', N'عرض وطباعة وتصدير التقارير فقط', CAST(0 AS BIT)),
    (N'مدير المدرسة', N'دور إداري مخصص', CAST(0 AS BIT)),
    (N'وكيل المدرسة', N'دور أكاديمي مخصص', CAST(0 AS BIT)),
    (N'شؤون الموظفين', N'الموظفون والعقود والحضور', CAST(0 AS BIT)),
    (N'أمين المكتبة', N'إدارة المكتبة', CAST(0 AS BIT)),
    (N'مسؤول النقل', N'إدارة النقل', CAST(0 AS BIT)),
    (N'موظف الاستقبال', N'بحث وعرض محدود', CAST(0 AS BIT)),
    (N'مدقق', N'عرض التقارير وسجل الأنشطة دون تعديل', CAST(0 AS BIT))
) V(RoleName, Description, IsSystemRole)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles R WHERE R.RoleName = V.RoleName);
GO

/* ربط الأدوار القديمة بالمستخدمين بدون حذف RoleName أو Permissions. */
INSERT INTO dbo.UserRoles(UserID, RoleID)
SELECT U.UserID, R.RoleID
FROM dbo.Users U
JOIN dbo.Roles R ON R.RoleName = LTRIM(RTRIM(U.RoleName))
WHERE U.UserID IS NOT NULL
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.UserRoles UR
      WHERE UR.UserID = U.UserID AND UR.RoleID = R.RoleID
  );
GO

/* منح جميع المفاتيح المعيارية لمدير النظام فقط. */
INSERT INTO dbo.RolePermissions(RoleID, PermissionID)
SELECT R.RoleID, P.PermissionID
FROM dbo.Roles R CROSS JOIN dbo.Permissions P
WHERE R.RoleName = N'مدير النظام'
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.RolePermissions RP
      WHERE RP.RoleID = R.RoleID AND RP.PermissionID = P.PermissionID
  );
GO

PRINT N'تم إنشاء كتالوج RBAC المعياري وربط المستخدمين بالأدوار دون حذف بيانات التوافق.';
GO
