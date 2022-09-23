using AppSmartKids.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKids.VM
{
    public partial class BaseVM: ObservableObject   
    {
        [ObservableProperty]
        private bool isRefreshing;
        [ObservableProperty]
        private bool isRunning; 
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        protected static ObservableCollection<OrderDetail> _ListCart;
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

        #region go back
        [ICommand]
        public async void BackBtn()
        {
            await AppShell.Current.GoToAsync("..");
        }
        #endregion
    }
}
