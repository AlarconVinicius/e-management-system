﻿using EMS.Core.Handlers;
using EMS.WebApi.Business.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApi.Business.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyHandler, CompanyHandler>();
        services.AddScoped<IPlanHandler, PlanHandler>();
        services.AddScoped<IEmployeeHandler, EmployeeHandler>();
        services.AddScoped<IClientHandler, ClientHandler>();
        services.AddScoped<IServiceHandler, ServiceHandler>();
        services.AddScoped<IServiceAppointmentHandler, ServiceAppointmentHandler>();
        services.AddScoped<IProductHandler, ProductHandler>();
        services.AddScoped<IDashboardHandler, DashboardHandler>();

        return services;
    }
}