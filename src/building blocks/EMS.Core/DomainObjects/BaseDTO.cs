using FluentValidation.Results;

namespace EMS.Core.DomainObjects;
public abstract class BaseDTO
{
    private ValidationResult _validationResult;

    protected BaseDTO()
    {
        _validationResult = new ValidationResult();
    }

    public ValidationResult GetValidationResult()
    {
        return _validationResult;
    }

    protected void SetValidationResult(ValidationResult validationResult)
    {
        _validationResult = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
