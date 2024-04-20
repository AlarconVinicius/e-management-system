using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface IUserService
{
    Task<ValidationResult> AddUser(UserViewModel user);
    Task<ValidationResult> UpdateUser(Guid id, UpdateUserViewModel updateUserVM);
    Task<ValidationResult> DeleteUser(Guid userId);
}
