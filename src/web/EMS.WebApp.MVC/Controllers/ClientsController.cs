using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Controllers;

public class ClientsController : Controller
{
    private readonly IClientRepository _clientRepository;
    private readonly IAspNetUser _appUser;
    private readonly IUserRepository _userRepository;

    public ClientsController(IClientRepository clientRepository, IAspNetUser appUser, IUserRepository userRepository)
    {
        _clientRepository = clientRepository;
        _appUser = appUser;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var clientDb = await _clientRepository.GetAllClients(ps, page, q);
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
                Cpf = p.Cpf.Number,
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
        var clientDb = await _clientRepository.GetById(id);
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
            Cpf = clientDb.Cpf.Number,
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
        var userDb = await _userRepository.GetById(userId);
        var tenantId = _appUser.GetTenantId();
        if (ModelState.IsValid)
        {
            var mappedClient = new Client(userDb.CompanyId, tenantId, client.Name, client.LastName, client.Email, client.PhoneNumber, client.Cpf, client.IsActive);
            _clientRepository.AddClient(mappedClient);
            await _clientRepository.UnitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var clientDb = await _clientRepository.GetById(id);
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
            Cpf = clientDb.Cpf.Number,
            IsActive = clientDb.IsActive,
            CreatedAt = clientDb.CreatedAt,
            UpdatedAt = clientDb.UpdatedAt

        };
        return View(mappedClient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ClientViewModel client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var clientDb = await _clientRepository.GetById(id);
                clientDb.SetName(client.Name);
                clientDb.SetLastName(client.LastName);
                clientDb.SetPhoneNumber(client.PhoneNumber);
                clientDb.SetIsActive(client.IsActive);

                _clientRepository.UpdateClient(clientDb);
                await _clientRepository.UnitOfWork.Commit();
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
        var clientDb = await _clientRepository.GetById(id);
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
            Cpf = clientDb.Cpf.Number,
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
        var clientDb = await _clientRepository.GetById(id);
        if (clientDb is null)
        {
            return NotFound();
        }

        await _clientRepository.DeleteClient(clientDb);

        await _clientRepository.UnitOfWork.Commit();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ClientExists(Guid id)
    {
        return await _clientRepository.GetById(id) is not null;
    }
}
