 
using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using AppSmartKids.View;
using AppSmartKids.VM;
using AppSmartKids.Helper;
using AppSmartKids.View;
using AppSmartKids.VM;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace AppSmartKids;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // Services
       // builder.Services.AddSingleton(typeof(IGetDataUrlService<Empty>),typeof(GetDataUrlService<Empty>));      

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
        builder.Services.AddTransient<ProductsView>();
        builder.Services.AddTransient<PostsView>();
        builder.Services.AddTransient<OrderSeccess>();
        builder.Services.AddTransient<CategoriesView>();
        builder.Services.AddTransient<ChatView>();
        builder.Services.AddTransient<RegisterView>();

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
        builder.Services.AddTransient<ProductsVM>();
        builder.Services.AddTransient<PostsVM>();
        builder.Services.AddTransient<CategoriesVM>();
        builder.Services.AddTransient<ChatVM>();
        builder.Services.AddTransient<RegisterVM>();
        return builder.Build();
	}
}
