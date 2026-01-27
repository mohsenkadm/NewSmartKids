-- إضافة عمود Url إلى جدول المنتجات إذا لم يكن موجوداً
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'Url')
BEGIN
    ALTER TABLE [dbo].[Products]
    ADD [Url] NVARCHAR(500) NULL;
    PRINT 'Url column added to Products table';
END
ELSE
BEGIN
    PRINT 'Url column already exists in Products table';
END
GO

-- حذف أعمدة الفيديو من جدول Images إذا كانت موجودة
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Images]') AND name = 'IsVideo')
BEGIN
    ALTER TABLE [dbo].[Images] DROP COLUMN [IsVideo];
    PRINT 'IsVideo column dropped from Images table';
END
ELSE
BEGIN
    PRINT 'IsVideo column not found in Images table';
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Images]') AND name = 'VideoGuid')
BEGIN
    ALTER TABLE [dbo].[Images] DROP COLUMN [VideoGuid];
    PRINT 'VideoGuid column dropped from Images table';
END
ELSE
BEGIN
    PRINT 'VideoGuid column not found in Images table';
END
GO
