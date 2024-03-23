using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models.Subscription;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class PlanRepository : IPlanRepository
{
    private readonly EMSDbContext _context;

    public PlanRepository(EMSDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;

    public async Task<IEnumerable<Plan>> GetAll()
    {
        return await _context.Plans.AsNoTracking().ToListAsync();
    }

    public async Task<Plan> GetById(Guid id)
    {
        return await _context.Plans.FindAsync(id) ?? null;
    }

    public void AddPlan(Plan plan)
    {
        _context.Plans.Add(plan);
    }

    public void UpdatePlan(Plan plan)
    {
        _context.Plans.Update(plan);
    }

    public async Task EnablePlan(Guid id, bool enable)
    {
        var planDb = await GetById(id);
        if (enable)
        {
            planDb.IsActive = true;
        }
        _context.Plans.Update(planDb);
    }

    public async Task DisablePlan(Guid id, bool disable)
    {
        var planDb = await GetById(id);
        if (disable)
        {
            planDb.IsActive = false;
        }
        _context.Plans.Update(planDb);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}