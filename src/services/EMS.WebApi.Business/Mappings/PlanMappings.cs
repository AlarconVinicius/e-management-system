using EMS.Core.Requests.Plans;
using EMS.Core.Responses;
using EMS.Core.Responses.Plans;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class PlanMappings
{
    public static PlanResponse MapPlanToPlanResponse(this Plan plan)
    {
        if (plan == null)
        {
            return null;
        }

        return new PlanResponse(plan.Id, plan.Title, plan.Subtitle, plan.Price, plan.Benefits, plan.IsActive, plan.CreatedAt, plan.UpdatedAt);
    }

    public static Plan MapPlanResponseToPlan(this PlanResponse planResponse)
    {
        if (planResponse == null)
        {
            return null;
        }

        return new Plan(planResponse.Id, planResponse.Title, planResponse.Subtitle, planResponse.Price, planResponse.Benefits, planResponse.IsActive);
    }

    public static PagedResponse<PlanResponse> MapPagedPlansToPagedResponsePlans(this PagedResult<Plan> plan)
    {
        if (plan == null)
        {
            return null;
        }

        return new PagedResponse<PlanResponse>(plan.List.Select(x => x.MapPlanToPlanResponse()).ToList(), plan.TotalResults, plan.PageIndex, plan.PageSize);
    }

    public static Plan MapCreatePlanRequestToPlan(this CreatePlanRequest planRequest)
    {
        if (planRequest == null)
        {
            return null;
        }

        return new Plan(planRequest.Title, planRequest.Subtitle, planRequest.Price, planRequest.Benefits, planRequest.IsActive);
    }
}