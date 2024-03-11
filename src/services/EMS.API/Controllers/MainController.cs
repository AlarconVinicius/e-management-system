using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    //private readonly INotifier _notifier;

    //protected MainController(INotifier notifier)
    //{
    //    _notifier = notifier;
    //}

    //protected ActionResult CustomResponse(ModelStateDictionary modelState)
    //{
    //    if (!modelState.IsValid) NotifyInvalidModelError(modelState);
    //    return CustomResponse();
    //}

    //protected ActionResult CustomResponse(object result = null!)
    //{
    //    if (IsOperationValid())
    //    {
    //        return Ok(result);
    //    }

    //    return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
    //    {
    //        { "Messages", _notifier.GetNotifications().Select(n => n.Message.ToString()).ToArray() }
    //    }));
    //}
    //protected void NotifyInvalidModelError(ModelStateDictionary modelState)
    //{
    //    var erros = modelState.Values.SelectMany(e => e.Errors);
    //    foreach (var erro in erros)
    //    {
    //        var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
    //        NotifyError(errorMsg);
    //    }
    //}
    //protected void NotifyError(string message)
    //{
    //    _notifier.Handle(new Notification(message));
    //}

    //protected bool IsOperationValid()
    //{
    //    return !_notifier.HasNotification();
    //}
}