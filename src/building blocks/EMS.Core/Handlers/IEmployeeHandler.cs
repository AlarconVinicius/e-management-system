using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;

namespace EMS.Core.Handlers;

public interface IEmployeeHandler
{
    Task CreateAsync(CreateEmployeeRequest request);
    Task UpdateAsync(UpdateEmployeeRequest request);
    Task DeleteAsync(DeleteEmployeeRequest request);
    Task<EmployeeResponse> GetByIdAsync(GetEmployeeByIdRequest request);
    Task<PagedResponse<EmployeeResponse>> GetAllAsync(GetAllEmployeesRequest request);
}