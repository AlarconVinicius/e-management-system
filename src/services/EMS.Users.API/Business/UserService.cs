using EMS.Users.API.Application.DTO;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public interface IUserService
{
    Task<ValidationResult> AddSubscriber(UserAddDto subscriber);
    Task<ValidationResult> DeleteSubscriber(Guid id);
}
public class UserService : MainService, IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(INotifier notifier, IUserRepository userRepository) : base(notifier)
    {
        _userRepository = userRepository;
    }

    public async Task<ValidationResult> AddSubscriber(UserAddDto subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        if (!subscriber.IsValid()) return subscriber.GetValidationResult();

        if (await UserExists(subscriber.Id, subscriber.Cpf)) return _validationResult;

        var subscriberMap = new User(subscriber.Id, subscriber.Name, subscriber.Email, subscriber.Cpf);
        _userRepository.Add(subscriberMap);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteSubscriber(Guid id)
    {
        var userIdExist = await _userRepository.GetById(id);
        if (userIdExist is null)
        {
            Notify("Id não encontrado.");
            return _validationResult;
        };

        _userRepository.Delete(userIdExist);

        await PersistData();

        return _validationResult;
    }

    private async Task<bool> UserExists(Guid id, string cpf)
    {
        var userIdExist = await _userRepository.GetById(id);
        var userCpfExist = await _userRepository.GetByCpf(cpf);

        if (userIdExist != null!)
        {
            Notify("Este id já está em uso.");
            return true;
        };
        if (userCpfExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private async Task<ValidationResult> PersistData()
    {
        if (!await _userRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }
}
