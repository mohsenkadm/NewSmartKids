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
    public partial class CategoriesVM  :BaseVM
    {

        #region prop
                                                   
        [ObservableProperty]
        public List<Categories> _Items;
        [ObservableProperty]

        public List<Categories> _ItemsSearch;
        IGetDataUrlService<Categories> urlService;
        [ObservableProperty]               
        public string searchtxt;
        #endregion

        #region const
        public CategoriesVM()
        {
            this.urlService = new GetDataUrlService<Categories>();    

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
                IsBusy = true;
                if (!CheckConnection())
                {
                    await AppShell.Current.DisplayAlert("خطا", "لا يوجد اتصال بلانترنت", "نعم"); return;
                }                
                ResponseList<Categories> response = await urlService.GetListAllAsync("Categories/GetAll");

                if (response.success == false)
                {
                    await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
                }
                else
                {
                    Items =ItemsSearch= response.data;
                    if (Searchtxt.Trim().Length > 0)
                        Items = Items.Where(x => x.CategoriesName.Contains(Searchtxt)).ToList();
                    else
                        Items = ItemsSearch;
                }                   
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("خطا", "حدث خطا", "نعم");
            }
            finally
            {
                IsBusy = false;
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
