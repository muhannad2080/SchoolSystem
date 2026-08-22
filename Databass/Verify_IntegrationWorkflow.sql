/*
    SchoolSystem - فحص تكاملي شامل لمسار التشغيل
    هذا الملف للقراءة فقط ولا يعدل أو يحذف أي بيانات.
    ينفذ على قاعدة SchoolDB بعد ترحيلات الحماية.
*/
SET NOCOUNT ON;

PRINT N'1) التسجيلات المكررة للطالب والعام';
SELECT StudentID,
       REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') AS AcademicYearKey,
       COUNT(*) AS DuplicateCount
FROM dbo.Enrollments
GROUP BY StudentID, REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
HAVING COUNT(*) > 1;

PRINT N'2) توزيعات بلا تسجيل مقبول في العام نفسه';
SELECT sc.StudentClassID, sc.StudentID, sc.ClassID, sc.Section, sc.AcademicYear
FROM dbo.StudentClasses sc
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Enrollments e
    WHERE e.StudentID = sc.StudentID
      AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/')
      AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted')
);

PRINT N'3) التوزيعات المكررة للطالب والعام';
SELECT StudentID,
       REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') AS AcademicYearKey,
       COUNT(*) AS DuplicateCount
FROM dbo.StudentClasses
GROUP BY StudentID, REPLACE(ISNULL(AcademicYear, N''), N'-', N'/')
HAVING COUNT(*) > 1;

PRINT N'4) رسوم تشير إلى طالب غير موجود';
SELECT f.FeeID, f.StudentID, f.AcademicYear
FROM dbo.Fees f
LEFT JOIN dbo.Students s ON s.StudentID = f.StudentID
WHERE s.StudentID IS NULL;

PRINT N'5) رسوم خطة تشير إلى خطة غير موجودة';
SELECT f.FeeID, f.FeePlanID, f.StudentID, f.AcademicYear
FROM dbo.Fees f
LEFT JOIN dbo.FeePlans fp ON fp.FeePlanID = f.FeePlanID
WHERE f.FeePlanID IS NOT NULL AND fp.FeePlanID IS NULL;

PRINT N'6) رسوم بلا توزيع سنوي مطابق';
SELECT f.FeeID, f.StudentID, f.AcademicYear, f.FeePlanID
FROM dbo.Fees f
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.StudentClasses sc
    WHERE sc.StudentID = f.StudentID
      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')
);

PRINT N'7) درجات بلا توزيع سنوي مطابق للصف والشعبة';
SELECT g.GradeID, g.StudentID, g.ClassID, g.Section, g.AcademicYear
FROM dbo.Grades g
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.StudentClasses sc
    WHERE sc.StudentID = g.StudentID
      AND sc.ClassID = g.ClassID
      AND LTRIM(RTRIM(ISNULL(sc.Section, N''))) = LTRIM(RTRIM(ISNULL(g.Section, N'')))
      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(g.AcademicYear, N''), N'-', N'/')
);

PRINT N'8) حضور الطلاب المكرر في اليوم نفسه';
SELECT StudentID, AttendanceDate, COUNT(*) AS DuplicateCount
FROM dbo.StudentAttendance
GROUP BY StudentID, AttendanceDate
HAVING COUNT(*) > 1;

PRINT N'9) حضور المعلمين المكرر في اليوم نفسه';
SELECT TeacherID, AttendanceDate, COUNT(*) AS DuplicateCount
FROM dbo.TeacherAttendance
GROUP BY TeacherID, AttendanceDate
HAVING COUNT(*) > 1;

PRINT N'10) تعارضات الجدول للمعلم أو الصف أو الغرفة النصية';
SELECT a.TimetableID AS FirstTimetableID,
       b.TimetableID AS SecondTimetableID,
       a.AcademicYear,
       a.TermName,
       a.DayName,
       a.PeriodNo
FROM dbo.SchoolTimetable a
INNER JOIN dbo.SchoolTimetable b ON b.TimetableID > a.TimetableID
    AND REPLACE(ISNULL(b.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(a.AcademicYear, N''), N'-', N'/')
    AND ISNULL(b.TermName, N'') = ISNULL(a.TermName, N'')
    AND ISNULL(b.DayName, N'') = ISNULL(a.DayName, N'')
    AND ISNULL(b.PeriodNo, 0) = ISNULL(a.PeriodNo, 0)
    AND (b.TeacherID = a.TeacherID
         OR (b.ClassID = a.ClassID AND LTRIM(RTRIM(ISNULL(b.Section, N''))) = LTRIM(RTRIM(ISNULL(a.Section, N''))))
         OR (NULLIF(LTRIM(RTRIM(ISNULL(b.RoomName, N''))), N'') IS NOT NULL
             AND LTRIM(RTRIM(b.RoomName)) = LTRIM(RTRIM(a.RoomName))));

PRINT N'انتهى الفحص. يجب أن تكون النتائج فارغة، أو موثقة كسجلات تاريخية تحتاج معالجة.';
GO

/*
    ملاحظة: هذا الفحص لا يصلح البيانات تلقائياً.
    إذا ظهرت نتائج، راجع السجل أولاً ثم عالجها من الواجهة أو بترحيل مستقل معتمد.
*/
