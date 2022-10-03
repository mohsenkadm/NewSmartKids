using AppSmartKidsXa;
using AppSmartKidsXa.Helper;
using AppSmartKidsXa.VM;
using AppSmartKidsXa.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;               
using AppSmartKidsXa.View;
 
using AppSmartKidsXa.Helper.IServices;
using OrderDetail = AppSmartKidsXa.Entity.OrderDetail;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{                                                        
    public  class ProductsVM :BaseVM
    {

        #region prop    
                                
        public List<TblAges> listAgeParam;    
        public Products productFilter;
        public Products ProductFilter
        {
            get { return productFilter; }
            set { productFilter = value; OnPropertyChanged(nameof(ProductFilter)); }
        }
                        
        private readonly GetDataUrlService<Like> _likeservice;
        private readonly GetDataUrlService<Products> _service;    
        public int _CategoriesId;  
        public string name; 
        public string Name
        {
            get { return name;}
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private ObservableCollection<Products> items;
         public ObservableCollection<Products> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }                      
        public string searchtxt;
        public string Searchtxt
        {
            get { return searchtxt; }
            set { searchtxt = value; Search(false); OnPropertyChanged(nameof(Searchtxt)); }
        }
        #endregion

        #region const
        public ProductsVM(INavigation navigation,int CategoriesId,List<TblAges> tblAges)
        {
            this.Navigation = navigation;
            _CategoriesId = CategoriesId;
            listAgeParam = tblAges;
            _service = new GetDataUrlService<Products>();
            _likeservice = new GetDataUrlService<Like>();
            Search();
        }
        #endregion

        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await Search();
            IsRefreshing = false;
        });
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
        #region click event search
        public ICommand SearchCommand => new Command(async () =>
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
            }
        });
        #endregion      

        #region Load Item    
        public async Task Search(bool f=true)
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه","لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }
                             if(f)
                UserDialogs.Instance.ShowLoading("انتظار");
                productFilter = new Products();
                productFilter.Name = Searchtxt == null ? "" : Searchtxt;
                productFilter.CategoriesId = _CategoriesId;
                productFilter.UserId = InfoAccess.Id;
                foreach(var item in listAgeParam) {
                    if (productFilter.AgeFilter == null)
                        productFilter.AgeFilter = new List<AgeFilter>();
                    if(item.State==true)
                    productFilter.AgeFilter.Add(new AgeFilter() { Id = item.AgeId });
                }
                ResponseCollection<Products> response = await _service.PostToGetCollectionAsync("Products/GetProdForApp", productFilter );
                     
                if (response.success == false)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
                else
                {
                     Items = response.data;
                   
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
            finally
            {
                if (f)
                    UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
                   

        #region click event open Shopping Cart
        public ICommand ShoppingCartBtn => new Command(async () =>
        { 
            try
            {
                await Navigation.PushAsync(new ShoppingCart(), true);    
            }
            catch (Exception ex)
            {
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
                    var response = Items.FirstOrDefault(i => i.ProductsId == Id);
                    if (response.Count<=itm.Count)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "بيعت كل الكمية", "نعم");
                        return;
                    }
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
                    var response = Items.FirstOrDefault(i => i.ProductsId == Id);
                    if(response.Count==0)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "بيعت كل الكمية", "نعم");
                        return;
                    }
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
                         ,Image = response.Image
                    };
                    //var prod = Items.FirstOrDefault(x => x.ProductsId == Id);
                    //prod.Count -= 1;
                    ListCart.Add(master);

                    await App.Current.MainPage.DisplayAlert("تنبيه", "تم اضافة المنتج الى السلة", "نعم");
                }

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
       });
        #endregion

       #region BtnLikeValue
        public ICommand BtnLikeValue => new Command<int>(async (Id) =>
        {
            if (InfoAccess.Id == 0) { return; }
            var source = Items.SingleOrDefault(x => x.ProductsId == Id);
            if (String.Equals(source.SourceLike, "heartempty.png"))
            {
                int newIndex = Items.IndexOf(source);
                Items.Remove(source);
                source.SourceLike = "heart.png";
                source.LikeCount = source.LikeCount + 1;
                Items.Add(source);
                int oldIndex = Items.IndexOf(source);
                Items.Move(oldIndex, newIndex);
                Like Like = new Like()
                {
                    LikeId = 0,
                    ProductsId = Id,
                    UserId = InfoAccess.Id
                };

                await _likeservice.PostAsync("Products/PostLike", Like);
            }
            else
            {
                int newIndex = Items.IndexOf(source);
                Items.Remove(source);
                source.SourceLike = "heartempty.png";
                source.LikeCount = source.LikeCount - 1;
                Items.Add(source);
                int oldIndex = Items.IndexOf(source);
                Items.Move(oldIndex, newIndex);
                Response<Like> response = await _likeservice.DeleteAsync("Products/RemoveLike?ProductsId=" + Id.ToString() + "&UserId=" + InfoAccess.Id, "");
                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
            }
        });
        #endregion
    }
}
