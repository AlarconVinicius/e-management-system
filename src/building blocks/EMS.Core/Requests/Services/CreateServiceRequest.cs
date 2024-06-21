using EMS.Core.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Services;

public class CreateServiceRequest : CompaniesRequest
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Preço")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [RegularExpression(RegexUtils.TimeSpanWithoutHourPattern, ErrorMessage = "O campo {0} deve estar no formato hh:mm.")]
    [DefaultValue("00:10")]
    [DisplayName("Duração")]
    public string Duration { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    public CreateServiceRequest() { }

    public CreateServiceRequest(Guid companyId, string name, decimal price, string duration, bool isActive) : base(companyId)
    {
        Name = name;
        Price = price;
        Duration = duration;
        IsActive = isActive;
    }
}