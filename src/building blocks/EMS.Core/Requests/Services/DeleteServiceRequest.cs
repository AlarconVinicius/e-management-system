namespace EMS.Core.Requests.Services;

public class DeleteServiceRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    public DeleteServiceRequest() { }

    public DeleteServiceRequest(Guid id)
    {
        Id = id;
    }

    public DeleteServiceRequest(Guid id, Guid companyId) : base(companyId)
    {
        Id = id;
    }
}
