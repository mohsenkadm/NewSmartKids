using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.Helper;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSmartKidsXa.View;
using AppSmartKidsXa.View;             
using Categories = AppSmartKidsXa.Entity.Categories;
using Xamarin.Forms;
using System.Windows.Input;
using AppSmartKidsXa.Entity;

namespace AppSmartKidsXa.VM
{
    public   class MainPageVM:BaseVM
    {
        #region prop

        private readonly GetDataUrlService<Carousel> _Carouselservice;   
        private List<Carousel> _ItemsCarousel;
        public List<Carousel> ItemsCarousel
        {
            get { return _ItemsCarousel; }
            set { _ItemsCarousel = value; OnPropertyChanged(nameof(ItemsCarousel)); }
        }

        public List<Categories> _Items;
        public List<Categories> Items
        {
            get { return _Items; }
            set { _Items = value; OnPropertyChanged(nameof(Items)); }
        }
        GetDataUrlService<Categories> urlService;
        private int _Position;
        public int Position
        {
            get { return _Position; }
            set { _Position = value; OnPropertyChanged(nameof(Position)); }
        }
        #endregion

        #region const
        public MainPageVM(INavigation navigation)
        {
            this.Navigation = navigation;
            this.urlService = new GetDataUrlService<Categories>();
            this._Carouselservice = new GetDataUrlService<Carousel>();
             GetData(); 
            loadcarousel();
        }
        #endregion
        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await GetData(); loadcarousel();
            IsRefreshing = false;
        });
        #endregion 

        #region ShoppingCart
        public ICommand ShoppingCart => new Command(async () =>
        {
            if (ListCart == null)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                return;
            }    
            if (ListCart.Count==0)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا توجد منتجات في السلة", "نعم");
                return;
            }
            await this.Navigation.PushAsync(new ShoppingCart(), true);
        });
        #endregion  

        #region Products
        public ICommand Products => new Command(async () =>
        {
            List<TblAges> tblAges = new List<TblAges>();
            tblAges.Add(new TblAges() { AgeId = 1, State = true });
            await Navigation.PushAsync(new ProductsView(0,tblAges), true);
        });
        #endregion

        #region CategoriesView
        public ICommand CategoriesView => new Command(async () =>
        {
            await Navigation.PushAsync(new CategoriesView(), true);
        });
        #endregion    

        #region Notification
        public ICommand Notification => new Command(async () =>
        {
            await Navigation.PushAsync(new View.Notification(), true);
        });
        #endregion  

        #region PostsView
        public ICommand PostsView => new Command(async () =>
        {
            await Navigation.PushAsync(new PostsView(), true);
        });
        #endregion   

        #region ChatView
        public ICommand ChatView => new Command(async () =>
        {
            await Navigation.PushAsync(new ChatView(), true);
        });
        #endregion 

        #region SendOrder
        public ICommand SendOrder => new Command(async () =>
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
        });
        #endregion     
        #region Orders
        public ICommand Orders => new Command(async () =>
        {
            await Navigation.PushAsync(new View.Orders(), true);
        });
        #endregion

        #region click to Share 
        public ICommand BtnShare => new Command(async () =>
        {
            try
            {
                string url = "";
                if (Device.RuntimePlatform == Device.Android)
                { url = "https://play.google.com/store/apps/details?id=com.companyname.AppSmartKids"; }
                else
                {
                    url = "https://apps.apple.com/us/app/surratalghazalapp/id1626621778";
                }
                await Plugin.Share.CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage { Text = url, Title = "مشاركة التطبيق" });

            }
            catch (Exception ex)
            {
            }
        });
        #endregion

        #region load image from url    
        public async Task loadcarousel()
        {
            if (!CheckConnection())
            {
                return;
            }

            try
            {
                ResponseList<Carousel> response = await _Carouselservice.GetListAllAsync("Carousel/GetAll");
                if (response.data.Count == 0) return;
                ItemsCarousel = response.data;
                Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
                {
                    Position = (Position + 1) % response.data.Count;
                    return true;
                }));
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region GetData    
        public async Task GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await App.Current.MainPage.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                IsRunning = true;
                ResponseList<Categories> response = await urlService.GetListAllAsync("Categories/GetAll");

                if (response.success == false)
                {
                    await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items = response.data;
                }
                IsRunning = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
        }
        #endregion

        #region click event open OpenAge
        public ICommand OpenAgeCommand => new Command<int>(async (CategoriesId) =>
        {
            try
            {
                await Navigation.PushAsync(new ChooseAge(CategoriesId), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion

    }
}
