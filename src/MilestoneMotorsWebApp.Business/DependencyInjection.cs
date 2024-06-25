using Microsoft.Extensions.DependencyInjection;

namespace MilestoneMotorsWebApp.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediatrInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly)
            );

            return services;
        }
    }
}
