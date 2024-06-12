using EMS.Core.Extensions;
using EMS.Core.Notifications;
using EMS.Core.User;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Core.Configuration;
public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        return services;
    }
}