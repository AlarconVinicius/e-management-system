using EMS.Core.Configuration;
using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace EMS.WebApp.MVC.Configuration;

public static class WebAppConfig
{
    public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

        services.AddHttpClient(
                    WebConfigurationDefault.HttpClientName,
                    options =>
                    {
                        options.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
                    }
                );
    }

    public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, WebApplication webApp)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityConfiguration();

        var supportedCultures = new[] { new CultureInfo("pt-BR") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
        webApp.MapRazorPages();
    }
}
