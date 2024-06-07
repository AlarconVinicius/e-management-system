using EMS.Core.Handlers;
using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.WebApp.SPA.Configuration;
using EMS.WebApp.SPA.Model;
using System.Net.Http.Json;

namespace EMS.WebApp.SPA.Handlers;

public interface IClientHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateClientRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteClientRequest request);
    Task<CustomHttpResponse<PagedResponse<ClientResponse>>> GetAllAsync(GetAllClientsRequest request);
    Task<CustomHttpResponse<ClientResponse>> GetByIdAsync(GetClientByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateClientRequest request);
}

public class ClientHandler(IHttpClientFactory httpClientFactory) : IClientHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfigurationDefault.HttpClientName);
    private readonly string _baseUrl = "api/v1/clients";
    public async Task<CustomHttpResponse<ClientResponse>> GetByIdAsync(GetClientByIdRequest request)
    {
        return await _httpClient.GetFromJsonAsync<CustomHttpResponse<ClientResponse>>($"{_baseUrl}/{request.Id}") ?? null!;
    }

    public async Task<CustomHttpResponse<PagedResponse<ClientResponse>>> GetAllAsync(GetAllClientsRequest request)
    {
        return await _httpClient.GetFromJsonAsync<CustomHttpResponse<PagedResponse<ClientResponse>>>($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}") ?? null!;
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateClientRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(_baseUrl, request);
        return await result.Content.ReadFromJsonAsync<CustomHttpResponse>() ?? null!;
    }

    public async Task<CustomHttpResponse> UpdateAsync(UpdateClientRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<CustomHttpResponse>() ?? null!;
    }

    public async Task<CustomHttpResponse> DeleteAsync(DeleteClientRequest request)
    {
        var result = await _httpClient.DeleteAsync($"{_baseUrl}/{request.Id}") ?? null!;
        return await result.Content.ReadFromJsonAsync<CustomHttpResponse>() ?? null!;
    }
}
