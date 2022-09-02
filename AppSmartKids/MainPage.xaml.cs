using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid;

public partial class MainPage : ContentPage 
{					 

	public MainPage(MainPageVM mainPageVM)
	{
		InitializeComponent();
		this.BindingContext=mainPageVM;
	}

	 
}

