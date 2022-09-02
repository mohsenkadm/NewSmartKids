 
using AppSmartKid.Helper;
using AppSmartKid.Helper.IServices;
using AppSmartKid.View;
using AppSmartKid.VM;
using AppSmartKids.Helper;

namespace AppSmartKid;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // Services
        builder.Services.AddSingleton(typeof(IGetDataUrlService<Empty>),typeof(GetDataUrlService<Empty>));      

        //Views Registration
        builder.Services.AddSingleton<ChooseAge>();
        builder.Services.AddTransient<DetailItem>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Notification>();
        builder.Services.AddTransient<OrderDetail>();
        builder.Services.AddTransient<OrderSeccess>();
        builder.Services.AddTransient<Orders>();
        builder.Services.AddTransient<SendOrder>();
        builder.Services.AddTransient<ShoppingCart>();

        //View Modles 
        builder.Services.AddSingleton<ChooseAgeVM>();
        builder.Services.AddTransient<DetailItemVM>();
        builder.Services.AddTransient<MainPageVM>();
        builder.Services.AddTransient<NotificationVM>();
        builder.Services.AddTransient<OrderDetailVM>();
        builder.Services.AddTransient<OrderSeccessVM>();
        builder.Services.AddTransient<OrdersVM>();
        builder.Services.AddTransient<SendOrderVM>();
        builder.Services.AddTransient<ShoppingCartVM>();
        return builder.Build();
	}
}
