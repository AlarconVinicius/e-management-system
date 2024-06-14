using EMS.Core.Notifications;
using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.Core.User;
using EMS.WebApp.Business.Mappings;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Utils;
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
        var role = ERole.Client;
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        request.CompanyId = GetTenant();
        request.Role = role.MapERoleToERoleCore();
        //var result = await _clientHandler.CreateAsync(request);
            
        //if (result != null && !result.IsSuccess)
        //{
        //    Notify(result.Message);
        //    TempData["Failure"] = "Falha ao adicionar cliente: " + string.Join("; ", await GetNotificationErrors());
        //    return View(request);
        //}
        //if (!IsValidOperation())
        //{
        //    TempData["Failure"] = "Falha ao adicionar cliente: " + string.Join("; ", await GetNotificationErrors());
        //    return View(request);
        //}
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
        //var request = new UpdateClientRequest(id, response.Data.CompanyId, response.Data.Name, response.Data.LastName, response.Data.Email, response.Data.PhoneNumber, response.Data.IsActive);
        //ViewBag.Cpf = response.Data.Cpf;
        //return View(request);
        return View();
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

        request.CompanyId = GetTenant();
        //var result = await _clientHandler.UpdateAsync(request);

        //if (result != null && !result.IsSuccess)
        //{
        //    Notify(result.Message);
        //    TempData["Failure"] = "Falha ao atualizar cliente: " + string.Join("; ", await GetNotificationErrors());
        //    return View(request);
        //}
        //if (!IsValidOperation())
        //{
        //    TempData["Failure"] = "Falha ao atualizar cliente: " + string.Join("; ", await GetNotificationErrors());
        //    return View(request);
        //}
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

        await _clientHandler.DeleteAsync(new DeleteClientRequest { Id = id });

        if (!IsValidOperation())
        {
            TempData["Failure"] = "Falha ao deletar cliente: " + string.Join("; ", await GetNotificationErrors());
            return View(response);
        }

        TempData["Success"] = "Cliente deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private Guid GetTenant()
    {
        return _appUser.GetTenantId();
    }
    private async Task<ClientResponse> GetById(Guid id)
    {
        return await _clientHandler.GetByIdAsync(new GetClientByIdRequest { Id = id });
    }
}
