using EMS.Core.Requests.Identities;

namespace EMS.Core.Requests.Employees;

public class CreateEmployeeAndUserRequest : Request
{
    public CreateUserRequest User { get; set; }
    public CreateEmployeeRequest Employee { get; set; }

    public CreateEmployeeAndUserRequest() { }

    public CreateEmployeeAndUserRequest(CreateUserRequest user, CreateEmployeeRequest employee)
    {
        User = user;
        Employee = employee;
    }
}