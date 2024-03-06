using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using MilestoneMotorsWebApp.Infrastructure.Repositories;

namespace MilestoneMotorsWebApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureInjection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DbConnect");

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICarsRepository, CarsRepository>();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            return services;
        }
    }
}
