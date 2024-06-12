using EMS.Core.Requests.Plans;
using EMS.Core.Responses.Plans;
using EMS.Core.User;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IAspNetUser _appUser;
    private readonly IPlanHandler _planHandler;

    public HomeController(IAspNetUser appUser, IPlanHandler planHandler)
    {
        _appUser = appUser;
        _planHandler = planHandler;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        if (_appUser.IsAuthenticated()) return RedirectToAction("Index", "Dashboard");
        var ps = 10;
        var request = new GetAllPlansRequest { PageNumber = page, PageSize = ps, Query = q };
        var response = await _planHandler.GetAllAsync(request);

        var mappedPlans = new PagedViewModel<PlanResponse>
        {
            List = response.Data.List,
            PageIndex = response.Data.PageIndex,
            PageSize = response.Data.PageSize,
            Query = request.Query,
            TotalResults = response.Data.TotalResults
        };
        ViewBag.Search = q;
        mappedPlans.ReferenceAction = "Index";

        return View(mappedPlans);
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