using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
}
