using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebApp.MVC.Controllers;

public class MainController : Controller
{
    protected bool HasErrorsInResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) return true;
        return false;
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

}