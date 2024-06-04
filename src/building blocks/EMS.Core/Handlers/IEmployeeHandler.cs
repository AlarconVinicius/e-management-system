using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;

namespace EMS.Core.Handlers;
public interface IEmployeeHandler
{
    Task<Response<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request);
    Task<Response<EmployeeResponse>> UpdateAsync(UpdateEmployeeRequest request);
    Task<Response<EmployeeResponse>> DeleteAsync(DeleteEmployeeRequest request);
    Task<Response<EmployeeResponse>> GetByIdAsync(GetEmployeeByIdRequest request);
    Task<PagedResponse<List<EmployeeResponse>>> GetAllAsync(GetAllEmployeesRequest request);
}