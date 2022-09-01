namespace AppSmartKid;

public partial class AppShell : Shell
{
	public Dictionary<string,Type> Routes { get; set; }=new Dictionary<string,Type>();
	public AppShell()
	{
		InitializeComponent();
		Routes.Add(nameof(MainPage), typeof(MainPage));
		foreach (var item in Routes)
			Routing.RegisterRoute(item.Key,item.Value);
	}
}
