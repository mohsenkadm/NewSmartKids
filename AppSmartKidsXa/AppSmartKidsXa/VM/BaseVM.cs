using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Entity;                   
using Entity.Entity;                  
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppSmartKidsXa.VM
{
    public  class BaseVM: INotifyPropertyChanged
    {
        private bool isVisibleLoginContent;
        public bool IsVisibleLoginContent
        {
            get { return isVisibleLoginContent; }
            set { isVisibleLoginContent = value; OnPropertyChanged(nameof(IsVisibleLoginContent)); }
        }
        private bool _IsVisibleContent = true;
        public bool IsVisibleContent
        {
            get { return _IsVisibleContent; }
            set { _IsVisibleContent =value; OnPropertyChanged(nameof(IsVisibleContent)); }
        }


        public INavigation Navigation { get; set; }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }                       
        private bool isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }                       
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        protected static ObservableCollection<OrderDetail> ListCart;
        #region CheckConnection
        public bool CheckConnection()
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            { return false; }
            else
            {
                return true;
            }
        }
        #endregion

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region click to Back 
        public ICommand Back => new Command(async () =>
        {
            try
            {
                await Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
