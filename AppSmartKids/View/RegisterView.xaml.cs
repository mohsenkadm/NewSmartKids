using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterVM registerVM)
	{
		InitializeComponent();
		this.BindingContext = registerVM;   
    }
}