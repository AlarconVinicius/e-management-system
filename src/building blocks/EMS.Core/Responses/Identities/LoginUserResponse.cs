using System.Security.Claims;

namespace EMS.Core.Responses.Identities;

public class LoginUserResponse
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; }

    public LoginUserResponse() { }

    public LoginUserResponse(string accessToken, double expiresIn, UserToken userToken)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        UserToken = userToken;
    }
}
public class UserToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserClaim> Claims { get; set; }

    public UserToken() { }

    public UserToken(string id, string email, IEnumerable<UserClaim> claims)
    {
        Id = id;
        Email = email;
        Claims = claims;
    }
}
public class UserClaim
{
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public UserClaim() { }

    public UserClaim(string type, string value)
    {
        Value = value;
        Type = type;
    }
}