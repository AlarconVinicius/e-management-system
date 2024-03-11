using System.Security.Claims;

namespace EMS.API.WebAPI.Core.User;

public interface IAspNetUser
{
    string Name { get; }
    Guid ObterUserId();
    string ObterUserEmail();
    string ObterUserToken();
    string ObterUserRefreshToken();
    bool EstaAutenticado();
    bool PossuiRole(string role);
    IEnumerable<Claim> ObterClaims();
    HttpContext ObterHttpContext();
}
