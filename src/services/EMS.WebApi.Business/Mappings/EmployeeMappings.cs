using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class EmployeeMappings
{
    public static EmployeeResponse MapEmployeeToEmployeeResponse(this Employee employee)
    {
        if (employee == null)
        {
            return null;
        }

        return new EmployeeResponse(employee.Id, employee.CompanyId, employee.Name, employee.LastName, employee.Email.Address, employee.PhoneNumber, employee.Document.Number, employee.Salary, employee.Role.MapERoleToERoleCore(), employee.IsActive, employee.CreatedAt, employee.UpdatedAt);
    }
    
    public static Employee MapEmployeeResponseToEmployee(this EmployeeResponse employeeResponse)
    {
        if (employeeResponse == null)
        {
            return null;
        }

        return new Employee(employeeResponse.Id, employeeResponse.CompanyId, employeeResponse.Name, employeeResponse.LastName, employeeResponse.Email, employeeResponse.PhoneNumber, employeeResponse.Document, employeeResponse.Role.MapERoleCoreToERole(), employeeResponse.Salary);
    }
    public static PagedResponse<EmployeeResponse> MapPagedEmployeesToPagedResponseEmployees(this PagedResult<Employee> employees)
    {
        if (employees == null)
        {
            return null;
        }

        return new PagedResponse<EmployeeResponse>(employees.List.Select(x => x.MapEmployeeToEmployeeResponse()).ToList(), employees.TotalResults, employees.PageIndex, employees.PageSize);
    }
    public static Employee MapCreateEmployeeRequestToEmployee(this CreateEmployeeRequest employeeRequest)
    {
        if (employeeRequest == null)
        {
            return null;
        }

        return new Employee(employeeRequest.Id, employeeRequest.CompanyId, employeeRequest.Name, employeeRequest.LastName, employeeRequest.Email, employeeRequest.PhoneNumber, employeeRequest.Document, employeeRequest.Role.MapERoleCoreToERole(), employeeRequest.Salary);
    }
}