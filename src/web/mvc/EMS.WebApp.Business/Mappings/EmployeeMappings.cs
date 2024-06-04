using EMS.Core.Requests.Employees;
using EMS.Core.Responses.Employees;
using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Mappings;

public static class EmployeeMappings
{
    public static EmployeeResponse MapEmployeeToEmployeeResponse(this Employee employee)
    {
        if (employee == null)
        {
            return null;
        }

        return new EmployeeResponse
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            Name = employee.Name,
            LastName = employee.LastName,
            Email = employee.Email.Address,
            PhoneNumber = employee.PhoneNumber,
            Document = employee.Document.Number,
            Salary = employee.Salary,
            Role = employee.Role.MapERoleToERoleCore(),
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    public static Employee MapEmployeeResponseToEmployee(this EmployeeResponse employeeResponse)
    {
        if (employeeResponse == null)
        {
            return null;
        }

        return new Employee(employeeResponse.Id, employeeResponse.CompanyId, employeeResponse.Name, employeeResponse.LastName, employeeResponse.Email, employeeResponse.PhoneNumber, employeeResponse.Document, employeeResponse.Role.MapERoleCoreToERole(), employeeResponse.Salary);
    }

    public static Employee MapCreateEmployeeRequestToEmployee(this CreateEmployeeRequest employeeRequest)
    {
        if (employeeRequest == null)
        {
            return null;
        }

        return new Employee(employeeRequest.CompanyId, employeeRequest.Name, employeeRequest.LastName, employeeRequest.Email, employeeRequest.PhoneNumber, employeeRequest.Document, employeeRequest.Role.MapERoleCoreToERole(), employeeRequest.Salary);
    }
}