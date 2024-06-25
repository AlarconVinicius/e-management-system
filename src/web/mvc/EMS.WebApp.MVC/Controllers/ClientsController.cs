using EMS.Core.Notifications;
using EMS.Core.Requests.Clients;
using EMS.Core.Responses.Clients;
using EMS.Core.User;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class ClientsController : MainController
{
    private readonly IClientHandler _clientHandler;
    private readonly IAspNetUser _appUser;

    public ClientsController(INotifier notifier, IAspNetUser appUser, IClientHandler clientHandler) : base(notifier)
    {
        _appUser = appUser;
        _clientHandler = clientHandler;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 10;
        var request = new GetAllClientsRequest { PageNumber = page , PageSize = ps, Query = q};
        var response = await _clientHandler.GetAllAsync(request);

        var mappedClients = new PagedViewModel<ClientResponse>
        {
            List = response.Data.List,
            PageIndex = response.Data.PageIndex,
            PageSize = response.Data.PageSize,
            Query = request.Query,
            TotalResults = response.Data.TotalResults
        };
        ViewBag.Search = q;
        mappedClients.ReferenceAction = "Index";

        //return View(mappedClients);
        return View(mappedClients);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        return View(response);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateClientRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _clientHandler.CreateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Cliente adicionado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        var request = new UpdateClientRequest(id, response.CompanyId, response.Name, response.LastName, response.Email, response.PhoneNumber, response.IsActive);
        ViewBag.Document = response.Document;
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateClientRequest request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _clientHandler.UpdateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Cliente atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }

        return View(response);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }

        var result = await _clientHandler.DeleteAsync(new DeleteClientRequest { Id = id });

        if (HasErrorsInResponse(result)) return View(response);

        //if (!IsValidOperation())
        //{
        //    TempData["Failure"] = "Falha ao deletar cliente: " + string.Join("; ", await GetNotificationErrors());
        //    return View(response);
        //}

        TempData["Success"] = "Cliente deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ClientResponse> GetById(Guid id)
    {
        var result = await _clientHandler.GetByIdAsync(new GetClientByIdRequest(id));
        return result.Data;
    }
}
