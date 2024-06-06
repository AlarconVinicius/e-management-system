using EMS.Core.Handlers;
using EMS.WebApi.Business.Handlers;
using EMS.WebApi.Business.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApi.Business.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IEmployeeHandler, EmployeeHandler>();

        return services;
    }
}