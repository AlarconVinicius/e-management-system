using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApi.Data.Repository;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(EMSDbContext context) : base (context)
    {
    }

    public async Task<Employee> GetByIdAsync(Guid id, Guid tenantId)
    {
        if(tenantId == Guid.Empty) return null;

        return await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == tenantId) ?? null;
    }

    public async Task<PagedResult<Employee>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null)
    {
        if (tenantId == Guid.Empty) return null;

        var responseQuery = DbSet.AsNoTracking().Where(p => p.CompanyId == tenantId);

        if (!string.IsNullOrEmpty(query))
        {
            responseQuery = responseQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var result = await responseQuery.OrderBy(p => p.Name)
                                        .ThenByDescending(p => p.UpdatedAt)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize)
                                        .ToListAsync();
        var total = await responseQuery.CountAsync();

        return new PagedResult<Employee>()
        {
            List = result,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task<EmployeeStatusData> GetEmployeeStatusDataAsync(Guid tenantId)
    {
        var result = await DbSet
            .Where(sa => sa.CompanyId == tenantId)
            .GroupBy(sa => sa.IsActive)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var statusData = new EmployeeStatusData();

        foreach (var item in result)
        {
            if (item.Status)
            {
                statusData.Active = item.Count;
            }
            else
            {
                statusData.Inactive = item.Count;
            }
        }

        return statusData;
    }


}