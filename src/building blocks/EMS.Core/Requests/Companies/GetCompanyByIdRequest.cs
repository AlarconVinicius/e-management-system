namespace EMS.Core.Requests.Companies;

public class GetCompanyByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetCompanyByIdRequest() { }

    public GetCompanyByIdRequest(Guid id)
    {
        Id = id;
    }
}