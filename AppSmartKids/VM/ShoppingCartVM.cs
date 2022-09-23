using AppSmartKids.Helper;
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
using OrderDetail = Entity.Entity.OrderDetail;

namespace AppSmartKids.VM
{
    public partial class ShoppingCartVM: BaseVM
    {
        #region prop  
        [ObservableProperty]
        private ObservableCollection<OrderDetail> items;
        [ObservableProperty]
        private string _Sum;     
        [ObservableProperty]
        private string _TotalDiscount;  
        [ObservableProperty]
        private string _NetAmount;    
        #endregion

        #region const
        public ShoppingCartVM()
        {               
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
        [ICommand]
        public async void ShowItem(int Id)
        {
            try
            {
                var navParam = new Dictionary<string, object>();
                navParam.Add("Id", Id);
                await AppShell.Current.GoToAsync(nameof(DetailItem), true, navParam);
            }
            catch (Exception ex)
            {
            }
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

        #region click event open SendOrder
        [ICommand]
        public async void SendOrderBtn()
        {
            try
            {
                if (ListCart.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                    return;
                }
               await  AppShell.Current.GoToAsync(nameof(SendOrder), true);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region click event Remove From Cart
        [ICommand]
        public async void RemoveFromCart(int Id)
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
                await App.Current.MainPage.DisplayAlert("تنبيه","حدث خطأ", "نعم");

            }
        }
        #endregion


        #region click event MaxBtn
        [ICommand]
        public async void MaxBtn(int Id)
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
                          
        }
        #endregion


        #region click event MinBtn
        [ICommand]
        public async void MinBtn(int Id)
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
        }
        #endregion
    }
}
