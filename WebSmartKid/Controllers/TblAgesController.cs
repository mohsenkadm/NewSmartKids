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
using WebSmartKid.Helper.Repository;

namespace WebSmartKid.Controllers
{                       
    [Authorize]                            
    public class TblAgesController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly ITblAgesService _TblAgesService;        
        #endregion

        #region Const
        public TblAgesController(
            ILoggerRepository logger,
            ITblAgesService TblAgesService)
        {
            _logger = logger;
            _TblAgesService = TblAgesService;     
        }
        #endregion

        #region Get Info ProductAndAge by ProductId    
        [HttpGet]
        public async Task<IActionResult> GetProductAndAge(int Id)
        {
            try
            {
                ResObj res = await _TblAgesService.GetProductAndAge(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgesController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion 

        #region Get Info TblAges 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResObj res = await _TblAgesService.GetAll();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgesController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion
                            
        #region insert or update Info TblAges 
        [HttpPost]
        public async Task<IActionResult> Post(TblAges TblAges)
        {
            try
            { 
                ResObj res; 
                if (TblAges.AgeId == 0)
                { 
                    res = await _TblAgesService.Post(TblAges);
                }
                else
                {
                    res = await _TblAgesService.Update(TblAges);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgesController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info TblAges 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _TblAgesService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgesController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get TblAges ById Info TblAges 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _TblAgesService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgesController => GetById => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region SetProductAndAge  
        [HttpPost]
        public async Task<IActionResult> SetProductAndAge(ProductAndAge productAndAge)
        {
            try
            {

                ResObj res = await _TblAgesService.SetProductAndAge(productAndAge);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "TblAgeController => SetProductAndAge ");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion 
    }

}
