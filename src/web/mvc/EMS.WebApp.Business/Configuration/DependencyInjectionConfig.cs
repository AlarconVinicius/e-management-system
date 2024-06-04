using EMS.Core.Handlers;
using EMS.WebApp.Business.Handlers;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Services;
using EMS.WebApp.Business.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApp.Business.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IClientHandler, ClientHandler>();
        services.AddScoped<IProductHandler, ProductHandler>();

        return services;
    }
}