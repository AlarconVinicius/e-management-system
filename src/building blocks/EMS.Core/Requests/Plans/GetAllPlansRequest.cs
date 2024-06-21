using EMS.Core.Configuration;

namespace EMS.Core.Requests.Plans;

public class GetAllPlansRequest : PagedRequest
{
    public GetAllPlansRequest() { }

    public GetAllPlansRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
