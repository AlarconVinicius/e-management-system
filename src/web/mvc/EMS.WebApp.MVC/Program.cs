using EMS.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcConfiguration(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguration(app.Environment, app);

app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
