 
using AppSmartKids.VM;
using Entity.Entity;

namespace AppSmartKids.View;

public partial class PostsView : ContentPage
{
    PostsVM _viewmodel;

    public PostsView(PostsVM postsVM)
	{
		InitializeComponent();
		this.BindingContext = postsVM;
        _viewmodel = postsVM;
        _viewmodel.IsBusy = false;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.GetDataCommand.Execute(null);
    }
}