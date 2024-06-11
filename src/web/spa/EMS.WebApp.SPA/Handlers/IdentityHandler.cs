using EMS.Core.Requests.Identities;
using EMS.Core.Responses;
using EMS.Core.Responses.Identities;
using EMS.WebApp.SPA.Configuration;
using EMS.WebApp.SPA.Model;
using System.Net.Http.Json;

namespace EMS.WebApp.SPA.Handlers;

public interface IIdentityHandler
{
    Task<CustomHttpResponse<LoginUserResponse>> LoginAsync(LoginUserRequest request);
    Task Logout();
    Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateUserRequest request);
    Task<CustomHttpResponse> UpdateEmailAsync(UpdateUserEmailRequest request);
    Task<CustomHttpResponse> UpdatePasswordAsync(UpdateUserPasswordRequest request);
    Task<CustomHttpResponse> AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteUserRequest request);
    Task<CustomHttpResponse<UserResponse>> GetByIdAsync(GetUserByIdRequest request);
    Task<CustomHttpResponse<PagedResponse<UserResponse>>> GetAllAsync(GetAllUsersRequest request);
}

public class IdentityHandler(IHttpClientFactory httpClientFactory) : BaseHandler, IIdentityHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfigurationDefault.HttpClientName);
    private readonly string _baseUrl = "api/v1/identities/login";


    public async Task<CustomHttpResponse<LoginUserResponse>> LoginAsync(LoginUserRequest request)
    {
        var content = GetContent(request);

        var response = await _httpClient.PostAsJsonAsync(_baseUrl, content);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }
        var result = await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        return result;
    }
    
    public async Task Logout()
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateUserRequest request)
    {
        var content = GetContent(request);

        var response = await _httpClient.PostAsJsonAsync(_baseUrl, content);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
    }

    public async Task<CustomHttpResponse> UpdateEmailAsync(UpdateUserEmailRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse> UpdatePasswordAsync(UpdateUserPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse> AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse> DeleteAsync(DeleteUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse<UserResponse>> GetByIdAsync(GetUserByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomHttpResponse<PagedResponse<UserResponse>>> GetAllAsync(GetAllUsersRequest request)
    {
        throw new NotImplementedException();
    }
}
