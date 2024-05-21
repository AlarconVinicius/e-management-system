using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Data.Context;

namespace EMS.WebApp.Data.Repository;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
    public PlanRepository(EMSDbContext context) : base(context)
    {
    }

    public async Task EnablePlan(Guid id, bool enable)
    {
        var planDb = await GetByIdAsync(id);
        if (enable)
        {
            planDb.IsActive = true;
        }
        Db.Plans.Update(planDb);
    }

    public async Task DisablePlan(Guid id, bool disable)
    {
        var planDb = await GetByIdAsync(id);
        if (disable)
        {
            planDb.IsActive = false;
        }
        Db.Plans.Update(planDb);
    }
}