using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class CategoriesView : ContentPage
{
    public CategoriesVM _viewmodel;
    public CategoriesView(CategoriesVM CategoriesVM)
	{
		InitializeComponent();
        this.BindingContext = CategoriesVM;
        _viewmodel = CategoriesVM;
        _viewmodel.IsBusy = false;
    }
                         
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);    
    }
}