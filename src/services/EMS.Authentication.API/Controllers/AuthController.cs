using EasyNetQ;
using EMS.Authentication.API.Models;
using EMS.Authentication.API.Services;
using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Authentication.API.Controllers;

[Route("api/identity")]
public class AuthController : MainController
{
    private readonly AuthenticationService _authenticationService;
    private readonly IMessageBus _bus;

    public AuthController(AuthenticationService authenticationService, IMessageBus bus, INotifier notifier) : base(notifier)
    {
        _authenticationService = authenticationService;
        _bus = bus;
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

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }
            return CustomResponse();
            //return CustomResponse(await _authenticationService.GenerateJwt(userRegistration.Email!));
        }
        var clientResult = await RegisterClient(userRegistration);

        if (!clientResult.ValidationResult.IsValid)
        {
            await _authenticationService.UserManager.DeleteAsync(user);
            return CustomResponse(clientResult.ValidationResult);
        }

        return CustomResponse(await _authenticationService.GenerateJwt(userRegistration.Email!));
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
            NotifyError("User temporarily locked out due to invalid attempts");
            return CustomResponse();
        }

        NotifyError("Incorrect username or password");
        return CustomResponse();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            NotifyError("Invalid refresh token");
            return CustomResponse();
        }

        var token = await _authenticationService.GetRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            NotifyError("Expired refresh token");
            return CustomResponse();
        }

        return CustomResponse(await _authenticationService.GenerateJwt(token.Username));
    }

    private async Task<ResponseMessage> RegisterClient(UserRegistration userRegistration)
    {
        var userDb = await _authenticationService.UserManager.FindByEmailAsync(userRegistration.Email);

        var registerUserEvent = new RegisteredIdentityIntegrationEvent(Guid.Parse(userDb!.Id), Guid.Parse(userDb!.Id), userRegistration.Name, userRegistration.Email, userRegistration.Cpf);

        try
        {
            return await _bus.RequestAsync<RegisteredIdentityIntegrationEvent, ResponseMessage>(registerUserEvent);
        }
        catch
        {
            //await _authenticationService.UserManager.DeleteAsync(userDb);
            return null;
            throw;
        }
    }
}
