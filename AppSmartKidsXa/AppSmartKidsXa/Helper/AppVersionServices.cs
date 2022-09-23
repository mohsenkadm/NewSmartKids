using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace AppSmartKidsXa
{
    public class AppVersionServices
    {
        public static string GetAndroidStoreAppVersion()
        {
            string androidStoreAppVersion = null;

            try
            {
                //using (var client = new HttpClient())
                //{
                //    var doc = client.GetAsync("https://play.google.com/store/apps/details?id=" + AppInfo.PackageName + "&hl=en_CA").Result.Parse();
                //    var versionElement = doc.Select("div:containsOwn(Current Version)");

                //    androidStoreAppVersion = versionElement.Text;

                //    Element headElement = versionElement[0];
                //    Elements siblingsOfHead = headElement.SiblingElements;
                //    Element contentElement = siblingsOfHead.First;
                //    Elements childrenOfContentElement = contentElement.Children;
                //    Element childOfContentElement = childrenOfContentElement.First;
                //    Elements childrenOfChildren = childOfContentElement.Children;
                //    Element childOfChild = childrenOfChildren.First;

                //    androidStoreAppVersion = childOfChild.Text;
                //}
            }
            catch (Exception ex)
            {
                // do something
                Console.WriteLine(ex.Message);
            }

            return androidStoreAppVersion;
        }

        public static string GetIosStoreAppVersion()
        {
            string iOsStoreAppVersion = null;

            string bundleId = AppInfo.PackageName;

            string url = "http://itunes.apple.com/lookup?bundleId=" + bundleId;


            try
            {
                //using (var webClient = new System.Net.WebClient())
                //{
                //    string jsonString = webClient.DownloadString(string.Format(url));

                //    var lookup = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);


                //    if (lookup != null
                //        && lookup.Count >= 1
                //        && lookup["resultCount"] != null
                //        && Convert.ToInt32(lookup["resultCount"].ToString()) > 0)
                //    {

                //        var results = JsonConvert.DeserializeObject<List<object>>(lookup[@"results"].ToString());


                //        if (results != null && results.Count > 0)
                //        {
                //            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(results[0].ToString());
                //            iOsStoreAppVersion = values.ContainsKey("version") ? values["version"].ToString() : string.Empty;

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // do something
                Console.WriteLine(ex.Message);
            }

            return iOsStoreAppVersion;
        }
         
    }
}
