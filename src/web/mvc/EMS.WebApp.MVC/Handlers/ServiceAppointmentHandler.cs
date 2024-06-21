using EMS.Core.Configuration;
using EMS.Core.Requests.ServiceAppointments;
using EMS.Core.Responses;
using EMS.Core.Responses.ServiceAppointments;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface IServiceAppointmentHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateServiceAppointmentRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteServiceAppointmentRequest request);
    Task<CustomHttpResponse<PagedResponse<ServiceAppointmentResponse>>> GetAllAsync(GetAllServiceAppointmentsRequest request);
    Task<CustomHttpResponse<ServiceAppointmentResponse>> GetByIdAsync(GetServiceAppointmentByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateServiceAppointmentRequest request);
}

public class ServiceAppointmentHandler : BaseHandler, IServiceAppointmentHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/service-appointments";

    public ServiceAppointmentHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<ServiceAppointmentResponse>> GetByIdAsync(GetServiceAppointmentByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<ServiceAppointmentResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<ServiceAppointmentResponse>>> GetAllAsync(GetAllServiceAppointmentsRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<ServiceAppointmentResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateServiceAppointmentRequest request)
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

    public async Task<CustomHttpResponse> UpdateAsync(UpdateServiceAppointmentRequest request)
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

    public async Task<CustomHttpResponse> DeleteAsync(DeleteServiceAppointmentRequest request)
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
