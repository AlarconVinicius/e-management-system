using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.Core.User;
using EMS.WebApp.MVC.Configuration;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface IClientHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateClientRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteClientRequest request);
    Task<CustomHttpResponse<PagedResponse<ClientResponse>>> GetAllAsync(GetAllClientsRequest request);
    Task<CustomHttpResponse<ClientResponse>> GetByIdAsync(GetClientByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateClientRequest request);
}

public class ClientHandler(IAspNetUser aspNetUser, IHttpClientFactory httpClientFactory) : BaseHandler(aspNetUser), IClientHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfigurationDefault.HttpClientName);
    private readonly string _baseUrl = "api/v1/clients";

    public async Task<CustomHttpResponse<ClientResponse>> GetByIdAsync(GetClientByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<ClientResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<ClientResponse>>> GetAllAsync(GetAllClientsRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<ClientResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateClientRequest request)
    {
        var content = GetContent(request);

        var response = await _httpClient.PostAsJsonAsync(_baseUrl, content);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse>(response);
    }

    public async Task<CustomHttpResponse> UpdateAsync(UpdateClientRequest request)
    {
        var content = GetContent(request);

        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{request.Id}", content);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse>(response);
    }

    public async Task<CustomHttpResponse> DeleteAsync(DeleteClientRequest request)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{request.Id}");

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return await DeserializeResponseObject<CustomHttpResponse>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse>(response);
    }
}
