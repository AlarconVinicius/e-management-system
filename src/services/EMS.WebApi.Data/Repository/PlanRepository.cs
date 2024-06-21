using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApi.Data.Repository;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
    public PlanRepository(EMSDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Plan>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null)
    {
        var responseQuery = DbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(query))
        {
            responseQuery = responseQuery.Where(p => p.Title.Contains(query) || p.Subtitle.Contains(query) || p.Benefits.Contains(query));
        }
        var result = await responseQuery.OrderBy(p => p.Price)
                                        .ThenByDescending(p => p.UpdatedAt)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize)
                                        .ToListAsync();
        var total = await responseQuery.CountAsync();

        return new PagedResult<Plan>()
        {
            List = result,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
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