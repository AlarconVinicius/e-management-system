using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class EmployeesController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeesController(IAspNetUser appUser, IUserService userService, IUserRepository userRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _appUser = appUser;
        _userService = userService;
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var usersDb = await _userRepository.GetAllUsers(ps, page, q);
        var mappedUsers = new PagedViewModel<UserViewModel>
        {
            List = usersDb.List.Select(p => new UserViewModel (p.Id, p.CompanyId, p.TenantId,p.Name, p.LastName, p.Email.Address, p.PhoneNumber, p.Cpf.Number, p.Role, p.CreatedAt, p.UpdatedAt)),
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
        var userDb = await _userRepository.GetById(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = new UserViewModel(userDb.Id, userDb.CompanyId, userDb.TenantId, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number, userDb.Role, userDb.CreatedAt, userDb.UpdatedAt);

        return View(mappedUser);
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(UserViewModel userVM, string returnUrl = null)
    {
        var userId = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(userId);

        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = userVM.Email,
                Email = userVM.Email,
                EmailConfirmed = true
            };
            var password = $"{userVM.Cpf[..5]}@{userVM.Name[..1].ToUpper()}{userVM.LastName[..1].ToLower()}";

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var role = "Employee";
                var userMapped = new UserViewModel(Guid.Parse(user.Id), userDb.CompanyId, userDb.TenantId, userVM.Name, userVM.LastName, userVM.Email, userVM.PhoneNumber, userVM.Cpf, role);
                await AddUser(userMapped);

                if (HasErrorsInResponse(ModelState))
                {
                    await _userManager.DeleteAsync(user);
                    return View(userVM);
                }

                await _userRepository.UnitOfWork.Commit();

                var tenantIdClaim = new Claim("Tenant", userMapped.TenantId.ToString());
                await _userManager.AddClaimAsync(user, tenantIdClaim);
                await AddRole(user, role);
                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index");

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }
        }

        return View(userVM);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var userDb = await _userRepository.GetById(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = new UserViewModel(userDb.Id, userDb.CompanyId, userDb.TenantId, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number, userDb.Role, userDb.CreatedAt, userDb.UpdatedAt);
        
        return View(mappedUser);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserViewModel user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var userDb = await _userRepository.GetById(id);
                userDb.SetName(user.Name);
                userDb.SetLastName(user.LastName);
                if (user.Email != userDb.Email.Address)
                {
                    userDb.SetEmail(user.Email);
                }

                userDb.SetPhoneNumber(user.PhoneNumber);
                userDb.SetIsActive(user.IsActive);

                _userRepository.UpdateUser(userDb);
                await _userRepository.UnitOfWork.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.Id))
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
        //ViewData["CompanyId"] = new SelectList(_productRepository.Companies, "Id", "Name", product.CompanyId);
        return View(user);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userDb = await _userRepository.GetById(id);
        if (userDb is null)
        {
            return NotFound();
        }
        var mappedUser = new UserViewModel(userDb.Id, userDb.CompanyId, userDb.TenantId, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number, userDb.Role, userDb.CreatedAt, userDb.UpdatedAt);

        return View(mappedUser);
    }

    [HttpPost("Delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userDb = await _userRepository.GetById(id);
        if (userDb is null)
        {
            return NotFound();
        }

        await _userRepository.DeleteUser(userDb);

        await _userRepository.UnitOfWork.Commit();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserExists(Guid id)
    {
        return await _userRepository.GetById(id) is not null;
    }
    private async Task AddUser(UserViewModel userVM)
    {
        var userResult = await _userService.AddUser(userVM);
        if (!userResult.IsValid)
        {
            AddError(userResult);
        }
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
