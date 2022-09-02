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

        #region Get Info TblAges    
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
                await _logger.WriteAsync(ex, "TblAgesController => GetAll => name:" + UserManager.Id);
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
                await _logger.WriteAsync(ex, "TblAgesController => Post => name:" + UserManager.Id);
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
                await _logger.WriteAsync(ex, "TblAgesController => Delete => name:" + UserManager.Id);
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
                await _logger.WriteAsync(ex, "TblAgesController => GetById => name:" + UserManager.Id);
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion   
    }
}
