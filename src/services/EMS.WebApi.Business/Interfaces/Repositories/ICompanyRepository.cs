using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<PagedResult<Company>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
    Task Block(Guid id);
}
