using EMS.WebApp.MVC.Data;
using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EMS.WebApp.MVC.Configuration;

public static class WebAppConfig
{
    public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddDefaultIdentity<IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //                .AddCookie(options =>
        //                {
        //                    options.LoginPath = "/Identity/Account/Login";
        //                    options.AccessDeniedPath = "/erro/403";
        //                });
        services.AddDbContext<EMSDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
    }

    public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, WebApplication webApp)
    {
        //app.UseForwardedHeaders();

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

        app.UseAuthentication();
        app.UseAuthorization();

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
    public static void CheckAndApplyDatabaseMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var emsDbContext = scope.ServiceProvider.GetRequiredService<EMSDbContext>();
        if (identityDbContext.Database.GetPendingMigrations().Any())
        {
            identityDbContext.Database.Migrate();
        }
        if (emsDbContext.Database.GetPendingMigrations().Any())
        {
            emsDbContext.Database.Migrate();
        }

        //var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        //new ConfigureInitialAuthSeed(dbContext, userManager!).StartConfig();
        //new ConfigureInitialBlogSeed(dbContext).StartConfig();
    }
}
