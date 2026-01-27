-- SQL Script to update Images table
-- Add IsVideo and VideoGuid columns

-- Check if IsVideo column exists, if not add it
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Images]') AND name = 'IsVideo')
BEGIN
    ALTER TABLE [dbo].[Images]
    ADD [IsVideo] BIT NOT NULL DEFAULT 0
END

-- Check if VideoGuid column exists, if not add it
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Images]') AND name = 'VideoGuid')
BEGIN
    ALTER TABLE [dbo].[Images]
    ADD [VideoGuid] NVARCHAR(255) NULL
END

PRINT 'Images table updated successfully with IsVideo and VideoGuid columns'
