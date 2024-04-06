using EMS.WebApp.MVC.Business.DomainObjects;
using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class ClientViewModel
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id da Empresa")]
    public Guid CompanyId { get; set; }

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
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }
}
