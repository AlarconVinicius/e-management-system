using System.Net;

namespace EMS.WebApp.SPA.Model;

public class CustomHttpResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
    public object? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class CustomHttpResponse<TData> : CustomHttpResponse
{
    public new TData? Data { get; set; }
}