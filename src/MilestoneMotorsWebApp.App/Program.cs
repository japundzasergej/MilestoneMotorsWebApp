using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Services;
using MilestoneMotorsWebApp.Business.Utilities;

var builder = WebApplication.CreateBuilder(args);
StaticDetails.ApiBase = builder.Configuration["ApiUrl"];
var uriBuilder = new UriBuilder(StaticDetails.ApiBase);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IMvcMapperService, MvcMapperService>();
builder
    .Services
    .AddHttpClient<ICarService, CarService>(client =>
    {
        client.BaseAddress = uriBuilder.Uri.ExtendPath("cars");
    });
builder
    .Services
    .AddHttpClient<IUserService, UserService>(client =>
    {
        client.BaseAddress = uriBuilder.Uri.ExtendPath("user");
    });
builder
    .Services
    .AddHttpClient<IAccountService, AccountService>(client =>
    {
        client.BaseAddress = uriBuilder.Uri.ExtendPath("account");
    });

builder.Services.AddScoped<JwtSessionAuthenticationAttribute>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
