 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class Orders : ContentPage	 
{
    public OrdersVM _viewmodel;
	public Orders(OrdersVM orders)
	{
		InitializeComponent();
		this.BindingContext=orders;
        _viewmodel = orders;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}