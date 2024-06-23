using EMS.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Employees;

public class UpdateEmployeeRequest : CompaniesRequest
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

    [DisplayName("Salário")]
    public decimal Salary { get; set; }

    [DisplayName("Permissão")]
    public ERoleCore Role { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; }

    public UpdateEmployeeRequest() { }

    public UpdateEmployeeRequest(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, decimal salary, ERoleCore role, bool isActive) : base(companyId)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Salary = salary;
        Role = role;
        IsActive = isActive;
    }
}
