using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Services;

public interface IProductService
{
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(Guid id);
}
