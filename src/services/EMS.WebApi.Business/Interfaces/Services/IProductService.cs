using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Services;

public interface IProductService
{
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(Guid id);
}
