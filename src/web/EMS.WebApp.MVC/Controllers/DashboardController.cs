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

        UserViewModel userViewModel;
        if (userDb is not null)
        {
            userViewModel = new UserViewModel(userDb.Id, userDb.CompanyId, userDb.TenantId, userDb.Name, userDb.LastName, userDb.Email.Address, userDb.PhoneNumber, userDb.Cpf.Number, userDb.Role);
        }
        else
        {
            return NotFound();
        }

        var updateUserViewModel = new UpdateUserViewModel();

        var viewModel = new UserUpdateUserViewModel
        {
            User = userViewModel,
            UpdateUser = updateUserViewModel
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("atualizar-perfil")]
    public async Task<IActionResult> UpdateProfile(UpdateUserViewModel updatedUser, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var id = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(id);

        if (userDb != null)
        {
            var subsResult = await _userService.UpdateUser(id, updatedUser);
            if (!subsResult.IsValid)
            {
                AddError(subsResult);
                return View();
            }
        }
        return RedirectToAction(nameof(UpdateProfile));
    }
}
