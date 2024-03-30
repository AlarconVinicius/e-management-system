using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.Subscription;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
[Route("dashboard")]
public class DashboardController : MainController
{
    private readonly IAspNetUser _appUser;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly ISubscriberService _subscriberService;

    public DashboardController(IAspNetUser appUser, ISubscriberRepository subscriberRepository, ISubscriberService subscriberService)
    {
        _appUser = appUser;
        _subscriberRepository = subscriberRepository;
        _subscriberService = subscriberService;
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
        var subscriberDb = await _subscriberRepository.GetById(id);
        //var employeeDb = await _employeeRepository.GetById(id);

        UserViewModel userViewModel;
        if (subscriberDb is not null)
        {
            userViewModel = new UserViewModel(subscriberDb.Id, subscriberDb.Name, subscriberDb.Email.Address, subscriberDb.Cpf.Number);
        }
        //else if (employeeDb != null)
        //{
        //    userViewModel = new UserViewModel(employeeDb.Id, employeeDb.Name, employeeDb.Email.Address, employeeDb.EmployeeId);
        //}
        else
        {
            //userViewModel = new UserViewModel();
            return NotFound();
        }

        var updateUserViewModel = new UpdateUserViewModel();
        //var updateUserViewModel = new UpdateUserViewModel
        //{
        //    Id = userViewModel.Id,
        //    Name = userViewModel.Name,
        //    Email = userViewModel.Email
        //};

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
        var subscriberDb = await _subscriberRepository.GetById(id);

        if (subscriberDb != null)
        {
            var subsResult = await _subscriberService.UpdateSubscriber(id, updatedUser);
            if (!subsResult.IsValid)
            {
                AddError(subsResult);
                return View();
            }
        }
        return RedirectToAction(nameof(UpdateProfile));
    }
}
