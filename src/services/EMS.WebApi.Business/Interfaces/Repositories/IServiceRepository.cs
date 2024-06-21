using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<Service> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<Service>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
}
