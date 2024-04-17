using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _planRepository;
    private readonly IUserService _userService;
    private readonly ICompanyService _companyService;
    private readonly ITenantRepository _tenantRepository;

    public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IAspNetUser appUser, IPlanRepository planRepository, ICompanyService companyService, IUserService userService, ITenantRepository tenantRepository, RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appUser = appUser;
        _planRepository = planRepository;
        _companyService = companyService;
        _userService = userService;
        _tenantRepository = tenantRepository;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId)
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");

        var plan = await _planRepository.GetById(planId);
        if (plan is null)
            return NotFound();

        var registerCompany = new RegisterCompanyViewModel();

        var viewModel = new PlanCompanyViewModel
        {
            Plan = new PlanViewModel().ToViewModel(plan),
            RegisterCompany = registerCompany
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId, RegisterCompanyViewModel registerCompany, string returnUrl = null)
    {
        var plan = await _planRepository.GetById(planId);
        if (plan is null)
            return NotFound();

        var viewModel = new PlanCompanyViewModel
        {
            Plan = new PlanViewModel().ToViewModel(plan),
            RegisterCompany = registerCompany
        };
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = registerCompany.Email,
                Email = registerCompany.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerCompany.Password);

            if (result.Succeeded)
            {
                var tenant = await AddTenant();

                var companyVM = new CompanyViewModel(Guid.NewGuid(), registerCompany.PlanId, tenant.Id, registerCompany.CompanyName, registerCompany.CpfOrCnpj);
                await AddCompany(companyVM);
                var role = "Admin";
                var userVM = new UserViewModel(Guid.Parse(user.Id), companyVM.Id, tenant.Id, registerCompany.Name, registerCompany.LastName, registerCompany.Email, registerCompany.PhoneNumber, registerCompany.Cpf, role);
                await AddUser(userVM);

                if (HasErrorsInResponse(ModelState))
                {
                    await _userManager.DeleteAsync(user);
                    return View(viewModel);
                }

                await _planRepository.UnitOfWork.Commit();

                var tenantIdClaim = new Claim("Tenant", tenant.Id.ToString());
                await _userManager.AddClaimAsync(user, tenantIdClaim);
                await AddRole(user, role);
                await _signInManager.SignInAsync(user, false);

                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }
        }

        return View(viewModel);
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login(string returnUrl = null!)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUser loginUser, string returnUrl = null!)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                AddError("Usuário temporariamente bloqueado devido às tentativas inválidas.");
                return View(loginUser);
            }
            else
            {
                AddError("Usuário ou senha inválidos.");
                return View(loginUser);
            }
        }
        return View();
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task<Tenant> AddTenant()
    {
        return await _tenantRepository.AddTenant();
    }

    private async Task AddCompany(CompanyViewModel registerCompany)
    {
        var companyResult = await _companyService.AddCompany(registerCompany);
        if (!companyResult.IsValid)
        {
            AddError(companyResult);
        }
    }
    private async Task AddUser(UserViewModel registerUser)
    {
        var userResult = await _userService.AddUser(registerUser);
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


    //public async Task<UserResponse> AddClaimAsync(AddUserClaim userClaim)
    //{
    //    IdentityResult result;
    //    var userIdentity = await _userManager.FindByEmailAsync(userClaim.Email);
    //    var existingClaims = await _userManager.GetClaimsAsync(userIdentity!);
    //    var existingClaim = existingClaims.FirstOrDefault(c => c.Type == userClaim.Type);

    //    if (existingClaim != null)
    //    {
    //        var updatedClaim = new Claim(userClaim.Type, $"{existingClaim.Value},{userClaim.Value}");
    //        result = await _userManager.ReplaceClaimAsync(userIdentity!, existingClaim, updatedClaim);
    //    }
    //    else
    //    {
    //        result = await _userManager.AddClaimAsync(userIdentity!, new Claim(userClaim.Type, userClaim.Value));
    //    }

    //    if (result.Succeeded)
    //    {
    //        return await GenerateJwt(userClaim.Email);
    //    }

    //    foreach (var error in result.Errors)
    //    {
    //        Notify(error.Description);
    //    }

    //    return null!;
    //}

    //public async Task<UserResponse> RemoveClaimAsync(AddUserClaim userClaim)
    //{
    //    IdentityResult result;
    //    var userIdentity = await _userManager.FindByEmailAsync(userClaim.Email);
    //    var existingClaims = await _userManager.GetClaimsAsync(userIdentity!);
    //    var existingClaim = existingClaims.FirstOrDefault(c => c.Type == userClaim.Type);

    //    if (existingClaim != null)
    //    {
    //        var values = existingClaim.Value.Split(',');

    //        values = values.Where(v => v != userClaim.Value).ToArray();

    //        var updatedClaim = new Claim(userClaim.Type, string.Join(",", values));

    //        result = await _userManager.ReplaceClaimAsync(userIdentity!, existingClaim, updatedClaim);
    //    }
    //    else
    //    {
    //        Notify($"Claim {userClaim.Type} não encontrada.");
    //        return null!;
    //    }

    //    if (result.Succeeded)
    //    {
    //        return await GenerateJwt(userClaim.Email);
    //    }

    //    foreach (var error in result.Errors)
    //    {
    //        Notify(error.Description);
    //    }

    //    return null!;
    //}
}
