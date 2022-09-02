using Entity.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Helper.Repository;
using WebSmartKid.Model;

namespace WebSmartKid.Controllers
{
    public class NotificationController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly INotificationService _NotificationServices;
        DB_Context _Context;
        #endregion

        #region Const
        public NotificationController(
            ILoggerRepository logger,
            INotificationService NotificationServices,
            DB_Context dB_Context)
        {
            _logger = logger;
            _NotificationServices = NotificationServices;
            _Context=dB_Context;
        }
        #endregion

        #region insert  Notification 
        [HttpPost]
        public async Task<IActionResult> Post(Notification Notification)
        {
            try
            {
                ResObj res;                          
                    res = await _NotificationServices.Post(Notification);
                await sendnot(Notification);
                return Response(res.success, res.msg);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "NotificationController => Post  ");
                return Response(false, "حدث خطا اثناء عملية الحفظ");
            }
        }
        #endregion
        private async Task sendnot(Notification notifications)
        {
            try
            {
                List<string> ids = new List<string>();  
                List<Users> users = await _Context.Users.ToListAsync();
                foreach (var item in users)
                {
                    ids.Add(item.UserId.ToString());
                }
                await OneSignalSender(notifications.Title, notifications.Details, ids.ToArray());
            }
            catch (Exception ex)
            { await _logger.WriteAsync(ex, " PostController => sendnot"); }
        }

    }
}
