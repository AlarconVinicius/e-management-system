using EMS.WebApp.MVC.Business.DomainObjects;
using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Cpf { get; set; }
    public bool IsActive { get; set; }


    public UserViewModel() { }

    public UserViewModel(Guid id, Guid companyId, Guid tenantId, string name, string lastName, string email, string phoneNumber, string cpf)
    {
        Id = id;
        CompanyId = companyId;
        TenantId = tenantId;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Cpf = cpf;
        IsActive = true;
    }
}

public class RegisterUser
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 2)]
    [DisplayName("Nome")]
    public string Name { get; set; } = string.Empty;

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    [DisplayName("E-mail")]
    public string Email { get; set; } = string.Empty;

    //[Required(ErrorMessage = "O campo {0} é obrigatório")]
    //public ERole Role { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    [DisplayName("Senha")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    [DisplayName("Confirme sua senha")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class LoginUser
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
    [DisplayName("Senha")]
    public string Password { get; set; } = string.Empty;
}

public class UpdateUserViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 2)]
    [DisplayName("Nome")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    [DisplayName("E-mail")]
    public string Email { get; set; } = string.Empty;
}

public class UserUpdateUserViewModel
{
    public UserViewModel User;
    public UpdateUserViewModel UpdateUser;
}