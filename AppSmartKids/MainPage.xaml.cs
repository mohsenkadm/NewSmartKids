using AppSmartKids.Helper;
using AppSmartKids.View;
using AppSmartKids.VM;

namespace AppSmartKids;

public partial class MainPage : ContentPage 
{

	public MainPageVM _viewmodel;

	public MainPage(MainPageVM mainPageVM)
	{
		InitializeComponent();
		this.BindingContext=mainPageVM;
		_viewmodel=mainPageVM;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
       _viewmodel.loadcarousel();
    }

	private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{					    
        await AppShell.Current.GoToAsync(nameof(ShoppingCart), true);
    }

	private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
	{
        await AppShell.Current.GoToAsync(nameof(CategoriesView), true);
    }

	private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
	{
        await AppShell.Current.GoToAsync(nameof(PostsView), true);
    }

	private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
	{

        await AppShell.Current.GoToAsync(nameof(ChatView), true);
    }

	private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
	{
        await AppShell.Current.GoToAsync(nameof(ChatView), true);
    }

	private async void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
	{			 
        await AppShell.Current.GoToAsync(nameof(Notification), true);
    }
}

