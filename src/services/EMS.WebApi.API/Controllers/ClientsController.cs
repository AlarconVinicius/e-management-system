using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Clients;
using EMS.WebApi.API.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.API.Controllers;

[Route("api/v1/clients/")]
public class ClientsController : ApiController
{
    public readonly IClientHandler _clientHandler;
    public ClientsController(INotifier notifier, IClientHandler clientHandler) : base(notifier)
    {
        _clientHandler = clientHandler;
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        GetClientByIdRequest request = new(id);
        var result = await _clientHandler.GetByIdAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();

    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ps = ConfigurationDefault.DefaultPageSize, [FromQuery] int page = ConfigurationDefault.DefaultPageNumber, [FromQuery] string q = null)
    {
        GetAllClientsRequest request = new()
        {
            PageSize = ps,
            PageNumber = page,
            Query = q
        };
        var result = await _clientHandler.GetAllAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateClientRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }
        await _clientHandler.CreateAsync(request);

        return IsOperationValid() ? ResponseCreated() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateClientRequest request)
    {
        if (id != request.Id) return ResponseBadRequest("Os IDs não correspondem.");
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }

        await _clientHandler.UpdateAsync(request);

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _clientHandler.DeleteAsync(new DeleteClientRequest(id));

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }
}
