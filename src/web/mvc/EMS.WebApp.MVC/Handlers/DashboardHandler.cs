using EMS.Core.Configuration;
using EMS.Core.Requests.Dashboards;
using EMS.Core.Responses;
using EMS.Core.Responses.Dashboards;
using EMS.Core.User;
using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Handlers;

public interface IDashboardHandler
{
    Task<CustomHttpResponse<DashboardResponse>> GetDashboardDetailsAsync(GetDashboardDetailsRequest request);
}

public class DashboardHandler : BaseHandler, IDashboardHandler
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "api/v1/dashboards";

    public DashboardHandler(IAspNetUser aspNetUser, HttpClient httpClient) : base(aspNetUser)
    {
        httpClient.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
        _httpClient = httpClient;
    }

    public async Task<CustomHttpResponse<DashboardResponse>> GetDashboardDetailsAsync(GetDashboardDetailsRequest request)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}?selectedYear={request.SelectedYear}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<CustomHttpResponse<DashboardResponse>>(response);
    }
}
