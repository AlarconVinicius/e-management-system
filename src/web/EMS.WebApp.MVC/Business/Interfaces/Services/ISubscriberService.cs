using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface ISubscriberService
{
    Task<ValidationResult> AddSubscriber(Guid id, RegisterUser subscriber);
    Task<ValidationResult> UpdateSubscriber(Guid id, UpdateUserViewModel subscriber);
}
