/*
    SchoolSystem - Repair student assignment mirror
    Safe to run repeatedly. It does not delete students or assignments.
    It synchronizes Students from the latest StudentClasses row so attendance,
    reports and student lists use the same assignment source.
*/

USE [SchoolDB];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

;WITH RankedAssignments AS
(
    SELECT
        sc.StudentID,
        sc.ClassID,
        sc.Section,
        sc.AcademicYear,
        ROW_NUMBER() OVER
        (
            PARTITION BY sc.StudentID
            ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC
        ) AS RowNumber
    FROM dbo.StudentClasses sc
), LatestAssignments AS
(
    SELECT StudentID, ClassID, Section, AcademicYear
    FROM RankedAssignments
    WHERE RowNumber = 1
)
UPDATE s
SET
    s.ClassID = la.ClassID,
    s.Section = la.Section,
    s.AcademicYear = la.AcademicYear,
    s.UpdatedAt = GETDATE()
FROM dbo.Students s
INNER JOIN LatestAssignments la ON la.StudentID = s.StudentID;

UPDATE s
SET
    s.ClassID = NULL,
    s.Section = NULL,
    s.AcademicYear = NULL,
    s.UpdatedAt = GETDATE()
FROM dbo.Students s
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.StudentClasses sc
    WHERE sc.StudentID = s.StudentID
)
AND (s.ClassID IS NOT NULL OR s.Section IS NOT NULL OR s.AcademicYear IS NOT NULL);

COMMIT TRANSACTION;

SELECT
    (SELECT COUNT(*) FROM dbo.StudentClasses) AS AssignmentCount,
    (SELECT COUNT(*) FROM dbo.Students WHERE ClassID IS NOT NULL) AS StudentsWithClass;
GO

PRINT N'تمت مزامنة Students مع آخر توزيع موجود في StudentClasses.';
GO

/* End of migration */

/*
    Recommended verification:
    SELECT s.StudentID, s.FullName, s.ClassID, s.Section, s.AcademicYear,
           sc.ClassID AS AssignmentClassID, sc.Section AS AssignmentSection,
           sc.AcademicYear AS AssignmentAcademicYear
    FROM dbo.Students s
    OUTER APPLY
    (
        SELECT TOP (1) sc.ClassID, sc.Section, sc.AcademicYear
        FROM dbo.StudentClasses sc
        WHERE sc.StudentID = s.StudentID
        ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC
    ) sc
    WHERE ISNULL(s.ClassID, 0) <> ISNULL(sc.ClassID, 0)
       OR ISNULL(s.Section, N'') <> ISNULL(sc.Section, N'')
       OR ISNULL(s.AcademicYear, N'') <> ISNULL(sc.AcademicYear, N'');
*/

GO

