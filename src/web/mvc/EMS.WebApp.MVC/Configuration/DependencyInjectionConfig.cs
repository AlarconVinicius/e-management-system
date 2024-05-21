using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Services;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.Data.Context;
using EMS.WebApp.Data.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Services;
using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<EMSDbContext>();
    }
}