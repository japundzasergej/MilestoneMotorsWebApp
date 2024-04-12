using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWebApp.Business;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Mapper;
using MilestoneMotorsWebApp.Business.Services;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using MilestoneMotorsWebApp.Infrastructure.Repositories;
using MilestoneMotorsWebApp.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
var url = builder.Configuration["JwtSettings:Audience"];
var connectionString = builder.Configuration.GetConnectionString("DbConnect");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder
    .Services
    .AddAutoMapper(typeof(CarMapperProfile).Assembly, typeof(UserMapperProfile).Assembly);
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder
    .Services
    .Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddMediatrInjection();
builder.Services.AddTransient<GlobalExceptionHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarsRepository, CarsRepository>();

builder
    .Services
    .AddDbContext<ApplicationDbContext>(
        options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("MilestoneMotorsWebApp.WebApi")
            )
    );
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<JwtMiddleware>();
builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            "AllowSpecific",
            builder =>
            {
                builder
                    .WithOrigins(url)
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
            }
        );
    });

var app = builder.Build();

if (args.Length == 1 && args[0].Equals("seeddata", StringComparison.CurrentCultureIgnoreCase))
{
    Seed.SeedData(app);
    //await Seed.SeedUsersAndRolesAsync(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
