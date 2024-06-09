using EMS.WebApp.Business.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    private readonly INotifier _notifier;

    public SummaryViewComponent(INotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notificacoes = await Task.FromResult(_notifier.GetNotifications());
        notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

        return View();
    }
}