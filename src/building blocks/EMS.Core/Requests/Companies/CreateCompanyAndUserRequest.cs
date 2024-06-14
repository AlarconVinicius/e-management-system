using EMS.Core.Requests.Employees;
using EMS.Core.Requests.Identities;

namespace EMS.Core.Requests.Companies;

public class CreateCompanyAndUserRequest : Request
{
    public CreateCompanyRequest Company { get; set; }
    public CreateUserRequest User { get; set; }
    public CreateEmployeeRequest Employee { get; set; }

    public CreateCompanyAndUserRequest() { }

    public CreateCompanyAndUserRequest(CreateCompanyRequest company, CreateUserRequest user, CreateEmployeeRequest employee)
    {
        Company = company;
        User = user;
        Employee = employee;
    }
}