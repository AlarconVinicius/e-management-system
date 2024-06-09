namespace EMS.Core.Requests.Employees;

public class GetEmployeeByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetEmployeeByIdRequest() { }

    public GetEmployeeByIdRequest(Guid id)
    {
        Id = id;
    }
}