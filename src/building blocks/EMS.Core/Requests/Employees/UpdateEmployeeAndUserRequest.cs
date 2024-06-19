using EMS.Core.Requests.Identities;

namespace EMS.Core.Requests.Employees;

public class UpdateEmployeeAndUserRequest : Request
{
    public UpdateUserEmailRequest User { get; set; }
    public UpdateEmployeeRequest Employee { get; set; }

    public UpdateEmployeeAndUserRequest() { }

    public UpdateEmployeeAndUserRequest(UpdateUserEmailRequest user, UpdateEmployeeRequest employee)
    {
        User = user;
        Employee = employee;
    }
}