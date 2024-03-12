using EMS.WebAPI.Core.Services;
using EMS.WebAPI.Core.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebAPI.Core.Utils;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddServiceNotifier(this IServiceCollection services)
    {

        services.AddScoped<INotifier, Notifier>();

        return services;
    }
}
