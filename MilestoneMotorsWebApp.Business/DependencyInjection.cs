using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilestoneMotorsWebApp.Business.Accounts;
using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Services;
using MilestoneMotorsWebApp.Business.Users;

namespace MilestoneMotorsWebApp.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessInjection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAccountCommand, AccountCommand>();
            services.AddScoped<ICarCommand, CarCommands>();
            services.AddScoped<IUserCommand, UserCommands>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;
        }
    }
}
