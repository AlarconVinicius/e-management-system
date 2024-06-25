using EMS.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Companies;

public class CreateCompanyRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome da Empresa")]
    public string Name { get; set; }

    [DisplayName("Logo da Empresa")]
    public string Brand { get; set; }

    [Cpf]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF/CNPJ")]
    public string Document { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; } = true;

    public CreateCompanyRequest() { }

    public CreateCompanyRequest(Guid planId, string name, string brand, string document, bool isActive)
    {
        PlanId = planId;
        Name = name;
        Brand = brand;
        Document = document;
        IsActive = isActive;
    }
}
