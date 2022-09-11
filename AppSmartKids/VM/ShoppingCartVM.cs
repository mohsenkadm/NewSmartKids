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

namespace AppSmartKids.VM
{
    public partial class ShoppingCartVM: BaseVM
    {
        #region prop  
        [ObservableProperty]
        private ObservableCollection<Products> items;
        [ObservableProperty]
        private string _Sum;    
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
                    await App.Current.MainPage.DisplayAlert("تنبيه","لا توجد منتجات في السلة", "نعم");
                    return;
                }
                Items = ListCart;
                Sum = Items.Sum(x => x.Price*x.Count).ToString();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطا اثناء عملية جلب البيانات", "نعم");

            }
        }
        #endregion

        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
             GetData();
            IsRefreshing = false;
        });
        #endregion

        #region click event open SendOrder
        public ICommand SendOrderBtn => new Command(async () =>
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
        });
        #endregion

        #region click event Remove From Cart
        public ICommand RemoveFromCart => new Command<string>(async (Name) =>
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }
                if (Name == "")
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    return;
                }
                Items.Remove(Items.FirstOrDefault(x => x.Name == Name));
                Sum = Items.Sum(X => X.Price * X.Count).ToString();
                await App.Current.MainPage.DisplayAlert("حذف", "تم حذف المنتج من السلة", "نعم");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه","حدث خطأ", "نعم");

            }
        });
        #endregion 
        #region click event MinBtn
        public ICommand  MaxBtn=> new Command<string>(async (Name) =>
        {
            try
            {    
                if (Name == "")
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    return;
                }
                var itm=Items.FirstOrDefault(x => x.Name == Name);
                if (itm != null)
                    itm.Count += 1;  
                Sum = Items.Sum(X => X.Price * X.Count).ToString();
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه","حدث خطأ", "نعم");

            }
        });
        #endregion

        #region click event MinBtn
        public ICommand  MinBtn=> new Command<string>(async (Name) =>
        {
            try
            {    
                if (Name == "")
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    return;
                }
                var itm=Items.FirstOrDefault(x => x.Name == Name);
                if (itm != null)
                {       
                    itm.Count -= 1;
                    if (itm.Count == 0)
                        Items.Remove(Items.FirstOrDefault(x => x.Name == Name));
                }
                Sum = Items.Sum(X => X.Price * X.Count).ToString();
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه","حدث خطأ", "نعم");

            }
        });
        #endregion
    }
}
