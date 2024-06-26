﻿using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Data.Context;
using EMS.WebApi.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.WebApi.Data.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EMSDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection String is not found")));

        services.AddScoped<EMSDbContext>();

        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceAppointmentRepository, ServiceAppointmentRepository>();

        return services;
    }
}