using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EMS.WebApp.Business.Utils;
public interface IAspNetUser
{
    string Name { get; }
    Guid GetUserId();
    Guid GetTenantId();
    string GetUserEmail();
    bool IsAuthenticated();
    bool HasRole(string role);
    bool HasClaim(string claimType, string claimValue);
}

public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AspNetUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    private HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public string Name => HttpContext.User.Identity.Name;

    public Guid GetUserId()
    {
        if (!IsAuthenticated())
            return Guid.Empty;

        var userIdClaim = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.NameIdentifier);

        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }

    public string GetUserEmail()
    {
        if (!IsAuthenticated())
            return string.Empty;

        var emailClaim = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Email);

        return emailClaim?.Value ?? string.Empty;
    }

    public bool IsAuthenticated()
    {
        return HttpContext.User.Identity.IsAuthenticated;
    }

    public bool HasRole(string role)
    {
        return HttpContext.User.IsInRole(role);
    }

    public bool HasClaim(string claimType, string claimValue)
    {
        var claim = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(claimType);

        return claim != null && claim.Value == claimValue;
    }

    public Guid GetTenantId()
    {
        if (!IsAuthenticated())
            return Guid.Empty;

        var tenantClaim = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("Tenant");

        return tenantClaim != null ? Guid.Parse(tenantClaim.Value) : Guid.Empty;
    }

}
