using WebSmartKid.Classes;
using WebSmartKid.Model.General;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebSmartKid.Controllers
{  
      
    public class MasterController : ControllerBase
    {

        protected UserManager UserManager
        {
            get
            {                                                     
                // reading claim "UserProfile" from JWT Token
                string user = HttpContext.User.Claims
                    .Where(x => x.Type == ClaimInfo.UserManager)
                    .FirstOrDefault().Value;

                // if claim is exists then deserialize it 
                if (!string.IsNullOrWhiteSpace(user))
                {
                    return JsonConvert.DeserializeObject<UserManager>(user);
                }

                // if no claim is found return null means the user is not logged in
                return null;
            }
        }  
        [NonAction]
        public new IActionResult Response(bool success)
        {
            return Ok(new { success });
        }

        [NonAction]
        public new IActionResult Response(bool success, object data)
        {
            return Ok(new { success, data });
        }
         
        [NonAction]
        public new IActionResult Response(bool success, string msgCode)
        { 
                return Ok(new { success, msg = msgCode });
        }

        [NonAction]
        public new IActionResult Response(bool success, string msgCode, object data)
        { 
                return Ok(new { success, msg = msgCode, data });
        }

        [NonAction]
        public async Task OneSignalSender(string Title, string body, string[] id)
        {
            string onesignalAppID = "86509cbb-2e1b-49ab-af76-246c2772ac75";
            string onesignalRestID = "MDMyNDU5NjAtMWVkMC00ZGM5LTg1MDMtNTEwMjlkNTdjYjVh";


            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + onesignalRestID);

            var obj = new
            {
                app_id = onesignalAppID,
                contents = new { en = body },
                headings = new { en = Title },
                channel_for_external_user_ids = "push",
                include_external_user_ids = id
            };



            var param = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            { 
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }
        }
    }
}
