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
using Entity.Entity;

namespace WebSmartKid.Controllers
{                       
    [Authorize]                            
    public class ProductsController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly IProductsService _ProductsService;
        private string FilePath;
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion

        #region Const
        public ProductsController(
            ILoggerRepository logger,
            IProductsService ProductsService,
             IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _ProductsService = ProductsService;
            _hostEnvironment=hostEnvironment;
        }
        #endregion

        #region Get Info Products 
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetProdForApp([FromBody]Products productFilter)
        {
            try
            {
                ResObj res = await _ProductsService.GetProdForApp(productFilter);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => GetProdForApp => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion  

        #region Get Info Products       
        [HttpGet]
        public async Task<IActionResult> GetAll(string? Name, int? CategoriesId, int index)
        {
            try
            {
                ResObj res = await _ProductsService.GetAll(Name,CategoriesId,index);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => GetAll => name:"  );
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion


        #region Get Info GetImagesByProductsId 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetImagesByProductsId(int Id)
        {
            try
            {
                ResObj res = await _ProductsService.GetImagesByProductsId(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => GetImagesByPostId ");
                return Response(false, "حدث خطا");
            }
        }
        #endregion

        #region UploadFileAsync
        [HttpPost]
        [Route("Products/UploadFile/{id}")]  
        public async Task UploadFile3Async(int id)
        {
            try
            {
                Random random = new Random();
                var files = Request.Form.Files;
                foreach (IFormFile file in files)
                {
                    int r = random.Next(99999, 1000000);
                    string imgex = Path.GetExtension(file.FileName);
                    //// if (imgex == ".jpg" || imgex == ".png" || imgex== ".JPG" || imgex == ".PNG")
                    // {
                    var imgsave = _hostEnvironment.WebRootPath + $@"\Uplouds\Images\image-{r}{imgex}";
                    var stream = new FileStream(imgsave, FileMode.Create);
                    await file.CopyToAsync(stream);
                    string path = Key.CurrentUrl + $@"/Uplouds/Images/image-{r}{imgex}";
                    Images images = new Images() { ImagePath = path, ProductsId = id };
                    await _ProductsService.PostImages(images);
                    // }
                }
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, " PostsController => Upload_File");
            }

        }
        #endregion

        #region insert or update Info Products 
        [HttpPost]
        public async Task<IActionResult> Post(Products Products)
        {
            try
            { 
                ResObj res; 
                if (Products.ProductsId == 0)
                { 
                    res = await _ProductsService.Post(Products);
                }
                else
                {
                    res = await _ProductsService.Update(Products);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => Post => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info Products 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _ProductsService.DeleteImageForPost(Id);
                ResObj res = await _ProductsService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => Delete => name:"  );
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Products ById Info Products 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _ProductsService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => GetById => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region delete Image 
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int Id)
        {
            try
            {
                ResObj res = await _ProductsService.DeleteImage(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => Delete ");
                return Response(false, "حدث خطا");
            }
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostLike([FromBody] Like Like)
        {
            try
            {
                ResObj res = await _ProductsService.PostLike(Like);

                return Response(res.success);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => PostLike => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }

        }
        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> RemoveLike(int ProductsId, int UserId)
        {
            try
            {
                ResObj res = await _ProductsService.RemoveLike(ProductsId, UserId);

                return Response(res.success);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ProductsController => RemoveLike => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }

        }
    }
}
