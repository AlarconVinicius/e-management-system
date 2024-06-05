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

        return await DbSet.Where(p => p.CompanyId == tenantId)
                          .FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<PagedResult<Employee>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null)
    {
        if (tenantId == Guid.Empty) return null;

        var employeesQuery = DbSet.AsNoTracking().Where(p => p.CompanyId == tenantId);

        if (!string.IsNullOrEmpty(query))
        {
            employeesQuery = employeesQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var users = await employeesQuery.OrderBy(p => p.Name)
                                        .ThenByDescending(p => p.UpdatedAt)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize)
                                        .ToListAsync();
        var total = await employeesQuery.CountAsync();

        return new PagedResult<Employee>()
        {
            List = users,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

}