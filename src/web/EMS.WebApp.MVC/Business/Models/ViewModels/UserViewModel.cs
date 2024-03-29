﻿using EMS.WebApp.MVC.Business.DomainObjects;
using EMS.WebApp.MVC.Business.Models.Users;
using EMS.WebApp.MVC.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public abstract class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public bool Deleted { get; private set; }

    public Address Address { get; private set; }

    // EF Relation
    protected UserViewModel() { }

    public UserViewModel(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        Deleted = false;
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