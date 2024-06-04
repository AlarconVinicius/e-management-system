using EMS.Core.Handlers;
using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.WebApp.Business.Mappings;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.Identity.Business.Interfaces.Services;
using EMS.WebApp.Identity.Business.Models;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class EmployeesController : MainController
{
    private readonly IEmployeeHandler _employeeHandler;
    private readonly IAuthService _authService;
    private readonly IAspNetUser _appUser;

    public EmployeesController(INotifier notifier, IAspNetUser appUser, IEmployeeHandler employeeHandler, IAuthService authService) : base(notifier)
    {
        _appUser = appUser;
        _employeeHandler = employeeHandler;
        _authService = authService;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 10;
        var request = new GetAllEmployeesRequest { PageNumber = page, PageSize = ps, Query = q };
        var response = await _employeeHandler.GetAllAsync(request);

        var mappedClients = new PagedViewModel<EmployeeResponse>
        {
            List = response.Data,
            PageIndex = request.PageNumber,
            PageSize = request.PageSize,
            Query = request.Query,
            TotalResults = response.TotalCount
        };
        ViewBag.Search = q;
        mappedClients.ReferenceAction = "Index";

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

    //[Authorize(Roles = "Admin")]
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateEmployeeRequest request, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        Guid id = Guid.NewGuid();
        var role = ERole.Employee;
        request.Id = id;
        request.CompanyId = GetTenant();
        request.Role = role.MapERoleToERoleCore();

        var result = await _employeeHandler.CreateAsync(request);

        if (result != null && !result.IsSuccess)
        {
            Notify(result.Message);
            TempData["Failure"] = "Falha ao adicionar colaborador: " + string.Join("; ", await GetNotificationErrors());
            return View(request);
        }

        if (!await AddIdentityUser(request))
        {
            TempData["Failure"] = "Falha ao adicionar colaborador: " + string.Join("; ", await GetNotificationErrors());
            await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest { Id = id });
            return View(request);
        }

        await _authService.AddOrUpdateUserClaim(request.Id.ToString(), "Tenant", request.CompanyId.ToString());

        if (!IsValidOperation())
        {
            await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest { Id = id });
            await _authService.DeleteUser(request.Id.ToString());
            return View(request);

        }
        TempData["Success"] = "Colaborador adicionado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        var request = new UpdateEmployeeRequest(id, response.Data.CompanyId, response.Data.Name, response.Data.LastName, response.Data.Email, response.Data.PhoneNumber, response.Data.Salary, response.Data.Role, response.Data.IsActive);
        ViewBag.Document = response.Data.Document;
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateEmployeeRequest request)
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
        var identityUser = await _authService.GetUserById(request.Id.ToString());
        if (identityUser != null && identityUser.Email != request.Email)
        {
            await _authService.UpdateUserEmail(request.Id.ToString(), request.Email);
            if (!IsValidOperation())
            {
                TempData["Failure"] = "Falha ao atualizar colaborador: " + string.Join("; ", await GetNotificationErrors());
                return View(request);
            }
        }
        var result = await _employeeHandler.UpdateAsync(request);

        if (result != null && !result.IsSuccess)
        {
            Notify(result.Message);
            TempData["Failure"] = "Falha ao atualizar colaborador: " + string.Join("; ", await GetNotificationErrors());
            return View(request);
        }
        if (!IsValidOperation())
        {
            TempData["Failure"] = "Falha ao atualizar colaborador: " + string.Join("; ", await GetNotificationErrors());
            return View(request);
        }
        TempData["Success"] = "Colaborador atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var identityUser = await _authService.GetUserById(id.ToString());
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        if (identityUser is null)
        {
            return NotFound();
        }

        return View(response);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var response = await GetById(id);
        var identityUser = await _authService.GetUserById(id.ToString());
        if (response is null)
        {
            return NotFound();
        }
        if (identityUser is null)
        {
            return NotFound();
        }
        await _authService.DeleteUser(id.ToString());
        await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest { Id = id });

        if (!IsValidOperation())
        {
            TempData["Failure"] = "Falha ao deletar colaborador: " + string.Join("; ", await GetNotificationErrors());
            return View(response);
        }

        TempData["Success"] = "Colaborador deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> AddIdentityUser(CreateEmployeeRequest request)
    {
        var identityUser = new RegisterUser
        {
            Id = request.Id,
            Role = request.Role.ToString(),
            Email = request.Email,
            Password = GeneratePassword(request)
        };
        await _authService.RegisterUser(identityUser);
        if (!IsValidOperation())
        {
            return false;
        }
        return true;
    }
    private Guid GetTenant()
    {
        return _appUser.GetTenantId();
    }
    private async Task<Response<EmployeeResponse>> GetById(Guid id)
    {
        return await _employeeHandler.GetByIdAsync(new GetEmployeeByIdRequest { Id = id });
    }

    private static string GeneratePassword(CreateEmployeeRequest request)
    {
        return $"{request.Document[..5]}@{request.Name[..1].ToUpper()}{request.LastName[..1].ToLower()}";
    }

}
