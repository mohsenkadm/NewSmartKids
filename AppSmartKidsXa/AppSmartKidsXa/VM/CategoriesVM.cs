using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.Helper;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;        
using AppSmartKidsXa.View; 
using Categories = AppSmartKidsXa.Entity.Categories;
using System.Windows.Input;
using Xamarin.Forms;
using AppSmartKidsXa.VM;
using AppSmartKidsXa;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{
    public  class CategoriesVM  :BaseVM
    {

        #region prop

        private List<Categories> _item;

        public List<Categories> Items
        {
            get { return _item; }
            set { _item = value;
                OnPropertyChanged(nameof(Items));
            }
        }
                               

        public List<Categories> _ItemsSearch;
        public List<Categories> ItemsSearch
        {
            get { return _ItemsSearch; }
            set
            {
                _ItemsSearch = value;
                OnPropertyChanged(nameof(ItemsSearch));
            }
        }
        GetDataUrlService<Categories> urlService;    
        private string searchtxt;

        public string Searchtxt
        {
            get { return searchtxt; }
            set { searchtxt = value; Search(); OnPropertyChanged(nameof(Searchtxt)); }
        }

        #endregion

        #region const
        public CategoriesVM(INavigation navigation)
        {
            this.Navigation = navigation;
            this.urlService = new GetDataUrlService<Categories>();
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
        public async Task Search()
        {
            if (Searchtxt.Trim().Length > 0)
                Items = Items.Where(x => x.CategoriesName.Contains(Searchtxt)).ToList();
            else
                Items = ItemsSearch;
        }
        #endregion
        #region GetData  
        public async Task GetData()
        {
            try
            {
                IsBusy = true;
                if (!CheckConnection())
                {                                         
                    await App.Current.MainPage.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }                               
                UserDialogs.Instance.ShowLoading("انتظار");
                ResponseList<Categories> response = await urlService.GetListAllAsync("Categories/GetAll");

                if (response.success == false)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items =ItemsSearch= response.data;     
                }                   
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
                IsBusy = false;
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
