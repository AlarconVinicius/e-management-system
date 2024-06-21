using EMS.Core.Configuration;

namespace EMS.Core.Requests.Companies;

public class GetAllCompaniesRequest : PagedRequest
{
    public GetAllCompaniesRequest() { }

    public GetAllCompaniesRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
