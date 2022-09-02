using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class Orders : ContentPage	 
{
	public Orders(OrdersVM orders)
	{
		InitializeComponent();
		this.BindingContext=orders;
	}
}