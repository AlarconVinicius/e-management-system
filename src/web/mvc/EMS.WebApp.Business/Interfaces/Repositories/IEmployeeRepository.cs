using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<PagedResult<Employee>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
}
