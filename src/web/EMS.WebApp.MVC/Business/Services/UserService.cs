﻿using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using EMS.WebApp.MVC.Business.Utils.User;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class UserService : MainService, IUserService
{
    public readonly IUserRepository _userRepository;
    public readonly IAuthService _authService;
    public readonly IAspNetUser _aspNetUser;

    public UserService(INotifier notifier, IUserRepository userRepository, IAuthService authService, IAspNetUser aspNetUser) : base(notifier)
    {
        _userRepository = userRepository;
        _authService = authService;
        _aspNetUser = aspNetUser;
    }

    public async Task<ValidationResult> AddUser(UserViewModel user)
    {
        if (await IsCpfInUse(user.Cpf)) return _validationResult;
        _userRepository.AddUser(new User(user.Id, user.CompanyId, user.TenantId, user.Name, user.LastName, user.Email, user.PhoneNumber, user.Cpf, user.Role));
        return _validationResult;
    }

    public async Task<ValidationResult> UpdateUser(Guid id, UpdateUserViewModel updateUserVM)
    {

        if (!await IdentityUserExists(id.ToString())) return _validationResult;
        if (!await UserExists(id)) return _validationResult;

        var userDb = await _userRepository.GetById(updateUserVM.Id);
        var identityUserDb = await _authService.GetUserById(updateUserVM.Id.ToString());

        if (id != updateUserVM.Id && userDb.Role != "Admin")
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }


        if (updateUserVM.Email != userDb.Email.Address || updateUserVM.Email != identityUserDb.Email)
        {
            var updateIdentityEmailResult = await _authService.UpdateUserEmail(id.ToString(), updateUserVM.Email);
            if (!updateIdentityEmailResult.IsValid)
            {
                Notify(updateIdentityEmailResult);
                return _validationResult;
            }
            userDb.SetEmail(updateUserVM.Email);
        }

        userDb.SetName(updateUserVM.Name);
        userDb.SetLastName(updateUserVM.LastName);
        userDb.SetPhoneNumber(updateUserVM.PhoneNumber);

        _userRepository.UpdateUser(userDb);
        await _userRepository.UnitOfWork.Commit();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteUser(Guid userId)
    {
        var loggedId = _aspNetUser.GetUserId();

        if (!await IdentityUserExists(userId.ToString())) return _validationResult;
        if (!await UserExists(userId)) return _validationResult;

        var userDb = await _userRepository.GetById(userId);

        if (loggedId != userId && !_aspNetUser.HasRole("Admin"))
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }

        if (userDb.Role == "Admin" || userDb.Role == "Master")
        {
            Notify("Você não tem permissão para excluir este usuário.");
            return _validationResult;
        }

        var deleteIdentityResult = await _authService.DeleteUser(userId.ToString());
        if (!deleteIdentityResult.IsValid)
        {
            Notify(deleteIdentityResult);
            return _validationResult;
        }
        await _userRepository.DeleteUser(userDb);
        await _userRepository.UnitOfWork.Commit();

        return _validationResult;
    }

    private async Task<bool> IsCpfInUse(string cpf)
    {
        var userExist = await _userRepository.GetByCpf(cpf);

        if (userExist != null)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }

    private async Task<bool> UserExists(Guid userId)
    {
        var userExist = await _userRepository.GetById(userId);

        if (userExist != null)
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }

    private async Task<bool> IdentityUserExists(string userId)
    {
        var identityUserExists = await _authService.GetUserById(userId);

        if (identityUserExists != null)
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }
}
