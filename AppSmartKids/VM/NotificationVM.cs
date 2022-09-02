using AppSmartKid.Helper;
using AppSmartKid.Helper.IServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppSmartKid.VM
{
    public partial class NotificationVM  :BaseVM
    {
        #region prop
        [ObservableProperty]
        public List<Notification> _Items;
        IGetDataUrlService<Notification> urlService;
        #endregion

        #region const
        public NotificationVM(IGetDataUrlService<Notification> urlService)
        {                                 
            this.urlService = urlService;

           // Task.Run(() => GetData());
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
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا","لا يوجد اتصال بلانترنت","نعم"); return;
                }                                                    
                ResponseList<Notification> response = await urlService.GetListAllAsync("Notification/GetAll?UserId=" + InfoAccess.Id);

                if (response.success == false)
                {
                    await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    _Items = response.data;
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
