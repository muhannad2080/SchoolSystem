/*
    Migration 008
    يمنع تكرار الحصة النشطة لنفس:
    - الصف والشعبة
    - المعلم
    - القاعة
    داخل العام/الفصل/اليوم/رقم الحصة.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.SchoolTimetable', N'U') IS NULL
    THROW 51140, N'جدول SchoolTimetable غير موجود؛ لا يمكن تطبيق Migration 008.', 1;

/* Empty room names are stored as NULL so they do not consume a room slot. */
UPDATE dbo.SchoolTimetable
SET RoomName = NULL
WHERE RoomName IS NOT NULL
  AND NULLIF(LTRIM(RTRIM(RoomName)), N'') IS NULL;

IF EXISTS
(
    SELECT ClassID,
           LTRIM(RTRIM(ISNULL(Section, N''))) AS SectionNormalized,
           REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
           LTRIM(RTRIM(ISNULL(TermName, N''))) AS TermNormalized,
           LTRIM(RTRIM(ISNULL(DayName, N''))) AS DayNormalized,
           PeriodNo,
           COUNT(*) AS DuplicateCount
    FROM dbo.SchoolTimetable
    WHERE IsActive = 1
    GROUP BY ClassID,
             LTRIM(RTRIM(ISNULL(Section, N''))),
             REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/'),
             LTRIM(RTRIM(ISNULL(TermName, N''))),
             LTRIM(RTRIM(ISNULL(DayName, N''))),
             PeriodNo
    HAVING COUNT(*) > 1
)
    THROW 51141, N'توجد حصص مكررة للصف والشعبة في نفس رقم الحصة. عالج التكرارات قبل تطبيق Migration 008.', 1;

IF EXISTS
(
    SELECT TeacherID,
           REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
           LTRIM(RTRIM(ISNULL(TermName, N''))) AS TermNormalized,
           LTRIM(RTRIM(ISNULL(DayName, N''))) AS DayNormalized,
           PeriodNo,
           COUNT(*) AS DuplicateCount
    FROM dbo.SchoolTimetable
    WHERE IsActive = 1
    GROUP BY TeacherID,
             REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/'),
             LTRIM(RTRIM(ISNULL(TermName, N''))),
             LTRIM(RTRIM(ISNULL(DayName, N''))),
             PeriodNo
    HAVING COUNT(*) > 1
)
    THROW 51142, N'المعلم لديه أكثر من حصة نشطة في رقم الحصة نفسه. عالج التكرارات قبل تطبيق Migration 008.', 1;

IF EXISTS
(
    SELECT LTRIM(RTRIM(ISNULL(RoomName, N''))) AS RoomNormalized,
           REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') AS AcademicYearNormalized,
           LTRIM(RTRIM(ISNULL(TermName, N''))) AS TermNormalized,
           LTRIM(RTRIM(ISNULL(DayName, N''))) AS DayNormalized,
           PeriodNo,
           COUNT(*) AS DuplicateCount
    FROM dbo.SchoolTimetable
    WHERE IsActive = 1
      AND NULLIF(LTRIM(RTRIM(ISNULL(RoomName, N''))), N'') IS NOT NULL
    GROUP BY LTRIM(RTRIM(ISNULL(RoomName, N''))),
             REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/'),
             LTRIM(RTRIM(ISNULL(TermName, N''))),
             LTRIM(RTRIM(ISNULL(DayName, N''))),
             PeriodNo
    HAVING COUNT(*) > 1
)
    THROW 51143, N'القاعة مستخدمة لأكثر من حصة نشطة في رقم الحصة نفسه. عالج التكرارات قبل تطبيق Migration 008.', 1;

IF COL_LENGTH(N'dbo.SchoolTimetable', N'TimetableSectionNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolTimetable
    ADD TimetableSectionNormalized AS
    (
        LTRIM(RTRIM(ISNULL(Section, N'')))
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.SchoolTimetable', N'TimetableAcademicYearNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolTimetable
    ADD TimetableAcademicYearNormalized AS
    (
        REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/')
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.SchoolTimetable', N'TimetableTermNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolTimetable
    ADD TimetableTermNormalized AS
    (
        LTRIM(RTRIM(ISNULL(TermName, N'')))
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.SchoolTimetable', N'TimetableDayNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolTimetable
    ADD TimetableDayNormalized AS
    (
        LTRIM(RTRIM(ISNULL(DayName, N'')))
    ) PERSISTED;
END;

IF COL_LENGTH(N'dbo.SchoolTimetable', N'TimetableRoomNormalized') IS NULL
BEGIN
    ALTER TABLE dbo.SchoolTimetable
    ADD TimetableRoomNormalized AS
    (
        LTRIM(RTRIM(ISNULL(RoomName, N'')))
    ) PERSISTED;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.SchoolTimetable')
      AND name = N'UX_SchoolTimetable_Class_Slot'
)
BEGIN
    CREATE UNIQUE INDEX UX_SchoolTimetable_Class_Slot
        ON dbo.SchoolTimetable
        (
            ClassID,
            TimetableSectionNormalized,
            TimetableAcademicYearNormalized,
            TimetableTermNormalized,
            TimetableDayNormalized,
            PeriodNo
        )
        WHERE IsActive = 1;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.SchoolTimetable')
      AND name = N'UX_SchoolTimetable_Teacher_Slot'
)
BEGIN
    CREATE UNIQUE INDEX UX_SchoolTimetable_Teacher_Slot
        ON dbo.SchoolTimetable
        (
            TeacherID,
            TimetableAcademicYearNormalized,
            TimetableTermNormalized,
            TimetableDayNormalized,
            PeriodNo
        )
        WHERE IsActive = 1;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.SchoolTimetable')
      AND name = N'UX_SchoolTimetable_Room_Slot'
)
BEGIN
    CREATE UNIQUE INDEX UX_SchoolTimetable_Room_Slot
        ON dbo.SchoolTimetable
        (
            TimetableRoomNormalized,
            TimetableAcademicYearNormalized,
            TimetableTermNormalized,
            TimetableDayNormalized,
            PeriodNo
        )
        WHERE IsActive = 1
          AND RoomName IS NOT NULL;
END;