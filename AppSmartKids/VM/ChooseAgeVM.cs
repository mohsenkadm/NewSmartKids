using AppSmartKids.Helper.IServices;
using AppSmartKids.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
using Entity.Entity;
 
using AppSmartKids.View;
using Microsoft.Toolkit.Mvvm.Input;
using AppSmartKids.View;

namespace AppSmartKids.VM
{
    [QueryProperty(nameof(CategoriesId), "CategoriesId")]
    public partial class ChooseAgeVM :BaseVM
    {
        #region prop              
        [ObservableProperty]
        public List<TblAges> listAgeParam;
        IGetDataUrlService<TblAges> urlService;
        [ObservableProperty]      
        public int _CategoriesId;
        #endregion

        #region const
        public ChooseAgeVM()
        {
            this.urlService = new GetDataUrlService<TblAges>();    
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
            await GetData();
            IsRefreshing = false;
        }
        #endregion

        #region GetData
        [ICommand]
        public async Task GetData()
        {
            try
            {
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }
                ResponseList<TblAges> response = await urlService.GetListAllAsync("TblAges/GetAll");

                if (response.success == false)
                {
                    await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    ListAgeParam = response.data;
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
        }
        #endregion

        #region click event OpenProduct
        [ICommand]
        public async void OpenProduct()
        {
            try
            {
                if (CategoriesId == 0) { await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم"); return; }
                if (ListAgeParam == null)
                {
                    await AppShell.Current.DisplayAlert("خطا", "يجب تحديد العمر اولا", "نعم"); return;
                }
                if (ListAgeParam.Count == 0)
                {
                    await AppShell.Current.DisplayAlert("خطا", "يجب تحديد العمر اولا", "نعم"); return;
                }
                var navParam = new Dictionary<string, object>();
                navParam.Add("CategoriesId", CategoriesId);
                navParam.Add("listAgeParam", ListAgeParam);
                await AppShell.Current.GoToAsync(nameof(ProductsView), true, navParam);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
