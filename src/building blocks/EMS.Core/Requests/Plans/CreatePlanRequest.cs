using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Plans;

public class CreatePlanRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Subtítulo")]
    public string Subtitle { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Preço")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Benefícios")]
    public string Benefits { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Status")]
    public bool IsActive { get; set; }

    public CreatePlanRequest() { }

    public CreatePlanRequest(string title, string subtitle, decimal price, string benefits, bool isActive)
    {
        Title = title;
        Subtitle = subtitle;
        Price = price;
        Benefits = benefits;
        IsActive = isActive;
    }
}
