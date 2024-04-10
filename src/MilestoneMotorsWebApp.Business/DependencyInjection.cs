using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Services;
using MilestoneMotorsWebApp.Infrastructure;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiInjection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddTransient<IMapperService, MapperService>();
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
