using AppSmartKids.VM;
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
    public partial class PostsView : ContentPage
    {
        public PostsView()
        {
            InitializeComponent();
            this.BindingContext = new PostsVM(this.Navigation);
        }
    }
}