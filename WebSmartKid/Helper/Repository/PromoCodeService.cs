using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace WebSmartKid.Helper.Repository
{
    public class PromoCodeService : IPromoCodeService, IRegisterScopped
    {
        private readonly DB_Context _context;
        private readonly IDapperRepository<PromoCode> _promoCodeRepository;
        private readonly IDapperRepository<UserPromoCode> _userPromoCodeRepository;

        public PromoCodeService(
            DB_Context context,
            IDapperRepository<PromoCode> promoCodeRepository,
            IDapperRepository<UserPromoCode> userPromoCodeRepository)
        {
            _context = context;
            _promoCodeRepository = promoCodeRepository;
            _userPromoCodeRepository = userPromoCodeRepository;
        }

        public async Task<ResObj> GetAll(string? name, bool? isActive, int index)
        {
            List<PromoCode> data = await _promoCodeRepository.GetEntityListAsync(
                "dbo.GetAllPromoCodes", 
                new { Name = name, IsActive = isActive, Index = index });
            return Result.Return(true, data);
        }

        public async Task<ResObj> GetById(int id)
        {
            PromoCode promoCode = await _context.PromoCode
                .Where(p => p.PromoCodeId == id)
                .FirstOrDefaultAsync();
                
            if (promoCode == null)
                return Result.Return(false, "البرومو كود غير موجود");
                
            return Result.Return(true, promoCode);
        }

        public async Task<ResObj> Post(PromoCode promoCode)
        {
            // التحقق من عدم وجود نفس الاسم
            var exists = await _context.PromoCode
                .AnyAsync(p => p.Name == promoCode.Name);
                
            if (exists)
                return Result.Return(false, "يوجد برومو كود بنفس الاسم");

            promoCode.CreatedDate = DateTime.Now;
            promoCode.IsActive = true;
            
            await _context.PromoCode.AddAsync(promoCode);
            await _context.SaveChangesAsync();
            
            return Result.Return(true, "تم الحفظ بنجاح", promoCode);
        }

        public async Task<ResObj> Update(PromoCode promoCode)
        {
            PromoCode existingPromoCode = await _context.PromoCode
                .Where(p => p.PromoCodeId == promoCode.PromoCodeId)
                .FirstOrDefaultAsync();
                
            if (existingPromoCode == null)
                return Result.Return(false, "البرومو كود غير موجود");

            // التحقق من عدم وجود نفس الاسم لبرومو كود آخر
            var duplicateName = await _context.PromoCode
                .AnyAsync(p => p.Name == promoCode.Name && p.PromoCodeId != promoCode.PromoCodeId);
                
            if (duplicateName)
                return Result.Return(false, "يوجد برومو كود آخر بنفس الاسم");

            existingPromoCode.Name = promoCode.Name;
            existingPromoCode.Amount = promoCode.Amount;
            existingPromoCode.IsActive = promoCode.IsActive;

            _context.Entry(existingPromoCode).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Result.Return(true, "تم التحديث بنجاح", existingPromoCode);
        }

        public async Task<ResObj> Delete(int id)
        {
            PromoCode promoCode = await _context.PromoCode
                .Where(p => p.PromoCodeId == id)
                .FirstOrDefaultAsync();
                
            if (promoCode == null)
                return Result.Return(false, "البرومو كود غير موجود");

            // حذف سجلات الاستخدام أولاً
            await _promoCodeRepository.RunScriptAsync(
                $"DELETE FROM UserPromoCode WHERE PromoCodeId = {id}");

            _context.Entry(promoCode).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            
            return Result.Return(true, "تم الحذف بنجاح");
        }

        public async Task<ResObj> ToggleActive(int id, bool isActive)
        {
            PromoCode promoCode = await _context.PromoCode
                .Where(p => p.PromoCodeId == id)
                .FirstOrDefaultAsync();
                
            if (promoCode == null)
                return Result.Return(false, "البرومو كود غير موجود");

            promoCode.IsActive = isActive;
            _context.Entry(promoCode).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            string message = isActive ? "تم تفعيل البرومو كود" : "تم إيقاف البرومو كود";
            return Result.Return(true, message);
        }

        public async Task<ResObj> UsePromoCode(int userId, string promoCodeName)
        {
            try
            {
                // Use Dapper to call stored procedure with correct result type
                var connection = _context.Database.GetDbConnection();
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "dbo.UsePromoCode",
                    new { UserId = userId, PromoCodeName = promoCodeName },
                    commandType: System.Data.CommandType.StoredProcedure);
                
                if (result == null)
                {
                    return Result.Return(false, "حدث خطأ أثناء استخدام الكود");
                }
                
                // Convert result to proper response
                bool success = result.Success == 1;
                string message = result.Message;
                
                // Build response data object
                var responseData = new 
                { 
                    success = success, 
                    message = message,
                    newBalance = result.NewBalance != null ? (decimal?)result.NewBalance : null,
                    amount = result.Amount != null ? (decimal?)result.Amount : null
                };
                
                return Result.Return(success, message, responseData);
            }
            catch (Exception ex)
            {
                return Result.Return(false, "حدث خطأ أثناء استخدام الكود");
            }
        }

        public async Task<ResObj> GetPromoCodeUsage(int? promoCodeId)
        {
            List<UserPromoCode> data = await _userPromoCodeRepository.GetEntityListAsync(
                "dbo.GetPromoCodeUsage",
                new { PromoCodeId = promoCodeId });
                
            return Result.Return(true, data);
        }

        public async Task<ResObj> CanUsePromoCode(int userId, string promoCodeName)
        {
            try
            {
                // Use Dapper to call stored procedure with correct result type
                var connection = _context.Database.GetDbConnection();
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "dbo.CanUsePromoCode",
                    new { UserId = userId, PromoCodeName = promoCodeName },
                    commandType: System.Data.CommandType.StoredProcedure);
                
                if (result == null)
                {
                    return Result.Return(false, "حدث خطأ أثناء التحقق من الكود");
                }
                
                // Convert result to proper response
                bool canUse = result.CanUse == 1;
                string message = result.Message;
                
                return Result.Return(canUse, message, new 
                { 
                    canUse = canUse, 
                    message = message 
                });
            }
            catch (Exception ex)
            {
                return Result.Return(false, "حدث خطأ أثناء التحقق من الكود");
            }
        }

        public async Task<ResObj> ValidatePromoCodeForOrder(int userId, string promoCodeName)
        {
            try
            {
                // Check if promo code exists and is active
                var promoCode = await _context.PromoCode
                    .Where(p => p.Name == promoCodeName && p.IsActive)
                    .FirstOrDefaultAsync();

                if (promoCode == null)
                {
                    return Result.Return(false, "كود الخصم غير صحيح أو غير نشط");
                }

                // Check if user already used this promo code
                var alreadyUsed = await _context.UserPromoCode
                    .AnyAsync(up => up.UserId == userId && up.PromoCodeId == promoCode.PromoCodeId);

                if (alreadyUsed)
                {
                    return Result.Return(false, "لقد استخدمت هذا الكود من قبل");
                }

                // Return promo code details
                var validationResult = new
                {
                    promoCodeId = promoCode.PromoCodeId,
                    promoCodeName = promoCode.Name,
                    amount = promoCode.Amount,
                    message = $"كود الخصم صحيح! سيتم إضافة {promoCode.Amount} د.ع إلى رصيدك"
                };

                return Result.Return(true, "كود الخصم صحيح", validationResult);
            }
            catch (Exception ex)
            {
                return Result.Return(false, "حدث خطأ أثناء التحقق من الكود");
            }
        }
    }
}
