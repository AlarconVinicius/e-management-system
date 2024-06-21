using EMS.Core.Configuration;
using EMS.Core.Requests.Companies;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;
using EMS.Core.Responses.Identities;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;
using System.Net;

namespace EMS.WebApp.MVC.Handlers;

public interface ICompanyHandler
{
    Task<CustomHttpResponse> CreateAsync(CreateCompanyRequest request);
    Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateCompanyAndUserRequest request);
    Task<CustomHttpResponse> DeleteAsync(DeleteCompanyRequest request);
    Task<CustomHttpResponse<PagedResponse<CompanyResponse>>> GetAllAsync(GetAllCompaniesRequest request);
    Task<CustomHttpResponse<CompanyResponse>> GetByIdAsync(GetCompanyByIdRequest request);
    Task<CustomHttpResponse> UpdateAsync(UpdateCompanyRequest request);
}

public class CompanyHandler : BaseHandler, ICompanyHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/companies";

    public CompanyHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<CompanyResponse>> GetByIdAsync(GetCompanyByIdRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{request.Id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<CompanyResponse>>(response);
    }

    public async Task<CustomHttpResponse<PagedResponse<CompanyResponse>>> GetAllAsync(GetAllCompaniesRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?ps={request.PageSize}&page={request.PageNumber}&q={request.Query}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<PagedResponse<CompanyResponse>>>(response);
    }

    public async Task<CustomHttpResponse> CreateAsync(CreateCompanyRequest request)
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

    public async Task<CustomHttpResponse<LoginUserResponse>> CreateAsync(CreateCompanyAndUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/user", request);

        if (!HandleErrorResponse(response))
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
        }

        return await DeserializeResponseObject<CustomHttpResponse<LoginUserResponse>>(response);
    }

    public async Task<CustomHttpResponse> UpdateAsync(UpdateCompanyRequest request)
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

    public async Task<CustomHttpResponse> DeleteAsync(DeleteCompanyRequest request)
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
