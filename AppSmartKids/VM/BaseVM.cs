using AppSmartKid.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKid.VM
{
    public partial class BaseVM: ObservableObject   
    {
        [ObservableProperty]
        private bool isRefreshing;

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
    }
}
