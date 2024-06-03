using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EMS.Core.Extensions;
using EMS.Core.Enums;

namespace EMS.Core.Requests.Clients;
public class CreateClientRequest : CompaniesRequest
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Sobrenome")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    [DisplayName("E-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Celular")]
    public string PhoneNumber { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; } = ERoleCore.Client;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    public CreateClientRequest()
    {}
    public CreateClientRequest(Guid companyId, string name, string lastName, string email, string phoneNumber, string cpf, ERoleCore role, bool isActive) : base(companyId)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Cpf = cpf;
        Role = role;
        IsActive = isActive;
    }
}
