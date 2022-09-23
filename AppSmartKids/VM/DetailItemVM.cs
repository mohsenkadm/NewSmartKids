 
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
using OrderDetail = Entity.Entity.OrderDetail;

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
        private List<Images> images;  
        [ObservableProperty]      
        private Products products;

        [ObservableProperty]
        private string _Name;
        [ObservableProperty]
        private string _Detail;
        [ObservableProperty]

        private int _Position;
        [ObservableProperty]
        private string _Price;   
        [ObservableProperty]
        private string _DiscountPercentage;   
        #endregion


        #region cons
        public DetailItemVM()
        {                                  
            _service = new GetDataUrlService<Products>();  
            _serviceImage = new GetDataUrlService<Images>();  
            
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
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }                       
                Response<Products> response = await _service.GetAsync("Products/GetById?Id=" + Id);
                              
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
                    if (dataa.IsDiscount)
                    {
                        DiscountPercentage = dataa.DiscountPercentage.ToString();
                    }else
                    {
                        DiscountPercentage = "";
                    }
                    products = dataa;
                    ResponseList<Images> response1 = await _serviceImage.GetListAllAsync("Products/GetImagesByProductsId?Id=" + Id);
                    if (response1.success == false)
                    {         
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    }
                    else
                    {
                        Images = response1.data;
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
                if (Images.Count > 0)
                    if (Position < Images.Count - 1)
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
                if (Images.Count > 0)
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


        #region click event MaxBtn
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
                    ListCart = new ObservableCollection<OrderDetail>();


                var itm = ListCart.FirstOrDefault(x => x.ProductsId == Id);
                if (itm != null)
                {
                    itm.Count += 1;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {
                        itm.TotalDiscount = (itm.NetAmount * itm.DiscountPercentage / 100);
                        itm.NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }                

                    await App.Current.MainPage.DisplayAlert("تنبيه", "تم زيادة المنتج", "نعم");
                }
                else
                {
                    var response = products;
                    int DiscountPercentage = 0;
                    decimal NetAmount = response.Price,
                        TotalDiscount = 0;
                    if (response.IsDiscount == true)
                    {
                        DiscountPercentage = response.DiscountPercentage;
                        TotalDiscount = (NetAmount * DiscountPercentage / 100);
                        NetAmount = NetAmount - (NetAmount * DiscountPercentage / 100);
                    }
                    OrderDetail master = new OrderDetail()
                    {
                        ProductsId = Id,
                        Name = response.Name,
                        Price = response.Price,
                        Count = 1
                        ,
                        Total = response.Price
                        ,
                        IsDiscount = response.IsDiscount
                        ,
                        DiscountPercentage = DiscountPercentage
                        ,
                        TotalDiscount = TotalDiscount
                        ,
                        NetAmount = NetAmount

                    };                
                    ListCart.Add(master);

                    await App.Current.MainPage.DisplayAlert("تنبيه", "تم اضافة المنتج الى السلة", "نعم");
                }

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
                var itm = ListCart.FirstOrDefault(x => x.ProductsId == Id);
                if (itm != null)
                {
                    itm.Count -= 1;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {
                        itm.TotalDiscount = (itm.NetAmount * itm.DiscountPercentage / 100);
                        itm.NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }
                    if (itm.Count == 0)
                        ListCart.Remove(ListCart.FirstOrDefault(x => x.ProductsId == Id));

                    await App.Current.MainPage.DisplayAlert("تنبيه", "تم نقصان  المنتج", "نعم");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        }
        #endregion

        #region BtnLikeValue
        [ICommand]
        public async void BtnLikeValue(int para)
        {
            //if (InfoAccess.Id == 0) { return; }
            //var source = Items.SingleOrDefault(x => x.ProductsId == para);
            //if (String.Equals(source.SourceLike, "disHeart.png"))
            //{
            //    int newIndex = Items.IndexOf(source);
            //    Items.Remove(source);
            //    source.SourceLike = "Heart.png";
            //    source.LikeCount = source.LikeCount + 1;
            //    Items.Add(source);
            //    int oldIndex = Items.IndexOf(source);
            //    Items.Move(oldIndex, newIndex);
            //    Like Like = new Like()
            //    {
            //        LikeId = 0,
            //        ProductsId = para,
            //        UserId = InfoAccess.Id
            //    };

            //    await _likeservice.PostAsync("Magazine/PostLike", Like);
            //}
            //else
            //{
            //    int newIndex = Items.IndexOf(source);
            //    Items.Remove(source);
            //    source.SourceLike = "disHeart.png";
            //    source.LikeCount = source.LikeCount - 1;
            //    Items.Add(source);
            //    int oldIndex = Items.IndexOf(source);
            //    Items.Move(oldIndex, newIndex);
            //    Response<Like> response = await _likeservice.DeleteAsync("Magazine/RemoveLike?MagazineId=" + para.ToString() + "&UserId=" + InfoAccess.Id, "");
            //    if (response.success == false)
            //    {
            //        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
            //    }
            //}
        }
        #endregion
    }
}
