using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class SendOrder : ContentPage  
{
	public SendOrder(SendOrderVM sendOrderVM)
	{
		InitializeComponent();
		this.BindingContext = sendOrderVM;
	}
}