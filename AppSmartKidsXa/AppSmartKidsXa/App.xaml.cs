using AppSmartKidsXa.Helper;
using Com.OneSignal;                
using Com.OneSignal.Abstractions;
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
                OneSignal.Current.StartInit("86509cbb-2e1b-49ab-af76-246c2772ac75")
                    .HandleNotificationOpened(HandleNotificationOpened)
             .HandleNotificationReceived(HandleNotificationReceived)
             .Settings(new Dictionary<string, bool>() {
                { IOSSettings.kOSSettingsKeyAutoPrompt, false },
                { IOSSettings.kOSSettingsKeyInAppLaunchURL, false } })
             .InFocusDisplaying(OSInFocusDisplayOption.Notification)
             .EndInit();
                OneSignal.Current.RegisterForPushNotifications();
                if (InfoAccess.Id > 0)
                {
                    string externalUserId = InfoAccess.Id.ToString(); // You will supply the external user id to the OneSignal SDK 

                    OneSignal.Current.SetExternalUserId(externalUserId);

                }
            }
            catch (Exception ex) { }
            MainPage = new NavigationPage( new MainPage());
        }
        private async void HandleNotificationReceived(OSNotification notification)
        {
            try
            {
                string str1 = notification.payload.title;
                string str2 = notification.payload.body;

                await App.Current.MainPage.DisplayAlert(str1, str2, "نعم");
            }
            catch (Exception ex)
            { }
        }

        private async void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            try
            {
                string str1 = result.notification.payload.title;
                string str2 = result.notification.payload.body;

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
