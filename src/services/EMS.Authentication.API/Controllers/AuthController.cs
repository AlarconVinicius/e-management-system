using EMS.Authentication.API.Models;
using EMS.Authentication.API.Services;
using EMS.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Authentication.API.Controllers;

[Route("api/identity")]
public class AuthController : MainController
{
    private readonly AuthenticationService _authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegistration userRegistration)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = userRegistration.Email,
            Email = userRegistration.Email,
            EmailConfirmed = true
        };

        var result = await _authenticationService.UserManager.CreateAsync(user, userRegistration.Password!);

        if (result.Succeeded)
        {
            return CustomResponse(await _authenticationService.GenerateJwt(userRegistration.Email!));
        }

        foreach (var error in result.Errors)
        {
            AddProcessingError(error.Description);
        }

        return CustomResponse();
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> Login(UserLogin userLogin)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authenticationService.SignInManager.PasswordSignInAsync(userLogin.Email!, userLogin.Password!,
            false, true);

        if (result.Succeeded)
        {
            return CustomResponse(await _authenticationService.GenerateJwt(userLogin.Email!));
        }

        if (result.IsLockedOut)
        {
            AddProcessingError("User temporarily locked out due to invalid attempts");
            return CustomResponse();
        }

        AddProcessingError("Incorrect username or password");
        return CustomResponse();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            AddProcessingError("Invalid refresh token");
            return CustomResponse();
        }

        var token = await _authenticationService.GetRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            AddProcessingError("Expired refresh token");
            return CustomResponse();
        }

        return CustomResponse(await _authenticationService.GenerateJwt(token.Username));
    }
}
