using EMS.WebApp.MVC.Business.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace EMS.WebApp.MVC.Controllers;
public class AuthenticationController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("nova-conta/{id}")]
    public async Task<IActionResult> Register(Guid id)
    {
        //if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");
        //var plan = await _subscriptionService.GetById(id);
        //var registerUser = new RegisterUser();

        //var viewModel = new PlanUserViewModel
        //{
        //    Plan = plan,
        //    RegisterUser = registerUser
        //};

        return View();
    }

    [HttpPost]
    [Route("nova-conta/{id}")]
    public async Task<IActionResult> Register(Guid id, RegisterUser registerUser, string? returnUrl = null)
    {
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
                await _signInManager.SignInAsync(user, false);

                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(registerUser);
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
                return View(loginUser);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
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
