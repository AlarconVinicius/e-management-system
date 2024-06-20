using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.Services;

public class ServiceResponse
{
    [DisplayName("Id")]
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

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ServiceResponse() { }

    public ServiceResponse(Guid id, string name, decimal price, TimeSpan duration, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Price = price;
        Duration = duration;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
