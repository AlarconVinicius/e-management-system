using EMS.WebApi.Business.Models;
using EMS.Core.Notifications;
using FluentValidation;
using FluentValidation.Results;
using EMS.WebApi.Business.Utils;
using Microsoft.Extensions.Logging;

namespace EMS.WebApi.Business.Services;

public class MainService
{
    private readonly INotifier _notifier;
    protected ValidationResult _validationResult;
    public readonly IAspNetUser AppUser;
    protected Guid UserId { get; set; }
    protected Guid TenantId = Guid.Empty;
    protected bool IsUserAuthenticated { get; set; }

    protected MainService(INotifier notifier, IAspNetUser appUser)
    {
        _notifier = notifier;
        _validationResult = new ValidationResult();

        AppUser = appUser;

        //if (appUser.IsAuthenticated())
        //{
        //    UserId = appUser.GetUserId();
        //    IsUserAuthenticated = true;
        TenantId = Guid.Parse("3eb1ed86-802c-4355-8045-482c274ac6ca");
        //TenantId = AppUser.GetTenantId() != Guid.Empty ? _aspNetUser.GetTenantId() : Guid.Empty;

        //}
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