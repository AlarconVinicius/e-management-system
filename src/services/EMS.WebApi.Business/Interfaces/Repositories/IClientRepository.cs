using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<PagedResult<Client>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
}
