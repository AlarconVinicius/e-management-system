using EMS.Authentication.API.Data;
using EMS.Authentication.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Jwa;

namespace EMS.Authentication.API.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("AppTokenSettings");
        services.Configure<AppTokenSettings>(appSettingsSection);

        services.AddJwksManager(options => 
            options.Jws = Algorithm.Create(DigitalSignaturesAlgorithm.EcdsaSha256))
            .PersistKeysToDatabaseStore<ApplicationDbContext>()
            .UseJwtValidation();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}