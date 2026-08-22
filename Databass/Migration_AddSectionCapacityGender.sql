/*
    Migration: add optional section capacity and gender policy.
    Safe to run repeatedly. Existing sections remain unlimited and mixed.
*/
IF OBJECT_ID(N'dbo.SchoolSections', N'U') IS NULL
    THROW 50100, N'يجب إنشاء جدول SchoolSections أولاً.', 1;

IF COL_LENGTH(N'dbo.SchoolSections', N'Capacity') IS NULL
    ALTER TABLE dbo.SchoolSections ADD Capacity INT NULL;

IF COL_LENGTH(N'dbo.SchoolSections', N'AllowedGender') IS NULL
    ALTER TABLE dbo.SchoolSections ADD AllowedGender NVARCHAR(20) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_SchoolSections_Capacity')
BEGIN
    EXEC sys.sp_executesql N'ALTER TABLE dbo.SchoolSections ADD CONSTRAINT CK_SchoolSections_Capacity CHECK (Capacity IS NULL OR Capacity > 0);';
END;

UPDATE dbo.SchoolSections
SET AllowedGender = NULL
WHERE AllowedGender IS NOT NULL AND LTRIM(RTRIM(AllowedGender)) = N'';

/* Existing sections intentionally remain unlimited and accept both genders. */
GO
