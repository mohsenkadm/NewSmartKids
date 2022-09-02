using AppSmartKid.Helper;
using AppSmartKid.VM;

namespace AppSmartKid.View;

public partial class ChooseAge : ContentPage  
{
	public ChooseAge(ChooseAgeVM chooseAgeVM)
	{
		InitializeComponent();
		this.BindingContext = chooseAgeVM;
	}
}