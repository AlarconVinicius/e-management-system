namespace EMS.Core.Requests.Plans;

public class GetPlanByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetPlanByIdRequest() { }

    public GetPlanByIdRequest(Guid id)
    {
        Id = id;
    }
}