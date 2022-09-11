 
using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class OrderDetail : ContentPage 
{
    public OrderDetailVM _viewmodel;
	public OrderDetail(OrderDetailVM orderDetailVM)
	{
		InitializeComponent();
		this.BindingContext=orderDetailVM;
        _viewmodel=orderDetailVM;   
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}