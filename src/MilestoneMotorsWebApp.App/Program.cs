using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Mapper;
using MilestoneMotorsWebApp.App.Middleware;
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
builder.Services.AddTransient<GlobalExceptionHandler>();
builder
    .Services
    .AddAutoMapper(typeof(CarMapperProfile).Assembly, typeof(UserMapperProfile).Assembly);
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
