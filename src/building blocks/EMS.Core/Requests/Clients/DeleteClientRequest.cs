namespace EMS.Core.Requests.Clients;

public class DeleteClientRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    public DeleteClientRequest() { }

    public DeleteClientRequest(Guid id)
    {
        Id = id;
    }

    public DeleteClientRequest(Guid id, Guid companyId) : base(companyId)
    {
        Id = id;
    }
}
