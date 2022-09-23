using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
using AppSmartKidsXa.Entity;
 
using AppSmartKidsXa.View;             
using AppSmartKidsXa.View;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{                                                          
    public  class ChooseAgeVM :BaseVM
    {
        #region prop              
        public List<TblAges> _listAgeParam;
        public List<TblAges> ListAgeParam
        {
            get { return _listAgeParam; }
            set { _listAgeParam = value; OnPropertyChanged(nameof(ListAgeParam)); }
        }
        GetDataUrlService<TblAges> urlService;        
        public int _CategoriesId;
        #endregion

        #region const
        public ChooseAgeVM(INavigation navigation,int CategoriesId)
        {
            this.Navigation = navigation;
            _CategoriesId = CategoriesId;
            this.urlService = new GetDataUrlService<TblAges>();
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
                     await App.Current.MainPage.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                ResponseList<TblAges> response = await urlService.GetListAllAsync("TblAges/GetAll");

                if (response.success == false)
                {
                     await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    ListAgeParam = response.data;
                }
            }
            catch (Exception ex)
            {
                 await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region click event OpenProduct   
        public ICommand OpenProductCommand => new Command(async () =>
        {
            try
            {
                if (_CategoriesId == 0) {  await App.Current.MainPage.DisplayAlert("خطا", "حدث خطا", "نعم"); return; }
                if (ListAgeParam == null)
                {
                      await App.Current.MainPage.DisplayAlert("خطا", "يجب تحديد العمر اولا", "نعم"); return;
                }
                if (ListAgeParam.Count == 0)
                {
                      await App.Current.MainPage.DisplayAlert("خطا", "يجب تحديد العمر اولا", "نعم"); return;
                }
                var list = ListAgeParam.Where(i => i.State == true).ToList();
                if (list.Count == 0)
                {
                      await App.Current.MainPage.DisplayAlert("خطا", "يجب تحديد العمر اولا", "نعم"); return;
                }                                                                        
                await Navigation.PushAsync(new ProductsView(_CategoriesId, ListAgeParam), true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
