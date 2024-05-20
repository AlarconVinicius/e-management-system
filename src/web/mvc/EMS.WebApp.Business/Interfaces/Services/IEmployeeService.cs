using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Services;

public interface IEmployeeService
{
    Task Add(Employee employee);
    Task Update(Employee employee);
    Task Delete(Guid id);
}
