using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Services;
using MilestoneMotorsWebApp.Infrastructure;

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

        public static IServiceCollection AddAuthInjection(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JwtSettings:Issuer"],
                        ValidAudience = config["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["JwtSettings:Key"])
                        )
                    };
                });
            return services;
        }
    }
}
