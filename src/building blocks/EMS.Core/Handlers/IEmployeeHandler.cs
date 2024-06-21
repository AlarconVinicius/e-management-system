using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;

namespace EMS.Core.Handlers;

public interface IEmployeeHandler
{
    Task CreateAsync(CreateEmployeeRequest request);
    Task CreateAsync(CreateEmployeeAndUserRequest request);
    Task UpdateAsync(UpdateEmployeeRequest request);
    Task UpdateAsync(UpdateEmployeeAndUserRequest request);
    Task DeleteAsync(DeleteEmployeeRequest request);
    Task<EmployeeResponse> GetByIdAsync(GetEmployeeByIdRequest request);
    Task<PagedResponse<EmployeeResponse>> GetAllAsync(GetAllEmployeesRequest request);
}