using AppSmartKids;
using AppSmartKids.Helper;
using AppSmartKids.VM;   
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AppSmartKids.View;
 
using AppSmartKids.Helper.IServices;
using OrderDetail = Entity.Entity.OrderDetail;

namespace AppSmartKids.VM
{
    [QueryProperty(nameof(CategoriesId), "CategoriesId")]
    [QueryProperty(nameof(ListAgeParam), "listAgeParam")]
    public partial class ProductsVM :BaseVM
    {

        #region prop    

        [ObservableProperty]
        public List<TblAges> listAgeParam;  
        [ObservableProperty]
        public Products productFilter;

        [ObservableProperty]
        public ObservableCollection<Products> _ItemsSearch;
        private readonly IGetDataUrlService<Like> _likeservice;
        private readonly IGetDataUrlService<Products> _service;
        [ObservableProperty]
        public int _CategoriesId;    
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        private ObservableCollection<Products> items;

        [ObservableProperty]
        public string searchtxt;
        #endregion

        #region const
        public ProductsVM()
        {
            _service = new GetDataUrlService<Products>();
            _likeservice = new GetDataUrlService<Like>();   
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
            await Search();
            IsRefreshing = false;
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
                await AppShell.Current.GoToAsync(nameof(DetailItem), true,navParam);       
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Load Item 
        [ICommand]
        public async Task Search()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه","لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }

                productFilter = new Products();
                productFilter.Name = Name == null ? "" : Name;
                productFilter.CategoriesId = CategoriesId;
                foreach(var item in ListAgeParam) {
                    if (productFilter.AgeFilter == null)
                        productFilter.AgeFilter = new List<AgeFilter>();
                    if(item.State==true)
                    productFilter.AgeFilter.Add(new AgeFilter() { Id = item.AgeId });
                }
                ResponseCollection<Products> response = await _service.PostToGetCollectionAsync("Products/GetProdForApp", productFilter );
                     
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
                else
                {
                     Items= ItemsSearch = response.data;
                   
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        }
        #endregion

        #region Load Item 
        [ICommand]
        public async Task GetData()
        {
            if (Searchtxt.Trim().Length > 0)
                Items = (ObservableCollection<Products>)Items.Where(x => x.Name.Contains(Searchtxt));
            else
                Items = ItemsSearch;
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
                    itm.Count += 1;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {
                        itm.TotalDiscount=(itm.NetAmount * itm.DiscountPercentage / 100);  
                        itm. NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }                  

                        await App.Current.MainPage.DisplayAlert("تنبيه", "تم زيادة المنتج", "نعم");
                }
                else
                {
                    var response = Items.FirstOrDefault(i => i.ProductsId == Id);
                    int DiscountPercentage = 0;
                    decimal NetAmount=response.Price,
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
                        ,Total = response.Price 
                        ,IsDiscount=response.IsDiscount
                        ,DiscountPercentage = DiscountPercentage
                        ,TotalDiscount = TotalDiscount
                        ,NetAmount = NetAmount

                    };
                    var prod = Items.FirstOrDefault(x => x.ProductsId == Id);
                    prod.Count = 1;
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
        public async void MinBtn(int Id)
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
            if (InfoAccess.Id == 0) { return; }
            var source = Items.SingleOrDefault(x => x.ProductsId == para);
            if (String.Equals(source.SourceLike, "disHeart.png"))
            {
                int newIndex = Items.IndexOf(source);
                Items.Remove(source);
                source.SourceLike = "Heart.png";
                source.LikeCount = source.LikeCount + 1;
                Items.Add(source);
                int oldIndex = Items.IndexOf(source);
                Items.Move(oldIndex, newIndex);
                Like Like = new Like()
                {
                    LikeId = 0,   ProductsId=para     ,
                    UserId = InfoAccess.Id
                };

                await _likeservice.PostAsync("Magazine/PostLike", Like);
            }
            else
            {
                int newIndex = Items.IndexOf(source);
                Items.Remove(source);
                source.SourceLike = "disHeart.png";
                source.LikeCount = source.LikeCount - 1;
                Items.Add(source);
                int oldIndex = Items.IndexOf(source);
                Items.Move(oldIndex, newIndex);
                Response<Like> response = await _likeservice.DeleteAsync("Magazine/RemoveLike?MagazineId=" + para.ToString() + "&UserId=" + InfoAccess.Id, "");
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
            }
        }
        #endregion
    }
}
