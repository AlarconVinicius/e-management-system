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
    [DisplayName("Duração")]
    public TimeSpan Duration { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    public UpdateServiceRequest() { }

    public UpdateServiceRequest(Guid id, Guid companyId, string name, decimal price, TimeSpan duration, bool isActive) : base(companyId)
    {
        Id = id;
        Name = name;
        Price = price;
        Duration = duration;
        IsActive = isActive;
    }
}
