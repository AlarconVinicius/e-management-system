using EMS.Core.Requests.Dashboards;
using EMS.Core.Responses.Dashboards;

namespace EMS.Core.Handlers;

public interface IDashboardHandler
{
    Task<DashboardResponse> GetDashboardDetailsAsync(GetDashboardDetailsRequest request);
}