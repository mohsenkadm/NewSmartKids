using Entity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;

namespace WebSmartKid.Controllers
{
    public class ChatController : MasterController
    {
        #region Readonly 
        private readonly ILoggerRepository _logger;
        private readonly IChatService _chatService;
        #endregion

        #region Const
        public ChatController(
            ILoggerRepository logger,
            IChatService chatService )
        {
            _logger = logger;
            _chatService = chatService;
        }
        #endregion

        #region GetMessageChat for app
        [HttpGet]
        public async Task<IActionResult> GetMessageChat(int UserReciverId)
        {
            try
            {
                ResObj res = await _chatService.GetMessageChat(UserReciverId);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ChatController => GetMessageChat");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion  

        #region GetMessageChat 
        [HttpGet]
        public async Task<IActionResult> GetMessageChatForWeb()
        {
            try
            {
                ResObj res = await _chatService.GetMessageChat((int)HttpContext.Session.GetInt32("Id"));

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ChatController => GetMessageChat");
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion  

        #region GetMessageList 
        [HttpGet]
        public async Task<IActionResult> GetMessageList()
        {
            try
            {
                ResObj res = await _chatService.GetMessageList();

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ChatController => GetMessageList => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        #region Register 
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Users users)
        {
            try
            {
                ResObj res = await _chatService.Register(users);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ChatController => Register => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            try
            {

                Messages messag = new Messages
                {                            
                    MessageText = message,
                    UserReciverId = (int)HttpContext.Session.GetInt32("Id"),
                    UserSenderId = 1,
                };
                ResObj res = await _chatService.PostMessage(messag);
                Notification notifications = new Notification
                {
                    Title = "رسالة",
                    Details = message, DateInsert = Key.DateTimeIQ,
                    UserId = (int)HttpContext.Session.GetInt32("Id")
                };                                         
                try
                {
                    await OneSignalSender(notifications.Title, notifications.Details,
                        new string[] { HttpContext.Session.GetInt32("Id").ToString() });
                }
                catch (Exception ex) { }

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {                
                await _logger.WriteAsync(ex, "ChatController => PostMessage => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #region PostMessage 
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] Messages messages)
        {
            try
            {
                ResObj res = await _chatService.PostMessage(messages);

                return Response(res.success, res.data);
            }
            catch (Exception ex)
            {
                await _logger.WriteAsync(ex, "ChatController => PostMessage => name:"  );
                return Response(false, "حدث خطا اثناء عملية جلب البيانات");
            }
        }
        #endregion

    }
}
