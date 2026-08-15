/*
    توحيد بيانات الشعب الموجودة مسبقاً.
    يحول القيم المختصرة أو ذات المسافات إلى الأسماء القياسية:
    ألف، باء، جيم، دال.
    السكربت آمن لإعادة التنفيذ ولا ينشئ سجلات جديدة.
*/
SET NOCOUNT ON;

IF DB_NAME() <> N'SchoolDB'
BEGIN
    RAISERROR(N'أوقف التنفيذ: يجب تشغيل هذا السكربت داخل قاعدة SchoolDB. قاعدة الاتصال الحالية: %s', 16, 1, DB_NAME());
    RETURN;
END;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
    BEGIN
        UPDATE dbo.StudentClasses
        SET Section = CASE LTRIM(RTRIM(Section))
                          WHEN N'أ' THEN N'ألف'
                          WHEN N'ا' THEN N'ألف'
                          WHEN N'ألف' THEN N'ألف'
                          WHEN N'ب' THEN N'باء'
                          WHEN N'باء' THEN N'باء'
                          WHEN N'ج' THEN N'جيم'
                          WHEN N'جيم' THEN N'جيم'
                          WHEN N'د' THEN N'دال'
                          WHEN N'دال' THEN N'دال'
                          ELSE NULLIF(LTRIM(RTRIM(Section)), N'')
                       END
        WHERE Section IS NOT NULL;
    END;

    IF OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
    BEGIN
        UPDATE dbo.StudentAttendance
        SET Section = CASE LTRIM(RTRIM(Section))
                          WHEN N'أ' THEN N'ألف'
                          WHEN N'ا' THEN N'ألف'
                          WHEN N'ب' THEN N'باء'
                          WHEN N'ج' THEN N'جيم'
                          WHEN N'د' THEN N'دال'
                          ELSE NULLIF(LTRIM(RTRIM(Section)), N'')
                      END
        WHERE Section IS NOT NULL;
    END;

    IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL
    BEGIN
        UPDATE dbo.Enrollments
        SET Section = CASE LTRIM(RTRIM(Section))
                          WHEN N'أ' THEN N'ألف'
                          WHEN N'ا' THEN N'ألف'
                          WHEN N'ب' THEN N'باء'
                          WHEN N'ج' THEN N'جيم'
                          WHEN N'د' THEN N'دال'
                          ELSE NULLIF(LTRIM(RTRIM(Section)), N'')
                      END
        WHERE Section IS NOT NULL;
    END;

    IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL
    BEGIN
        UPDATE dbo.SchoolTimetable
        SET Section = CASE LTRIM(RTRIM(Section))
                          WHEN N'أ' THEN N'ألف'
                          WHEN N'ا' THEN N'ألف'
                          WHEN N'ب' THEN N'باء'
                          WHEN N'ج' THEN N'جيم'
                          WHEN N'د' THEN N'دال'
                          ELSE NULLIF(LTRIM(RTRIM(Section)), N'')
                      END
        WHERE Section IS NOT NULL;
    END;

    IF COL_LENGTH(N'dbo.Students', N'Section') IS NOT NULL
    BEGIN
        UPDATE dbo.Students
        SET Section = CASE LTRIM(RTRIM(Section))
                          WHEN N'أ' THEN N'ألف'
                          WHEN N'ا' THEN N'ألف'
                          WHEN N'ب' THEN N'باء'
                          WHEN N'ج' THEN N'جيم'
                          WHEN N'د' THEN N'دال'
                          ELSE NULLIF(LTRIM(RTRIM(Section)), N'')
                      END
        WHERE Section IS NOT NULL;
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

IF OBJECT_ID(N'dbo.StudentClasses', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.StudentAttendance', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NOT NULL
BEGIN
SELECT N'StudentClasses' AS TableName, Section, COUNT(*) AS RecordCount
FROM dbo.StudentClasses
WHERE Section IS NOT NULL
GROUP BY Section
UNION ALL
SELECT N'StudentAttendance', Section, COUNT(*)
FROM dbo.StudentAttendance
WHERE Section IS NOT NULL
GROUP BY Section
UNION ALL
SELECT N'SchoolTimetable', Section, COUNT(*)
FROM dbo.SchoolTimetable
WHERE Section IS NOT NULL
GROUP BY Section;
END
ELSE
BEGIN
    PRINT N'تم التطبيع، لكن تعذر عرض الملخص لأن أحد جداول المصدر غير موجود.';
END;
GO
