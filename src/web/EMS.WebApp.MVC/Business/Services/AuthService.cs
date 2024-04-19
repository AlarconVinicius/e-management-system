using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using EMS.WebApp.MVC.Migrations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS.WebApp.MVC.Business.Services;

public class AuthService : MainService, IAuthService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AuthService(INotifier notifier, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(notifier)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityUser> GetUserById(string userId)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return null;
        }
        return userDb;
    }
    public Task<ValidationResult> Login(LoginUser loginUser)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> RegisterUser(RegisterUser registerUser)
    {
        var identityUser = new IdentityUser
        {
            Id = registerUser.Id.ToString(),
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, registerUser.Password);

        if (result.Succeeded)
        {
            var addRoleResult = await AddRole(identityUser, registerUser.Role);
            if (!addRoleResult) return _validationResult;
            return _validationResult;
        }
        foreach(var error in result.Errors)
        {
            Notify(error.Description);
        }
        return _validationResult;
    }
    public async Task<ValidationResult> DeleteUser(string userId)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }
        await _userManager.DeleteAsync(userDb);
        return _validationResult;
    }

    public async Task<ValidationResult> AddOrUpdateUserClaim(string userId, string type, string value)
    {
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }
        var user = await _userManager.FindByIdAsync(userId);
        var newClaim = new Claim(type, value);

        var existingClaims = await _userManager.GetClaimsAsync(user);
        var existingClaim = existingClaims.FirstOrDefault(c => c.Type == newClaim.Type);

        if (existingClaim != null)
        {
            await _userManager.RemoveClaimAsync(user, existingClaim);
        }

        await _userManager.AddClaimAsync(user, newClaim);
        return _validationResult;
    }

    private async Task<bool> AddRole(IdentityUser user, string roleName)
    {
        if (!await RoleExists(roleName))
        {
            var role = new IdentityRole(roleName);
            var createRoleResult = await _roleManager.CreateAsync(role);

            if (!createRoleResult.Succeeded)
            {
                foreach (var error in createRoleResult.Errors)
                {
                    Notify(error.Description);
                }
                return false;
            }
        }

        if (await UserExists(user.Id))
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!addToRoleResult.Succeeded)
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    Notify(error.Description);
                }
                return false;
            }
        }
        return true;
    }

    private async Task<bool> RoleExists(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }
    private async Task<bool> UserExists(string userId)
    {
        return await _userManager.FindByIdAsync(userId) != null;
    }
}
