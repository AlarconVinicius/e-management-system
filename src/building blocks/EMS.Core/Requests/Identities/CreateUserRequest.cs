using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Identities;

public class CreateUserRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public CreateUserRequest() { }

    public CreateUserRequest(Guid id, string email, string password, string confirmPassword)
    {
        Id = id;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
    }
}
