﻿using EMS.Core.Enums;
using EMS.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.Employees;

public class EmployeeResponse
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

    [DisplayName("Status")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public EmployeeResponse() { }

    public EmployeeResponse(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, decimal salary, ERoleCore role, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = document;
        Salary = salary;
        Role = role;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
