using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant> AddTenant();
    Task<bool> DeleteTenant(Guid id);
    Task<IEnumerable<Tenant>> GetAllTenants();
    Task<Tenant> GetTenantById(Guid id);
    Task<bool> Block(Guid id);
}
