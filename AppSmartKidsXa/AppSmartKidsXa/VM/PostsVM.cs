using AppSmartKidsXa;
using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.VM;                     
using AppSmartKidsXa.Entity;           
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{
    public partial class PostsVM : BaseVM
    {
        #region prop

        public List<Posts> items;
        public List<Posts>  Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }
        GetDataUrlService<Posts> urlService;
        #endregion

        #region const
        public PostsVM(INavigation navigation)
        {
            this.Navigation = navigation;
            this.urlService = new GetDataUrlService<Posts>();
            GetData();
        }
        #endregion


        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await GetData();
            IsRefreshing = false;
        });
        #endregion

        #region GetData    
        public async Task GetData()
        {
            try
            {

                UserDialogs.Instance.ShowLoading("انتظار");
                IsBusy = true;
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                ResponseList<Posts> response = await urlService.GetListAllAsync("Posts/GetAll");

                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items = response.data;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }
        #endregion

        #region open url     
        public ICommand OpenUrlCommand => new Command<string>(async (url) =>
        {
            try
            {
                IsBusy = true;
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                await Browser.OpenAsync(url, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("4F92C9"),
                    PreferredControlColor = Color.FromHex("4F92C9"),
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
            finally
            {
                IsBusy = false;
            }
        });
        #endregion
    }
}
