using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EMS.Core.User;
public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value!;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("email");
        return claim?.Value!;
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("JWT");
        return claim?.Value!;
    }

    public static Guid GetTenantId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("Tenant");

        return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
    }
}