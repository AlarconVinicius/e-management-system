using EMS.Core.Configuration;

namespace EMS.Core.Requests.Clients;

public class GetAllClientsRequest : PagedRequest
{
    public GetAllClientsRequest() { }

    public GetAllClientsRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
