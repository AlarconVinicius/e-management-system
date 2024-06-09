using EMS.Core.Enums;
using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Models;

public class EmployeeViewModel
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id da Companhia")]
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
    [DisplayName("Documento")]
    public string Document { get; set; }

    [DisplayName("Salário")]
    public decimal Salary { get; set; }

    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; }

    [DisplayName("Ativo")]
    public bool IsActive { get; set; } = true;

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }


    public EmployeeViewModel() { }
    public EmployeeViewModel(string name, string lastName, string email, string phoneNumber, string cpf, ERoleCore role, decimal salary)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = cpf;
        IsActive = true;
        Role = role;
        Salary = salary;
    }
    public EmployeeViewModel(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string cpf, ERoleCore role, decimal salary)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = cpf;
        IsActive = true;
        Role = role;
        Salary = salary;
    }
    public EmployeeViewModel(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string cpf, ERoleCore role, decimal salary, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = cpf;
        IsActive = true;
        Role = role;
        Salary = salary;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}