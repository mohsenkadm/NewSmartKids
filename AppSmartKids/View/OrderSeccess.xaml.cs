using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class OrderSeccess : ContentPage	 
{
	public OrderSeccess(OrderSeccessVM orderSeccessVM)
	{
		InitializeComponent();
		this.BindingContext=orderSeccessVM;
	}
}