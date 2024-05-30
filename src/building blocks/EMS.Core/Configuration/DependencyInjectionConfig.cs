using EMS.Core.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Core.Configuration;
public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

        return services;
    }
}