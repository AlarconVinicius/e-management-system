using EMS.Core.Configuration;
using System.Text.Json.Serialization;

namespace EMS.Core.Responses;

public class Response<TData>
{
    private int _code = ConfigurationDefault.DefaultStatusCode;

    [JsonConstructor]
    public Response()
        => _code = ConfigurationDefault.DefaultStatusCode;

    public Response(TData data, int code = ConfigurationDefault.DefaultStatusCode, string message = null)
    {
        Data = data;
        _code = code;
        Message = message;
    }

    public TData Data { get; set; }
    public string Message { get; set; }

    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;
}
