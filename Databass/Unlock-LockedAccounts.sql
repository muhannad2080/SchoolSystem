USE [SchoolDB];
GO

/*
  استرداد الطوارئ بعد التأكد من هوية مسؤول قاعدة البيانات.
  لا يغير كلمات المرور ولا يحذف المستخدمين؛ يعيد التفعيل ويصفر عداد القفل فقط.
*/
BEGIN TRY
    BEGIN TRANSACTION;

    UPDATE Users
    SET IsActive = 1,
        FailedLoginAttempts = 0,
        LockedAt = NULL,
        UpdatedAt = GETDATE()
    WHERE IsActive = 0
      AND (FailedLoginAttempts >= 3 OR LockedAt IS NOT NULL);

    COMMIT TRANSACTION;
    PRINT N'تمت إعادة تفعيل الحسابات المقفلة وتصفير عداداتها.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
