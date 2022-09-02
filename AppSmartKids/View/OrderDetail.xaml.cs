using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class OrderDetail : ContentPage 
{
	public OrderDetail(OrderDetailVM orderDetailVM)
	{
		InitializeComponent();
		this.BindingContext=orderDetailVM;
	}
}