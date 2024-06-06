using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<Client> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<Client>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
}
