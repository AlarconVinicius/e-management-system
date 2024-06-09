namespace EMS.Core.Requests.Identities;

public class DeleteUserRequest : Request
{
    public Guid Id { get; set; }

    public DeleteUserRequest() { }

    public DeleteUserRequest(Guid id)
    {
        Id = id;
    }
}
