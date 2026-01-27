-- ============================================
-- Stored Procedures للبرومو كود
-- ============================================

-- 1. SP للحصول على جميع البرومو كودات
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAllPromoCodes')
    DROP PROCEDURE GetAllPromoCodes
GO

CREATE PROCEDURE GetAllPromoCodes
    @Name NVARCHAR(100) = NULL,
    @IsActive BIT = NULL,
    @Index INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @PageSize INT = 50
    DECLARE @Skip INT = (@Index - 1) * @PageSize
    
    SELECT 
        PromoCodeId,
        Name,
        Amount,
        IsActive,
        CreatedDate,
        (SELECT COUNT(*) FROM UserPromoCode WHERE PromoCodeId = pc.PromoCodeId) AS UsedCount
    FROM PromoCode pc
    WHERE 
        (@Name IS NULL OR Name LIKE '%' + @Name + '%')
        AND (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY PromoCodeId DESC
    OFFSET @Skip ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
GO

-- 2. SP للحصول على برومو كود بالـ ID
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPromoCodeById')
    DROP PROCEDURE GetPromoCodeById
GO

CREATE PROCEDURE GetPromoCodeById
    @PromoCodeId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        PromoCodeId,
        Name,
        Amount,
        IsActive,
        CreatedDate
    FROM PromoCode
    WHERE PromoCodeId = @PromoCodeId
END
GO

-- 3. SP لاستخدام البرومو كود
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UsePromoCode')
    DROP PROCEDURE UsePromoCode
GO

CREATE PROCEDURE UsePromoCode
    @UserId INT,
    @PromoCodeName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- التحقق من وجود البرومو كود وأنه نشط
        DECLARE @PromoCodeId INT
        DECLARE @Amount DECIMAL(18,2)
        
        SELECT @PromoCodeId = PromoCodeId, @Amount = Amount
        FROM PromoCode
        WHERE Name = @PromoCodeName AND IsActive = 1
        
        IF @PromoCodeId IS NULL
        BEGIN
            SELECT 0 AS Success, 'كود الخصم غير موجود أو غير نشط' AS Message
            ROLLBACK TRANSACTION
            RETURN
        END
        
        -- التحقق من عدم استخدام البرومو كود مسبقاً
        IF EXISTS (SELECT 1 FROM UserPromoCode WHERE UserId = @UserId AND PromoCodeId = @PromoCodeId)
        BEGIN
            SELECT 0 AS Success, 'لقد استخدمت هذا الكود مسبقاً' AS Message
            ROLLBACK TRANSACTION
            RETURN
        END
        
        -- إضافة المبلغ إلى حساب المستخدم
        UPDATE Users
        SET AccountBalance = AccountBalance + @Amount
        WHERE UserId = @UserId
        
        -- تسجيل استخدام البرومو كود
        INSERT INTO UserPromoCode (UserId, PromoCodeId)
        VALUES (@UserId, @PromoCodeId)
        
        -- الحصول على الرصيد الجديد
        DECLARE @NewBalance DECIMAL(18,2)
        SELECT @NewBalance = AccountBalance FROM Users WHERE UserId = @UserId
        
        COMMIT TRANSACTION
        
        SELECT 1 AS Success, 'تم إضافة المبلغ إلى حسابك بنجاح' AS Message, @NewBalance AS NewBalance, @Amount AS Amount
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT 0 AS Success, ERROR_MESSAGE() AS Message
    END CATCH
END
GO

-- 4. SP للحصول على استخدامات البرومو كود
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPromoCodeUsage')
    DROP PROCEDURE GetPromoCodeUsage
GO

CREATE PROCEDURE GetPromoCodeUsage
    @PromoCodeId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        upc.Id,
        upc.UserId,
        upc.PromoCodeId,
        upc.UsedDate,
        u.Name AS UserName,
        u.Phone,
        pc.Name AS PromoCodeName,
        pc.Amount
    FROM UserPromoCode upc
    INNER JOIN Users u ON upc.UserId = u.UserId
    INNER JOIN PromoCode pc ON upc.PromoCodeId = pc.PromoCodeId
    WHERE (@PromoCodeId IS NULL OR upc.PromoCodeId = @PromoCodeId)
    ORDER BY upc.UsedDate DESC
END
GO

-- 5. SP للتحقق من إمكانية استخدام البرومو كود
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CanUsePromoCode')
    DROP PROCEDURE CanUsePromoCode
GO

CREATE PROCEDURE CanUsePromoCode
    @UserId INT,
    @PromoCodeName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @PromoCodeId INT
    DECLARE @IsActive BIT
    
    SELECT @PromoCodeId = PromoCodeId, @IsActive = IsActive
    FROM PromoCode
    WHERE Name = @PromoCodeName
    
    IF @PromoCodeId IS NULL
    BEGIN
        SELECT 0 AS CanUse, 'كود الخصم غير موجود' AS Message
        RETURN
    END
    
    IF @IsActive = 0
    BEGIN
        SELECT 0 AS CanUse, 'كود الخصم غير نشط' AS Message
        RETURN
    END
    
    IF EXISTS (SELECT 1 FROM UserPromoCode WHERE UserId = @UserId AND PromoCodeId = @PromoCodeId)
    BEGIN
        SELECT 0 AS CanUse, 'لقد استخدمت هذا الكود مسبقاً' AS Message
        RETURN
    END
    
    SELECT 1 AS CanUse, 'يمكنك استخدام هذا الكود' AS Message
END
GO

PRINT 'All Stored Procedures created successfully!'
