using AppSmartKidsXa.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppSmartKidsXa.VM;
namespace AppSmartKidsXa
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageVM(this.Navigation);
        }                       
    }
}
