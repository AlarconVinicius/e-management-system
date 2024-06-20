namespace EMS.Core.Requests.Services;

public class GetServiceByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetServiceByIdRequest() { }

    public GetServiceByIdRequest(Guid id)
    {
        Id = id;
    }
}