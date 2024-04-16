using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class ClientRepository : IClientRepository
{
    private readonly EMSDbContext _context;
    private readonly IAspNetUser _aspNetUser;

    public ClientRepository(EMSDbContext context, IAspNetUser aspNetUser)
    {
        _context = context;
        _aspNetUser = aspNetUser;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<PagedResult<Client>> GetAllClients(int pageSize, int pageIndex, string query = null)
    {
        var tenantId = _aspNetUser.GetTenantId();
        if (tenantId == Guid.Empty)
        {
            return null;
        }
        var clientsQuery = _context.Clients
            .AsNoTracking();
        if (!string.IsNullOrEmpty(query))
        {
            clientsQuery = clientsQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var clients = await clientsQuery.Where(p => p.TenantId == tenantId)
                                     .OrderBy(p => p.Name)
                                     .ThenByDescending(p => p.UpdatedAt)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize)
                                     .ToListAsync();
        var total = await clientsQuery.CountAsync();

        return new PagedResult<Client>()
        {
            List = clients,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task<Client> GetById(Guid id)
    {
        var tenantId = _aspNetUser.GetTenantId();
        if (tenantId == Guid.Empty)
        {
            return null;
        }
        return await _context.Clients.Where(p => p.TenantId == tenantId).FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public void AddClient(Client client)
    {
        _context.Clients.Add(client);
    }

    public void UpdateClient(Client client)
    {
        client.UpdateEntityDate();
        _context.Clients.Update(client);
    }

    public async Task<bool> DeleteClient(Client client)
    {
        var clientDb = await _context.Clients
            .FirstOrDefaultAsync(s => s.Id == client.Id);

        if (client == null!)
        {
            return false;
        }
        _context.Clients.Remove(client);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
