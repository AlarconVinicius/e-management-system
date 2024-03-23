using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _iPlanRepository;

    public HomeController(IAspNetUser appUser, IPlanRepository iPlanRepository)
    {
        _appUser = appUser;
        _iPlanRepository = iPlanRepository;
    }

    public async Task<IActionResult> Index()
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Privacy", "Home");
        var plans = (await _iPlanRepository.GetAll()).Select(new PlanViewModel().ToViewModel);
        return View(plans);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("erro/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if (id == 500)
        {
            modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.Title = "Ocorreu um erro!";
            modelErro.ErrorCode = id;
        }
        else if (id == 404)
        {
            modelErro.Message =
                "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.Title = "Ops! Página não encontrada.";
            modelErro.ErrorCode = id;
        }
        else if (id == 403)
        {
            modelErro.Message = "Você não tem permissão para fazer isto.";
            modelErro.Title = "Acesso Negado";
            modelErro.ErrorCode = id;
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelErro);
    }
}