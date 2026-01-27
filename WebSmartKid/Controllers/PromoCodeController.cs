using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using Entity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebSmartKid.Controllers
{
    [Authorize]
    public class PromoCodeController : MasterController
    {
        #region Readonly
        private readonly ILoggerRepository _logger;
        private readonly IPromoCodeService _promoCodeService;
        #endregion

        #region Constructor
        public PromoCodeController(
            ILoggerRepository logger,
            IPromoCodeService promoCodeService)
        {
            _logger = logger;
            _promoCodeService = promoCodeService;
        }
        #endregion

        #region Get All PromoCode
        [HttpGet]
        public async Task<IActionResult> GetAll(string? name, bool? isActive, int index)
        {
            try
            {
                ResObj res = await _promoCodeService.GetAll(name, isActive, index);
                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => GetAll");
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Get PromoCode By Id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                ResObj res = await _promoCodeService.GetById(id);
                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => GetById");
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Post PromoCode
        [HttpPost]
        public async Task<IActionResult> Post(PromoCode promoCode)
        {
            try
            {
                ResObj res;
                if (promoCode.PromoCodeId == 0)
                {
                    res = await _promoCodeService.Post(promoCode);
                }
                else
                {
                    res = await _promoCodeService.Update(promoCode);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => Post");
                return Response(false, "حدث خطأ اثناء عملية الحفظ");
            }
        }
        #endregion

        #region Delete PromoCode
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ResObj res = await _promoCodeService.Delete(id);
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => Delete");
                return Response(false, "حدث خطأ اثناء عملية الحذف");
            }
        }
        #endregion

        #region Toggle Active
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, bool isActive)
        {
            try
            {
                ResObj res = await _promoCodeService.ToggleActive(id, isActive);
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => ToggleActive");
                return Response(false, "حدث خطأ اثناء العملية");
            }
        }
        #endregion

        #region Use PromoCode (للتطبيق)
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UsePromoCode([FromBody] UsePromoCodeRequest request)
        {
            try
            {
                ResObj res = await _promoCodeService.UsePromoCode(request.UserId, request.PromoCodeName);
                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => UsePromoCode");
                return Response(false, "حدث خطأ اثناء استخدام البرومو كود");
            }
        }
        #endregion

        #region Get PromoCode Usage
        [HttpGet]
        public async Task<IActionResult> GetPromoCodeUsage(int? promoCodeId)
        {
            try
            {
                ResObj res = await _promoCodeService.GetPromoCodeUsage(promoCodeId);
                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => GetPromoCodeUsage");
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Can Use PromoCode (للتطبيق)
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CanUsePromoCode([FromBody] UsePromoCodeRequest request)
        {
            try
            {
                ResObj res = await _promoCodeService.CanUsePromoCode(request.UserId, request.PromoCodeName);
                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => CanUsePromoCode");
                return Response(false, "حدث خطأ اثناء العملية");
            }
        }
        #endregion

        #region Validate PromoCode For Order (للتطبيق)
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ValidatePromoCodeForOrder(int userId, string promoCodeName)
        {
            try
            {
                ResObj res = await _promoCodeService.ValidatePromoCodeForOrder(userId, promoCodeName);
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PromoCodeController => ValidatePromoCodeForOrder");
                return Response(false, "حدث خطأ اثناء التحقق من الكود");
            }
        }
        #endregion
    }

    public class UsePromoCodeRequest
    {
        public int UserId { get; set; }
        public string PromoCodeName { get; set; }
    }
}
