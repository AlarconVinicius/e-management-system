using EMS.Core.Configuration;
using System.Text.Json.Serialization;

namespace EMS.Core.Responses;

public class PagedResponse<TData>
{
    public IEnumerable<TData> List { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalResults / (double)PageSize);

    public PagedResponse() { }

    [JsonConstructor]
    public PagedResponse(IEnumerable<TData> list, int totalResults, int pageIndex = 1, int pageSize = ConfigurationDefault.DefaultPageSize)
    {
        List = list;
        TotalResults = totalResults;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

}