using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Identities;

public class UpdateUserPasswordRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Compare("NewPassword", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;

    public UpdateUserPasswordRequest() { }

    public UpdateUserPasswordRequest(Guid id, string oldPassword, string newPassword, string confirmNewPassword)
    {
        Id = id;
        OldPassword = oldPassword;
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;
    }
}
