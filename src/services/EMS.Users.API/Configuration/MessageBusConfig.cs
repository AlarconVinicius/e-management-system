using EMS.Core.Utils;
using EMS.MessageBus;
using EMS.Users.API.Services;

namespace EMS.Users.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<UserIntegrationHandler>();
                //.AddHostedService<UserDeletionIntegrationHandler>();
    }
}