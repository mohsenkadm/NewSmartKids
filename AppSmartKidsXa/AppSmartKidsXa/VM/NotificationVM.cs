using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;       
using AppSmartKidsXa.Entity;        
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{
    public   class NotificationVM  :BaseVM
    {
        #region prop                
        public List<Notification> items;
        public List<Notification> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }
        GetDataUrlService<Notification> urlService;
        #endregion

        #region const
        public NotificationVM(INavigation navigation)
        {
            this.Navigation = navigation;
            this.urlService = new GetDataUrlService<Notification>();
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
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("خطا","لا يوجد اتصال بلانترنت","نعم"); return;
                }                                                    
                ResponseList<Notification> response = await urlService.GetListAllAsync("Notification/GetNotificationAll?UserId=" + InfoAccess.Id);

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
                await App.Current.MainPage.DisplayAlert("خطا","حدث خطا","نعم");
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
    }
}
