using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface IPlanRepository : IRepository<Plan>
{
    Task DisablePlan(Guid id, bool disable);
    Task EnablePlan(Guid id, bool enable);
}