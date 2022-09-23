 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class DetailItem : ContentPage  
{
    DetailItemVM _viewmodel;

    public DetailItem(DetailItemVM detailItemVM)
	{
		InitializeComponent();
		this.BindingContext=detailItemVM;
        _viewmodel = detailItemVM;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}