using EMS.Users.API.Business;
using EMS.Users.API.Data;
using EMS.Users.API.Data.Repository;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.User;
using EMS.WebAPI.Core.Utils;

namespace EMS.Users.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<UsersContext>();
        services.AddServiceNotifier();
    }
}