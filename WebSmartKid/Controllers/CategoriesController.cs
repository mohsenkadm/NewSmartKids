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
    public class CategoriesController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly ICategoriesService _CategoriesService;
        private string FilePath;
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion

        #region Const
        public CategoriesController(
            ILoggerRepository logger,
            ICategoriesService CategoriesService,
             IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _CategoriesService = CategoriesService;
            _hostEnvironment=hostEnvironment;
        }
        #endregion

        #region Get Info Categories 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResObj res = await _CategoriesService.GetAll();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CategoriesController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region UploadFileAsync
        [HttpPost]
        [Route("Categories/UploadFile/{id}")]
        public async Task UploadFileAsync(int id)
        {
            IFormFileCollection file = Request.Form.Files;
            try
            {
                FilePath = "";
                long size = 0;

                Random random = new Random();
                int r = random.Next(99999, 1000000);
                var filename = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Value.Trim('"');
                string imgex = Path.GetExtension(filename);
                FilePath = _hostEnvironment.WebRootPath + $@"\Uplouds\CategoriesImage\image-{r}{imgex}";
                size += file[0].Length;
                if (imgex == ".jpg" || imgex == ".png" || imgex == ".JPG" || imgex == ".PNG")
                {
                    await using (FileStream fs = System.IO.File.Create(FilePath))
                    {

                        file[0].CopyTo(fs);
                        fs.Flush();

                    }
                    string path = Key.CurrentUrl + $@"/Uplouds/CategoriesImage/image-{r}{imgex}";
                    Categories Categories = await _CategoriesService.GetCategoriesById(id);
                    Categories.Image = path;
                    await _CategoriesService.Update(Categories);
                }
            }

            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CategoriesController => UploadFileAsync");
            }

        }
        #endregion

        #region insert or update Info Categories 
        [HttpPost]
        public async Task<IActionResult> Post(Categories Categories)
        {
            try
            { 
                ResObj res; 
                if (Categories.CategoriesId == 0)
                { 
                    res = await _CategoriesService.Post(Categories);
                }
                else
                {
                    res = await _CategoriesService.Update(Categories);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CategoriesController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info Categories 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _CategoriesService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CategoriesController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Categories ById Info Categories 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _CategoriesService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CategoriesController => GetById => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion   
    }
}
