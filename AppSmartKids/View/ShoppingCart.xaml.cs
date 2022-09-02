using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class ShoppingCart : ContentPage	 
{
	public ShoppingCart(ShoppingCartVM shoppingCartVM)
	{
		InitializeComponent();
		this.BindingContext = shoppingCartVM;
	}
}