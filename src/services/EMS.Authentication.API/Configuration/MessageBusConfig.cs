using EMS.Core.Utils;
using EMS.MessageBus;
namespace EMS.Authentication.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus")); ;
    }
}