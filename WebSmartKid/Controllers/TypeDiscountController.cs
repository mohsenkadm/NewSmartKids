using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;
using Entity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebSmartKid.Controllers
{                       
    [Authorize]                            
    public class TypeDiscountController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly ITypeDiscountService _TypeDiscountService;        
        #endregion

        #region Const
        public TypeDiscountController(
            ILoggerRepository logger,
            ITypeDiscountService TypeDiscountService)
        {
            _logger = logger;
            _TypeDiscountService = TypeDiscountService;     
        }
        #endregion


        #region Get Info TypeDiscount    
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(string? NameDis)
        {
            try
            {
                ResObj res = await _TypeDiscountService.GetAll(NameDis);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TypeDiscountController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion
                            
        #region insert or update Info TypeDiscount 
        [HttpPost]
        public async Task<IActionResult> Post(TypeDiscount TypeDiscount)
        {
            try
            { 
                ResObj res; 
                if (TypeDiscount.Id == 0)
                { 
                    res = await _TypeDiscountService.Post(TypeDiscount);
                }
                else
                {
                    res = await _TypeDiscountService.Update(TypeDiscount);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TypeDiscountController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info TypeDiscount 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _TypeDiscountService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TypeDiscountController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get TypeDiscount ById Info TypeDiscount 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _TypeDiscountService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TypeDiscountController => GetById => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion   
    }
}
