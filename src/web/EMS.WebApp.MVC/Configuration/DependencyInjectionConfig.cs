using EMS.WebApp.MVC.Business.Services.Notifications;
﻿using EMS.WebApp.MVC.Business.Utils.User;
using EMS.WebApp.MVC.Data;
using EMS.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<INotifier, Notifier>();
        
        services.AddScoped<EMSDbContext>();
    }
}