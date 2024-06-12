using EMS.Core.User;
using System.Net.Http.Headers;

namespace EMS.WebApp.MVC.Handlers;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IAspNetUser _user;

    public HttpClientAuthorizationDelegatingHandler(IAspNetUser user) => _user = user;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _user.GetHttpContext().Request.Headers.Authorization;

        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            request.Headers.Add("Authorization", [authorizationHeader!]);
        }

        var token = _user.GetUserToken();

        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}