using AppSmartKids;
using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using AppSmartKids.VM;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppSmartKids.VM
{
    public partial class PostsVM : BaseVM
    {
        #region prop
        [ObservableProperty]
        public List<Posts> items;
        IGetDataUrlService<Posts> urlService;
        #endregion

        #region const
        public PostsVM()
        {
            this.urlService = new GetDataUrlService<Posts>();
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
            GetData();
            IsRefreshing = false;
        }
        #endregion

        #region GetData
        [ICommand]
        public async void GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                ResponseList<Posts> response = await urlService.GetListAllAsync("Posts/GetAll");

                if (response.success == false)
                {
                    await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items = response.data;
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
        }
        #endregion   
        #region open url
        [ICommand]
        public async void OpenUrl(string url)
        {
            try
            {
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
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
                await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
        }
        #endregion
    }
}
