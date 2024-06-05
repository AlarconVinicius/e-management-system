using EMS.Core.Notifications;
using EMS.WebApi.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace EMS.WebApi.API.Controllers.Shared;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private readonly INotifier _notifier;

    protected ApiController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected IActionResult ResponseOk(object result) =>
        Response(HttpStatusCode.OK, result);

    protected IActionResult ResponseOk() =>
        Response(HttpStatusCode.OK);

    protected IActionResult ResponseCreated() =>
        Response(HttpStatusCode.Created);

    protected IActionResult ResponseCreated(object data) =>
        Response(HttpStatusCode.Created, data);

    protected IActionResult ResponseNoContent() =>
        Response(HttpStatusCode.NoContent);

    protected IActionResult ResponseNotModified() =>
        Response(HttpStatusCode.NotModified);

    protected IActionResult ResponseBadRequest(ModelStateDictionary modelState)
    {
        NotifyInvalidModelError(modelState);
        return Response(HttpStatusCode.BadRequest);
    }
    
    protected IActionResult ResponseBadRequest(string errorMessage) =>
        Response(HttpStatusCode.BadRequest, errorMessage: errorMessage);

    protected IActionResult ResponseBadRequest() =>
        Response(HttpStatusCode.BadRequest, errorMessage: "A requisição é inválida");

    protected IActionResult ResponseNotFound(string errorMessage) =>
        Response(HttpStatusCode.NotFound, errorMessage: errorMessage);

    protected IActionResult ResponseNotFound() =>
        Response(HttpStatusCode.NotFound, errorMessage: "O recurso não foi encontrado");

    protected IActionResult ResponseUnauthorized(string errorMessage) =>
        Response(HttpStatusCode.Unauthorized, errorMessage: errorMessage);

    protected IActionResult ResponseUnauthorized() =>
        Response(HttpStatusCode.Unauthorized, errorMessage: "Permissão negada");

    protected IActionResult ResponseInternalServerError() =>
        Response(HttpStatusCode.InternalServerError);

    protected IActionResult ResponseInternalServerError(string errorMessage) =>
        Response(HttpStatusCode.InternalServerError, errorMessage: errorMessage);

    protected IActionResult ResponseInternalServerError(Exception exception) =>
        Response(HttpStatusCode.InternalServerError, errorMessage: exception.Message);

    protected new JsonResult Response(HttpStatusCode statusCode, object data, string errorMessage)
    {
        CustomResult result;

        if (string.IsNullOrWhiteSpace(errorMessage) && IsOperationValid())
        {
            var success = statusCode.IsSuccess();

            if (data != null)
                result = new CustomResult(statusCode, success, data);
            else
                result = new CustomResult(statusCode, success);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(errorMessage) && !_notifier.HasNotification())
                NotifyError(errorMessage);

            var errors = _notifier.GetNotifications().Select(n => n.Message);

            result = new CustomResult(statusCode, false, errors);
        }
        return new JsonResult(result) { StatusCode = (int)result.StatusCode };
    }

    protected new JsonResult Response(HttpStatusCode statusCode, object result) =>
        Response(statusCode, result, null);

    protected new JsonResult Response(HttpStatusCode statusCode, string errorMessage) =>
        Response(statusCode, null, errorMessage);

    protected new JsonResult Response(HttpStatusCode statusCode) =>
        Response(statusCode, null, null);

    protected void NotifyInvalidModelError(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotifyError(errorMsg);
        }
    }
    protected void NotifyError(string message)
    {
        _notifier.Handle(new Notification(message));
    }
    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }
}
