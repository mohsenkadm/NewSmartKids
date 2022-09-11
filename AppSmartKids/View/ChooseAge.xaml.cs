 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class ChooseAge : ContentPage  
{
    public ChooseAgeVM _viewmodel;

    public ChooseAge(ChooseAgeVM chooseAgeVM)
	{
		InitializeComponent();

		this.BindingContext = chooseAgeVM;
        _viewmodel=chooseAgeVM;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);      
    }
}