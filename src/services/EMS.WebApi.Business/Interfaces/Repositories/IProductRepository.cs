using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<Product>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
}
