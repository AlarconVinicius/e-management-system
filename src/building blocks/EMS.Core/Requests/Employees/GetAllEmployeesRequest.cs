using EMS.Core.Configuration;

namespace EMS.Core.Requests.Employees;

public class GetAllEmployeesRequest : PagedRequest
{
    public GetAllEmployeesRequest() { }

    public GetAllEmployeesRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {
        
    }
}
