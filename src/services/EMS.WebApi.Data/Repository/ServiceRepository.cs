using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApi.Data.Repository;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(EMSDbContext context) : base (context)
    {
    }

    public async Task<Service> GetByIdAsync(Guid id, Guid tenantId)
    {
        if (tenantId == Guid.Empty) return null;

        return await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == tenantId) ?? null;
    }

    public async Task<PagedResult<Service>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null)
    {
        if (tenantId == Guid.Empty) return null;

        var responseQuery = DbSet.AsNoTracking().Where(p => p.CompanyId == tenantId);

        if (!string.IsNullOrEmpty(query))
        {
            responseQuery = responseQuery.Where(p => p.Name.Contains(query));
        }
        var result = await responseQuery.OrderBy(p => p.Name)
                                       .ThenByDescending(p => p.UpdatedAt)
                                       .Skip(pageSize * (pageIndex - 1))
                                       .Take(pageSize)
                                       .ToListAsync();
        var total = await responseQuery.CountAsync();

        return new PagedResult<Service>()
        {
            List = result,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

}