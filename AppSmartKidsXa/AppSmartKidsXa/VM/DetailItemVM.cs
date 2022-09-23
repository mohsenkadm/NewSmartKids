 
using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.View;                     
using AppSmartKidsXa.Entity;          
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using OrderDetail = AppSmartKidsXa.Entity.OrderDetail;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{                                    
    public   class DetailItemVM  :BaseVM
    {
        #region prop          
        private int _Id = 0;        
        private readonly GetDataUrlService<Products> _service;
        private readonly GetDataUrlService<Images> _serviceImage;   
        private List<Images> images;
        public List<Images> Images
        {
            get { return images; }
            set { images = value; OnPropertyChanged(nameof(Images)); }
        }                         
        private Products products;
                             
        private string _Name; 
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(nameof(Name)); }
        }
        private string _Detail;
        public string Detail
        {
            get { return _Detail; }
            set { _Detail = value; OnPropertyChanged(nameof(Detail)); }
        }

        private int _Position;
        public int Position
        {
            get { return _Position; }
            set { _Position = value; OnPropertyChanged(nameof(Position)); }
        }
        private string _Price;         
        public string Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(nameof(Price)); }
        }
        private string _DiscountPercentage;  
        public string DiscountPercentage
        {
            get { return _DiscountPercentage; }
            set { _DiscountPercentage = value; OnPropertyChanged(nameof(DiscountPercentage)); }
        }
        private int _Count;

        public int Count
        {
            get { return _Count; }
            set { _Count = value; OnPropertyChanged(nameof(Count)); }
        }

        #endregion


        #region cons
        public DetailItemVM(INavigation navigation,int Id)
        {
            this.Navigation = navigation;
            _Id = Id;
            _service = new GetDataUrlService<Products>();  
            _serviceImage = new GetDataUrlService<Images>();
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
                    await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                    return;
                }                       
                Response<Products> response = await _service.GetAsync("Products/GetById?Id=" + _Id);
                              
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
                        DiscountPercentage = "0";
                    }
                    if (ListCart != null)
                    {
                        var co = ListCart.FirstOrDefault(x => x.ProductsId == _Id);
                        if (co != null)
                        {
                            Count = co.Count;
                        }
                    }
                    products = dataa;
                    ResponseList<Images> response1 = await _serviceImage.GetListAllAsync("Products/GetImagesByProductsId?Id=" + _Id);
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
            finally
            {

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

        #region Next Image    
        public ICommand NextCommand => new Command(async () =>
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
        });
        #endregion

        #region Prev Image     
        public ICommand PrevCommand => new Command(async () =>
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
        });
        #endregion

        #region click event open Shopping Cart
        public ICommand ShoppingCartBtnCommand => new Command(async () =>
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

        public ICommand MaxBtn => new Command(async () =>
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


                var itm = ListCart.FirstOrDefault(x => x.ProductsId == _Id);
                if (itm != null)
                {
                    var response = await _service.GetAsync("Products/GetById?Id=" + _Id);
                    if (response == null)
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
                    itm.Count += 1;
                    Count = itm.Count;
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
                        ProductsId = _Id,
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
                    Count = 1;
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
        public ICommand MinBtn => new Command(async () =>
        {
            try
            {
                var itm = ListCart.FirstOrDefault(x => x.ProductsId == _Id);
                if (itm != null)
                {
                    itm.Count -= 1;

                    Count = itm.Count;
                    itm.Total = itm.Count * itm.Price;
                    itm.NetAmount = itm.Total;
                    if (itm.IsDiscount == true)
                    {
                        itm.TotalDiscount = (itm.NetAmount * itm.DiscountPercentage / 100);
                        itm.NetAmount = itm.NetAmount - (itm.NetAmount * itm.DiscountPercentage / 100);
                    }
                    if (itm.Count == 0)
                        ListCart.Remove(ListCart.FirstOrDefault(x => x.ProductsId == _Id));

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
        public ICommand BtnLikeValueCommand => new Command(async () =>
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
        });
        #endregion
    }
}
