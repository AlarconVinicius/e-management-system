using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface IProductRepository : IRepository<Product>
{
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    Task<bool> DeleteProduct(Product product);
    Task<PagedResult<Product>> GetAllProducts(int pageSize, int pageIndex, string query = null);
    Task<Product> GetById(Guid id);
}
