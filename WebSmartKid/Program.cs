using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebSmartKid;
using WebSmartKid.Classes;
using WebSmartKid.Helper;
using WebSmartKid.Model;
                                  
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.WebHost.UseWebRoot("wwwroot");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.SaveToken = true;
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new()
        {
            ValidIssuer = "smartkids",
            ValidAudience = "Subscriber",
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromHours(10),
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Key.SecretKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<DB_Context>(option => option.UseSqlServer(DBConn.ConnectionString),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);
builder.Services.AddSignalR();
builder.Services.AddSession(o => {
    o.IdleTimeout = TimeSpan.FromDays(10);
});
AppRegisterServices.RegisterServices<IRegisterScopped>(builder.Services);
AppRegisterServices.RegisterServices<IRegisterSingleton>(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.MapHub<Signalr>("/Signalr");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();