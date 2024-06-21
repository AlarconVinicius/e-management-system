using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IPlanRepository : IRepository<Plan>
{
    Task<PagedResult<Plan>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
    Task DisablePlan(Guid id, bool disable);
    Task EnablePlan(Guid id, bool enable);
}