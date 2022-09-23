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
    public class CarouselController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly ICarouselService _CarouselService;
        private string FilePath;
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion

        #region Const
        public CarouselController(
            ILoggerRepository logger,
            ICarouselService CarouselService,
             IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _CarouselService = CarouselService;
            _hostEnvironment=hostEnvironment;
        }
        #endregion

        #region Get Info Carousel 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResObj res = await _CarouselService.GetAll();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CarouselController => GetAll");
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region UploadFileAsync
        [HttpPost]
        [Route("Carousel/UploadFile/{id}")]
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
                FilePath = _hostEnvironment.WebRootPath + $@"\Uplouds\CarouselImage\image-{r}{imgex}";
                size += file[0].Length;
                if (imgex == ".jpg" || imgex == ".png" || imgex == ".JPG" || imgex == ".PNG")
                {
                    await using (FileStream fs = System.IO.File.Create(FilePath))
                    {

                        file[0].CopyTo(fs);
                        fs.Flush();

                    }
                    string path = Key.CurrentUrl + $@"/Uplouds/CarouselImage/image-{r}{imgex}";
                    Carousel carousel = await _CarouselService.GetCarouselById(id);
                    carousel.Image = path;
                    await _CarouselService.Update(carousel);
                }
            }

            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CarouselController => UploadFileAsync");
            }

        }
        #endregion

        #region insert or update Info Carousel 
        [HttpPost]
        public async Task<IActionResult> Post(Carousel Carousel)
        {
            try
            { 
                ResObj res; 
                if (Carousel.CarouseId == 0)
                { 
                    res = await _CarouselService.Post(Carousel);
                }
                else
                {
                    res = await _CarouselService.Update(Carousel);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CarouselController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info Carousel 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _CarouselService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CarouselController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Carousel ById Info Carousel 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _CarouselService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "CarouselController => GetById");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion   
    }
}
