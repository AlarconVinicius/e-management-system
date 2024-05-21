using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Controllers;

public class ClientsController : Controller
{
    private readonly IClientRepository _clientRepository;
    private readonly IAspNetUser _appUser;
    private readonly IEmployeeRepository _employeeRepository;

    public ClientsController(IClientRepository clientRepository, IAspNetUser appUser, IEmployeeRepository employeeRepository)
    {
        _clientRepository = clientRepository;
        _appUser = appUser;
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var clientDb = await _clientRepository.GetAllPagedAsync(ps, page, q);
        var mappedClients = new PagedViewModel<ClientViewModel>
        {
            List = clientDb.List.Select(p => new ClientViewModel
            {
                Id = p.Id,
                CompanyId = p.CompanyId,
                Name = p.Name,
                LastName = p.LastName,
                Email = p.Email.Address,
                PhoneNumber = p.PhoneNumber,
                Cpf = p.Document.Number,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }),
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
        var mappedClient = new ClientViewModel
        {
            Id = clientDb.Id,
            CompanyId = clientDb.CompanyId,
            Name = clientDb.Name,
            LastName = clientDb.LastName,
            Email = clientDb.Email.Address,
            PhoneNumber = clientDb.PhoneNumber,
            Cpf = clientDb.Document.Number,
            IsActive = clientDb.IsActive,
            CreatedAt = clientDb.CreatedAt,
            UpdatedAt = clientDb.UpdatedAt
        };
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
        var userId = _appUser.GetUserId();
        var userDb = await _employeeRepository.GetByIdAsync(userId);
        var tenantId = _appUser.GetTenantId();
        var role = "Client"; 
        if (ModelState.IsValid)
        {
            var mappedClient = new Client(client.Id, userDb.CompanyId, client.Name, client.LastName, client.Email, client.PhoneNumber, client.Cpf, role);
            await _clientRepository.AddAsync(mappedClient);
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
        var mappedClient = new UpdateClientViewModel
        {
            Id = clientDb.Id,
            Name = clientDb.Name,
            LastName = clientDb.LastName,
            Email = clientDb.Email.Address,
            PhoneNumber = clientDb.PhoneNumber,
            Cpf = clientDb.Document.Number,
            IsActive = clientDb.IsActive

        };
        return View(mappedClient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateClientViewModel client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var clientDb = await _clientRepository.GetByIdAsync(id);
                clientDb.SetName(client.Name);
                clientDb.SetLastName(client.LastName);
                clientDb.SetPhoneNumber(client.PhoneNumber);
                clientDb.SetIsActive(client.IsActive);

                await _clientRepository.UpdateAsync(clientDb);
                TempData["Success"] = "Cliente atualizado com sucesso!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClientExists(client.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
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
        var mappedProduct = new ClientViewModel
        {
            Id = clientDb.Id,
            CompanyId = clientDb.CompanyId,
            Name = clientDb.Name,
            LastName = clientDb.LastName,
            Email = clientDb.Email.Address,
            PhoneNumber = clientDb.PhoneNumber,
            Cpf = clientDb.Document.Number,
            IsActive = clientDb.IsActive,
            CreatedAt = clientDb.CreatedAt,
            UpdatedAt = clientDb.UpdatedAt
        };

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

        await _clientRepository.DeleteAsync(id);

        TempData["Success"] = "Cliente deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ClientExists(Guid id)
    {
        return await _clientRepository.GetByIdAsync(id) is not null;
    }
}
