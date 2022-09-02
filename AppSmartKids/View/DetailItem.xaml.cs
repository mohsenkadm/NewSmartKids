using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class DetailItem : ContentPage  
{
	public DetailItem(DetailItemVM detailItemVM)
	{
		InitializeComponent();
		this.BindingContext=detailItemVM;
	}
}