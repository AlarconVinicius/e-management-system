namespace EMS.Core.Requests.Employees;

public class DeleteEmployeeRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    public DeleteEmployeeRequest()
    {
        
    }
    public DeleteEmployeeRequest(Guid id, Guid companyId) : base(companyId)
    {
        Id = id;
    }
}
