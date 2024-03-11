using EMS.Authentication.API.Services;
using EMS.WebAPI.Core.Authentication;
using EMS.WebAPI.Core.User;

namespace EMS.Authentication.API.Configuration;

public static class ApiConfig
{
    public static IConfigurationBuilder AddFileEnvConfig(this IConfigurationBuilder builder, IWebHostEnvironment env)
    {
        return builder
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseJwksDiscovery();

        return app;
    }
}