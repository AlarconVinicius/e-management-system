using AutoMapper;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class EmployeesController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public EmployeesController(INotifier notifier, IAspNetUser appUser, IEmployeeService employeeService, IEmployeeRepository userRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : base(notifier)
    {
        _appUser = appUser;
        _employeeService = employeeService;
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var usersDb = await _userRepository.GetAllPagedAsync(ps, page, q);
        var mappedUsers = new PagedViewModel<UserViewModel>
        {
            List = usersDb.List.Select(p => new UserViewModel (p.Id, p.CompanyId, p.Name, p.LastName, p.Email.Address, p.PhoneNumber, p.Document.Number, p.Role, p.CreatedAt, p.UpdatedAt)),
            PageIndex = usersDb.PageIndex,
            PageSize = usersDb.PageSize,
            Query = usersDb.Query,
            TotalResults = usersDb.TotalResults
        };
        ViewBag.Search = q;
        mappedUsers.ReferenceAction = "Index";
        return View(mappedUsers);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var userDb = await _userRepository.GetByIdAsync(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = new UserViewModel(userDb.Id, userDb.CompanyId, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Document.Number, userDb.Role, userDb.CreatedAt, userDb.UpdatedAt);

        return View(mappedUser);
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(UserViewModel employeeVM, string returnUrl = null)
    {
        var userId = _appUser.GetUserId();
        var userDb = await _userRepository.GetByIdAsync(userId);

        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = employeeVM.Email,
                Email = employeeVM.Email,
                EmailConfirmed = true
            };
            var password = $"{employeeVM.Cpf[..5]}@{employeeVM.Name[..1].ToUpper()}{employeeVM.LastName[..1].ToLower()}";

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var role = "Employee";
                var userMapped = _mapper.Map<EmployeeViewModel>(user);
                await AddUser(userMapped);

                if (HasErrorsInResponse(ModelState))
                {
                    await _userManager.DeleteAsync(user);
                    return View(employeeVM);
                }

                var tenantIdClaim = new Claim("Tenant", userMapped.CompanyId.ToString());
                await _userManager.AddClaimAsync(user, tenantIdClaim);
                await AddRole(user, role);
                TempData["Success"] = "Colaborador adicionado com sucesso!";
                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index");

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }
        }

        return View(employeeVM);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var userLoggedId = _appUser.GetUserId();
        var userLoggedDb = await _userRepository.GetByIdAsync(userLoggedId);

        if (id != userLoggedId && userLoggedDb.Role != "Admin")
        {
            return RedirectToAction("Error", "Home", new { id = 403 });
        }

        var userDb = await _userRepository.GetByIdAsync(id);
        if (userDb is null)
        {
            return NotFound();
        }

        EmployeeViewModel updateEmployeeVM;
        if (userLoggedDb is not null)
        {
            updateEmployeeVM = _mapper.Map<EmployeeViewModel>(userDb);
        }
        else
        {
            return NotFound();
        }

        return View(updateEmployeeVM);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EmployeeViewModel updateEmployeeVM)
    {
        if (id != updateEmployeeVM.Id)
        {
            return NotFound();
        }
        ModelState.Remove("UpdateUserPasswordViewModel");
        if (!ModelState.IsValid)
        {
            return View(updateEmployeeVM);
        }
        var userDb = await _userRepository.GetByIdAsync(id);

        if (userDb is null)
        {
            return NotFound();
        }
        await _employeeService.Update(_mapper.Map<Employee>(updateEmployeeVM));
        if (!IsValidOperation())
        {
            TempData["Failure"] = "Falha ao atualizar colaborador: " + string.Join("; ", GetModelStateErrors());
            return View(updateEmployeeVM);
        }
        TempData["Success"] = "Colaborador atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userDb = await _userRepository.GetByIdAsync(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = _mapper.Map<EmployeeViewModel>(userDb);

        return View(mappedUser);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userDb = await _userRepository.GetByIdAsync(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = _mapper.Map<EmployeeViewModel>(userDb);
        await _employeeService.Delete(id);
        if (!IsValidOperation())
        {
            TempData["Failure"] = "Falha ao atualizar colaborador: " + string.Join("; ", GetModelStateErrors());
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = "Colaborador deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserExists(Guid id)
    {
        return await _userRepository.GetByIdAsync(id) is not null;
    }
    private async Task AddUser(EmployeeViewModel employeeVM)
    {
        await _employeeService.Add(_mapper.Map<Employee>(employeeVM));
    }
    private async Task AddRole(IdentityUser user, string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var role = new IdentityRole(roleName);
            var createRoleResult = await _roleManager.CreateAsync(role);

            if (!createRoleResult.Succeeded)
            {
                foreach (var error in createRoleResult.Errors)
                {
                    AddError(error.Description);
                }
            }
        }

        if (user != null)
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!addToRoleResult.Succeeded)
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    AddError(error.Description);
                }
            }
        }
    }

}
