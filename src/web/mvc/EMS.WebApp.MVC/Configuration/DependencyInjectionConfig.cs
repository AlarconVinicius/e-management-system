using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Handlers;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterMVCServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        #region HttpServices
        services.AddHttpClient<IIdentityHandler, IdentityHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        services.AddHttpClient<IPlanHandler, PlanHandler>();
        services.AddHttpClient<ICompanyHandler, CompanyHandler>();
        services.AddHttpClient<IEmployeeHandler, EmployeeHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        services.AddHttpClient<IClientHandler, ClientHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        services.AddHttpClient<IServiceHandler, ServiceHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        services.AddHttpClient<IServiceAppointmentHandler, ServiceAppointmentHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        services.AddHttpClient<IDashboardHandler, DashboardHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        #endregion

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}