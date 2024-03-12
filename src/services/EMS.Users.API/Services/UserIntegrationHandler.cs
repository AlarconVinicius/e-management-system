using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.Users.API.Application.DTO;
using EMS.Users.API.Business;
using FluentValidation.Results;

namespace EMS.Users.API.Services;

public class UserIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UserIntegrationHandler> _logger;

    public UserIntegrationHandler(
                        IServiceProvider serviceProvider,
                        IMessageBus bus,
                        ILogger<UserIntegrationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
        _logger = logger;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<RegisteredIdentityIntegrationEvent, ResponseMessage>(RegisterUser);

        _bus.AdvancedBus.Connected += OnConnect!;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void OnConnect(object s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> RegisterUser(RegisteredIdentityIntegrationEvent message)
    {
        ValidationResult result;
        var user = new UserAddDto(message.Id, message.Name, message.Email, message.Cpf);

        using (var scope = _serviceProvider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            result = await service.AddSubscriber(user);
            if (!result.IsValid)
            {
                _logger.LogInformation("Falha ao adicionar usuario!");
                return new ResponseMessage(result);
            }
            _logger.LogInformation("Usuário adicionado!");

            // Request para api de subscription

            var clientResult = await LinkUserToPlan(message);

            if (!clientResult.ValidationResult.IsValid)
            {
                _logger.LogInformation("Falha ao vincular usuario a um plano!");
                await service.DeleteSubscriber(message.Id);
                return new ResponseMessage(result);
            }
            _logger.LogInformation("Usuário vinculado!");
        }
        return new ResponseMessage(result);

    }
    private async Task<ResponseMessage> LinkUserToPlan(RegisteredIdentityIntegrationEvent message)
    {
        var linkUserToPlanEvent = new RegisteredUserIntegrationEvent(message.Id, message.Id, message.Name, message.Email, message.Cpf);

        try
        {
            return await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(linkUserToPlanEvent);
        }
        catch
        {
            throw;
        }
    }
}