using EMS.WebApp.Business.Configuration;
using EMS.WebApp.Data.Configuration;
using EMS.WebApp.Identity.Configuration;
using EMS.WebApp.MVC.Configuration;
using EMS.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcConfiguration(builder.Configuration);

builder.Services.RegisterMVCServices()
                .RegisterCoreServices()
                .RegisterDataServices(builder.Configuration)
                .RegisterIdentityServices(builder.Configuration)
                .RegisterBusinessServices();

var app = builder.Build();

app.UseMvcConfiguration(app.Environment, app);

app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
