using AppSmartKidsXa.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppSmartKidsXa.VM;
using Xamarin.Essentials;

namespace AppSmartKidsXa
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageVM(this.Navigation);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
           
            await Browser.OpenAsync("https://www.facebook.com/الطفل-العبقري-لوسائل-التعليم-1331183486981211/", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("4F92C9"),
                PreferredControlColor = Color.FromHex("4F92C9"),
            });

        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            
            await Browser.OpenAsync("https://www.instagram.com/baby.iq11/", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("4F92C9"),
                PreferredControlColor = Color.FromHex("4F92C9"),
            });
        }

        private async void ImageButton_Clicked_2(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://t.me/iqbaby00", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("4F92C9"),
                PreferredControlColor = Color.FromHex("4F92C9"),
            });
        }
    }
}
