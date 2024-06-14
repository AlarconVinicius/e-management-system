using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Plans;
using EMS.Core.Responses;
using EMS.Core.Responses.Plans;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;
using EMS.WebApi.Business.Utils;

namespace EMS.WebApi.Business.Handlers;

public class PlanHandler : BaseHandler, IPlanHandler
{
    public readonly IPlanRepository _planRepository;
    public readonly ICompanyRepository _companyRepository;

    public PlanHandler(INotifier notifier, IAspNetUser aspNetUser, IPlanRepository planRepository, ICompanyRepository companyRepository) : base(notifier, aspNetUser)
    {
        _planRepository = planRepository;
        _companyRepository = companyRepository;
    }

    public async Task<PlanResponse> GetByIdAsync(GetPlanByIdRequest request)
    {
        try
        {
            var plan = await _planRepository.GetByIdAsync(request.Id);

            if (plan is null)
            {
                Notify("Plano não encontrado.");
                return null;
            }
            return plan.MapPlanToPlanResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o plano.");
            return null;
        }
    }

    public async Task<PagedResponse<PlanResponse>> GetAllAsync(GetAllPlansRequest request)
    {
        try
        {
            return (await _planRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, request.Query)).MapPagedPlansToPagedResponsePlans();
        }
        catch
        {
            Notify("Não foi possível consultar os planos.");
            return null;
        }
    }

    public async Task CreateAsync(CreatePlanRequest request)
    {
        //if (!ExecuteValidation(new PlanValidation(), plan)) return;
        var planMapped = request.MapCreatePlanRequestToPlan();
        try
        {
            await _planRepository.AddAsync(planMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o plano.");
            return;
        }
    }

    public async Task UpdateAsync(UpdatePlanRequest request)
    {
        //if (!ExecuteValidation(new PlanValidation(), plan)) return;
        if (!PlanExists(request.Id)) return;
        var planDb = await _planRepository.GetByIdAsync(request.Id);

        try
        {
            planDb.SetTitle(request.Title);
            planDb.SetSubtitle(request.Subtitle);
            planDb.SetPrice(request.Price);
            planDb.SetBenefits(request.Benefits);
            planDb.SetIsActive(request.IsActive);

            await _planRepository.UpdateAsync(planDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o plano.");
            return;
        }
    }

    public async Task DeleteAsync(DeletePlanRequest request)
    {
        try
        {
            if (!PlanExists(request.Id)) return;

            await _planRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o plano.");
            return;
        }
    }

    private bool PlanExists(Guid id)
    {
        if (_planRepository.SearchAsync(f => f.Id == id).Result.Any())
        {
            return true;
        };

        Notify("Plano não encontrado.");
        return false;
    }

}
