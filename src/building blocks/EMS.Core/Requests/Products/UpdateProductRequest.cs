using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Products;

public class UpdateProductRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Descrição")]
    public string Description { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Preço Unitário")]
    public decimal UnitaryValue { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    public UpdateProductRequest() { }

    public UpdateProductRequest(Guid id, Guid companyId, string title, string description, decimal unitaryValue, bool isActive) : base(companyId)
    {
        Id = id;
        Title = title;
        Description = description;
        UnitaryValue = unitaryValue;
        IsActive = isActive;
    }
}
