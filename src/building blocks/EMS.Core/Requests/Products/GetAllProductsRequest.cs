using EMS.Core.Configuration;

namespace EMS.Core.Requests.Products;

public class GetAllProductsRequest : PagedRequest
{
    public GetAllProductsRequest() { }

    public GetAllProductsRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
