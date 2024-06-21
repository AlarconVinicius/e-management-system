using EMS.Core.Extensions;
using EMS.Core.Notifications;
using EMS.Core.Requests.Services;
using EMS.Core.Responses.Services;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class ServicesController : MainController
{
    private readonly IServiceHandler _serviceHandler;

    public ServicesController(INotifier notifier, IServiceHandler serviceHandler) : base(notifier)
    {
        _serviceHandler = serviceHandler;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 10;
        var request = new GetAllServicesRequest { PageNumber = page , PageSize = ps, Query = q};
        var response = await _serviceHandler.GetAllAsync(request);

        var mappedServices = new PagedViewModel<ServiceResponse>
        {
            List = response.Data.List,
            PageIndex = response.Data.PageIndex,
            PageSize = response.Data.PageSize,
            Query = request.Query,
            TotalResults = response.Data.TotalResults
        };
        ViewBag.Search = q;
        mappedServices.ReferenceAction = "Index";

        return View(mappedServices);
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
    public async Task<IActionResult> Create(CreateServiceRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _serviceHandler.CreateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Serviço adicionado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        var request = new UpdateServiceRequest(id, response.CompanyId, response.Name, response.Price, response.Duration.ToFormattedString(), response.IsActive);
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateServiceRequest request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _serviceHandler.UpdateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Serviço atualizado com sucesso!";
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

        var result = await _serviceHandler.DeleteAsync(new DeleteServiceRequest { Id = id });

        if (HasErrorsInResponse(result)) return View(response);

        TempData["Success"] = "Serviço deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ServiceResponse> GetById(Guid id)
    {
        var result = await _serviceHandler.GetByIdAsync(new GetServiceByIdRequest(id));
        return result.Data;
    }
}
