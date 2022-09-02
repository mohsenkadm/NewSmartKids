using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class Notification : ContentPage	   
{
	public Notification(NotificationVM notificationVM)
	{
		InitializeComponent();
		this.BindingContext=notificationVM;
	}
}