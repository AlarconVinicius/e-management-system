using AutoMapper;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanRepository _planRepository;
    private readonly IMapper _mapper;

    public HomeController(IAspNetUser appUser, IPlanRepository planRepository, IMapper mapper)
    {
        _appUser = appUser;
        _planRepository = planRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Dashboard");
        var plans = _mapper.Map<List<PlanViewModel>>(await _planRepository.GetAllAsync());
        return View(plans);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("Error/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        if (_appUser.IsAuthenticated())
        {
            TempData["Template"] = "_LayoutAdmin";
        } else
        {
            TempData["Template"] = "_Layout";
        }
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