using EMS.WebAPI.Core.Services;
using EMS.WebAPI.Core.Services.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    private readonly INotifier _notifier;

    protected MainController(INotifier notifier)
    {
        _notifier = notifier;
    }
    protected ActionResult CustomResponse(object result = null!)
    {
        if (IsOperationValid())
        {
            return Ok(result);
        }

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", _notifier.GetNotifications().Select(n => n.Message.ToString()).ToArray() }
        }));
    }

    protected ActionResult CustomResponse(string error)
    {
        NotifyError(error);
        return CustomResponse();
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotifyError(errorMsg);
        }

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            NotifyError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    //protected ActionResult CustomResponse(ResponseResult resposta)
    //{
    //    ResponsePossuiErros(resposta);

    //    return CustomResponse();
    //}

    //protected bool ResponsePossuiErros(ResponseResult resposta)
    //{
    //    if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

    //    foreach (var mensagem in resposta.Errors.Mensagens)
    //    {
    //        AdicionarErroProcessamento(mensagem);
    //    }
    //    return true;
    //}

    protected void NotifyError(string message)
    {
        _notifier.Handle(new Notification(message));
    }

    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }
}