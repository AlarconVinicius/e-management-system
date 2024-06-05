using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Services;

public interface IEmployeeService
{
    Task Add(Employee employee);
    Task Update(Employee employee);
    Task Delete(Guid id);
}
