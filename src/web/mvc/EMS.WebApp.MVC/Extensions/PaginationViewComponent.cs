using EMS.WebApp.MVC.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Extensions;

public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList pagedModel)
    {
        return View(pagedModel);
    }
}
