using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Business.Models;

public class CompanyViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome da Empresa")]
    public string Name { get; set; }

    [DisplayName("Logo da Empresa")]
    public string Brand { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF/CNPJ")]
    public string Document { get; set; }

    public CompanyViewModel() { }

    public CompanyViewModel(Guid id, Guid planId, string name, string document, string brand = "")
    {
        Id = id;
        PlanId = planId;
        Name = name;
        Document = document;
        Brand = brand;
    }
}

public class RegisterCompanyViewModel
{
    public CompanyViewModel Company { get; set; }
    public EmployeeViewModel Employee { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    [DisplayName("Senha")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    [DisplayName("Confirme sua senha")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
