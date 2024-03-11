using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using EMS.Authentication.API.Data;
using EMS.Authentication.API.Extensions;
using EMS.Authentication.API.Models;
using EMS.WebAPI.Core.Authentication;
using EMS.WebAPI.Core.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EMS.Authentication.API.Services;

public class AuthenticationService
{
    public readonly SignInManager<IdentityUser> SignInManager;
    public readonly UserManager<IdentityUser> UserManager;
    private readonly AppSettings _appSettings;
    private readonly AppTokenSettings _appTokenSettingsSettings;
    private readonly ApplicationDbContext _context;

    private readonly IAspNetUser _aspNetUser;
    private readonly IJwtService _jwksService;

    public AuthenticationService(
                                SignInManager<IdentityUser> signInManager,
                                UserManager<IdentityUser> userManager,
                                IOptions<AppSettings> appSettings,
                                IOptions<AppTokenSettings> appTokenSettingsSettings,
                                ApplicationDbContext context,
                                IJwtService jwksService,
                                IAspNetUser aspNetUser)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        _appSettings = appSettings.Value;
        _appTokenSettingsSettings = appTokenSettingsSettings.Value;
        _jwksService = jwksService;
        _aspNetUser = aspNetUser;
        _context = context;
    }

    public async Task<UserLoginResponse> GenerateJwt(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var claims = await UserManager.GetClaimsAsync(user!);

        var identityClaims = await GetUserClaims(claims, user!);
        var encodedToken = await EncodeToken(identityClaims);

        var refreshToken = await GenerateRefreshToken(email);

        return GetTokenResponse(encodedToken, user!, claims, refreshToken);
    }

    private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
    {
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private async Task<string> EncodeToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer = $"{_aspNetUser.GetHttpContext().Request.Scheme}://{_aspNetUser.GetHttpContext().Request.Host}";
        var key = await _jwksService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private UserLoginResponse GetTokenResponse(string encodedToken, IdentityUser user, IEnumerable<Claim> claims, RefreshToken refreshToken)
    {
        return new UserLoginResponse
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    private async Task<RefreshToken> GenerateRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettingsSettings.RefreshTokenExpiration)
        };

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
        await _context.RefreshTokens.AddAsync(refreshToken);

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<RefreshToken> GetRefreshToken(Guid refreshToken)
    {
        var token = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now
             ? token
             : null;
    }
}