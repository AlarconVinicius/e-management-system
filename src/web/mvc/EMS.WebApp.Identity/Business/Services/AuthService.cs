using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Services;
using EMS.WebApp.Identity.Business.Interfaces.Services;
using EMS.WebApp.Identity.Business.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EMS.WebApp.Identity.Business.Services;

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

    public async Task<User> GetUserById(string userId)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return null;
        }
        return new User(Guid.Parse(userDb.Id), userDb.Email);
    }
    public async Task Login(LoginUser loginUser)
    {
        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
        if (result.Succeeded)
        {
            return;
        }
        if (result.IsLockedOut)
        {
            Notify("Usuário temporariamente bloqueado devido às tentativas inválidas.");
            return;
        }

        Notify("Usuário ou senha inválidos.");
        return;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task RegisterUser(RegisterUser registerUser)
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
            if (!addRoleResult) return;
            return;
        }
        foreach (var error in result.Errors)
        {
            Notify(error.Description);
        }
        return;
    }

    public async Task UpdateUserEmail(string userId, string newEmail)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }
        userDb.UserName = newEmail;
        userDb.NormalizedUserName = newEmail.ToUpper();
        userDb.Email = newEmail;
        userDb.NormalizedEmail = newEmail.ToUpper();
        await _userManager.UpdateAsync(userDb);
        return;
    }

    public async Task UpdatePassword(UpdateUserPassword passwordVM)
    {
        string userId = passwordVM.Id.ToString();
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }

        var passwordCheckResult = await _userManager.CheckPasswordAsync(userDb, passwordVM.OldPassword);

        if (!passwordCheckResult)
        {
            Notify("A senha atual está incorreta.");
            return;
        }

        if (passwordVM.Password != passwordVM.ConfirmPassword)
        {
            Notify("As senhas não conferem.");
            return;
        }

        var updatePasswordResult = await _userManager.ChangePasswordAsync(userDb, passwordVM.OldPassword, passwordVM.Password);

        if (!updatePasswordResult.Succeeded)
        {
            foreach (var error in updatePasswordResult.Errors)
            {
                Notify(error.Description);
            }
            return;
        }

        return;
    }

    public async Task DeleteUser(string userId)
    {
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }
        await _userManager.DeleteAsync(userDb);
        return;
    }

    public async Task AddOrUpdateUserClaim(string userId, string type, string value)
    {
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
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
        return;
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
