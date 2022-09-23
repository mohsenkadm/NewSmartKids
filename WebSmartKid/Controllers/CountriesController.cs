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
    public class CountriesController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly ICountriesService _CountriesService;        
        #endregion

        #region Const
        public CountriesController(
            ILoggerRepository logger,
            ICountriesService CountriesService)
        {
            _logger = logger;
            _CountriesService = CountriesService;     
        }
        #endregion

        #region Get Info Countries    
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResObj res = await _CountriesService.GetAll();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CountriesController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion
                            
        #region insert or update Info Countries 
        [HttpPost]
        public async Task<IActionResult> Post(Countries Countries)
        {
            try
            { 
                ResObj res; 
                if (Countries.CountryId == 0)
                { 
                    res = await _CountriesService.Post(Countries);
                }
                else
                {
                    res = await _CountriesService.Update(Countries);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CountriesController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info Countries 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _CountriesService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CountriesController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Countries ById Info Countries 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _CountriesService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CountriesController => GetById => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion   
    }
}
