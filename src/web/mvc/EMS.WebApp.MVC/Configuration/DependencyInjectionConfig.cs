using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterMVCServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}