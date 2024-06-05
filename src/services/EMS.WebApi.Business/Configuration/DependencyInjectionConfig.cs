using EMS.Core.Handlers;
using EMS.WebApi.Business.Handlers;
using EMS.WebApi.Business.Interfaces.Services;
using EMS.WebApi.Business.Notifications;
using EMS.WebApi.Business.Services;
using EMS.WebApi.Business.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApi.Business.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAspNetUser, AspNetUser>();

        //services.AddScoped<IClientService, ClientService>();
        //services.AddScoped<IEmployeeService, EmployeeService>();
        //services.AddScoped<ICompanyService, CompanyService>();
        //services.AddScoped<IClientHandler, ClientHandler>();
        //services.AddScoped<IEmployeeHandler, EmployeeHandler>();
        services.AddScoped<IEmployeeHandler2, EmployeeHandler2>();
        //services.AddScoped<IProductHandler, ProductHandler>();

        return services;
    }
}