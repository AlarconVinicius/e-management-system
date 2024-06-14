namespace EMS.Core.Requests.Companies;

public class DeleteCompanyRequest : Request
{
    public Guid Id { get; set; }

    public DeleteCompanyRequest() { }

    public DeleteCompanyRequest(Guid id)
    {
        Id = id;
    }
}
