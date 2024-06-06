namespace EMS.WebApi.API.Configuration;
public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}