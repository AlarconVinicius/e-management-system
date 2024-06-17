using EMS.Core.Configuration;
using EMS.Core.Requests.Identities;
using EMS.Core.Responses;
using EMS.Core.Responses.Identities;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EMS.WebApp.MVC.Handlers;

public interface IIdentityHandler
{
    Task<CustomHttpResponse<LoginUserResponse>> LoginAsync(LoginUserRequest request);
    Task PerformLogin(LoginUserResponse response);
    Task Logout();
    Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateUserRequest request);
    Task<CustomHttpResponse> UpdateEmailAsync(UpdateUserEmailRequest request);
    Task<CustomHttpResponse> UpdatePasswordAsync(UpdateUserPasswordRequest request);
    Task<CustomHttpResponse> AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteUserRequest request);
    Task<CustomHttpResponse<UserResponse>> GetByIdAsync(GetUserByIdRequest request);
    Task<CustomHttpResponse<PagedResponse<UserResponse>>> GetAllAsync(GetAllUsersRequest request);
}

public class IdentityHandler : BaseHandler, IIdentityHandler
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authenticationService;
    private readonly string _baseUrl = "api/v1/identities/login";

    public IdentityHandler(IAspNetUser aspNetUser, HttpClient httpClient, IAuthenticationService authenticationService) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
        _authenticationService = authenticationService;
    }

    public async Task<CustomHttpResponse<LoginUserResponse>> LoginAsync(LoginUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, request);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }
        var result = await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        return result;
    }
    public async Task PerformLogin(LoginUserResponse response)
    {
        var token = GetFormatedToken(response.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", response.AccessToken));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true
        };

        await _authenticationService.SignInAsync(
            _aspNetUser.GetHttpContext(),
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
    public static JwtSecurityToken GetFormatedToken(string jwtToken)
    {
        return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
    }
    public async Task Logout()
    {
        await _authenticationService.SignOutAsync(
                _aspNetUser.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
    }

    public async Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, request);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
    }

    public Task<CustomHttpResponse> UpdateEmailAsync(UpdateUserEmailRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomHttpResponse> UpdatePasswordAsync(UpdateUserPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomHttpResponse> AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomHttpResponse> DeleteAsync(DeleteUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomHttpResponse<UserResponse>> GetByIdAsync(GetUserByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomHttpResponse<PagedResponse<UserResponse>>> GetAllAsync(GetAllUsersRequest request)
    {
        throw new NotImplementedException();
    }
}
