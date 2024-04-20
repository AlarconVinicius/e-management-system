using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
    public async Task<ValidationResult> Login(LoginUser loginUser)
    {
        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
        if (result.Succeeded)
        {
            return _validationResult;
        }
        if (result.IsLockedOut)
        {
            Notify("Usuário temporariamente bloqueado devido às tentativas inválidas."); 
            return _validationResult;
        }

        Notify("Usuário ou senha inválidos.");
        return _validationResult;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
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

    public async Task<ValidationResult> UpdateUserEmail(string userId, string newEmail)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }
        userDb.UserName = newEmail;
        userDb.NormalizedUserName = newEmail.ToUpper();
        userDb.Email = newEmail;
        userDb.NormalizedEmail = newEmail.ToUpper();
        await _userManager.UpdateAsync(userDb);
        return _validationResult;
    }

    public async Task<ValidationResult> UpdatePassword(string userId, UpdateUserPasswordViewModel passwordVM)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return _validationResult;
        }

        var passwordCheckResult = await _userManager.CheckPasswordAsync(userDb, passwordVM.OldPassword);

        if (!passwordCheckResult)
        {
            Notify("A senha atual está incorreta.");
            return _validationResult;
        }

        if (passwordVM.Password != passwordVM.ConfirmPassword)
        {
            Notify("As senhas não conferem.");
            return _validationResult;
        }

        var updatePasswordResult = await _userManager.ChangePasswordAsync(userDb, passwordVM.OldPassword, passwordVM.Password);

        if (!updatePasswordResult.Succeeded)
        {
            foreach (var error in updatePasswordResult.Errors)
            {
                Notify(error.Description);
            }
            return _validationResult;
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
