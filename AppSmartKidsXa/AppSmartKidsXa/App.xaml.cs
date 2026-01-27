using AppSmartKidsXa.Helper;
using OneSignalSDK.Xamarin;
using OneSignalSDK.Xamarin.Core;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("TajawalBold.ttf", Alias = "FontTB")]
[assembly: ExportFont("TajawalRegular.ttf", Alias = "FontTR")]
namespace AppSmartKidsXa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InfoAccess.Id = 0;
            try
            {
                int id = Preferences.Get("Id", 0);
                if (id != null)
                {
                    InfoAccess.Id = id;
                }
            }
            catch (Exception ex)
            {
                InfoAccess.Id = 0;

            }
            try
            {
                OneSignal.Default.Initialize("86509cbb-2e1b-49ab-af76-246c2772ac75");
                OneSignal.Default.PromptForPushNotificationsWithUserResponse();
                if (InfoAccess.Id > 0)
                {
                    string externalUserId = InfoAccess.Id.ToString(); // You will supply the external user id to the OneSignal SDK 

                    OneSignal.Default.SetExternalUserId(externalUserId);

                }
            }
            catch (Exception ex) { }
            MainPage = new NavigationPage( new MainPage());
        }
        private async void HandleNotificationReceived(Notification notification)
        {
            try
            {
                string str1 = notification.title;
                string str2 = notification.body;

                await App.Current.MainPage.DisplayAlert(str1, str2, "نعم");
            }
            catch (Exception ex)
            { }
        }

        private async void HandleNotificationOpened(NotificationOpenedResult result)
        {
            try
            {
                string str1 = result.notification.title;
                string str2 = result.notification.body;

                await App.Current.MainPage.DisplayAlert(str1, str2, "نعم");
            }
            catch (Exception ex)
            { }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
