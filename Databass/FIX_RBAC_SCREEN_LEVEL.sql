-- ============================================================================
-- FIX_RBAC_SCREEN_LEVEL.sql
-- إعادة بناء نظام الصلاحيات على مستوى الشاشات (Screen-Level) فقط:
--   - مصدر الحقيقة: Roles + RolePermissions (صلاحيات الدور) + UserPermissions (إضافات المستخدم)
--   - الصلاحيات الفعالة = اتحاد (صلاحيات الدور + صلاحيات المستخدم الإضافية)
--   - لا صلاحيات منفصلة للأزرار (Add/Edit/Delete...) بعد الآن.
--   - تنظيف البيانات القديمة غير المستخدمة (مفاتيح العمليات) من RolePermissions.
-- ============================================================================

-- 1) إنشاء جدول UserPermissions (الصلاحيات الإضافية للمستخدم فوق صلاحيات دوره)
IF OBJECT_ID(N'dbo.UserPermissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserPermissions
    (
        UserPermissionID INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_UserPermissions PRIMARY KEY,
        UserID INT NOT NULL,
        PermissionID INT NOT NULL,
        GrantedAt DATETIME2 NOT NULL CONSTRAINT DF_UserPermissions_GrantedAt DEFAULT GETDATE(),
        GrantedBy INT NULL,
        CONSTRAINT FK_UserPermissions_Users
            FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID),
        CONSTRAINT FK_UserPermissions_Permissions
            FOREIGN KEY (PermissionID) REFERENCES dbo.Permissions(PermissionID),
        CONSTRAINT UQ_UserPermissions_UserPermission UNIQUE (UserID, PermissionID)
    );

    CREATE INDEX IX_UserPermissions_UserID ON dbo.UserPermissions(UserID);
    CREATE INDEX IX_UserPermissions_PermissionID ON dbo.UserPermissions(PermissionID);
END

-- 2) ضمان وجود مفتاح شاشة Permissions.View (ناقص في الكتالوج)
IF NOT EXISTS (SELECT 1 FROM dbo.Permissions WHERE PermissionKey = N'Permissions.View')
BEGIN
    INSERT INTO dbo.Permissions (PermissionKey, DisplayName, ModuleName, ActionName, IsActive, CreatedAt)
    VALUES (N'Permissions.View', N'عرض الصلاحيات', N'Permissions', N'View', 1, GETDATE());
END

-- 3) إعادة بناء صلاحيات الأدوار على مستوى الشاشات فقط (مفاتيح Module.View)
IF OBJECT_ID(N'tempdb..#RoleScreens') IS NOT NULL DROP TABLE #RoleScreens;
CREATE TABLE #RoleScreens (RoleID INT NOT NULL, ScreenKey NVARCHAR(150) NOT NULL);

-- مدير النظام: كل الشاشات
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Students.View'),(N'Enrollment.View'),(N'ClassAssignment.View'),
    (N'Teachers.View'),(N'TeacherAttendance.View'),(N'StaffAttendance.View'),(N'TeacherContracts.View'),
    (N'Payroll.View'),(N'Subjects.View'),(N'Classes.View'),(N'Rooms.View'),(N'Timetable.View'),
    (N'Grades.View'),(N'Attendance.View'),(N'Fees.View'),(N'FeePlans.View'),(N'Vouchers.View'),
    (N'Expenses.View'),(N'Transport.View'),(N'Library.View'),(N'Reports.View'),
    (N'AuditLogs.View'),(N'Settings.View'),(N'Users.View'),(N'Roles.View'),(N'Permissions.View')
) AS m(KeyName)
WHERE r.RoleID = 1;

-- الإدارة / مدير المدرسة / وكيل المدرسة
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Students.View'),(N'Enrollment.View'),(N'ClassAssignment.View'),
    (N'Teachers.View'),(N'Subjects.View'),(N'Classes.View'),(N'Rooms.View'),(N'Timetable.View'),
    (N'Attendance.View'),(N'Grades.View'),(N'Reports.View'),(N'AuditLogs.View')
) AS m(KeyName)
WHERE r.RoleID IN (2, 9, 10);

