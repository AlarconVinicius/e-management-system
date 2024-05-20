using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace EMS.WebApp.Business.Services;

public class MainService
{
    private readonly INotifier _notifier;
    protected ValidationResult _validationResult;

    protected MainService(INotifier notifier)
    {
        _notifier = notifier;
        _validationResult = new ValidationResult();
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string message)
    {
        _notifier.Handle(new Notification(message));
        _validationResult.Errors.Add(new ValidationFailure(string.Empty, message));
    }

    protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }
}