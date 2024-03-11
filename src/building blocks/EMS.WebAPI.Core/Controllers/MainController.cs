using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected ICollection<string> Errors = new List<string>();

    protected ActionResult CustomResponse(object result = null!)
    {
        if (OperationValid())
        {
            return Ok(result);
        }

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", Errors.ToArray() }
        }));
    }

    protected ActionResult CustomResponse(string error)
    {
        AddProcessingError(error);
        return CustomResponse();
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            AddProcessingError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            AddProcessingError(error.ErrorMessage);
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

    protected bool OperationValid()
    {
        return !Errors.Any();
    }

    protected void AddProcessingError(string error)
    {
        Errors.Add(error);
    }

    protected void ClearProcessingErrors()
    {
        Errors.Clear();
    }
}