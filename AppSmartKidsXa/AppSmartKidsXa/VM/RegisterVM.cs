using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;
using AppSmartKidsXa.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                                     
using AppSmartKidsXa.View;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Essentials;
using OneSignalSDK.Xamarin;
using Acr.UserDialogs;

namespace AppSmartKidsXa.VM
{
    public  class RegisterVM  :BaseVM
    {
        #region prop          
        public string name; 
        public string Name
        {
            get { return name; }
            set { name = value;

                OnPropertyChanged(nameof(Name));
            }
        }
        public string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;

                OnPropertyChanged(nameof(Phone));
            }
        }
        private List<Countries> _countrys;
        public List<Countries> Countrys
        {
            get { return _countrys; }
            set { _countrys = value;
                OnPropertyChanged(nameof(Countrys));
            }
        }                       
        private Countries countryIdSelected;
        public Countries CountryIdSelected
        {
            get { return countryIdSelected; }
            set
            {
                countryIdSelected = value;
                OnPropertyChanged(nameof(CountryIdSelected));
            }
        }

        private readonly GetDataUrlService<Countries> _Countriesservice;
        private readonly GetDataUrlService<Users> _userservice;
        #endregion
        public RegisterVM(INavigation navigation)
        {
            this.Navigation = navigation;
            _Countriesservice = new GetDataUrlService<Countries>();
            _userservice = new GetDataUrlService<Users>(); 

            loadcombo();
        }
        #region load data for picker or combo 
        private async void loadcombo()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("انتظار");
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
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region click event to signin  
        public ICommand BtnSaveCommand => new Command(async () =>
        {
                try
                {
            UserDialogs.Instance.ShowLoading("انتظار");
            if (!CheckConnection())
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                return;
            }

            if (Name == null || Phone == null || CountryIdSelected == null)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "رجاءا اكمال المعلومات", "نعم");
                return;
            }

            if (Name.Trim().Length == 0 || Phone.Trim().Length == 0 || CountryIdSelected.CountryId == 0)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "رجاءا اكمال المعلومات", "نعم");
                return;
            }   
               

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
                        Preferences.Set("Id", response.data.UserId);
                        Preferences.Set("Name", Name);
                        Preferences.Set("Phone", Phone);
                        Preferences.Set("Details", "");
                        Preferences.Set("CountryId", CountryIdSelected.CountryId);
                        InfoAccess.Id = response.data.UserId;        
                        try {   OneSignal.Default.SetExternalUserId(response.data.UserId.ToString()); } catch (Exception ex) { }
                        await App.Current.MainPage.DisplayAlert("حفظ", "تم تسجيل حساب  بنجاح", "نعم");

                        await Navigation.PushAsync(new ChatView(), true);
                    }                          
                }
                catch (Exception ex)
                { await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم"); }
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
                 
        });
        #endregion
    }
}
