using AppSmartKids.Helper;
using AppSmartKids.View;
using AppSmartKids.View;
using AppSmartKids.VM;

namespace AppSmartKids;

public partial class AppShell : Shell 
{
	public Dictionary<string,Type> Routes { get; set; }=new Dictionary<string,Type>();
	public AppShell( )
	{
		InitializeComponent();
		this.BindingContext = new AppShellVM();
		Routes.Add(nameof(MainPage), typeof(MainPage));
		Routes.Add(nameof(ChooseAge), typeof(ChooseAge));
		Routes.Add(nameof(DetailItem), typeof(DetailItem));
		Routes.Add(nameof(Notification), typeof(Notification));
		Routes.Add(nameof(OrderDetail), typeof(OrderDetail));
		Routes.Add(nameof(Orders), typeof(Orders));
		Routes.Add(nameof(OrderSeccess), typeof(OrderSeccess));
		Routes.Add(nameof(SendOrder), typeof(SendOrder));
		Routes.Add(nameof(ShoppingCart), typeof(ShoppingCart));
		Routes.Add(nameof(ProductsView), typeof(ProductsView)); 
		foreach (var item in Routes)
			Routing.RegisterRoute(item.Key,item.Value);
	}
}
