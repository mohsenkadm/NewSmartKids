 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class ShoppingCart : ContentPage	 
{
    public ShoppingCartVM _viewmodel;

    public ShoppingCart(ShoppingCartVM shoppingCartVM)
	{
		InitializeComponent();
		this.BindingContext = shoppingCartVM;
        _viewmodel=shoppingCartVM;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}