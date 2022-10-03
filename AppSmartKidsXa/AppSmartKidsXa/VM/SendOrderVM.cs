using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                                     
using AppSmartKidsXa.View;
using Xamarin.Forms;
using System.Windows.Input;
using AppSmartKidsXa.VM;
using AppSmartKidsXa;
using System.Collections.ObjectModel;
using OrderDetail = AppSmartKidsXa.Entity.OrderDetail;
using OrderDetailV = AppSmartKidsXa.View.OrderDetail;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Com.OneSignal;
using Orders = AppSmartKidsXa.Entity.Orders;

namespace AppSmartKids.VM
{
    public  class SendOrderVM:BaseVM
    {
        #region prop
        private ObservableCollection<OrderDetail> items;
        public ObservableCollection<OrderDetail> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
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
        private string _Total;

        public string Total
        {
            get { return _Total; }
            set { _Total = value; OnPropertyChanged(nameof(Total)); }
        }
        private string _DeliveryPrice;

        public string DeliveryPrice
        {
            get { return _DeliveryPrice; }
            set { _DeliveryPrice = value; OnPropertyChanged(nameof(DeliveryPrice)); }
        }

        public string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

                OnPropertyChanged(nameof(Name));
            }
        }
        public string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;

                OnPropertyChanged(nameof(Phone));
            }
        }
        private List<Countries> _countrys;
        public List<Countries> Countrys
        {
            get { return _countrys; }
            set
            {
                _countrys = value;
                OnPropertyChanged(nameof(Countrys));
            }
        }
        private Countries countryIdSelected;
        public Countries CountryIdSelected
        {
            get { return countryIdSelected; }
            set
            {
                countryIdSelected = value;
                OnPropertyChanged(nameof(CountryIdSelected));
            }
        }                            
        public string details;  
        public string Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged(nameof(Details)); }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(nameof(Address)); }
        }


        private readonly GetDataUrlService<Countries> _Countriesservice;
        private readonly GetDataUrlService<OrderDetail> _service;
        private readonly GetDataUrlService<TypeDiscount> _TypeDiscountservice;
        List<TypeDiscount> typeDiscounts;
        #endregion
        public SendOrderVM(INavigation navigation)
        {
            this.Navigation = navigation;
            _Countriesservice = new GetDataUrlService<Countries>();
            _service = new GetDataUrlService<OrderDetail>();
            _TypeDiscountservice = new GetDataUrlService<TypeDiscount>();
            loadcombo();
            GetData();  
        }
        #region fill data
        public async void FillData()
        {
            try
            {
                if (ListCart.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                    return;
                }
                try
                {
                    InfoAccess.Id = Preferences.Get("Id", 0);
                    Name = Preferences.Get("Name", "");
                    Phone = Preferences.Get("Phone", "");
                    Details = Preferences.Get("Details", "");
                    int CountryId = Preferences.Get("CountryId", 0);
                    if (CountryId != null)
                    {
                        CountryIdSelected = Countrys.Where(x => x.CountryId == CountryId).FirstOrDefault();
                    }
                }
                catch { }
                ResponseList<TypeDiscount> response1 = await _TypeDiscountservice.GetListAllAsync("TypeDiscount/GetAll");
                if (response1.success && response1.data.Count > 0)
                {
                    typeDiscounts = response1.data;
                    var Deliveryprice1 = typeDiscounts.FirstOrDefault(x => x.NameDis == "Deliveryprice1");
                    var Deliveryprice2 = typeDiscounts.FirstOrDefault(x => x.NameDis == "Deliveryprice2");
                    var Discount = typeDiscounts.FirstOrDefault(x => x.NameDis == "Discount");
                    if (Discount != null)
                        if (Discount.Price > 0)
                        {
                            TotalDiscount = (Convert.ToDecimal(TotalDiscount) + (Convert.ToDecimal(Total)
                                * Discount.Price / 100)).ToString();
                            NetAmount = (Convert.ToDecimal(Total) -
                                Convert.ToDecimal(TotalDiscount)).ToString();
                        }
                    if (Deliveryprice1 != null)
                        if (Convert.ToDecimal(Total) > 25000)
                        {
                            NetAmount = (Convert.ToDecimal(NetAmount) + Deliveryprice1.Price).ToString();
                            DeliveryPrice = Deliveryprice1.Price.ToString();
                        }
                    if (Deliveryprice2 != null)
                        if (Convert.ToDecimal(Total) < 25000)
                        {
                            NetAmount = (Convert.ToDecimal(NetAmount) + Deliveryprice2.Price).ToString();
                            DeliveryPrice = Deliveryprice2.Price.ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطا اثناء عملية جلب البيانات", "نعم");

            }
        }
        #endregion

        #region Load Item    
        public async Task GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }     
                Items = new ObservableCollection<OrderDetail>();
                Items = ListCart;
                SumTotal();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطا اثناء عملية جلب البيانات", "نعم");

            }
        }

        private void SumTotal()
        {
            if (Items.Count > 0)
            {
                Total = Items.Sum(x => x.Total).ToString();
                NetAmount = Items.Sum(x => x.NetAmount).ToString();
                TotalDiscount = Items.Sum(x => x.TotalDiscount).ToString();
            }
            else
            {
                Total = NetAmount = TotalDiscount = "0";
            }
        }
        #endregion

        #region load data for picker or combo 
        private async void loadcombo()
        {
            try
            {
                ResponseList<Countries> response1 = await _Countriesservice.GetListAllAsync("Countries/GetAll");
                if (response1.success)
                {
                    Countrys = response1.data;
                    FillData();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
            }
            catch (Exception ex)
            { await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم"); }
        }
        #endregion

        #region click to SendOrder
        public ICommand SendOrderBtn => new Command(async () =>
        {
            try

            {

                if (IsBusy)
                    return;
                IsBusy = true;                          
                UserDialogs.Instance.ShowLoading("انتظار"); 
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }

                if (Name == null || Phone == null || CountryIdSelected == null   || Address==null)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "رجاءا اكمال المعلومات", "نعم");
                    return;
                }    
                if (Name.Trim().Length == 0 || Phone.Trim().Length == 0 || CountryIdSelected.CountryId == 0   || Address.Trim().Length==0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "رجاءا اكمال المعلومات", "نعم");
                    return;
                }      
                if (ListCart.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                    return;
                }
                UserDialogs.Instance.ShowLoading("انتظار");
                List<OrderDetail> order = new List<OrderDetail>();
                foreach (var item in ListCart)
                {
                    order.Add(new OrderDetail()
                    {
                        UserId = InfoAccess.Id,
                        Detail= Details,
                        Address = Address,
                        CountryId=CountryIdSelected.CountryId ,
                        Phone= Phone,
                        Name = Name,
                        ProductsId = item.ProductsId,  
                        Price = item.Price  ,
                        DiscountPercentage=item.DiscountPercentage,
                        Count=item.Count
                        , Total = Convert.ToDecimal(Total)
                        , TotalDiscount = Convert.ToDecimal(TotalDiscount),
                        NetAmount = Convert.ToDecimal(NetAmount)  ,
                        IsDiscount = item.IsDiscount
                    }) ;
                }
                Response<OrderDetail> response = await _service.PostListAsync("Orders/Post", order);
                if (response == null)
                {                                         
                    await App.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ", "نعم");
                }   
                if (response.success == false)
                {                                         
                    await App.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ", "نعم");
                }      
                else
                {                                
                    Preferences.Set("Id", response.data.UserId);
                    Preferences.Set("Name", Name);
                    Preferences.Set("Phone", Phone);
                    Preferences.Set("Details", Details);
                    Preferences.Set("CountryId", CountryIdSelected.CountryId);
                    Preferences.Set("Address", Address);
                    InfoAccess.Id = response.data.UserId;
                    try {   OneSignal.Current.SetExternalUserId(response.data.UserId.ToString()); } catch (Exception ex) { }
                }
                Total=NetAmount=TotalDiscount=DeliveryPrice = "0";
                ListCart = new ObservableCollection<OrderDetail>();
                Items = new ObservableCollection<OrderDetail>();                                    
                await Navigation.PushAsync(new OrderDetailV(response.data.OrderId), true);                                     

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ", "نعم");
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        });
        #endregion   
                                
        #region click to open main app 
        public ICommand MainAppBtn => new Command(async () =>
        {
            try
            {
                await Navigation.PushAsync(new MainPage(), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion   


    }
}
