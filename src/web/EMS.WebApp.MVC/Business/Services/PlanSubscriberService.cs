using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.Subscription;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class PlanSubscriberService : MainService, IPlanSubscriberService
{
    private readonly IPlanSubscriberRepository _planUserRepository;
    private readonly IPlanRepository _planRepository;
    public PlanSubscriberService(IPlanSubscriberRepository planUserRepository, IPlanRepository planRepository, INotifier notifier) : base(notifier)
    {
        _planUserRepository = planUserRepository;
        _planRepository = planRepository;
    }

    public async Task<ValidationResult> AddPlanSubscriber(Guid subscriberId, RegisterUser planSubs)
    {
        var planExist = await _planRepository.GetById(planSubs.PlanId);
        var planSubsExist = await _planUserRepository.GetByUserCpf(planSubs.Cpf);

        if (planExist is null)
        {
            Notify("Plano não encontrado.");
            return _validationResult;
        }
        if (planSubsExist is not null)
        {
            Notify($"CPF '{planSubsExist.UserCpf}' vinculado ao plano {planExist!.Title}.");
            return _validationResult;
        }
        _planUserRepository.AddPlanUser(
            new PlanSubscriber(planSubs.PlanId, subscriberId, planSubs.Name, planSubs.Email, planSubs.Cpf, true)
        );
        return _validationResult;
    }

    public Task<IEnumerable<PlanSubscriber>> GetAllPlanUsers()
    {
        throw new NotImplementedException();
    }

    public Task<PlanSubscriber> GetByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}
