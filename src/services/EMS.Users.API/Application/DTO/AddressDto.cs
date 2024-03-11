using EMS.Core.DomainObjects;
using FluentValidation;

namespace EMS.Users.API.Application.DTO;

public class AddressAddDto : BaseDTO
{
    public Guid UserId { get; private set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Complement { get; set; }
    public string Neighborhood { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    public AddressAddDto()
    {
    }

    public AddressAddDto(string street, string number, string complement,
        string neighborhood, string zipCode, string city, string state) : this()
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
    }

    public AddressAddDto(string street, string number, string complement,
        string neighborhood, string zipCode, string city, string state, Guid userId) : this(street, number, complement,
        neighborhood, zipCode, city, state)
    {
        UserId = userId;
    }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }

    public override bool IsValid()
    {
        SetValidationResult(new AddressValidation().Validate(this));
        return GetValidationResult().IsValid;
    }

    public class AddressValidation : AbstractValidator<AddressAddDto>
    {
        public AddressValidation()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithMessage("Inform the User Id");

            RuleFor(c => c.Street)
                .NotEmpty()
                .WithMessage("Inform the Street");

            RuleFor(c => c.Number)
                .NotEmpty()
                .WithMessage("Inform the Number");

            RuleFor(c => c.ZipCode)
                .NotEmpty()
                .WithMessage("Inform the Zip Code");

            RuleFor(c => c.Neighborhood)
                .NotEmpty()
                .WithMessage("Inform the Neighborhood");

            RuleFor(c => c.City)
                .NotEmpty()
                .WithMessage("Inform the City");

            RuleFor(c => c.State)
                .NotEmpty()
                .WithMessage("Inform the State");
        }
    }
}

public record AddressViewDto(Guid UserId, string Street, string Number, string Complement, string Neighborhood, string ZipCode, string City, string State);
