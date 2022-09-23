using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.View;                    
using Orders= AppSmartKidsXa.Entity.Orders;          
using OrderDetail = AppSmartKidsXa.View.OrderDetail;          
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using AppSmartKidsXa.VM;
using AppSmartKidsXa;
using AppSmartKidsXa.Entity;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{
    public   class OrdersVM   :BaseVM
    {
        #region prop
        private readonly GetDataUrlService< Orders> _service;
          
        private ObservableCollection<Orders> items;
        public ObservableCollection<Orders> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }
        public string search;
        public string SearchText
        {
            get { return search; }
            set
            {
                search = value;
               GetData(search);
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private string _SumCancel;

        public string SumCancel
        {
            get { return _SumCancel; }
            set { _SumCancel = value; OnPropertyChanged(nameof(SumCancel)); }
        }
        private string _SumWaiting;

        public string SumWaiting
        {
            get { return _SumWaiting; }
            set { _SumWaiting = value; OnPropertyChanged(nameof(SumWaiting)); }
        }
        private string _SumApproved;

        public string SumApproved
        {
            get { return _SumApproved; }
            set { _SumApproved = value; OnPropertyChanged(nameof(SumApproved)); }
        }
        private string _SumDone;

        public string SumDone
        {
            get { return _SumDone; }
            set { _SumDone = value; OnPropertyChanged(nameof(SumDone)); }
        }
        #endregion


        public OrdersVM(INavigation navigation)
        {
            this.Navigation = navigation;
            _service = new GetDataUrlService<Orders>();
            GetData();
        }

        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await GetData();
            IsRefreshing = false;
        });
        #endregion

        #region Load Item    
        public async Task GetData(string OrderNo = "")
        {
            try
            {
                UserDialogs.Instance.ShowLoading("انتظار");
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }
                if (InfoAccess.Id == 0) { IsVisibleContent = false; IsVisibleLoginContent = true; return; }

                ResponseCollection<Orders> response = await _service.GetCollectionAllAsync("Orders/GetOrdersByOrderNo?OrderNo=" +
                    (OrderNo == "" ? "%20" : OrderNo) + "&Id="  + InfoAccess.Id);
                          
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث حطأ", "نعم");
                }
                else
                {
                    ObservableCollection< Orders> dataa = response.data;
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
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region click event open detail order
        public ICommand ShowItem => new Command<int>(async (OrderId) =>
        {
            try
            {
                await Navigation.PushAsync(new OrderDetail(OrderId), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion   
    }
}
