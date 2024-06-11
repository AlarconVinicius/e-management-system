using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EMS.Core.User;
public interface IAspNetUser
{
    string Name { get; }
    Guid GetUserId();
    Guid GetTenantId();
    string GetUserEmail();
    string GetUserToken();
    bool IsAuthenticated();
    bool HasRole(string role);
    IEnumerable<Claim> GetClaims();
    HttpContext GetHttpContext();
}
public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private HttpContext HttpContext => _accessor.HttpContext;
    public string Name => HttpContext!.User.Identity!.Name!;

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(HttpContext!.User.GetUserId()) : Guid.Empty;
    }
    public Guid GetTenantId()
    {
        if (!IsAuthenticated())
            return Guid.Empty;

        var tenantClaim = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("Tenant");

        return tenantClaim != null ? Guid.Parse(tenantClaim.Value) : Guid.Empty;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? HttpContext!.User.GetUserEmail() : "";
    }

    public string GetUserToken()
    {
        return IsAuthenticated() ? HttpContext!.User.GetUserToken() : "";
    }

    public bool IsAuthenticated()
    {
        return HttpContext!.User.Identity!.IsAuthenticated;
    }

    public bool HasRole(string role)
    {
        return HttpContext!.User.IsInRole(role);
    }

    public IEnumerable<Claim> GetClaims()
    {
        return HttpContext!.User.Claims;
    }

    public HttpContext GetHttpContext()
    {
        return HttpContext!;
    }
}