using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AppSmartKidsXa.VM;
namespace AppSmartKidsXa.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailItem : ContentPage
    {
        public DetailItem(int Id)
        {
            InitializeComponent();
            this.BindingContext = new DetailItemVM(this.Navigation,Id);
        }
    }
}