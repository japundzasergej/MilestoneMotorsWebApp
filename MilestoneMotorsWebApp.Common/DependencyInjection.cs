using Microsoft.Extensions.DependencyInjection;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Services;

namespace MilestoneMotorsWebApp.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMapperInjection(this IServiceCollection services)
        {
            services.AddScoped<IMapperService, MapperService>();
            return services;
        }
    }
}
