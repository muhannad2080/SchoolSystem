/*
  Read-only production verification for the student workflow.
  Execute against SchoolDB after Migration_EnsureStudentAssignmentSchema.sql.
*/
SET NOCOUNT ON;

DECLARE @Required TABLE (ObjectName SYSNAME, ColumnName SYSNAME);
INSERT INTO @Required VALUES
(N'Students', N'StudentID'), (N'Students', N'FullName'),
(N'Enrollments', N'StudentID'), (N'Enrollments', N'AcademicYear'), (N'Enrollments', N'ClassID'), (N'Enrollments', N'Status'),
(N'StudentClasses', N'StudentClassID'), (N'StudentClasses', N'StudentID'), (N'StudentClasses', N'ClassID'),
(N'StudentClasses', N'Section'), (N'StudentClasses', N'AcademicYear'), (N'StudentClasses', N'AssignedDate'),
(N'SchoolSections', N'SectionID'), (N'SchoolSections', N'ClassID'), (N'SchoolSections', N'SectionName'),
(N'SchoolSections', N'AcademicYear'), (N'SchoolSections', N'Capacity'), (N'SchoolSections', N'AllowedGender');

SELECT r.ObjectName, r.ColumnName,
       CASE WHEN OBJECT_ID(N'dbo.' + r.ObjectName, N'U') IS NULL THEN N'MISSING_TABLE'
            WHEN COL_LENGTH(N'dbo.' + r.ObjectName, r.ColumnName) IS NULL THEN N'MISSING_COLUMN'
            ELSE N'PASS' END AS Result
FROM @Required r
ORDER BY r.ObjectName, r.ColumnName;

SELECT N'Orphan StudentClasses' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.StudentClasses sc
LEFT JOIN dbo.Students s ON s.StudentID = sc.StudentID
LEFT JOIN dbo.Classes c ON c.ClassID = sc.ClassID
WHERE s.StudentID IS NULL OR c.ClassID IS NULL;

SELECT N'Accepted enrollments without assignment' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.Enrollments e
LEFT JOIN dbo.StudentClasses sc
  ON sc.StudentID = e.StudentID
 AND REPLACE(sc.AcademicYear, N'/', N'-') = REPLACE(e.AcademicYear, N'/', N'-')
WHERE LTRIM(RTRIM(ISNULL(e.Status, N''))) = N'مقبول'
  AND sc.StudentClassID IS NULL;

SELECT N'Assignment without accepted enrollment' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.StudentClasses sc
LEFT JOIN dbo.Enrollments e
  ON e.StudentID = sc.StudentID
 AND REPLACE(e.AcademicYear, N'/', N'-') = REPLACE(sc.AcademicYear, N'/', N'-')
 AND LTRIM(RTRIM(ISNULL(e.Status, N''))) = N'مقبول'
WHERE e.EnrollmentID IS NULL;

SELECT N'Duplicate student assignment in year' AS CheckName, COUNT(*) AS IssueCount
FROM
(
    SELECT StudentID, REPLACE(AcademicYear, N'/', N'-') AS AcademicYearKey
    FROM dbo.StudentClasses
    GROUP BY StudentID, REPLACE(AcademicYear, N'/', N'-')
    HAVING COUNT(*) > 1
) d;

SELECT N'Section capacity violations' AS CheckName, COUNT(*) AS IssueCount
FROM dbo.SchoolSections ss
JOIN
(
    SELECT ClassID, LTRIM(RTRIM(Section)) AS SectionName,
           REPLACE(AcademicYear, N'/', N'-') AS AcademicYearKey, COUNT(*) AS AssignedCount
    FROM dbo.StudentClasses
    GROUP BY ClassID, LTRIM(RTRIM(Section)), REPLACE(AcademicYear, N'/', N'-')
) a ON a.ClassID = ss.ClassID
   AND a.SectionName = LTRIM(RTRIM(ss.SectionName))
   AND a.AcademicYearKey = REPLACE(ss.AcademicYear, N'/', N'-')
WHERE ss.Capacity IS NOT NULL AND ss.Capacity > 0 AND a.AssignedCount > ss.Capacity;
