using EMS.Core.Configuration;
using EMS.Core.Requests.Plans;
using EMS.Core.Responses;
using EMS.Core.Responses.Plans;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface IPlanHandler
{
    Task<CustomHttpResponse> CreateAsync(CreatePlanRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeletePlanRequest request);
    Task<CustomHttpResponse<PagedResponse<PlanResponse>>> GetAllAsync(GetAllPlansRequest request);
    Task<CustomHttpResponse<PlanResponse>> GetByIdAsync(GetPlanByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdatePlanRequest request);
}

public class PlanHandler : BaseHandler, IPlanHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/plans";
    public PlanHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);

        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<PlanResponse>> GetByIdAsync(GetPlanByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PlanResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<PlanResponse>>> GetAllAsync(GetAllPlansRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<PlanResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreatePlanRequest request)
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

    public async Task<CustomHttpResponse> UpdateAsync(UpdatePlanRequest request)
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

    public async Task<CustomHttpResponse> DeleteAsync(DeletePlanRequest request)
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
