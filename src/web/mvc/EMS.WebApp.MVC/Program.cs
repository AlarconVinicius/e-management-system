using EMS.Core.Configuration;
using EMS.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcConfiguration(builder.Configuration);

builder.Services.RegisterMVCServices()
                .RegisterCoreServices();

builder.Services.AddIdentityConfiguration();

var app = builder.Build();

app.UseMvcConfiguration(app.Environment, app);

//app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
