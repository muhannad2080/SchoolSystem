/*
    Migration 006
    يمنع وجود أكثر من سجل حضور للمعلم نفسه في التاريخ نفسه.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.TeacherAttendance', N'U') IS NULL
    THROW 51120, N'جدول TeacherAttendance غير موجود؛ لا يمكن تطبيق قيد تفرد حضور المعلمين.', 1;

IF EXISTS
(
    SELECT TeacherID, CONVERT(date, AttendanceDate) AS AttendanceDay, COUNT(*) AS DuplicateCount
    FROM dbo.TeacherAttendance
    GROUP BY TeacherID, CONVERT(date, AttendanceDate)
    HAVING COUNT(*) > 1
)
    THROW 51121, N'توجد سجلات حضور مكررة للمعلم في التاريخ نفسه. عالج التكرارات قبل تطبيق Migration 006.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.TeacherAttendance')
      AND name = N'UX_TeacherAttendance_Teacher_Date'
)
BEGIN
    CREATE UNIQUE INDEX UX_TeacherAttendance_Teacher_Date
        ON dbo.TeacherAttendance(TeacherID, AttendanceDate);
END;