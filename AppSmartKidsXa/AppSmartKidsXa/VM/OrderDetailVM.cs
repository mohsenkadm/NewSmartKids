 
using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;       
using AppSmartKidsXa.Entity;       
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{                                      
    public  class OrderDetailVM :BaseVM
    {
        #region prop             
        int _Id = 0;
        private readonly GetDataUrlService<OrderDetail> _service;
        private string _OrderNo;

        public string OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; OnPropertyChanged(nameof(OrderNo)); }
        }
        private string _OrderDate;

        public string OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; OnPropertyChanged(nameof(OrderDate)); }
        }
        private string _Total;

        public string Total
        {
            get { return _Total; }
            set { _Total = value; OnPropertyChanged(nameof(Total)); }
        }                      
        private string _TotalDiscount;
        public string TotalDiscount
        {
            get { return _TotalDiscount; }
            set { _TotalDiscount = value; OnPropertyChanged(nameof(TotalDiscount)); }
        }                    
        private string _NetAmount;
        public string NetAmount
        {
            get { return _NetAmount; }
            set { _NetAmount = value; OnPropertyChanged(nameof(NetAmount)); }
        }                   
        private bool _IsDone;

        public bool IsDone
        {
            get { return _IsDone; }
            set { _IsDone = value; OnPropertyChanged(nameof(IsDone)); }
        }
        private bool _IsApporve;

        public bool IsApporve
        {
            get { return _IsApporve; }
            set { _IsApporve = value; OnPropertyChanged(nameof(IsApporve)); }
        }
        private bool _IsCancel;

        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; OnPropertyChanged(nameof(IsCancel)); }
        }
        private ObservableCollection<OrderDetail> items;   
        public ObservableCollection<OrderDetail> Items
        {
            get { return items; }
            set { items = value;  OnPropertyChanged(nameof(Items)); }
        }     

        #endregion


        #region cons
        public OrderDetailVM(INavigation navigation,int Id)
        {
            this.Navigation = navigation;
            _Id = Id;
            _service = new GetDataUrlService<OrderDetail>();
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
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت","نعم");
                    return;
                }

                ResponseCollection<OrderDetail> response = await _service.GetCollectionAllAsync("Orders/GetOrdersWithDetailAll?Id=" + _Id);
                      
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
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
                              
    }
}
