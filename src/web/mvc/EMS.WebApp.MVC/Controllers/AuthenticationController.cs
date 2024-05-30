using AutoMapper;
using EMS.Core.Enums;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.Identity.Business.Interfaces.Services;
using EMS.WebApp.Identity.Business.Models;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _planRepository;
    private readonly IEmployeeService _employeeService;
    private readonly ICompanyService _companyService;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthenticationController(INotifier notifier, IAspNetUser appUser, IPlanRepository planRepository, ICompanyService companyService, IEmployeeService employeeService, IAuthService authService, IMapper mapper) : base(notifier)
    {
        _appUser = appUser;
        _planRepository = planRepository;
        _companyService = companyService;
        _employeeService = employeeService;
        _authService = authService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId)
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");

        var plan = await _planRepository.GetByIdAsync(planId);
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
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan is null)
            return NotFound();

        var viewModel = new PlanCompanyViewModel
        {
            Plan = _mapper.Map<PlanViewModel>(plan),
            RegisterCompany = registerCompany
        };

        if (!ModelState.IsValid)
            return View(viewModel);

        Guid companyId = Guid.NewGuid();
        Guid employeeId = Guid.NewGuid();
        registerCompany.Company.Id = companyId;
        registerCompany.Company.Brand = "";
        registerCompany.Employee.Id = employeeId;
        registerCompany.Employee.CompanyId = companyId;
        registerCompany.Employee.Role = ERoleCore.Admin;
        if (!await AddCompany(registerCompany.Company))
        {
            return View(viewModel);
        }
        if (!await AddEmployee(registerCompany.Employee))
        {
            await _companyService.Delete(companyId);
            return View(viewModel);
        }

        if (!await AddIdentityUser(registerCompany))
        {
            await _employeeService.Delete(employeeId);
            await _companyService.Delete(companyId);
            return View(viewModel);
        }

        await _authService.AddOrUpdateUserClaim(employeeId.ToString(), "Tenant", companyId.ToString());

        if (!IsValidOperation())
        {
            await _employeeService.Delete(employeeId);
            await _companyService.Delete(companyId);
            await _authService.DeleteUser(employeeId.ToString());
            return View(viewModel);
        }
        var loginUser = new LoginUserViewModel
        {
            Email = registerCompany.Employee.Email,
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
    public async Task<IActionResult> Login(LoginUserViewModel loginUser, string returnUrl = null!)
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
    private async Task<bool> AddIdentityUser(RegisterCompanyViewModel registerCompany)
    {
        var identityUser = new RegisterUser
        {
            Id = registerCompany.Employee.Id,
            Role = nameof(registerCompany.Employee.Role),
            Email = registerCompany.Employee.Email,
            Password = registerCompany.Password
        };
        await _authService.RegisterUser(identityUser);
        if (!IsValidOperation())
        {
            return false;
        }
        return true;
    }

    private async Task<bool> AddCompany(CompanyViewModel company)
    {
        await _companyService.Add(_mapper.Map<Company>(company));
        if (!IsValidOperation())
        {
            return false;
        }
        return true;
    }
    private async Task<bool> AddEmployee(EmployeeViewModel employee)
    {
        await _employeeService.Add(_mapper.Map<Employee>(employee));
        if (!IsValidOperation())
        {
            return false;
        }
        return true;
    }
    #endregion

    #region AuxLoginMethods
    private async Task<bool> PerformLogin(LoginUserViewModel loginUser)
    {
        var loginUserMapped = new LoginUser
        {
            Email = loginUser.Email,
            Password = loginUser.Password
        };
        await _authService.Login(loginUserMapped);
        if (!IsValidOperation())
        {
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
