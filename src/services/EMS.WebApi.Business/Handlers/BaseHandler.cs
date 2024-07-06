using EMS.Core.Notifications;
using EMS.Core.User;
using EMS.WebApi.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace EMS.WebApi.Business.Handlers;

public class BaseHandler
{
    private readonly INotifier _notifier;
    protected ValidationResult _validationResult;
    public readonly IAspNetUser AppUser;
    protected Guid UserId { get; set; }
    protected Guid TenantId = Guid.Empty;
    protected bool IsUserAuthenticated { get; set; }

    protected BaseHandler(INotifier notifier, IAspNetUser appUser)
    {
        _notifier = notifier;
        _validationResult = new ValidationResult();

        AppUser = appUser;

        if (appUser.IsAuthenticated())
        {
            UserId = appUser.GetUserId();
            IsUserAuthenticated = true;
            TenantId = appUser.GetTenantId() != Guid.Empty ? appUser.GetTenantId() : Guid.Empty;
        }
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
    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }
    protected void ClearNotifications()
    {
        _notifier.Clear();
        return;
    }
    protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }

    protected bool TenantIsEmpty()
    {
        if (TenantId == Guid.Empty)
        {
            Notify("TenantId não encontrado.");
            return true;
        }
        return false;
    }
}