using AutoMapper;
using EMS.Core.Notifications;
using EMS.Core.Requests.Companies;
using EMS.Core.Requests.Identities;
using EMS.Core.Requests.Plans;
using EMS.Core.User;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanHandler _planHandler;
    private readonly IIdentityHandler _identityHandler;
    private readonly ICompanyHandler _companyHandler;

    public AuthenticationController(INotifier notifier, IAspNetUser appUser, IIdentityHandler identityHandler, IPlanHandler planHandler, ICompanyHandler companyHandler) : base(notifier)
    {
        _appUser = appUser;
        _identityHandler = identityHandler;
        _planHandler = planHandler;
        _companyHandler = companyHandler;
    }

    [HttpGet]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId)
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");

        var planRequest = new GetPlanByIdRequest(planId);
        var planResponse = await _planHandler.GetByIdAsync(planRequest);
        if (planResponse is null)
            return NotFound();

        var viewModel = new PlanCompanyViewModel
        {
            Plan = planResponse.Data
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId, PlanCompanyViewModel request, string returnUrl = null)
    {
        var planRequest = new GetPlanByIdRequest(planId);
        var planResponse = await _planHandler.GetByIdAsync(planRequest);
        if (planResponse is null)
            return NotFound();

        request.Plan = planResponse.Data;
        request.CreateCompanyAndUserRequest.Employee.Email = request.CreateCompanyAndUserRequest.User.Email;
        request.CreateCompanyAndUserRequest.Company.PlanId = planId;
        request.CreateCompanyAndUserRequest.Company.Brand = "";

        ModelState.Remove("CreateCompanyAndUserRequest.Employee.Email");
        if (!ModelState.IsValid)
            return View(request);

        var result = await _companyHandler.CreateAsync(request.CreateCompanyAndUserRequest);

        if (HasErrorsInResponse(result)) return View(request);

        await _identityHandler.PerformLogin(result.Data);

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
    public async Task<IActionResult> Login(LoginUserRequest request, string returnUrl = null!)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid) return View(request);

        if (!await PerformLogin(request))
        {
            return View(request);
        }
        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Dashboard");

        return LocalRedirect(returnUrl);
    }

    [Authorize]
    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await _identityHandler.Logout();
        return RedirectToAction("Index", "Home");
    }

    #region AuxLoginMethods
    private async Task<bool> PerformLogin(LoginUserRequest request)
    {
        var response = await _identityHandler.LoginAsync(request);

        if (response.Data == null) return false;

        await _identityHandler.PerformLogin(response.Data);
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
