using EMS.Core.Responses.Identities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EMS.Core.Requests.Identities;

public class AddOrUpdateUserClaimRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public UserClaim NewClaim { get; set; }

    public AddOrUpdateUserClaimRequest() { }

    public AddOrUpdateUserClaimRequest(Guid id, UserClaim newClaim)
    {
        Id = id;
        NewClaim = newClaim;
    }
}
