using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Services;
using MilestoneMotorsWebApp.Common;
using MilestoneMotorsWebApp.Infrastructure;

namespace MilestoneMotorsWebApp.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppInjection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddMapperInjection();
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly)
            );

            services.AddInfrastructureInjection(configuration);

            return services;
        }
    }
}
