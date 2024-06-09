using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Extensions;

public class NotificationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var successMessage = TempData["Success"] as string;
        var failureMessage = TempData["Failure"] as string;

        if (!string.IsNullOrEmpty(successMessage))
        {
            ViewData["MessageType"] = "success";
            ViewData["Message"] = successMessage;
        }
        else if (!string.IsNullOrEmpty(failureMessage))
        {
            ViewData["MessageType"] = "danger";
            ViewData["Message"] = failureMessage;
        }

        return View();
    }
}
