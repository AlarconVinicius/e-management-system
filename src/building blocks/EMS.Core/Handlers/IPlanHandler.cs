using EMS.Core.Requests.Plans;
using EMS.Core.Responses;
using EMS.Core.Responses.Plans;

namespace EMS.Core.Handlers;

public interface IPlanHandler
{
    Task CreateAsync(CreatePlanRequest request);
    Task UpdateAsync(UpdatePlanRequest request);
    Task DeleteAsync(DeletePlanRequest request);
    Task<PlanResponse> GetByIdAsync(GetPlanByIdRequest request);
    Task<PagedResponse<PlanResponse>> GetAllAsync(GetAllPlansRequest request);
}