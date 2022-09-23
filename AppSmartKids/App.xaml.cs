using AppSmartKids.Helper;

namespace AppSmartKids;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        InfoAccess.Id = 0;
		try
		{
			int id=	Preferences.Default.Get<int>("Id",0);
			if (id != null)
			{
				InfoAccess.Id = id;   
            }
		}
		catch(Exception ex)
		{
			InfoAccess.Id = 0;

        }
        MainPage = new AppShell();
	}
}
