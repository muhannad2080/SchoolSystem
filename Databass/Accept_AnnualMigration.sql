/*
    اختبار قبول ترحيل الطلاب المعتمد
    الاستخدام: شغّل الملف على نسخة اختبار من SchoolDB بعد تطبيق:
    Migration_AnnualClosingAndStudentMigration.sql
    Verify_AnnualClosingSchema.sql

    سياسة الاختبار:
    - يقرأ المخطط ويفحص الإجراءات قبل التشغيل.
    - ينفذ حالات الاعتماد داخل معاملات ثم يتراجع عنها دائماً.
    - لا يحذف بيانات المستخدم ولا يغير بيانات الإنتاج.
    - يحتاج إلى بيانات اختبار عام مغلق، طالب موزع، وتسجيل مقبول للعام التالي.
*/
USE SchoolDB;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Failures TABLE
(
    TestName NVARCHAR(150) NOT NULL,
    Result NVARCHAR(20) NOT NULL,
    Details NVARCHAR(1000) NULL
);

INSERT @Failures(TestName, Result, Details)
SELECT N'وجود الإجراء الرئيسي', CASE WHEN OBJECT_ID(N'dbo.ApproveStudentYearMigration', N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END,
       N'ApproveStudentYearMigration';
INSERT @Failures(TestName, Result, Details)
SELECT N'وجود تقرير الترحيل', CASE WHEN OBJECT_ID(N'dbo.GetStudentMigrationReport', N'P') IS NULL THEN N'FAIL' ELSE N'PASS' END,
       N'GetStudentMigrationReport';
INSERT @Failures(TestName, Result, Details)
SELECT N'وجود سجل الترحيل', CASE WHEN OBJECT_ID(N'dbo.AnnualMigrationLog', N'U') IS NULL THEN N'FAIL' ELSE N'PASS' END,
       N'AnnualMigrationLog';

IF OBJECT_ID(N'dbo.ApproveStudentYearMigration', N'P') IS NULL
BEGIN
    SELECT TestName, Result, Details FROM @Failures;
    THROW 51100, N'اختبار القبول متوقف: إجراء اعتماد الترحيل غير موجود. طبّق ترحيل الإغلاق أولاً.', 1;
END;

DECLARE @MigrationID INT, @ToClassID INT, @ToSection NVARCHAR(50), @FromYear NVARCHAR(20), @ToYear NVARCHAR(20);
SELECT TOP (1)
    @MigrationID = MigrationID,
    @ToClassID = ToClassID,
    @ToSection = NULLIF(LTRIM(RTRIM(ToSection)), N''),
    @FromYear = FromAcademicYear,
    @ToYear = ToAcademicYear
FROM dbo.AnnualMigrationLog
WHERE MigrationStatus = N'مخطط'
ORDER BY MigrationID;

IF @MigrationID IS NULL
BEGIN
    INSERT @Failures VALUES (N'بيانات اختبار الترحيل', N'SKIP', N'لا يوجد سجل بحالة مخطط؛ أنشئ خطة ترحيل من شاشة الإغلاق أولاً.');
END
ELSE
BEGIN
    /* الحالة السلبية 1: رفض الصف غير الموجود، مع التراجع الكامل. */
    BEGIN TRANSACTION;
    BEGIN TRY
        EXEC dbo.ApproveStudentYearMigration @MigrationID=@MigrationID, @ToClassID=-2147483647,
             @ToSection=COALESCE(@ToSection, N'اختبار'), @ApprovedByUserID=NULL;
        INSERT @Failures VALUES (N'رفض الصف غير الموجود', N'FAIL', N'تم قبول صف غير موجود.');
        ROLLBACK TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        INSERT @Failures VALUES (N'رفض الصف غير الموجود', CASE WHEN ERROR_NUMBER() = 51034 THEN N'PASS' ELSE N'FAIL' END,
             ERROR_MESSAGE());
    END CATCH;

    /* الحالة السلبية 2: رفض الشعبة الفارغة أو غير الموجودة. */
    BEGIN TRANSACTION;
    BEGIN TRY
        SELECT TOP (1) @ToClassID = ClassID FROM dbo.Classes ORDER BY ClassID;
        EXEC dbo.ApproveStudentYearMigration @MigrationID=@MigrationID, @ToClassID=@ToClassID,
             @ToSection=N'__SECTION_NOT_FOUND_ACCEPTANCE__', @ApprovedByUserID=NULL;
        INSERT @Failures VALUES (N'رفض الشعبة غير الموجودة', N'FAIL', N'تم قبول شعبة غير موجودة.');
        ROLLBACK TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        INSERT @Failures VALUES (N'رفض الشعبة غير الموجودة', CASE WHEN ERROR_NUMBER() = 51036 THEN N'PASS' ELSE N'FAIL' END,
             ERROR_MESSAGE());
    END CATCH;

    /* الحالة السلبية 3: رفض الاعتماد المتكرر لسجل سبق تنفيذه. */
    DECLARE @ExecutedMigrationID INT = NULL;
    SELECT TOP (1) @ExecutedMigrationID = MigrationID FROM dbo.AnnualMigrationLog WHERE MigrationStatus = N'منفذ';
    IF @ExecutedMigrationID IS NULL
        INSERT @Failures VALUES (N'رفض الترحيل المكرر', N'SKIP', N'لا يوجد سجل منفذ لاختبار إعادة الاعتماد.');
    ELSE
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            EXEC dbo.ApproveStudentYearMigration @MigrationID=@ExecutedMigrationID, @ToClassID=@ToClassID,
                 @ToSection=COALESCE(@ToSection, N'اختبار'), @ApprovedByUserID=NULL;
            INSERT @Failures VALUES (N'رفض الترحيل المكرر', N'FAIL', N'تمت إعادة معالجة سجل منفذ.');
            ROLLBACK TRANSACTION;
        END TRY
        BEGIN CATCH
            IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
            INSERT @Failures VALUES (N'رفض الترحيل المكرر', CASE WHEN ERROR_NUMBER() = 51032 THEN N'PASS' ELSE N'FAIL' END,
                 ERROR_MESSAGE());
        END CATCH;
    END;

    /* حالة سلامة: لا تغيير دائم بعد الاختبارات السلبية. */
    INSERT @Failures
    SELECT N'سلامة سجل الترحيل بعد الاختبارات',
           CASE WHEN EXISTS (SELECT 1 FROM dbo.AnnualMigrationLog WHERE MigrationID=@MigrationID AND MigrationStatus=N'مخطط') THEN N'PASS' ELSE N'FAIL' END,
           N'يجب أن يبقى السجل مخططاً بعد التراجع عن اختبارات الرفض.';
END;

SELECT TestName AS [الاختبار], Result AS [النتيجة], Details AS [الملاحظات]
FROM @Failures
ORDER BY CASE Result WHEN N'FAIL' THEN 1 WHEN N'SKIP' THEN 2 ELSE 3 END, TestName;

IF EXISTS (SELECT 1 FROM @Failures WHERE Result=N'FAIL')
    THROW 51101, N'فشل اختبار قبول ترحيل الطلاب. راجع نتائج الاختبارات قبل الإنتاج.', 1;

PRINT N'اكتمل اختبار قبول الترحيل: لا توجد حالات فشل.';
GO
