/*
    Migration 012
    قيود قاعدة البيانات للهوية الوظيفية/الطلاب.
    تمنع التكرار حتى عند وجود عمليتي إضافة متزامنتين أو تجاوز فحص الواجهة.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
    THROW 51210, N'جدول Teachers غير موجود؛ لا يمكن تطبيق Migration 012.', 1;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
    THROW 51211, N'جدول Students غير موجود؛ لا يمكن تطبيق Migration 012.', 1;

/* تحقق من التكرارات قبل إنشاء القيود */
IF EXISTS
(
    SELECT EmployeeNumber
    FROM dbo.Teachers
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(EmployeeNumber, N''))), N'') IS NOT NULL
    GROUP BY EmployeeNumber
    HAVING COUNT(*) > 1
)
    THROW 51212, N'توجد أرقام وظيفية مكررة للمعلمين. عالج التكرارات قبل تطبيق Migration 012.', 1;

IF EXISTS
(
    SELECT NationalID
    FROM dbo.Teachers
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(NationalID, N''))), N'') IS NOT NULL
    GROUP BY NationalID
    HAVING COUNT(*) > 1
)
    THROW 51213, N'توجد أرقام هوية مكررة للمعلمين. عالج التكرارات قبل تطبيق Migration 012.', 1;

IF EXISTS
(
    SELECT NationalId
    FROM dbo.Students
    WHERE NULLIF(LTRIM(RTRIM(ISNULL(NationalId, N''))), N'') IS NOT NULL
    GROUP BY NationalId
    HAVING COUNT(*) > 1
)
    THROW 51214, N'توجد أرقام هوية مكررة للطلاب. عالج التكرارات قبل تطبيق Migration 012.', 1;

/* أرقام الموظفين */
IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Teachers')
      AND name = N'UX_Teachers_EmployeeNumber'
)
BEGIN
    CREATE UNIQUE INDEX UX_Teachers_EmployeeNumber
        ON dbo.Teachers(EmployeeNumber)
        WHERE EmployeeNumber IS NOT NULL AND EmployeeNumber <> N'';
END;

/* الهوية الوطنية للمعلم */
IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Teachers')
      AND name = N'UX_Teachers_NationalID'
)
BEGIN
    CREATE UNIQUE INDEX UX_Teachers_NationalID
        ON dbo.Teachers(NationalID)
        WHERE NationalID IS NOT NULL AND NationalID <> N'';
END;

/* الهوية الوطنية للطالب */
IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Students')
      AND name = N'UX_Students_NationalId'
)
BEGIN
    CREATE UNIQUE INDEX UX_Students_NationalId
        ON dbo.Students(NationalId)
        WHERE NationalId IS NOT NULL AND NationalId <> N'';
END;
