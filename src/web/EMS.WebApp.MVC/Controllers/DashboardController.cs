using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
[Route("dashboard")]
public class DashboardController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public DashboardController(IAspNetUser appUser, IUserRepository userRepository, IUserService userService, IAuthService authService)
    {
        _appUser = appUser;
        _userRepository = userRepository;
        _userService = userService;
        _authService = authService;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("atualizar-perfil")]
    public async Task<IActionResult> UpdateProfile()
    {
        var id = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(id);

        UpdateUserViewModel updateUserVM;
        if (userDb is not null)
        {
            updateUserVM = new UpdateUserViewModel(userDb.Id, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number);
        }
        else
        {
            return NotFound();
        }

        return View(updateUserVM);
    }

    [HttpPost]
    [Route("atualizar-perfil")]
    public async Task<IActionResult> UpdateProfile(UpdateUserViewModel updateUserVM, string returnUrl = null)
    {
        ModelState.Remove("UpdateUserPasswordViewModel");
        if (!ModelState.IsValid)
        {
            return View(updateUserVM);
        }
        var id = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(id);

        if (userDb is null)
        {
            return NotFound();
        }
        var updateUserResult = await _userService.UpdateUser(id, updateUserVM);
        if (!updateUserResult.IsValid)
        {
            AddError(updateUserResult);
            TempData["Failure"] = "Falha ao atualizar usuário: " + string.Join("; ", GetModelStateErrors());
            return View(updateUserVM);
        }
        TempData["Success"] = "Usuário atualizado com sucesso!";
        return RedirectToAction(nameof(UpdateProfile));
    }

    [HttpPost]
    [Route("atualizar-senha")]
    public async Task<IActionResult> UpdatePassword(UpdateUserViewModel updateUserPasswordVM, string returnUrl = null)
    {
        var id = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(id);
        UpdateUserViewModel updateUserVM;
        if (userDb is not null)
        {
            updateUserVM = new UpdateUserViewModel(userDb.Id, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number)
            {
                UpdateUserPasswordViewModel = updateUserPasswordVM.UpdateUserPasswordViewModel
            };
        }
        else
        {
            return NotFound();
        }
        ModelState.Remove("Id");
        ModelState.Remove("Name");
        ModelState.Remove("LastName");
        ModelState.Remove("Email");
        ModelState.Remove("PhoneNumber");
        ModelState.Remove("Cpf");
        if (!ModelState.IsValid)
        {
            return View("UpdateProfile", updateUserVM);
        }

        if (userDb is null)
        {
            return NotFound();
        }
        var updatePassword = new UpdateUserPasswordViewModel(updateUserPasswordVM.UpdateUserPasswordViewModel.Id, updateUserPasswordVM.UpdateUserPasswordViewModel.OldPassword, updateUserPasswordVM.UpdateUserPasswordViewModel.Password, updateUserPasswordVM.UpdateUserPasswordViewModel.ConfirmPassword);
        var updatePasswordResult = await _authService.UpdatePassword(id.ToString(), updatePassword);
        if (!updatePasswordResult.IsValid)
        {
            AddError(updatePasswordResult);
            TempData["Failure"] = "Falha ao atualizar senha: " + string.Join("; ", GetModelStateErrors());
            return View("UpdateProfile", updateUserVM);
        }
        TempData["Success"] = "Senha atualizada com sucesso!";
        return RedirectToAction(nameof(UpdateProfile));
    }
}
