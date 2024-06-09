namespace EMS.Core.Requests.Clients;

public class GetClientByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetClientByIdRequest() { }

    public GetClientByIdRequest(Guid id)
    {
        Id = id;
    }
}