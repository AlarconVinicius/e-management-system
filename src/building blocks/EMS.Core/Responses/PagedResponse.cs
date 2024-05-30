using EMS.Core.Configuration;
using System.Text.Json.Serialization;

namespace EMS.Core.Responses;

public class PagedResponse<TData> : Response<TData>
{
    [JsonConstructor]
    public PagedResponse(
        TData data,
        int totalCount,
        int currentPage = 1,
        int pageSize = ConfigurationDefault.DefaultPageSize)
        : base(data)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    public PagedResponse(
        TData data,
        int code = ConfigurationDefault.DefaultStatusCode,
        string message = null)
        : base(data, code, message)
    {
    }

    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; } = ConfigurationDefault.DefaultPageSize;
    public int TotalCount { get; set; }
}