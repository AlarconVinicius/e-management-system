using EMS.Authentication.API.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddFileEnvConfig(builder.Environment);
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();
