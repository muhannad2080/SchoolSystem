/*
    Migration 003
    يمنع وجود أكثر من سجل حضور للطالب نفسه في اليوم نفسه.
    قبل إنشاء القيد يتم التحقق من وجود تكرارات حتى لا يتم إخفاء أو حذف بيانات تاريخية تلقائياً.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NULL
    THROW 52003, N'جدول StudentAttendance غير موجود؛ لا يمكن تطبيق قيد تفرد الحضور.', 1;

IF EXISTS
(
    SELECT StudentID, CONVERT(date, AttendanceDate) AS AttendanceDay, COUNT(*) AS DuplicateCount
    FROM dbo.StudentAttendance
    GROUP BY StudentID, CONVERT(date, AttendanceDate)
    HAVING COUNT(*) > 1
)
    THROW 52004, N'توجد سجلات حضور مكررة للطالب في اليوم نفسه. عالج التكرارات قبل تطبيق Migration 003.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.StudentAttendance')
      AND name = N'UX_StudentAttendance_Student_Date'
)
BEGIN
    CREATE UNIQUE INDEX UX_StudentAttendance_Student_Date
        ON dbo.StudentAttendance(StudentID, AttendanceDate);
END;
