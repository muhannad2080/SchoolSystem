/* فحص توافق مخطط الإغلاق السنوي والترحيل مع الإجراءات المستخدمة. قراءة فقط. */
SET NOCOUNT ON;

DECLARE @Checks TABLE
(
    CheckName NVARCHAR(200) NOT NULL,
    Result NVARCHAR(10) NOT NULL,
    Details NVARCHAR(500) NULL
);

INSERT @Checks
SELECT N'جدول Students', CASE WHEN OBJECT_ID(N'dbo.Students',N'U') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'مطلوب لمطابقة الطلاب.';
INSERT @Checks
SELECT N'جدول StudentClasses', CASE WHEN OBJECT_ID(N'dbo.StudentClasses',N'U') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'مصدر توزيع الطلاب في العام السابق.';
INSERT @Checks
SELECT N'جدول AnnualClosings', CASE WHEN OBJECT_ID(N'dbo.AnnualClosings',N'U') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'سجل إغلاق الأعوام.';
INSERT @Checks
SELECT N'جدول AnnualMigrationLog', CASE WHEN OBJECT_ID(N'dbo.AnnualMigrationLog',N'U') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'سجل خطط الترحيل.';

INSERT @Checks
SELECT N'StudentClasses.StudentID', CASE WHEN COL_LENGTH(N'dbo.StudentClasses',N'StudentID') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'معرّف الطالب.';
INSERT @Checks
SELECT N'StudentClasses.ClassID', CASE WHEN COL_LENGTH(N'dbo.StudentClasses',N'ClassID') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'معرّف الصف.';
INSERT @Checks
SELECT N'StudentClasses.Section', CASE WHEN COL_LENGTH(N'dbo.StudentClasses',N'Section') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'الشعبة.';
INSERT @Checks
SELECT N'StudentClasses.AcademicYear', CASE WHEN COL_LENGTH(N'dbo.StudentClasses',N'AcademicYear') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'العام الدراسي.';

INSERT @Checks
SELECT N'PlanStudentYearMigration', CASE WHEN OBJECT_ID(N'dbo.PlanStudentYearMigration',N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'إجراء تخطيط الترحيل.';
INSERT @Checks
SELECT N'GetStudentMigrationReport', CASE WHEN OBJECT_ID(N'dbo.GetStudentMigrationReport',N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'إجراء تقرير المرشحين والمستبعدين والمنقولين.';
INSERT @Checks
SELECT N'VerifyAnnualClosing', CASE WHEN OBJECT_ID(N'dbo.VerifyAnnualClosing',N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'إجراء فحص الإغلاق.';
INSERT @Checks
SELECT N'CloseAcademicYear', CASE WHEN OBJECT_ID(N'dbo.CloseAcademicYear',N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'إجراء الإغلاق.';
INSERT @Checks
SELECT N'ApproveStudentYearMigration', CASE WHEN OBJECT_ID(N'dbo.ApproveStudentYearMigration',N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END, N'إجراء اعتماد وإنشاء توزيع العام الجديد.';

SELECT CheckName, Result, Details FROM @Checks ORDER BY CASE Result WHEN N'FAIL' THEN 1 ELSE 2 END, CheckName;
SELECT COUNT(*) AS CriticalIssueCount FROM @Checks WHERE Result=N'FAIL';
GO
