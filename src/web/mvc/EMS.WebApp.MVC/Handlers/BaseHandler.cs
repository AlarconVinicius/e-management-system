using System.Text.Json;
using System.Text;
using EMS.WebApp.MVC.Extensions;
using EMS.Core.User;

namespace EMS.WebApp.MVC.Handlers;

public abstract class BaseHandler(IAspNetUser aspNetUser)
{
    protected IAspNetUser _aspNetUser = aspNetUser;
    protected StringContent GetContent(object data)
    {
        return new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json");
    }

    protected async Task<T> DeserializeResponseObject<T>(HttpResponseMessage responseMessage)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options)!;
    }

    protected bool HandleErrorResponse(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpRequestException(response.StatusCode);

            case 400:
                return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}