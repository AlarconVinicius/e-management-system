using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<Employee>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
    Task<EmployeeStatusData> GetEmployeeStatusDataAsync(Guid tenantId);
}
