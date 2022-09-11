using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using AppSmartKids.View;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;             

namespace AppSmartKids.VM
{
    public partial class OrdersVM   :BaseVM
    {
        #region prop
        private readonly IGetDataUrlService<Entity.Entity.Orders> _service;
        [ObservableProperty]      
        private ObservableCollection<Entity.Entity.Orders> items;
        [ObservableProperty]
       
        public string searchtxt;

        [ObservableProperty]
        private string _SumCancel;
        [ObservableProperty]
        private string _SumWaiting;
        [ObservableProperty]
        private string _SumApproved;
        [ObservableProperty]
        private string _SumDone;    
        #endregion


        public OrdersVM()
        {
            _service = new GetDataUrlService<Entity.Entity.Orders>();     
        }

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
             GetData();
            IsRefreshing = false;
        }
        #endregion


        #region Load Item
        [ICommand]
        public async void GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }

                ResponseCollection<Entity.Entity.Orders> response = await _service.GetCollectionAllAsync("Orders/GetOrdersByOrderNo?OrderNo=" +
                    (Searchtxt == "" ? "%20" : Searchtxt) + "&Id=" + 1);// + InfoAccess.Id);
                          
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث حطأ", "نعم");
                }
                else
                {
                    ObservableCollection<Entity.Entity.Orders> dataa = response.data;
                    Items = dataa;
                    if (items.Count > 0)
                    {
                        SumCancel = items.Where(x => x.IsCancel == true).Count().ToString();
                        SumApproved = items.Where(x => x.IsApporve == true).Count().ToString();
                        SumDone = items.Where(x => x.IsDone == true).Count().ToString();
                        SumWaiting = items.Where(x => x.IsCancel == false && x.IsApporve == false && x.IsDone == false).Count().ToString();
                    }
                    else
                    {
                        SumWaiting = SumCancel = SumDone = SumApproved = "0";

                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        }
        #endregion


        #region click event open detail order
        [ICommand]
        public async void ShowItem (int OrderId) 
        {
            try
            {
                var navParam = new Dictionary<string, object>();
                navParam.Add("Id", OrderId);
                await AppShell.Current.GoToAsync(nameof(View.OrderDetail), true,navParam);     
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
