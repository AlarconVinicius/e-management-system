using EMS.WebApp.MVC.Business.DomainObjects;
using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class CompanyViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome da Empresa")]
    public string CompanyName { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF/CNPJ")]
    public string CpfOrCnpj { get; set; }

    public CompanyViewModel() { }

    public CompanyViewModel(Guid id, Guid planId, Guid tenantId, string companyName, string cpfOrCnpj)
    {
        Id = id;
        PlanId = planId;
        TenantId = tenantId;
        CompanyName = companyName;
        CpfOrCnpj = cpfOrCnpj;
    }
}

public class RegisterCompanyViewModel
{

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome da Empresa")]
    public string CompanyName { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF/CNPJ")]
    public string CpfOrCnpj { get; set; }

    //--------------------------------------------------\\

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
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    [DisplayName("Senha")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    [DisplayName("Confirme sua senha")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
