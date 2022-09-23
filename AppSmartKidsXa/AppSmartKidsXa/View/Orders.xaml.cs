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
    public partial class Orders : ContentPage
    {
        public Orders()
        {
            InitializeComponent();
            this.BindingContext = new OrdersVM(this.Navigation);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterView(), true);
        }
    }
}