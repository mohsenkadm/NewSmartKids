using AppSmartKids.Helper.IServices;
using AppSmartKids.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using AppSmartKids.View;
using AppSmartKids.View;             
using Categories = Entity.Entity.Categories;

namespace AppSmartKids.VM
{
    public partial class MainPageVM:BaseVM
    {
        #region prop

        private readonly IGetDataUrlService<Carousel> _Carouselservice;
        [ObservableProperty]
        private List<Carousel> _ItemsCarousel;
        [ObservableProperty]
        public List<Categories> _Items;
        IGetDataUrlService<Categories> urlService;
        [ObservableProperty]
        private int _Position;
        #endregion

        #region const
        public MainPageVM()
        {
            this.urlService = new GetDataUrlService<Categories>();
            this._Carouselservice = new GetDataUrlService<Carousel>();
            
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
             GetData();
            loadcarousel();
            IsRefreshing = false;
        }
        #endregion

        #region load image from url    
        public async void loadcarousel()
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
        [ICommand]
        public async void GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                IsRunning = true;
                ResponseList<Categories> response = await urlService.GetListAllAsync("Categories/GetAll");

                if (response.success == false)
                {
                    await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items = response.data;
                }
                IsRunning = false;
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
        }
        #endregion
        #region click event open OpenAge
        [ICommand]
        public async void OpenAge(int CategoriesId)
        {
            try
            {
                var navParam = new Dictionary<string, object>();
                navParam.Add("CategoriesId", CategoriesId);
                await AppShell.Current.GoToAsync(nameof(ChooseAge), true, navParam);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

    }
}
