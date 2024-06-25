using System.ComponentModel;

namespace EMS.Core.Responses.Plans;

public class PlanResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Título")]
    public string Title { get; set; }

    [DisplayName("Subtítulo")]
    public string Subtitle { get; set; }

    [DisplayName("Preço")]
    public decimal Price { get; set; }

    [DisplayName("Benefícios")]
    public string Benefits { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public PlanResponse() { }

    public PlanResponse(Guid id, string title, string subtitle, decimal price, string benefits, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Title = title;
        Subtitle = subtitle;
        Price = price;
        Benefits = benefits;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
