using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Http;

namespace EMS.WebApp.SPA.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterSPAServices(this IServiceCollection services)
    {
        services.AddTransient<IIdentityHandler, IdentityHandler>();
        services.AddTransient<IClientHandler, ClientHandler>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}