using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class UserService : MainService, IUserService
{
    public readonly IUserRepository _userRepository;

    public UserService(INotifier notifier, IUserRepository userRepository) : base(notifier)
    {
        _userRepository = userRepository;
    }

    public async Task<ValidationResult> AddUser(UserViewModel user)
    {
        if (await UserExists(user.Cpf)) return _validationResult;
        _userRepository.AddUser(new User(user.Id, user.CompanyId, user.TenantId, user.Name, user.LastName, user.Email, user.PhoneNumber, user.Cpf, user.Role));
        return _validationResult;
    }

    public async Task<ValidationResult> UpdateUser(Guid id, UpdateUserViewModel subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        var subscriberDb = await _userRepository.GetById(subscriber.Id);

        subscriberDb.ChangeName(subscriber.Name);
        subscriberDb.ChangeEmail(subscriber.Email);

        _userRepository.UpdateUser(subscriberDb);
        return _validationResult;
    }

    private async Task<bool> UserExists(string cpf)
    {
        var userExist = await _userRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
}
