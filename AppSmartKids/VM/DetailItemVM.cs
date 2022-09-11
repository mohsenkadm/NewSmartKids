 
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

namespace AppSmartKids.VM
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class DetailItemVM  :BaseVM
    {
        #region prop     
        [ObservableProperty]
        private int _Id = 0;        
        private readonly IGetDataUrlService<Products> _service;
        private readonly IGetDataUrlService<Images> _serviceImage;
        [ObservableProperty]      
        private List<Images> items;

        [ObservableProperty]
        private string _Name;
        [ObservableProperty]
        private string _Detail;
        [ObservableProperty]

        private int _Position;
        [ObservableProperty]
        private string _Price;   
        #endregion


        #region cons
        public DetailItemVM()
        {                                  
            _service = new GetDataUrlService<Products>();  
            _serviceImage = new GetDataUrlService<Images>();  
            Task.Run(() => GetData());
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void RefreshCommand()
        {
            IsRefreshing = true;
            await GetData();
            IsRefreshing = false;
        }
        #endregion

        #region GetData
        public async Task GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانرنت", "نعم");
                    return;
                }                       
                Response<Products> response = await _service.GetAsync("Sources/GetById?Id=" + Id);
                              
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
                else
                {
                    Products dataa = response.data;
                     
                    Detail = dataa.Detail;
                    Name = dataa.Name;   
                    Price = dataa.Price.ToString();
                    ResponseList<Images> response1 = await _serviceImage.GetListAllAsync("Sources/GetById?Id=" + Id);
                    if (response1.success == false)
                    {         
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    }
                    else
                    {
                        Items = response1.data;
                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه","حدث خطأ", "نعم");

            }
        }
        #endregion

        #region Next Image
        [ICommand]
        public async void Next()
        {
            try
            {
                if (Items.Count > 0)
                    if (Position < Items.Count - 1)
                        Position = (Position + 1);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Prev Image
        [ICommand]
        public async void Prev()
        {
            try
            {
                if (Items.Count > 0)
                    if (Position > 0)
                        Position = (Position - 1);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region click event open Shopping Cart
        [ICommand]
        public async void ShoppingCartBtn()
        {
            try
            {
                await AppShell.Current.GoToAsync(nameof(ShoppingCart), true);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region click event Add To Cart
        [ICommand]
        public async void MaxBtn()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }                                                     

                if (ListCart == null)
                    ListCart = new ObservableCollection<Products>();


                var itm = ListCart.FirstOrDefault(x=>x.ProductsId==Id);
                if (itm != null)
                    itm.Count += 1;
                else
                {
                    Response<Products> response = await _service.GetAsync("Magazine/GetById?Id=" + Id);
                    if (response == null)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    }

                    Products master = new Products()
                    {
                        Name = response.data.Name,
                        Detail = response.data.Detail,
                        Price = response.data.Price,
                        Count = 1
                    };

                    ListCart.Add(master);
                }              
                await App.Current.MainPage.DisplayAlert("تنبيه","تم اضافة المنتج الى السلة", "نعم");

            }
            catch (Exception ex)
            {                                       
                await App.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ", "نعم");

            }
        }
        #endregion


        #region click event MinBtn
        [ICommand]
        public async void MinBtn()
        {
            try
            {
                if (Name == "")
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    return;
                }
                var itm = ListCart.FirstOrDefault(x => x.Name == Name);
                if (itm != null)
                {
                    itm.Count -= 1;
                    if (itm.Count == 0)
                        ListCart.Remove(ListCart.FirstOrDefault(x => x.Name == Name));
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
