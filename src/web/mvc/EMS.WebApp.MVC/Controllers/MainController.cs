using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebApp.MVC.Controllers;

public class MainController : Controller
{
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