using EMS.Core.Configuration;

namespace EMS.Core.Requests;

public abstract class PagedRequest : Request
{
    public int PageSize { get; set; } = ConfigurationDefault.DefaultPageSize;
    public int PageNumber { get; set; } = ConfigurationDefault.DefaultPageNumber;
    public string Query { get; set; } = null;
}
