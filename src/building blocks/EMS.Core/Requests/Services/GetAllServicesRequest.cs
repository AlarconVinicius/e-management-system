using EMS.Core.Configuration;

namespace EMS.Core.Requests.Services;

public class GetAllServicesRequest : PagedRequest
{
    public GetAllServicesRequest() { }

    public GetAllServicesRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
