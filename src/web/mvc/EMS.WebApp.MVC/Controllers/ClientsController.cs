using AutoMapper;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class ClientsController : MainController
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientService _clientService;
    private readonly IAspNetUser _appUser;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public ClientsController(INotifier notifier, IClientRepository clientRepository, IAspNetUser appUser, IEmployeeRepository employeeRepository, IClientService clientService, IMapper mapper) : base(notifier)
    {
        _clientRepository = clientRepository;
        _appUser = appUser;
        _employeeRepository = employeeRepository;
        _clientService = clientService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var clientDb = await _clientRepository.GetAllPagedAsync(ps, page, q);
        var mappedClients = new PagedViewModel<ClientViewModel>
        {
            List = _mapper.Map<List<ClientViewModel>>(clientDb.List),
            PageIndex = clientDb.PageIndex,
            PageSize = clientDb.PageSize,
            Query = clientDb.Query,
            TotalResults = clientDb.TotalResults
        };
        ViewBag.Search = q;
        mappedClients.ReferenceAction = "Index";

        return View(mappedClients);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var clientDb = await _clientRepository.GetByIdAsync(id);
        if (clientDb is null)
        {
            return NotFound();
        }
        var mappedClient = _mapper.Map<ClientViewModel>(clientDb);
        return View(mappedClient);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientViewModel client)
    {
        var tenantId = _appUser.GetTenantId();
        var role = ERole.Client; 
        if (ModelState.IsValid)
        {
            client.CompanyId = tenantId;
            var mappedClient = _mapper.Map<Client>(client);
            mappedClient.SetRole(role);
            await _clientService.Add(mappedClient);
            if (!IsValidOperation())
            {
                return View(client);
            }
            TempData["Success"] = "Cliente adicionado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var clientDb = await _clientRepository.GetByIdAsync(id);
        if (clientDb is null)
        {
            return NotFound();
        }
        var mappedClient = _mapper.Map<ClientViewModel>(clientDb);
        return View(mappedClient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,CompanyId,Name,LastName,Email,PhoneNumber,IsActive")] ClientViewModel client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }
        ModelState.Remove("Cpf");
        if (ModelState.IsValid)
        {
            var mappedClient = _mapper.Map<Client>(client);
            await _clientService.Update(mappedClient);

            if (!IsValidOperation())
            {
                return View(client);
            }
            TempData["Success"] = "Cliente atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var clientDb = await _clientRepository.GetByIdAsync(id);
        if (clientDb is null)
        {
            return NotFound();
        }
        var mappedProduct = _mapper.Map<ClientViewModel>(clientDb);

        return View(mappedProduct);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var clientDb = await _clientRepository.GetByIdAsync(id);
        if (clientDb is null)
        {
            return NotFound();
        }

        await _clientService.Delete(id);

        if (!IsValidOperation())
        {
            return View(_mapper.Map<ClientViewModel>(clientDb));
        }

        TempData["Success"] = "Cliente deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ClientExists(Guid id)
    {
        return await _clientRepository.GetByIdAsync(id) is not null;
    }
}
