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
    }
}
