using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IPlanRepository : IRepository<Plan>
{
    Task DisablePlan(Guid id, bool disable);
    Task EnablePlan(Guid id, bool enable);
}