 
using AppSmartKids.VM;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class ProductsView : ContentPage
{
    ProductsVM _viewmodel;
    public ProductsView(ProductsVM productsVM)
	{
		InitializeComponent();
		this.BindingContext= productsVM;
        _viewmodel = productsVM;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.SearchCommand.Execute(null);
    }
}