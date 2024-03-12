using EMS.Users.API.Configuration;
using EMS.WebAPI.Core.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddFileEnvConfig(builder.Environment);
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddMessageBusConfiguration(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();
