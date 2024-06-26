using EMS.WebApi.DataSeeder.Seeds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApi.DataSeeder.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterDataSeederServices(this IServiceCollection services)
    {
        services.AddScoped<ISeeder, Seeder>();

        return services;
    }
}