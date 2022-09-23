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
    public partial class RegisterVM  :BaseVM
    {
        #region prop
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string phone; 
        [ObservableProperty]
        private List<Countries> _countrys;
        [ObservableProperty]
        private Countries countryIdSelected;

        private readonly IGetDataUrlService<Countries> _Countriesservice;
        private readonly IGetDataUrlService<Users> _userservice;
        #endregion
        public RegisterVM()
        {
            _Countriesservice = new GetDataUrlService<Countries>();
            _userservice = new GetDataUrlService<Users>(); 

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

        #region click event to signin
        [ICommand]
        public async void BtnSave()
        {                
            if (!CheckConnection())
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                return;
            }

            if (Name == null || Phone == null || CountryIdSelected.CountryId == 0)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                return;
            }
            else
            {        
                    try
                    {

                        Users user = new Users()
                        {
                            Name = Name,
                            Details = "",
                            Phone = Phone,
                            CountryId = CountryIdSelected.CountryId
                        };
                        Response<Users> response = await _userservice.PostAsync("Chat/Register", user);

                        if (response.success == false)
                        {
                            await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                        }
                        else
                        {
                        Preferences.Default.Set("Id", response.data.UserId);
                        Preferences.Default.Set("Name", Name);
                        Preferences.Default.Set("Phone", Phone);
                        Preferences.Default.Set("Details", "");
                        Preferences.Default.Set("CountryId", CountryIdSelected.CountryId);
                            InfoAccess.Id = response.data.UserId;                                       
                            await App.Current.MainPage.DisplayAlert("حفظ", "تم تسجيل حساب  بنجاح", "نعم");

                        await AppShell.Current.GoToAsync(nameof(ChatView), true);
                    }

                    }
                    catch (Exception ex)
                    { await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم"); }
           
            }
        }
        #endregion
    }
}
