using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
}
