using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApp.Data.Repository;

public class ClientRepository : Repository<Client>, IClientRepository
{
    private readonly IAspNetUser _aspNetUser;
    private readonly Guid _tenantId;

    public ClientRepository(EMSDbContext context, IAspNetUser aspNetUser) : base (context)
    {
        _aspNetUser = aspNetUser;

        _tenantId = _aspNetUser.GetTenantId() != Guid.Empty ? _aspNetUser.GetTenantId() : Guid.Empty;
    }

    public override async Task<IEnumerable<Client>> SearchAsync(Expression<Func<Client, bool>> predicate)
    {
        return await Db.Users.OfType<Client>()
                             .AsNoTracking()
                             .Where(p => p.CompanyId == _tenantId)
                             .Where(predicate)
                             .ToListAsync();
    }

    public async override Task<Client> GetByIdAsync(Guid id)
    {
        return await Db.Users.OfType<Client>()
                             .Where(p => p.CompanyId == _tenantId)
                             .FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<PagedResult<Client>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null)
    {
        var clientsQuery = Db.Users.AsNoTracking().Where(p => p.CompanyId == _tenantId);
        if (!string.IsNullOrEmpty(query))
        {
            clientsQuery = clientsQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var users = await clientsQuery.OfType<Client>()
                                    .OrderBy(p => p.Name)
                                    .ThenByDescending(p => p.UpdatedAt)
                                    .Skip(pageSize * (pageIndex - 1))
                                    .Take(pageSize)
                                    .ToListAsync();
        var total = await clientsQuery.CountAsync();

        return new PagedResult<Client>()
        {
            List = users,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

}