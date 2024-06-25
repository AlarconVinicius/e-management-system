using EMS.Core.Enums;
using EMS.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.Clients;

public class ClientResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id da Empresa")]
    public Guid CompanyId { get; set; }

    [DisplayName("Nome")]
    public string Name { get; set; }

    [DisplayName("Sobrenome")]
    public string LastName { get; set; }

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("Celular")]
    public string PhoneNumber { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Documento")]
    public string Document { get; set; }

    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ClientResponse() { }

    public ClientResponse(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERoleCore role, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = document;
        Role = role;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
