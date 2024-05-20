using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class TenantRepository : ITenantRepository
{
    private readonly EMSDbContext _context;

    public TenantRepository(EMSDbContext context)
    {
        _context = context;
    }
    public IUnitOfWork UnitOfWork => _context;

    public async Task<Tenant> AddTenant()
    {
        var lastTenant = await _context.Tenants
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();

        int lastTenantNumber = lastTenant != null ? ExtractTenantNumber(lastTenant.Name) : 0;
        var name = $"Tenant{lastTenantNumber + 1}";

        var tenant = new Tenant(name, true);
        _context.Tenants.Add(tenant);
        return tenant;
    }

    public async Task<bool> Block(Guid id)
    {
        var tenant = await GetTenantById(id);
        if (tenant != null)
        {
            tenant.UpdateEntityDate();
            tenant.SetIsActive(false);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteTenant(Guid id)
    {
        var tenantDb = await _context.Tenants
             .FirstOrDefaultAsync(s => s.Id == id);

        if (tenantDb == null!)
        {
            return false;
        }
        _context.Tenants.Remove(tenantDb);
        return true;
    }

    public async Task<IEnumerable<Tenant>> GetAllTenants()
    {
        return await _context.Tenants
                             .AsNoTracking()
                             .OrderBy(p => p.Name)
                             .ThenByDescending(p => p.UpdatedAt)
                             .ToListAsync();
    }

    public async Task<Tenant> GetTenantById(Guid id)
    {
        return await _context.Tenants.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }
    public void Dispose() => _context.Dispose();

    private static int ExtractTenantNumber(string name)
    {
        if (int.TryParse(name.Replace("Tenant", ""), out int number))
        {
            return number;
        }
        return 0;
    }
}
