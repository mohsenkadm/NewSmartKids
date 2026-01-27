-- ============================================
-- SQL Script لإنشاء جداول البرومو كود ورصيد المستخدم
-- ============================================

-- 1. إنشاء جدول PromoCode
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PromoCode')
BEGIN
    CREATE TABLE [dbo].[PromoCode](
        [PromoCodeId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Amount] DECIMAL(18,2) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
    )
    PRINT 'PromoCode table created successfully'
END
ELSE
BEGIN
    PRINT 'PromoCode table already exists'
END
GO

-- 2. إنشاء جدول UserPromoCode
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPromoCode')
BEGIN
    CREATE TABLE [dbo].[UserPromoCode](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] INT NOT NULL,
        [PromoCodeId] INT NOT NULL,
        [UsedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_UserPromoCode_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
        CONSTRAINT FK_UserPromoCode_PromoCode FOREIGN KEY (PromoCodeId) REFERENCES PromoCode(PromoCodeId)
    )
    
    -- إنشاء فهرس فريد لضمان استخدام البرومو كود مرة واحدة فقط لكل مستخدم
    CREATE UNIQUE INDEX UX_UserPromoCode_UserId_PromoCodeId 
    ON UserPromoCode(UserId, PromoCodeId)
    
    PRINT 'UserPromoCode table created successfully'
END
ELSE
BEGIN
    PRINT 'UserPromoCode table already exists'
END
GO

-- 3. إضافة عمود AccountBalance لجدول Users
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'AccountBalance')
BEGIN
    ALTER TABLE [dbo].[Users]
    ADD [AccountBalance] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'AccountBalance column added to Users table'
END
ELSE
BEGIN
    PRINT 'AccountBalance column already exists in Users table'
END
GO

-- 4. إضافة أعمدة للطلبات
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND name = 'UsedAccountBalance')
BEGIN
    ALTER TABLE [dbo].[Orders]
    ADD [UsedAccountBalance] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'UsedAccountBalance column added to Orders table'
END
ELSE
BEGIN
    PRINT 'UsedAccountBalance column already exists in Orders table'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND name = 'FinalAmount')
BEGIN
    ALTER TABLE [dbo].[Orders]
    ADD [FinalAmount] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'FinalAmount column added to Orders table'
END
ELSE
BEGIN
    PRINT 'FinalAmount column already exists in Orders table'
END
GO

-- 5. إدراج بيانات تجريبية (اختياري)
/*
INSERT INTO PromoCode (Name, Amount, IsActive)
VALUES 
    ('WELCOME50', 50.00, 1),
    ('SUMMER100', 100.00, 1),
    ('NEWYEAR200', 200.00, 0)
*/

PRINT 'All database changes completed successfully!'
GO
ى 