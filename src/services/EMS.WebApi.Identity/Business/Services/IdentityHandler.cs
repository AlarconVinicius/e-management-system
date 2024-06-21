using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Identities;
using EMS.Core.Responses;
using EMS.Core.Responses.Identities;
using EMS.Core.User;
using EMS.WebApi.Business.Handlers;
using EMS.WebApi.Business.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS.WebApi.Identity.Business.Services;

public class IdentityHandler : BaseHandler, IIdentityHandler
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly AppSettings _appSettings;
    public IdentityHandler(INotifier notifier, IAspNetUser appUser, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmployeeRepository employeeRepository, IOptions<AppSettings> appSettings) : base(notifier, appUser)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _employeeRepository = employeeRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

        if (result.Succeeded)
            return await GenerateJwt(request.Email);

        if (result.IsLockedOut)
        {
            Notify("Usuário temporariamente bloqueado devido às tentativas inválidas.");
            return null;
        }

        Notify("Usuário ou senha inválidos.");
        return null;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<LoginUserResponse> CreateAsync(CreateUserRequest request)
    {
        if(request.Password != request.ConfirmPassword)
        {
            Notify("As senhas não conferem.");
            return null;
        }
        var employeeDb = await _employeeRepository.GetByIdAsync(request.Id);
        if (employeeDb is null)
        {
            Notify("Colaborador não encontrado.");
            return null;
        };
        var user = new IdentityUser
        {
            Id = request.Id.ToString(),
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            foreach (var errors in result.Errors)
            {
                Notify(errors.Description);
            }
            return null;
        }

        await AddOrUpdateUserClaimAsync(new AddOrUpdateUserClaimRequest(request.Id, new UserClaim("Tenant", employeeDb.CompanyId.ToString())));

        await _signInManager.SignInAsync(user, false);

        return await GenerateJwt(request.Email);
    }

    public async Task UpdateEmailAsync(UpdateUserEmailRequest request)
    {
        var userId = request.Id.ToString();
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }
        userDb.UserName = request.NewEmail;
        userDb.NormalizedUserName = request.NewEmail.ToUpper();
        userDb.Email = request.NewEmail;
        userDb.NormalizedEmail = request.NewEmail.ToUpper();
        await _userManager.UpdateAsync(userDb);
        return;
    }

    public async Task UpdatePasswordAsync(UpdateUserPasswordRequest request)
    {
        string userId = request.Id.ToString();
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }

        var passwordCheckResult = await _userManager.CheckPasswordAsync(userDb, request.OldPassword);

        if (!passwordCheckResult)
        {
            Notify("A senha atual está incorreta.");
            return;
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            Notify("As senhas não conferem.");
            return;
        }

        var updatePasswordResult = await _userManager.ChangePasswordAsync(userDb, request.OldPassword, request.NewPassword);

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

    public async Task AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request)
    {
        var userId = request.Id.ToString();
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }
        //if(IsUserAuthenticated && request.NewClaim.Type == "Tenant")
        //{
        //    Notify("Claim inválida.");
        //    return;
        //}
        var userDb = await _userManager.FindByIdAsync(userId);

        var existingClaims = await _userManager.GetClaimsAsync(userDb);
        Claim existingClaim;
        existingClaim = existingClaims.FirstOrDefault(c => c.Type == request.NewClaim.Type);
        //if (IsUserAuthenticated)
        //{
        //    existingClaim = existingClaims.FirstOrDefault(c => c.Type != "Tenant" && c.Type == request.NewClaim.Type);
        //}
        //else
        //{
        //    existingClaim = existingClaims.FirstOrDefault(c => c.Type == request.NewClaim.Type);
        //}

        if (existingClaim != null)
        {
            await _userManager.RemoveClaimAsync(userDb, existingClaim);
        }
        var newClaim = new Claim(request.NewClaim.Type, request.NewClaim.Value);
        await _userManager.AddClaimAsync(userDb, newClaim);
        return;
    }

    public async Task DeleteAsync(DeleteUserRequest request)
    {
        var userId = request.Id.ToString();
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return;
        }

        var logins = await _userManager.GetLoginsAsync(userDb);
        foreach (var login in logins)
        {
            await _userManager.RemoveLoginAsync(userDb, login.LoginProvider, login.ProviderKey);
        }

        var claims = await _userManager.GetClaimsAsync(userDb);
        foreach (var claim in claims)
        {
            await _userManager.RemoveClaimAsync(userDb, claim);
        }

        var roles = await _userManager.GetRolesAsync(userDb);
        foreach (var role in roles)
        {
            await _userManager.RemoveFromRoleAsync(userDb, role);
        }

        var result = await _userManager.DeleteAsync(userDb);
        if (!result.Succeeded)
        {
            Notify("Erro ao deletar usuário.");
            return;
        }

        return;
    }

    public async Task<UserResponse> GetByIdAsync(GetUserByIdRequest request)
    {
        var userId = request.Id.ToString();
        var userDb = await _userManager.FindByIdAsync(userId);
        if (!await UserExists(userId))
        {
            Notify("Usuário não encontrado.");
            return null;
        }
        return new UserResponse(Guid.Parse(userDb.Id), userDb.Email);
    }

    public Task<PagedResponse<UserResponse>> GetAllAsync(GetAllUsersRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> UserExists(string userId)
    {
        return await _userManager.FindByIdAsync(userId) != null;
    }

    #region IdentityHelpers
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
    #endregion

    #region JwtHelpers
    private async Task<LoginUserResponse> GenerateJwt(string email)
    {
        var userDb = await _userManager.FindByEmailAsync(email);
        if (userDb == null)
        {
            return null!;
        }
        var claims = (await _userManager.GetClaimsAsync(userDb)).ToList();
        var userRoles = await _userManager.GetRolesAsync(userDb);
        AddStandardClaims(claims, userDb);
        AddUserRolesClaims(claims, userRoles);

        //claims = claims.Where(c => c.Type != "Tenant").ToList();

        var token = GenerateToken(claims);

        var response = CreateResponse(token, userDb, claims);

        return response;
    }

    private void AddStandardClaims(List<Claim> claims, IdentityUser user)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
    }
    private void AddUserRolesClaims(List<Claim> claims, IList<string> userRoles)
    {
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }
    }
    private SecurityToken GenerateToken(List<Claim> claims)
    {
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

    }
    private LoginUserResponse CreateResponse(SecurityToken token, IdentityUser user, List<Claim> claims)
    {
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginUserResponse
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email!,
                Claims = claims.Select(c => new UserClaim(c.Type, c.Value))
            }
        };
    }
    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - DateTimeOffset.UnixEpoch).TotalSeconds);

    #endregion

}
