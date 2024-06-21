using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.Data.Repository;

public class ServiceAppointmentRepository : Repository<ServiceAppointment>, IServiceAppointmentRepository
{
    public ServiceAppointmentRepository(EMSDbContext context) : base (context)
    {
    }

    public override async Task<IEnumerable<ServiceAppointment>> SearchAsync(Expression<Func<ServiceAppointment, bool>> predicate)
    {
        return await DbSet.AsNoTracking()
                          .Include(sa => sa.Employee)
                          .Include(sa => sa.Client)
                          .Include(sa => sa.Service)
                          .Where(predicate)
                          .ToListAsync();
    }

    public async Task<ServiceAppointment> GetByIdAsync(Guid id, Guid tenantId)
    {
        if (tenantId == Guid.Empty) return null;

        return await DbSet
                          .Include(sa => sa.Employee)
                          .Include(sa => sa.Client)
                          .Include(sa => sa.Service)
                          .FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == tenantId) ?? null;
    }

    public async Task<PagedResult<ServiceAppointment>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null)
    {
        if (tenantId == Guid.Empty) return null;

        var responseQuery = DbSet.AsNoTracking().Where(p => p.CompanyId == tenantId);

        if (!string.IsNullOrEmpty(query))
        {
            responseQuery = responseQuery.Where(p => p.Employee.Name.Contains(query) || p.Employee.LastName.Contains(query) || p.Client.Name.Contains(query) || p.Client.LastName.Contains(query) || p.Service.Name.Contains(query));
        }
        var result = await responseQuery.Include(sa => sa.Employee)
                                        .Include(sa => sa.Client)
                                        .Include(sa => sa.Service)
                                        .OrderBy(p => p.AppointmentStart)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize)
                                        .ToListAsync();
        var total = await responseQuery.CountAsync();

        return new PagedResult<ServiceAppointment>()
        {
            List = result,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

}