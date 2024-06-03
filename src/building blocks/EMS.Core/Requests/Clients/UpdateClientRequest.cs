using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Clients;

public class UpdateClientRequest : CompaniesRequest
{
    public Guid Id { get; set; }

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

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    public UpdateClientRequest()
    {
        
    }
    public UpdateClientRequest(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, bool isActive) : base(companyId)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        IsActive = isActive;
    }
}
