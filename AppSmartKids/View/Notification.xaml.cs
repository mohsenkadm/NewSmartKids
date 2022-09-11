 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class Notification : ContentPage	   
{
    public NotificationVM _viewmodel;

    public Notification(NotificationVM notificationVM)
	{
		InitializeComponent();
		this.BindingContext=notificationVM;
        _viewmodel=notificationVM;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}
