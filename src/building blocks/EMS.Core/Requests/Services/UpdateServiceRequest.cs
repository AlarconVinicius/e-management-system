using EMS.Core.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Services;

public class UpdateServiceRequest : CompaniesRequest
{
    public Guid Id { get; set; }

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

    public UpdateServiceRequest() { }

    public UpdateServiceRequest(Guid id, Guid companyId, string name, decimal price, string duration, bool isActive) : base(companyId)
    {
        Id = id;
        Name = name;
        Price = price;
        Duration = duration;
        IsActive = isActive;
    }
}
