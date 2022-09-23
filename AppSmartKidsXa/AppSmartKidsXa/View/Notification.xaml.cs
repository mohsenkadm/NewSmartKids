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
    public partial class Notification : ContentPage
    {
        public Notification()
        {
            InitializeComponent();
            this.BindingContext = new NotificationVM(this.Navigation) ;
        }
    }
}