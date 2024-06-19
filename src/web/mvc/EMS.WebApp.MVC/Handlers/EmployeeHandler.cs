using EMS.Core.Configuration;
using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.Core.Responses.Identities;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface IEmployeeHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateEmployeeRequest request);
    Task<CustomHttpResponse> CreateAsync(CreateEmployeeAndUserRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteEmployeeRequest request);
    Task<CustomHttpResponse<PagedResponse<EmployeeResponse>>> GetAllAsync(GetAllEmployeesRequest request);
    Task<CustomHttpResponse<EmployeeResponse>> GetByIdAsync(GetEmployeeByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateEmployeeRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateEmployeeAndUserRequest request);
}

public class EmployeeHandler : BaseHandler, IEmployeeHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/employees";

    public EmployeeHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<EmployeeResponse>> GetByIdAsync(GetEmployeeByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<EmployeeResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<EmployeeResponse>>> GetAllAsync(GetAllEmployeesRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<EmployeeResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateEmployeeRequest request)
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

    public async Task<CustomHttpResponse> CreateAsync(CreateEmployeeAndUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/user", request);

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

    public async Task<CustomHttpResponse> UpdateAsync(UpdateEmployeeRequest request)
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

    public async Task<CustomHttpResponse> UpdateAsync(UpdateEmployeeAndUserRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/user/{request.Employee.Id}", request);

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

    public async Task<CustomHttpResponse> DeleteAsync(DeleteEmployeeRequest request)
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
