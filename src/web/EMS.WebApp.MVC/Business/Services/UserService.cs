using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class UserService : MainService, IUserService
{
    public readonly IUserRepository _subscriberRepository;

    public UserService(INotifier notifier, IUserRepository subscriberRepository) : base(notifier)
    {
        _subscriberRepository = subscriberRepository;
    }

    public async Task<ValidationResult> AddUser(Guid id, RegisterUser subscriber)
    {
        if (await UserExists(subscriber.Cpf)) return _validationResult;
        _subscriberRepository.AddUser(new User(id, subscriber.Name, "", subscriber.Email, "", subscriber.Cpf));
        return _validationResult;
    }

    public async Task<ValidationResult> UpdateUser(Guid id, UpdateUserViewModel subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        var subscriberDb = await _subscriberRepository.GetById(subscriber.Id);

        subscriberDb.ChangeName(subscriber.Name);
        subscriberDb.ChangeEmail(subscriber.Email);

        _subscriberRepository.UpdateUser(subscriberDb);
        return _validationResult;
    }

    private async Task<bool> UserExists(string cpf)
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
