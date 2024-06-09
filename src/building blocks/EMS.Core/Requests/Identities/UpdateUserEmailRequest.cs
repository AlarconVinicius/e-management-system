using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Identities;

public class UpdateUserEmailRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido.")]
    public string NewEmail { get; set; }

    public UpdateUserEmailRequest() { }

    public UpdateUserEmailRequest(Guid id, string newEmail)
    {
        Id = id;
        NewEmail = newEmail;
    }
}
