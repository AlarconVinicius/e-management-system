using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.Identities;

public class UserResponse
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }

    public UserResponse() { }

    public UserResponse(Guid id, string email)
    {
        Id = id;
        Email = email;
    }
}
