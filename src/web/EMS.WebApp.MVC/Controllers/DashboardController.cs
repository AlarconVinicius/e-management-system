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

    public DashboardController(IAspNetUser appUser, IUserRepository userRepository, IUserService userService)
    {
        _appUser = appUser;
        _userRepository = userRepository;
        _userService = userService;
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
            return View(updateUserVM);
        }
        return RedirectToAction(nameof(UpdateProfile));
    }
}
