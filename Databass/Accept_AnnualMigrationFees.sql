/*
    اختبار قبول: ترحيل الطالب وعزل البيانات المالية والأكاديمية بين الأعوام.
    شغّل الملف على نسخة اختبار من SchoolDB بعد تطبيق الترحيلات السابقة.
    هذا الاختبار قراءة فقط ولا ينشئ أو يعدل أو يحذف أي بيانات.
*/
USE SchoolDB;
GO
SET NOCOUNT ON;

DECLARE @Results TABLE
(
    TestName NVARCHAR(200) NOT NULL,
    Result NVARCHAR(20) NOT NULL,
    Details NVARCHAR(1000) NULL
);

INSERT @Results
SELECT N'وجود أعمدة مرآة الطالب',
       CASE WHEN COL_LENGTH(N'dbo.Students', N'ClassID') IS NOT NULL
                  AND COL_LENGTH(N'dbo.Students', N'Section') IS NOT NULL
                  AND COL_LENGTH(N'dbo.Students', N'AcademicYear') IS NOT NULL
            THEN N'PASS' ELSE N'FAIL' END,
       N'التوزيع المعتمد يزامن المرآة المستخدمة في الشاشات القديمة.';

INSERT @Results
SELECT N'وجود طلاب مرحلين مع توزيعين سنويين',
       CASE WHEN EXISTS
       (
           SELECT 1
           FROM dbo.StudentClasses oldSc
           INNER JOIN dbo.StudentClasses newSc ON newSc.StudentID=oldSc.StudentID
           WHERE REPLACE(ISNULL(oldSc.AcademicYear,N''),N'-',N'/')<>REPLACE(ISNULL(newSc.AcademicYear,N''),N'-',N'/')
       ) THEN N'PASS' ELSE N'SKIP' END,
       N'يلزم تنفيذ اعتماد ترحيل واحد على الأقل لاختبار العزل الكامل.';

INSERT @Results
SELECT N'عدم وجود رسوم بلا توزيع مطابق',
       CASE WHEN EXISTS
       (
           SELECT 1
           FROM dbo.Fees f
           WHERE NOT EXISTS
           (
               SELECT 1 FROM dbo.StudentClasses sc
               WHERE sc.StudentID=f.StudentID
                 AND REPLACE(ISNULL(sc.AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(f.AcademicYear,N''),N'-',N'/')
           )
       ) THEN N'FAIL' ELSE N'PASS' END,
       N'كل رسوم الطالب يجب أن تعود إلى توزيعه في العام نفسه.';

INSERT @Results
SELECT N'عدم انتقال الرسوم القديمة إلى العام الجديد',
       CASE WHEN EXISTS
       (
           SELECT 1
           FROM dbo.StudentClasses oldSc
           INNER JOIN dbo.StudentClasses newSc ON newSc.StudentID=oldSc.StudentID
           WHERE REPLACE(ISNULL(oldSc.AcademicYear,N''),N'-',N'/')<>REPLACE(ISNULL(newSc.AcademicYear,N''),N'-',N'/')
             AND EXISTS (SELECT 1 FROM dbo.Fees f WHERE f.StudentID=oldSc.StudentID AND REPLACE(ISNULL(f.AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(newSc.AcademicYear,N''),N'-',N'/'))
             AND NOT EXISTS
             (
                 SELECT 1 FROM dbo.FeePlans fp
                 WHERE fp.ClassID=newSc.ClassID
                   AND REPLACE(ISNULL(fp.AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(newSc.AcademicYear,N''),N'-',N'/')
                   AND fp.FeePlanID=(SELECT TOP (1) f2.FeePlanID FROM dbo.Fees f2 WHERE f2.StudentID=newSc.StudentID AND REPLACE(ISNULL(f2.AcademicYear,N''),N'-',N'/')=REPLACE(ISNULL(newSc.AcademicYear,N''),N'-',N'/') ORDER BY f2.FeeID)
             )
       ) THEN N'REVIEW' ELSE N'PASS' END,
       N'وجود رسوم للعام الجديد يجب أن يكون ناتجاً عن خطة العام الجديد، وليس نسخاً من رسوم العام السابق.';

INSERT @Results
SELECT N'عدم وجود سندات بمرجع عام خاطئ',
       CASE WHEN OBJECT_ID(N'dbo.Vouchers',N'U') IS NULL THEN N'SKIP'
            WHEN EXISTS
            (
                SELECT 1 FROM dbo.Vouchers v
                INNER JOIN dbo.Fees f ON f.FeeID=v.ReferenceID
                WHERE v.ReferenceType IN (N'Fee',N'Fees',N'رسوم')
                  AND REPLACE(ISNULL(v.AcademicYear,N''),N'-',N'/')<>REPLACE(ISNULL(f.AcademicYear,N''),N'-',N'/')
            ) THEN N'FAIL' ELSE N'PASS' END,
       N'السند يجب أن يحتفظ بعام الرسم المرتبط به.';

INSERT @Results
SELECT N'سلامة الخطط حسب الصف والعام',
       CASE WHEN EXISTS
       (
           SELECT fp.ClassID, fp.AcademicYear, fp.FeeType
           FROM dbo.FeePlans fp
           GROUP BY fp.ClassID, fp.AcademicYear, fp.FeeType
           HAVING COUNT(*)>1
       ) THEN N'FAIL' ELSE N'PASS' END,
       N'لا توجد خطة رسوم مكررة لنفس الصف والعام ونوع الرسم.';

SELECT TestName AS [الاختبار], Result AS [النتيجة], Details AS [الملاحظات]
FROM @Results
ORDER BY CASE Result WHEN N'FAIL' THEN 1 WHEN N'REVIEW' THEN 2 WHEN N'SKIP' THEN 3 ELSE 4 END, TestName;

IF EXISTS (SELECT 1 FROM @Results WHERE Result=N'FAIL')
    THROW 51200, N'فشل اختبار عزل رسوم وترحيل الطلاب. راجع النتائج قبل الإنتاج.', 1;

PRINT N'اكتمل اختبار عزل الرسوم والبيانات المرتبطة بالترحيل دون أخطاء حرجة.';
GO

/* ملاحظة: توليد الرسوم الفعلي يتم من FeeService.GenerateStudentFeesFromPlans
   بعد نجاح التوزيع والقبول، ويستخدم FeePlan للصف والعام الجديد فقط. */

GO

/* نهاية الملف */
GO

-- تدقيق بنيوي متعمد: لا توجد أوامر INSERT/UPDATE/DELETE في بيانات التشغيل.
