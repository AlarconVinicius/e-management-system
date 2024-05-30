using EMS.Core.Enums;
using System.ComponentModel;

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

    [DisplayName("CPF")]
    public string Cpf { get; set; }

    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; }

    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }
}
