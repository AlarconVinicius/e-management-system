using EMS.Core.Configuration;
using EMS.WebApi.API.Configuration;
using EMS.WebApi.Business.Configuration;
using EMS.WebApi.Data.Configuration;
using EMS.WebApi.Identity.Configuration;
using EMS.WebApi.DataSeeder.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig();

builder.Services.RegisterApiServices()
                .RegisterCoreServices()
                .RegisterDataServices(builder.Configuration)
                .RegisterIdentityServices(builder.Configuration)
                .RegisterBusinessServices()
                .RegisterDataSeederServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseApiConfig(app.Environment);

app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
