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

        private readonly IAdminServices _AdminServices;
        public HomeController(IAdminServices AdminServices)
        {
            _AdminServices = AdminServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetCountForMain()
        {
            try
            {
                ResObj res = await _AdminServices.GetCountForMain();


                return Json(new { success = true, data = res.data });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, msg = "حدث خطأ اثناء عملية جلب البيانات" });
            }
        }
        public IActionResult Index()
        {
            return View();
        }             
        public IActionResult Admin()
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
        public IActionResult Notification()
        {
            return View();
        }      
        public IActionResult Posts()
        {
            return View();
        }        
        public IActionResult Countries()
        {
            return View();
        }          
        public IActionResult TblAges()
        {
            return View();
        }           
        public IActionResult Categories()
        {
            return View();
        }               
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }   
        public IActionResult TypeDiscount()
        {
            return View();
        }

    }
}