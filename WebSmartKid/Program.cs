using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebSmartKid.Classes;
using WebSmartKid.Helper;
using WebSmartKid.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.SaveToken = true;
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new()
        {
            ValidIssuer = "School",
            ValidAudience = "Subscriber",
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromHours(10),
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Key.SecretKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<DBContext>(option => option.UseSqlServer(DBConn.ConnectionString),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();