using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Services;
using EMS.WebApp.MVC.Business.Services.Notifications;
using EMS.WebApp.MVC.Business.Utils.User;
using EMS.WebApp.MVC.Data;
using EMS.WebApp.MVC.Data.Repository;
using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();

        services.AddScoped<EMSDbContext>();
    }
}