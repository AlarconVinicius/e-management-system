using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.ServiceAppointments;
using EMS.WebApi.API.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.API.Controllers;

[Authorize]
[Route("api/v1/service-appointments/")]
public class ServiceAppointmentsController : ApiController
{
    public readonly IServiceAppointmentHandler _serviceAppointmentHandler;
    public ServiceAppointmentsController(INotifier notifier, IServiceAppointmentHandler serviceAppointmentHandler) : base(notifier)
    {
        _serviceAppointmentHandler = serviceAppointmentHandler;
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        GetServiceAppointmentByIdRequest request = new(id);
        var result = await _serviceAppointmentHandler.GetByIdAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();

    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ps = ConfigurationDefault.DefaultPageSize, [FromQuery] int page = ConfigurationDefault.DefaultPageNumber, [FromQuery] string q = null)
    {
        GetAllServiceAppointmentsRequest request = new()
        {
            PageSize = ps,
            PageNumber = page,
            Query = q
        };
        var result = await _serviceAppointmentHandler.GetAllAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateServiceAppointmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }
        await _serviceAppointmentHandler.CreateAsync(request);

        return IsOperationValid() ? ResponseCreated() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateServiceAppointmentRequest request)
    {
        if (id != request.Id) return ResponseBadRequest("Os IDs não correspondem.");
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }

        await _serviceAppointmentHandler.UpdateAsync(request);

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _serviceAppointmentHandler.DeleteAsync(new DeleteServiceAppointmentRequest(id));

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }
}
