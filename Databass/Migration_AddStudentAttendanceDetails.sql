/* تشغيل هذا الملف على قاعدة SchoolDB فقط */
IF DB_NAME() <> N'SchoolDB'
    THROW 51000, N'يجب تشغيل ترحيل حضور الطلاب على قاعدة SchoolDB فقط.', 1;

IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NULL
    THROW 51001, N'جدول StudentAttendance غير موجود. شغّل Migration_OperationalTables.sql أولاً.', 1;

IF COL_LENGTH(N'dbo.StudentAttendance', N'DepartureTime') IS NULL
    ALTER TABLE dbo.StudentAttendance ADD DepartureTime TIME NULL;

IF COL_LENGTH(N'dbo.StudentAttendance', N'AbsenceReason') IS NULL
    ALTER TABLE dbo.StudentAttendance ADD AbsenceReason NVARCHAR(500) NULL;

/* توحيد القيم القديمة حتى لا تظهر قيم فارغة في القوائم */
UPDATE dbo.StudentAttendance
SET ExcuseStatus = N'بدون عذر'
WHERE ExcuseStatus IS NULL OR LTRIM(RTRIM(ExcuseStatus)) = N'';

PRINT N'تم تحديث مخطط حضور الطلاب بنجاح.';
