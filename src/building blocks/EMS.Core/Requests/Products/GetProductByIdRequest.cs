namespace EMS.Core.Requests.Products;

public class GetProductByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetProductByIdRequest()
    {

    }
    public GetProductByIdRequest(Guid id)
    {
        Id = id;
    }
}