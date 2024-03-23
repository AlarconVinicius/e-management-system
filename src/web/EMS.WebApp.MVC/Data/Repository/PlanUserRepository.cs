using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models.Subscription;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class PlanSubscriberRepository : IPlanSubscriberRepository
{
    private readonly EMSDbContext _context;

    public PlanSubscriberRepository(EMSDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;

    public async Task<IEnumerable<PlanSubscriber>> GetAll()
    {
        return await _context.PlanSubscribers.AsNoTracking().ToListAsync();
    }

    public async Task<PlanSubscriber> GetById(Guid id)
    {
        return await _context.PlanSubscribers.FindAsync(id) ?? null!;
    }
    public async Task<PlanSubscriber> GetByUserCpf(string cpf)
    {
        return await _context.PlanSubscribers.Include(pu => pu.Plan).FirstOrDefaultAsync(pu => pu.UserCpf == cpf) ?? null!;
    }

    public void AddPlanUser(PlanSubscriber produto)
    {
        _context.PlanSubscribers.Add(produto);
    }

    public void UpdatePlanUser(PlanSubscriber produto)
    {
        _context.PlanSubscribers.Update(produto);
    }

    public async Task EnablePlanUser(Guid id, bool enable)
    {
        var planDb = await GetById(id);
        if (enable)
        {
            planDb.IsActive = true;
        }
        _context.PlanSubscribers.Update(planDb);
    }

    public async Task DisablePlanUser(Guid id, bool disable)
    {
        var planDb = await GetById(id);
        if (disable)
        {
            planDb.IsActive = false;
        }
        _context.PlanSubscribers.Update(planDb);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}