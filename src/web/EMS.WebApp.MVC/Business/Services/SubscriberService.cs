using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.Users;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class SubscriberService : MainService, ISubscriberService
{
    public readonly ISubscriberRepository _subscriberRepository;

    public SubscriberService(INotifier notifier, ISubscriberRepository subscriberRepository) : base(notifier)
    {
        _subscriberRepository = subscriberRepository;
    }

    public async Task<ValidationResult> AddSubscriber(Guid id, RegisterUser subscriber)
    {
        if (await SubscriberExists(subscriber.Cpf)) return _validationResult;
        _subscriberRepository.AddSubscriber(new Subscriber(id, subscriber.Name, subscriber.Email, subscriber.Cpf));
        return _validationResult;
    }

    public async Task<ValidationResult> UpdateSubscriber(Guid id, UpdateUserViewModel subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        var subscriberDb = await _subscriberRepository.GetById(subscriber.Id);

        subscriberDb.ChangeName(subscriber.Name);
        subscriberDb.ChangeEmail(subscriber.Email);

        _subscriberRepository.UpdateSubscriber(subscriberDb);
        return _validationResult;
    }

    private async Task<bool> SubscriberExists(string cpf)
    {
        var userExist = await _subscriberRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
}
