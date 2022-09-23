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
    public partial class ChooseAge : ContentPage
    {
        public ChooseAge(int CategoriesId)
        {
            InitializeComponent();
            this.BindingContext = new ChooseAgeVM(this.Navigation,CategoriesId);
        }
    }
}