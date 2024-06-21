using EMS.Core.Enums;
using EMS.Core.Notifications;
using EMS.Core.Requests.Clients;
using EMS.Core.Requests.Employees;
using EMS.Core.Requests.ServiceAppointments;
using EMS.Core.Requests.Services;
using EMS.Core.Responses.ServiceAppointments;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class ServiceAppointmentsController : MainController
{
    private readonly IServiceAppointmentHandler _serviceAppointmentHandler;
    private readonly IEmployeeHandler _employeeHandler;
    private readonly IClientHandler _clientHandler;
    private readonly IServiceHandler _serviceHandler;

    public ServiceAppointmentsController(INotifier notifier, IServiceAppointmentHandler serviceAppointmentHandler, IEmployeeHandler employeeHandler, IClientHandler clientHandler, IServiceHandler serviceHandler) : base(notifier)
    {
        _serviceAppointmentHandler = serviceAppointmentHandler;
        _employeeHandler = employeeHandler;
        _clientHandler = clientHandler;
        _serviceHandler = serviceHandler;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 10;
        var request = new GetAllServiceAppointmentsRequest { PageNumber = page , PageSize = ps, Query = q};
        var response = await _serviceAppointmentHandler.GetAllAsync(request);

        var mappedServiceAppointments = new PagedViewModel<ServiceAppointmentResponse>
        {
            List = response.Data.List,
            PageIndex = response.Data.PageIndex,
            PageSize = response.Data.PageSize,
            Query = request.Query,
            TotalResults = response.Data.TotalResults
        };
        ViewBag.Search = q;
        mappedServiceAppointments.ReferenceAction = "Index";

        return View(mappedServiceAppointments);
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

    public async Task<IActionResult> Create()
    {
        await PopulateViewBags();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceAppointmentRequest request)
    {
        await PopulateViewBags();
        ModelState.Remove("AppointmentEnd");
        if (!ModelState.IsValid)
            return View(request);

        var result = await _serviceAppointmentHandler.CreateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Agendamento adicionado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await GetById(id);

        if (response is null)
        {
            return NotFound();
        }
        await PopulateViewBags();

        var request = new UpdateServiceAppointmentRequest(id, response.CompanyId, response.EmployeeId, response.ClientId, response.ServiceId, response.AppointmentStart, response.AppointmentEnd, response.Status);
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateServiceAppointmentRequest request)
    {
        ModelState.Remove("AppointmentEnd");
        if (id != request.Id)
        {
            return NotFound();
        }
        await PopulateViewBags();
        var response = await GetById(id);
        request.AppointmentEnd = response.AppointmentEnd;
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _serviceAppointmentHandler.UpdateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Agendamento atualizado com sucesso!";
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

        var result = await _serviceAppointmentHandler.DeleteAsync(new DeleteServiceAppointmentRequest { Id = id });

        if (HasErrorsInResponse(result)) return View(response);

        TempData["Success"] = "Agendamento deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ServiceAppointmentResponse> GetById(Guid id)
    {
        var result = await _serviceAppointmentHandler.GetByIdAsync(new GetServiceAppointmentByIdRequest(id));
        return result.Data;
    }
    private async Task PopulateViewBags()
    {
        ViewBag.Employees = (await _employeeHandler.GetAllAsync(new GetAllEmployeesRequest()))
                        .Data.List.Select(e => new { e.Id, e.Name });
        ViewBag.Clients = (await _clientHandler.GetAllAsync(new GetAllClientsRequest()))
                          .Data.List.Select(c => new { c.Id, c.Name });
        ViewBag.Services = (await _serviceHandler.GetAllAsync(new GetAllServicesRequest()))
                           .Data.List.Select(s => new { s.Id, s.Name });
        ViewBag.StatusList = Enum.GetValues(typeof(EServiceStatusCore))
                           .Cast<EServiceStatusCore>()
                           .Select(e => new { Id = (int)e, Name = e.ToString() });
    }
}
