using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class OrderSeccess : ContentPage	 
{
	public OrderSeccess(OrderSeccessVM orderSeccessVM)
	{
		InitializeComponent();
		this.BindingContext=orderSeccessVM;
	}
}