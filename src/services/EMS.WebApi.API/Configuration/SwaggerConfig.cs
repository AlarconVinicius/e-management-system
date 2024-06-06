using Microsoft.OpenApi.Models;

namespace EMS.WebApi.API.Configuration;
public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "E-Management System — API",
                Version = "V1",
                Description = "Backend E-Management System.",
                Contact = new OpenApiContact() { Name = "Vinícius Alarcon", Email = "alarcon.vinicius74@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Management System API V1");
        });
        return app;
    }
}