 
using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Maui.Hosting;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;     

namespace AppSmartKids.VM
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class OrderDetailVM :BaseVM
    {
        #region prop
        [ObservableProperty]
        int _Id = 0;
        private readonly IGetDataUrlService<OrderDetail> _service;
        [ObservableProperty]
        private string _OrderNo; 
        [ObservableProperty]
        private string _OrderDate;
        [ObservableProperty]
        private string _TotalDiscount; 
        [ObservableProperty]
        private string _NetAmount;   
        [ObservableProperty]
        private string _Total;
        [ObservableProperty]
        private bool _IsDone;
        [ObservableProperty]
        private bool _IsApporve;
        [ObservableProperty]
        private bool _IsCancel;
        [ObservableProperty]
        private ObservableCollection<OrderDetail> items;      
        #endregion


        #region cons
        public OrderDetailVM()
        {                                       
            _service = new GetDataUrlService<OrderDetail>();   
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
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت","نعم");
                    return;
                }

                ResponseCollection<OrderDetail> response = await _service.GetCollectionAllAsync("Orders/GetOrdersWithDetailAll?Id=" + Id);
                      
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ","نعم");
                }
                else
                {
                    ObservableCollection<OrderDetail> dataa = response.data;
                    if (response.data != null)
                    {
                        OrderDetail order = dataa.FirstOrDefault();
                        OrderNo = order.OrderNo.ToString();
                        OrderDate = order.OrderDate.ToString();
                        Total = order.Total.ToString();
                        TotalDiscount = order.TotalDiscount.ToString();
                        NetAmount = order.NetAmount.ToString();
                        IsDone = order.IsDone;
                        IsApporve = order.IsApporve;
                        IsCancel = order.IsCancel;     
                        Items = response.data;     
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ","نعم");

            }
        }
        #endregion
                              
    }
}