-- شؤون الطلاب
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Students.View'),(N'Enrollment.View'),(N'ClassAssignment.View'),
    (N'Attendance.View'),(N'Grades.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 3;

-- المعلمون
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Students.View'),(N'Attendance.View'),(N'Grades.View'),
    (N'Timetable.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 4;

-- المالية
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Fees.View'),(N'FeePlans.View'),(N'Vouchers.View'),
    (N'Expenses.View'),(N'Payroll.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 5;

-- المكتبة / أمين المكتبة
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Library.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID IN (6, 12);

-- النقل / مسؤول النقل
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Transport.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID IN (7, 13);

-- التقارير
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 8;

-- مدقق
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Reports.View'),(N'AuditLogs.View')
) AS m(KeyName)
WHERE r.RoleID = 15;

-- شؤون الموظفين
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Teachers.View'),(N'StaffAttendance.View'),(N'Payroll.View'),
    (N'TeacherContracts.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 11;

-- موظف الاستقبال
INSERT INTO #RoleScreens (RoleID, ScreenKey)
SELECT r.RoleID, m.KeyName
FROM dbo.Roles r
CROSS JOIN (VALUES
    (N'Dashboard.View'),(N'Students.View'),(N'Enrollment.View'),(N'Reports.View')
) AS m(KeyName)
WHERE r.RoleID = 14;

-- حذف كل الصلاحيات القديمة (مفاتيح العمليات) وإعادة بنائها من الشاشات
DELETE FROM dbo.RolePermissions;

INSERT INTO dbo.RolePermissions (RoleID, PermissionID, GrantedAt, GrantedBy)
SELECT DISTINCT rs.RoleID, p.PermissionID, GETDATE(), NULL
FROM #RoleScreens rs
INNER JOIN dbo.Permissions p ON p.PermissionKey = rs.ScreenKey;

DROP TABLE #RoleScreens;

-- 4) تنظيف بيانات يتيمة/مكررة (سلامة إضافية)
DELETE rp
FROM dbo.RolePermissions rp
LEFT JOIN dbo.Roles r ON r.RoleID = rp.RoleID
LEFT JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
WHERE r.RoleID IS NULL OR p.PermissionID IS NULL OR p.IsActive = 0;

-- 5) تهيئة صلاحيات المستخدمين الإضافية:
--    للمستخدمين الحاليين تُنقل صلاحيات الشاشات المحفوظة في Users.Permissions
--    (التي ليست من صلاحيات دورهم) إلى جدول UserPermissions كصلاحيات إضافية.
INSERT INTO dbo.UserPermissions (UserID, PermissionID, GrantedAt, GrantedBy)
SELECT DISTINCT u.UserID, p.PermissionID, GETDATE(), NULL
FROM dbo.Users u
CROSS APPLY STRING_SPLIT(u.Permissions, N',') s
INNER JOIN dbo.Permissions p ON p.PermissionKey = LTRIM(RTRIM(s.value))
WHERE p.ActionName = N'View'
  AND u.Permissions IS NOT NULL AND LTRIM(RTRIM(u.Permissions)) <> N''
  AND NOT EXISTS (
        SELECT 1
        FROM dbo.UserRoles ur
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = ur.RoleID
        INNER JOIN dbo.Permissions rpp ON rpp.PermissionID = rp.PermissionID
        WHERE ur.UserID = u.UserID
          AND rpp.PermissionKey = p.PermissionKey
  );

-- 6) تحديث كاش Users.Permissions بالصلاحيات الفعالة (دور + إضافات) لتبقى الواجهة دقيقة
UPDATE u
SET Permissions = COALESCE(agg.Perms, N''),
    UpdatedAt = GETDATE()
FROM dbo.Users u
OUTER APPLY (
    SELECT STUFF((
        SELECT N',' + x.PermissionKey
        FROM (
            SELECT DISTINCT p.PermissionKey
            FROM dbo.UserRoles ur
            INNER JOIN dbo.RolePermissions rp ON rp.RoleID = ur.RoleID
            INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
            WHERE ur.UserID = u.UserID
            UNION
            SELECT DISTINCT p.PermissionKey
            FROM dbo.UserPermissions up
            INNER JOIN dbo.Permissions p ON p.PermissionID = up.PermissionID
            WHERE up.UserID = u.UserID
        ) x
        ORDER BY x.PermissionKey
        FOR XML PATH('')
    ), 1, 1, N'') AS Perms
) agg;

-- 7) ملخص التحقق
SELECT r.RoleID, r.RoleName, COUNT(rp.PermissionID) AS ScreenCount
FROM dbo.Roles r
LEFT JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
GROUP BY r.RoleID, r.RoleName
ORDER BY r.RoleID;

SELECT u.UserID, u.UserName, u.RoleName, COUNT(up.UserPermissionID) AS ExtraScreens
FROM dbo.Users u
LEFT JOIN dbo.UserPermissions up ON up.UserID = u.UserID
GROUP BY u.UserID, u.UserName, u.RoleName
ORDER BY u.UserID;