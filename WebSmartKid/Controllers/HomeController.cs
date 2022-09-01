using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface; 

namespace WebSmartKid.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        //private readonly IPersonsServices _personsServices;
        //public HomeController(IPersonsServices personsServices)
        //{
        //    _personsServices = personsServices;
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetCountForMain()
        //{
        //    try
        //    {
        //        ResObj res = await _personsServices.GetCountForMain();


        //        return Json(new { success = true, data = res.data });   
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = true, msg= "حدث خطأ اثناء عملية جلب البيانات" }); 
        //    }
        //}
        public IActionResult Index()
        {
            return View();
        }             
        public IActionResult Users()
        {
            return View();
        }          
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Carousel()
        {
            return View();
        }      

    }
}