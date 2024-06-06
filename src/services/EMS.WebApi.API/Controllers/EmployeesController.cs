using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Employees;
using EMS.WebApi.API.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.API.Controllers;

[Route("api/v1/employees/")]
public class EmployeesController : ApiController
{
    public readonly IEmployeeHandler _employeeHandler;
    public EmployeesController(INotifier notifier, IEmployeeHandler employeeHandler) : base(notifier)
    {
        _employeeHandler = employeeHandler;
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        GetEmployeeByIdRequest request = new(id);
        var result = await _employeeHandler.GetByIdAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();

    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ps = ConfigurationDefault.DefaultPageSize, [FromQuery] int page = ConfigurationDefault.DefaultPageNumber, [FromQuery] string q = null)
    {
        GetAllEmployeesRequest request = new()
        {
            PageSize = ps,
            PageNumber = page,
            Query = q
        };
        var result = await _employeeHandler.GetAllAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateEmployeeRequest request)
    {
        if (!ModelState.IsValid)
        {
            ResponseBadRequest(ModelState);
        }
        await _employeeHandler.CreateAsync(request);

        return IsOperationValid() ? ResponseCreated(request.Id) : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateEmployeeRequest request)
    {
        if (id != request.Id) return ResponseBadRequest("Os IDs não correspondem.");
        if (!ModelState.IsValid)
        {
            ResponseBadRequest(ModelState);
        }

        await _employeeHandler.UpdateAsync(request);

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest(id));

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }
}
