using EMS.Core.Configuration;

namespace EMS.Core.Requests.Identities;

public class GetAllUsersRequest : PagedRequest
{
    public GetAllUsersRequest() { }

    public GetAllUsersRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}