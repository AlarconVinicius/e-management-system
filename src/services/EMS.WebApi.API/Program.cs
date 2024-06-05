using EMS.Core.Configuration;
using EMS.WebApi.Data.Configuration;
using EMS.WebApi.Business.Configuration;
using EMS.Core.Notifications;
using EMS.WebApi.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.RegisterApiServices()
                .RegisterCoreServices()
                .RegisterDataServices(builder.Configuration)
                //.RegisterIdentityServices(builder.Configuration)
                .RegisterBusinessServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
