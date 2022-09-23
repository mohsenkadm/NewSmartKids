using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using AppSmartKids.View;

namespace AppSmartKids.VM
{
    public partial class SendOrderVM:BaseVM
    {
        [ObservableProperty]
        private string _TotalDiscount;
        [ObservableProperty]
        private string _NetAmount;
        [ObservableProperty]
        private string _Total;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string phone;   
        [ObservableProperty]
        public string details;
        [ObservableProperty]
        private List<Countries> _countrys;
        [ObservableProperty]
        private Countries countryIdSelected;

        private readonly IGetDataUrlService<Countries> _Countriesservice;
        public SendOrderVM()
        {
            _Countriesservice = new GetDataUrlService<Countries>();   

            loadcombo();
        }
        #region load data for picker or combo 
        private async void loadcombo()
        {
            try
            {
                ResponseList<Countries> response1 = await _Countriesservice.GetListAllAsync("Countries/GetAll");
                if (response1.success)
                {
                    Countrys = response1.data;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                }
            }
            catch (Exception ex)
            { await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم"); }
        }
        #endregion
    }
}
