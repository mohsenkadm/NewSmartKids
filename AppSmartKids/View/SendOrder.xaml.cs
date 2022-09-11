using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class SendOrder : ContentPage  
{
	public SendOrder(SendOrderVM sendOrderVM)
	{
		InitializeComponent();
		this.BindingContext = sendOrderVM;
	}
}