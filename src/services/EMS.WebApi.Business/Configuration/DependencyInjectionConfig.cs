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

        services.AddScoped<ICompanyHandler, CompanyHandler>();
        services.AddScoped<IEmployeeHandler, EmployeeHandler>();
        services.AddScoped<IClientHandler, ClientHandler>();
        services.AddScoped<IProductHandler, ProductHandler>();

        return services;
    }
}