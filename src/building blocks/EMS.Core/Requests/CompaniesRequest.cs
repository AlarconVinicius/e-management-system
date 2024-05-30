using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EMS.Core.Requests;

public abstract class CompaniesRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id da Empresa")]
    public Guid CompanyId { get; set; }
}