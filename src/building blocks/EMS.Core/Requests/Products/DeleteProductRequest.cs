namespace EMS.Core.Requests.Products;

public class DeleteProductRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    public DeleteProductRequest()
    {
        
    }
    public DeleteProductRequest(Guid id, Guid companyId) : base(companyId)
    {
        Id = id;
    }
}
