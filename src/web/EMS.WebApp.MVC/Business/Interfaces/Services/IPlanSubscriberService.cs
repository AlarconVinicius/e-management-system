using EMS.WebApp.MVC.Business.Models.Subscription;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface IPlanSubscriberService
{
    Task<ValidationResult> AddPlanSubscriber(Guid subscriberId, RegisterUser user);
    Task<IEnumerable<PlanSubscriber>> GetAllPlanUsers();
    Task<PlanSubscriber> GetByUserId(Guid userId);
}