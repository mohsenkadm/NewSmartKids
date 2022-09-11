using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids;

public partial class MainPage : ContentPage 
{

	public MainPageVM _viewmodel;

	public MainPage(MainPageVM mainPageVM)
	{
		InitializeComponent();
		this.BindingContext=mainPageVM;
		_viewmodel=mainPageVM;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
       _viewmodel.loadcarousel();
    }

}

