namespace EMS.Core.Requests.Identities;

public class GetUserByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetUserByIdRequest() { }

    public GetUserByIdRequest(Guid id)
    {
        Id = id;
    }
}
