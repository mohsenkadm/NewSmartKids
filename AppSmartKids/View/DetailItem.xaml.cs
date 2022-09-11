using AppSmartKids.Helper;
using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class DetailItem : ContentPage  
{
	public DetailItem(DetailItemVM detailItemVM)
	{
		InitializeComponent();
		this.BindingContext=detailItemVM;
	}
}