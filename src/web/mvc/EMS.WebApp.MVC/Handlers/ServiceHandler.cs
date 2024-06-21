using EMS.Core.Configuration;
using EMS.Core.Requests.Services;
using EMS.Core.Requests.Services;
using EMS.Core.Responses;
using EMS.Core.Responses.Services;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface IServiceHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateServiceRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteServiceRequest request);
    Task<CustomHttpResponse<PagedResponse<ServiceResponse>>> GetAllAsync(GetAllServicesRequest request);
    Task<CustomHttpResponse<ServiceResponse>> GetByIdAsync(GetServiceByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateServiceRequest request);
}

public class ServiceHandler : BaseHandler, IServiceHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/services";

    public ServiceHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<ServiceResponse>> GetByIdAsync(GetServiceByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<ServiceResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<ServiceResponse>>> GetAllAsync(GetAllServicesRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<ServiceResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateServiceRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, request);

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

    public async Task<CustomHttpResponse> UpdateAsync(UpdateServiceRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{request.Id}", request);

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

    public async Task<CustomHttpResponse> DeleteAsync(DeleteServiceRequest request)
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
