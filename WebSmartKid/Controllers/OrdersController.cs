using Entity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;

namespace WebSmartKid.Controllers
{
    [Authorize]                                    
    public class OrdersController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly IOrdersService _OrdersService;
        private readonly INotificationService _noteService;
        #endregion

        #region Const
        public OrdersController(
            ILoggerRepository logger,
            IOrdersService OrdersService  ,
            INotificationService noteService)
        {
            _logger = logger;
            _OrdersService = OrdersService;
            _noteService = noteService;
        }
        #endregion



        #region Get Info order with detail
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetOrdersWithDetailAll(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.GetOrdersWithDetailAll(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => GetOrdersWithDetailAll => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion



        #region Get Info order 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetOrdersByOrderNo(string? OrderNo, int? Id)
        {
            try
            {
                ResObj res = await _OrdersService.GetOrdersByOrderNo(OrderNo, Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => GetOrdersByOrderNo");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Get Info order 
        [HttpGet]
        public async Task<IActionResult> GetAll(string? OrderNo, string? UserName, DateTime datefrom, DateTime dateto)
        {
            try
            {
                ResObj res = await _OrdersService.GetAll(OrderNo, UserName, datefrom, dateto);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => GetAll => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Set Order IsApporve
        [HttpPost]
        public async Task<IActionResult> SetIsApporve(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.SetIsApporve(Id);
                Orders orders = (Orders)res.data;
                List<string> ids = new List<string>();

                ids.Add(orders.UserId.ToString());
                Notification notifications = new Notification
                {
                    Title = "طلبك",
                    Details = "تمت الموافقة على طلبك يرجى الانتظار لتجهيز الطلب",
                    DateInsert = Key.DateTimeIQ,
                    UserId = orders.UserId
                };
                 await _noteService.Post(notifications);
                try
                {
                      await OneSignalSender(notifications.Title, notifications.Details,
                        ids.ToArray());
                }
                catch (Exception ex) { }  
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => SetIsApporve");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion 

        #region Set Order IsCancel
        [HttpDelete]
        public async Task<IActionResult> SetIsCancel(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.SetIsCancel(Id);
                Orders orders = (Orders)res.data;
                List<string> ids = new List<string>();

                ids.Add(orders.UserId.ToString()); 
                Notification notifications = new Notification
                {
                    Title = "طلبك",
                    Details = "تمت الغاء الموافقة على طلبك",
                    DateInsert = Key.DateTimeIQ,
                    UserId = orders.UserId
                };
                await _noteService.Post(notifications);
                try
                {
                    await OneSignalSender(notifications.Title, notifications.Details,
                      ids.ToArray());
                }
                catch (Exception ex) { }
                
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => SetIsCancel");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

       

        #region Set Order IsDone
        [HttpPost]
        public async Task<IActionResult> SetIsDone(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.SetIsDone(Id);
                Orders orders = (Orders)res.data;
                List<string> ids = new List<string>();

                ids.Add(orders.UserId.ToString());
                Notification notifications = new Notification
                {
                    Title = "طلبك",
                    Details = "تم تجهيز طلبك يرجى انتظار توصيل الطلب الى بيتك",
                    DateInsert = Key.DateTimeIQ,
                    UserId = orders.UserId
                };
                await _noteService.Post(notifications);
                try
                {
                    await OneSignalSender(notifications.Title, notifications.Details,
                      ids.ToArray());
                }
                catch (Exception ex) { }    
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => SetIsDone");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region delete Info Orders 
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.Delete(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => Delete");
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region delete Info Orders Details
        [HttpDelete]
        public async Task<IActionResult> DeleteDetails(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.DeleteDetails(Id);

                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => DeleteDetails");
                return Response(false, "حدث خطا اثناء عملية الحذف");
            }
        }
        #endregion

        #region Get Orders ById Info Orders 
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                ResObj res = await _OrdersService.GetById(Id);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => GetById");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion



        #region insert  Info orders
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<OrderDetail> orderDetail)
        {
            try
            {    
                ResObj res;
                res = await _OrdersService.Post(orderDetail);      
                return Response(res.success, res.msg, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "OrdersController => Post");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion
    }
}
