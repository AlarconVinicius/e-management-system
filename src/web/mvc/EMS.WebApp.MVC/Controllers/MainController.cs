using EMS.Core.Notifications;
using EMS.WebApp.MVC.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebApp.MVC.Controllers;

public class MainController : Controller
{
    private readonly INotifier _notifier;

    protected MainController(INotifier notifier)
    {
        _notifier = notifier;
    }

    //protected bool IsValidOperation()
    //{
    //    return !_notifier.HasNotification();
    //}
    protected void Notify(ValidationResult validationResult)
    {
        if (!validationResult.IsValid && validationResult.Errors.Any())
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }
    }

    protected void Notify(string message)
    {
        _notifier.Handle(new Notification(message));
    }
    protected void AddError(ValidationResult response)
    {
        if (!response.IsValid && response.Errors.Any())
        {
            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }
        }
    }

    protected async Task<List<string>> GetNotificationErrors()
    {
        var errors = new List<string>();
        var notificacoes = await Task.FromResult(_notifier.GetNotifications());
        notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));
        errors.AddRange(notificacoes.Select(error => error.Message));
        return errors;
    }protected List<string> GetModelStateErrors()
    {
        var errors = new List<string>();
        if (!ModelState.IsValid)
        {
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
        }
        return errors;
    }

    //MVC
    protected bool HasErrorsInResponse(ModelStateDictionary modelState)
    {
        return !modelState.IsValid;
    }
    protected bool HasErrorsInResponse(CustomHttpResponse response)
    {
        if (response?.Errors?.Any() == true)
        {
            foreach (var mensagem in response.Errors)
            {
                ModelState.AddModelError(string.Empty, mensagem);
            }

            return true;
        }
        return false;
    }
    protected void AddError(string message)
    {
        ModelState.AddModelError(string.Empty, message);
    }

    protected bool IsValidOperation()
    {
        return ModelState.ErrorCount == 0;
    }
}