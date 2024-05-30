namespace EMS.Core.Requests.Clients;

public class GetClientByIdRequest : Request
{
    public Guid Id { get; set; }
}