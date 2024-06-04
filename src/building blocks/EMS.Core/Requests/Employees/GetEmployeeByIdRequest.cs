namespace EMS.Core.Requests.Employees;

public class GetEmployeeByIdRequest : Request
{
    public Guid Id { get; set; }
}