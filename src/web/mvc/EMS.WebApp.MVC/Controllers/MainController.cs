﻿using Azure;
using EMS.WebApp.Business.Notifications;
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

    protected bool IsValidOperation()
    {
        return !_notifier.HasNotification();
    }
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
    protected bool HasErrorsInResponse(ModelStateDictionary modelState)
    {
        return !modelState.IsValid;
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
    protected void AddError(string message)
    {
        ModelState.AddModelError(string.Empty, message);
    }

    protected List<string> GetModelStateErrors()
    {
        var errors = new List<string>();
        if (!ModelState.IsValid)
        {
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
        }
        return errors;
    }
}