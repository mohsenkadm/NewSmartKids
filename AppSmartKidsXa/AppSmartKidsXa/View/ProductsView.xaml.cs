using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AppSmartKidsXa.VM;
using AppSmartKidsXa.Entity;

namespace AppSmartKidsXa.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        public ProductsView(int CategoriesId, List<TblAges> tblAges)
        {
            InitializeComponent();
            this.BindingContext = new ProductsVM(this.Navigation, CategoriesId,tblAges);
        }
    }
}