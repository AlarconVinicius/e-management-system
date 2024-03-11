using EMS.Core.DomainObjects;
using FluentValidation;

namespace EMS.Users.API.Application.DTO;

public class UserAddDto : BaseDTO
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }

    public UserAddDto(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = email;
        Cpf = cpf;
    }

    public override bool IsValid()
    {
        SetValidationResult(new UserAddDtoValidation().Validate(this));
        return GetValidationResult().IsValid;
    }

    public class UserAddDtoValidation : AbstractValidator<UserAddDto>
    {
        public UserAddDtoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer Id");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Customer name was not informed");

            RuleFor(c => c.Cpf)
                .Must(HaveValidCpf)
                .WithMessage("The informed CPF is not valid.");

            RuleFor(c => c.Email)
                .Must(HaveValidEmail)
                .WithMessage("The informed email is not valid.");
        }

        protected static bool HaveValidCpf(string cpf)
        {
            return Core.DomainObjects.Cpf.Validate(cpf);
        }

        protected static bool HaveValidEmail(string email)
        {
            return Core.DomainObjects.Email.Validate(email);
        }
    }
}
