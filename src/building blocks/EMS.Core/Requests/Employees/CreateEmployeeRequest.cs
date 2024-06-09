using EMS.Core.Enums;
using EMS.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Employees;

public class CreateEmployeeRequest : CompaniesRequest
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

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Documento")]
    public string Document { get; set; }

    [DisplayName("Salário")]
    public decimal Salary { get; set; }

    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; } = ERoleCore.Employee;

    [DisplayName("Ativo")]
    public bool IsActive { get; set; } = true;

    public CreateEmployeeRequest() { }

    public CreateEmployeeRequest(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, decimal salary, ERoleCore role, bool isActive) : base(companyId)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = document;
        Salary = salary;
        Role = role;
        IsActive = isActive;
    }
}
