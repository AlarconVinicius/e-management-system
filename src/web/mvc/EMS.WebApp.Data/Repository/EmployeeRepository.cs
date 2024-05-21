using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApp.Data.Repository;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    private readonly IAspNetUser _aspNetUser;
    private readonly Guid _tenantId;

    public EmployeeRepository(EMSDbContext context, IAspNetUser aspNetUser) : base (context)
    {
        _aspNetUser = aspNetUser;

        _tenantId = _aspNetUser.GetTenantId() != Guid.Empty ? _aspNetUser.GetTenantId() : Guid.Empty;
    }

    public override async Task<IEnumerable<Employee>> SearchAsync(Expression<Func<Employee, bool>> predicate)
    {
        return await Db.Users.OfType<Employee>()
                             .AsNoTracking()
                             .Where(p => p.CompanyId == _tenantId)
                             .Where(predicate).ToListAsync();
    }

    public async override Task<Employee> GetByIdAsync(Guid id)
    {
        return await Db.Users.OfType<Employee>()
                             .Where(p => p.CompanyId == _tenantId)
                             .FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<PagedResult<Employee>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null)
    {
        var employeesQuery = Db.Users.AsNoTracking().Where(p => p.CompanyId == _tenantId);

        if (!string.IsNullOrEmpty(query))
        {
            employeesQuery = employeesQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var users = await employeesQuery.OfType<Employee>()
                                    .OrderBy(p => p.Name)
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