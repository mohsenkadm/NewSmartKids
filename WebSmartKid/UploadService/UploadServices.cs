 
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebSmartKid.Classes;
using WebSmartKid.Helper;

namespace WebSmartKid
{
    public interface IStorageServices
    {
        Task<ResObj> UploadImageAsync(IFormFile keys, string WebRootPath);
    }
    public class StorageServices : IStorageServices, IRegisterSingleton
    {                                                            

        public StorageServices()
        {
        }
        #region UploadImageAsync
        public async Task<ResObj> UploadImageAsync(IFormFile keys, string WebRootPath)
        {
            try
            {
                // Get file extension
                var FileExt = Path.GetExtension(keys.FileName).ToLower();

                // Check file extension
                var allowedExtensions = new HashSet<string>
                {
                    ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".heic", ".heif", ".avif", ".jfif", ".svg"
                };
                if (!allowedExtensions.Contains(FileExt))
                {
                    return Result.Return(false, "رجاءا اختيار صيفة ملف كصورة");
                }
                var objname = $"{Guid.NewGuid()}{FileExt}";

                var imgsave = WebRootPath + $@"\Uplouds\image-" + objname;
                var stream = new FileStream(imgsave, FileMode.Create);
                await keys.CopyToAsync(stream);                          
                return Result.Return(true, "تم رفع الملف بنجاح", objname);
            }
            catch (Exception ex)
            {
                return Result.Return(false, "حدث خطا" + ex.Message);
            }
        }
        #endregion
    }
}
