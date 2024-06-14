namespace EMS.Core.Requests.Plans;

public class DeletePlanRequest : Request
{
    public Guid Id { get; set; }

    public DeletePlanRequest() { }

    public DeletePlanRequest(Guid id)
    {
        Id = id;
    }
}
