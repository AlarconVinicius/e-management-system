using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

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
}