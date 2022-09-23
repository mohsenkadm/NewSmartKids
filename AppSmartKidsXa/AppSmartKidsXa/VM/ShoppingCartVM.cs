using AppSmartKidsXa.Helper;
using AppSmartKidsXa.View;                      
using AppSmartKidsXa.Entity;         
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;                  
using OrderDetail = AppSmartKidsXa.Entity.OrderDetail;
using Xamarin.Forms;

namespace AppSmartKidsXa.VM
{
    public   class ShoppingCartVM: BaseVM
    {
        #region prop            

        private readonly GetDataUrlService<Products> _service;
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

        public string Sum
        {
            get { return _Total; }
            set { _Total = value; OnPropertyChanged(nameof(Sum)); }
        }
        #endregion

        #region const
        public ShoppingCartVM(INavigation navigation)
        {
            this.Navigation = navigation;

            _service = new GetDataUrlService<Products>();
            GetData();
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
                if (ListCart == null)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
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
                Sum = Items.Sum(x => x.Total).ToString();
                NetAmount = Items.Sum(x => x.NetAmount).ToString();
                TotalDiscount = Items.Sum(x => x.TotalDiscount).ToString();
            }
            else
            {
                Sum =NetAmount=TotalDiscount= "0";
            }
        }
        #endregion

        #region click event open detail Item
        public ICommand ShowItem => new Command<int>(async (Id) =>
        {
            try
            {
                await Navigation.PushAsync(new DetailItem(Id), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion



        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await GetData();
            IsRefreshing = false;
        });
        #endregion

        #region click event open SendOrder
        public ICommand SendOrderBtn => new Command(async () =>
        {
            try
            {
                if (ListCart == null)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                    return;
                }    
                if (ListCart.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                    return;
                }
                await Navigation.PushAsync(new SendOrder(), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion

        #region click event Remove From Cart
        public ICommand RemoveFromCartCommand => new Command<int>(async (Id) =>
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }
                if (Id == 0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    return;
                }
                Items.Remove(Items.FirstOrDefault(x => x.ProductsId == Id));
                SumTotal();
                await App.Current.MainPage.DisplayAlert("حذف", "تم حذف المنتج من السلة", "نعم");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        });
        #endregion


        #region click event MaxBtn
        public ICommand MaxBtn => new Command<int>(async (Id) =>
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }

                if (ListCart == null)
                    ListCart = new ObservableCollection<OrderDetail>();


                var itm = ListCart.FirstOrDefault(x => x.ProductsId == Id);
                if (itm != null)
                {
                    var response = await _service.GetAsync("Products/GetById?Id="+Id);
                    if(response ==null)
                    {         
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطا", "نعم");
                        return;
                    }
                    var prod = response.data;
                    if (prod.Count <= itm.Count)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "بيعت كل الكمية", "نعم");
                        return;
                    }
                    int newIndex = Items.IndexOf(itm);
                    Items.Remove(itm);
                    itm.Count += 1;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {                            
                        itm.TotalDiscount = (itm.NetAmount * itm.DiscountPercentage / 100);
                        itm.NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }
                    Items.Add(itm);
                    int oldIndex = Items.IndexOf(itm);
                    Items.Move(oldIndex, newIndex);

                    // await App.Current.MainPage.DisplayAlert("تنبيه", "تم زيادة المنتج", "نعم");
                }
                SumTotal();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ", "نعم");

            }

        });
        #endregion


        #region click event MinBtn
        public ICommand MinBtn => new Command<int>(async (Id) =>
        {
            try
            {
                var itm = ListCart.FirstOrDefault(x => x.ProductsId == Id);
                if (itm != null)
                {
                    int newIndex = Items.IndexOf(itm);
                    Items.Remove(itm);
                    itm.Count -= 1;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {
                        itm.TotalDiscount = (itm.NetAmount * itm.DiscountPercentage / 100);
                        itm.NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }
                    Items.Add(itm);
                    int oldIndex = Items.IndexOf(itm);
                    Items.Move(oldIndex, newIndex);
                    if (itm.Count == 0)
                        ListCart.Remove(ListCart.FirstOrDefault(x => x.ProductsId == Id));

                    //  await App.Current.MainPage.DisplayAlert("تنبيه", "تم نقصان المنتج", "نعم");
                    SumTotal();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        });
        #endregion
    }
}
