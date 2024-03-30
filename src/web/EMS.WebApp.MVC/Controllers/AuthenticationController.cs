using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _planRepository;
    private readonly IUserService _subscriberService;

    public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IAspNetUser appUser, IPlanRepository planRepository, IUserService subscriberService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appUser = appUser;
        _planRepository = planRepository;
        _subscriberService = subscriberService;
    }

    [HttpGet]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId)
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");

        var plan = await _planRepository.GetById(planId);
        if (plan is null)
            return NotFound();

        var registerUser = new RegisterUser();

        var viewModel = new PlanUserViewModel
        {
            Plan = new PlanViewModel().ToViewModel(plan),
            RegisterUser = registerUser
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("nova-conta/{planId}")]
    public async Task<IActionResult> Register(Guid planId, RegisterUser registerUser, string returnUrl = null)
    {
        var plan = await _planRepository.GetById(planId);
        if (plan is null)
            return NotFound();

        var viewModel = new PlanUserViewModel
        {
            Plan = new PlanViewModel().ToViewModel(plan),
            RegisterUser = registerUser
        };
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                //await AddSubscriber(registerUser, user);

                //await AddPlanSubscriber(registerUser, user);

                if (HasErrorsInResponse(ModelState))
                {
                    await _userManager.DeleteAsync(user);
                    return View(viewModel);
                }

                await _planRepository.UnitOfWork.Commit();
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
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user != null)
                {
                    // Adiciona a claim de e-mail
                    //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, loginUser.Email));
                }
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
    //private async Task AddSubscriber(RegisterUser registerUser, IdentityUser user)
    //{
    //    var subscriberResult = await _subscriberService.AddSubscriber(Guid.Parse(user.Id), registerUser);
    //    if (!subscriberResult.IsValid)
    //    {
    //        AddError(subscriberResult);
    //    }
    //}
    //private async Task AddPlanSubscriber(RegisterUser registerUser, IdentityUser user)
    //{
    //    var planSubscriberResult = await _planSubscriberService.AddPlanSubscriber(Guid.Parse(user.Id), registerUser);
    //    if (!planSubscriberResult.IsValid)
    //    {
    //        AddError(planSubscriberResult);
    //    }
    //}

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
