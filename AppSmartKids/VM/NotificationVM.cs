using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  

namespace AppSmartKids.VM
{
    public partial class NotificationVM  :BaseVM
    {
        #region prop
        [ObservableProperty]
        public List<Notification> items;
        IGetDataUrlService<Notification> urlService;
        #endregion

        #region const
        public NotificationVM()
        {                                 
            this.urlService = new GetDataUrlService<Notification>();   
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
                    await AppShell.Current.DisplayAlert("خطا","لا يوجد اتصال بلانترنت","نعم"); return;
                }                                                    
                ResponseList<Notification> response = await urlService.GetListAllAsync("Notification/GetNotificationAll?UserId=" + InfoAccess.Id);

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
                await AppShell.Current.DisplayAlert("خطا","حدث خطا","نعم");
            }
        }
        #endregion
    }
}
