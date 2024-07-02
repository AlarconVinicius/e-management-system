using EMS.Core.Configuration;

namespace EMS.Core.Requests;

public abstract class PagedRequest : Request
{
    public int PageSize { get; set; } = ConfigurationDefault.DefaultPageSize;
    public int PageNumber { get; set; } = ConfigurationDefault.DefaultPageNumber;
    public string Query { get; set; } = null;
    public bool ReturnAll { get; set; }

    public PagedRequest() { }

    protected PagedRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null, bool returnAll = false)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Query = query;
        ReturnAll = returnAll;
    }
}
