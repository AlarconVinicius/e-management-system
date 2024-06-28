using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Dashboards;
using EMS.WebApi.API.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.API.Controllers;

[Authorize]
[Route("api/v1/dashboards/")]
public class DashboardsController : ApiController
{
    public readonly IDashboardHandler _dashboardHandler;
    public DashboardsController(INotifier notifier, IDashboardHandler dashboardHandler) : base(notifier)
    {
        _dashboardHandler = dashboardHandler;
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int selectedYear = -1)
    {
        if (selectedYear == -1)
        {
            selectedYear = DateTime.Now.Year;
        }

        GetDashboardDetailsRequest request = new GetDashboardDetailsRequest
        {
            SelectedYear = selectedYear
        };

        var result = await _dashboardHandler.GetDashboardDetailsAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();
    }
}
