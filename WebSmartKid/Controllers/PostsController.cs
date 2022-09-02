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
using Microsoft.EntityFrameworkCore;

namespace WebSmartKid.Controllers
{                       
    [Authorize]                            
    public class PostsController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly IPostsService _PostsService;
        DB_Context _Context;
        #endregion

        #region Const
        public PostsController(
            ILoggerRepository logger,
            IPostsService PostsService,
            DB_Context dB_Context)
        {
            _logger = logger;
            _PostsService = PostsService;
            _Context = dB_Context;
        }
        #endregion

        #region Get Info Posts    
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResObj res = await _PostsService.GetAll();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => GetAll => name:" + UserManager.Id);
                return Response(false, "حدث خطأ اثناء عملية جلب البيانات");
            }
        }
        #endregion
                            
        #region insert or update Info Posts 
        [HttpPost]
        public async Task<IActionResult> Post(Posts Posts)
        {
            try
            { 
                ResObj res; 
                if (Posts.PostId == 0)
                { 
                    res = await _PostsService.Post(Posts);
                    await sendnot(Posts.Title);
                }
                else
                {
                    res = await _PostsService.Update(Posts);
                }
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => Post => name:" + UserManager.Id);
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion

        #region delete Info Posts 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _PostsService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => Delete => name:" + UserManager.Id);
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Posts ById Info Posts 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _PostsService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "PostsController => GetById => name:" + UserManager.Id);
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        private async Task sendnot(string title)
        {
            try
            {
                List<string> ids = new List<string>();
                List<Users> users = await _Context.Users.ToListAsync();
                foreach (var item in users)
                {
                    ids.Add(item.UserId.ToString());
                }
                await OneSignalSender("تم نشر منشور جديد", title, ids.ToArray());
            }
            catch (Exception ex)
            { await _logger.WriteAsync(ex, " PostController => sendnot"); }
        }
    }
}
