using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _planRepository;
    private readonly IUserService _userService;
    private readonly ICompanyService _companyService;
    private readonly ITenantRepository _tenantRepository;
    private readonly IAuthService _authService;

    public AuthenticationController( IAspNetUser appUser, IPlanRepository planRepository, ICompanyService companyService, IUserService userService, ITenantRepository tenantRepository, IAuthService authService)
    {
        _appUser = appUser;
        _planRepository = planRepository;
        _companyService = companyService;
        _userService = userService;
        _tenantRepository = tenantRepository;
        _authService = authService;
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

        if (!ModelState.IsValid)
            return View(viewModel);

        var identityUser = new RegisterUser
        {
            Id = Guid.NewGuid(),
            Role = "Admin",
            Email = registerCompany.Email,
            Password = registerCompany.Password
        };
        if (!await AddIdentityUser(identityUser))
            return View(viewModel);

        var tenant = await AddTenant();
        if (tenant is null)
        {
            await _authService.DeleteUser(identityUser.Id.ToString());
            return View(viewModel);
        }

        var companyVM = new CompanyViewModel(Guid.NewGuid(), registerCompany.PlanId, tenant.Id, registerCompany.CompanyName, registerCompany.CpfOrCnpj);
        if (!await AddCompany(companyVM))
        {
            await _authService.DeleteUser(identityUser.Id.ToString());
            return View(viewModel);
        }

        var userVM = new UserViewModel(identityUser.Id, companyVM.Id, tenant.Id, registerCompany.Name, registerCompany.LastName, registerCompany.Email, registerCompany.PhoneNumber, registerCompany.Cpf, identityUser.Role);
        if (!await AddUser(userVM))
        {
            await _authService.DeleteUser(identityUser.Id.ToString());
            return View(viewModel);
        }

        await _authService.AddOrUpdateUserClaim(identityUser.Id.ToString(), "Tenant", tenant.Id.ToString());

        if (HasErrorsInResponse(ModelState))
        {
            await _authService.DeleteUser(identityUser.Id.ToString());
            return View(viewModel);
        }
        await _planRepository.UnitOfWork.Commit();

        var loginUser = new LoginUser
        {
            Email = registerCompany.Email,
            Password = registerCompany.Password
        };
        if (!await PerformLogin(loginUser))
        {
            return View(loginUser);
        }

        if (string.IsNullOrEmpty(returnUrl)) 
            return RedirectToAction("Index", "Dashboard");

        return LocalRedirect(returnUrl);
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
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (!await PerformLogin(loginUser))
        {
            return View(loginUser);
        }
        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Dashboard");

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return RedirectToAction("Index", "Home");
    }

    #region AuxRegisterMethods
    private async Task<bool> AddIdentityUser(RegisterUser identityUser)
    {
        var userIdentityResult = await _authService.RegisterUser(identityUser);
        if (!userIdentityResult.IsValid)
        {
            AddError(userIdentityResult);
            return false;
        }
        return true;
    }

    private async Task<Tenant> AddTenant()
    {
        return await _tenantRepository.AddTenant();
    }

    private async Task<bool> AddCompany(CompanyViewModel registerCompany)
    {
        var companyResult = await _companyService.AddCompany(registerCompany);
        if (!companyResult.IsValid)
        {
            AddError(companyResult);
            return false;
        }
        return true;
    }
    private async Task<bool> AddUser(UserViewModel registerUser)
    {
        var userResult = await _userService.AddUser(registerUser);
        if (!userResult.IsValid)
        {
            AddError(userResult);
            return false;
        }
        return true;
    }
    #endregion

    #region AuxLoginMethods
    private async Task<bool> PerformLogin(LoginUser loginUser)
    {
        var loginResult = await _authService.Login(loginUser);
        if (!loginResult.IsValid)
        {
            AddError(loginResult);
            return false;
        }
        return true;
    }
    #endregion

    #region CodeToReview
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
    #endregion
}
