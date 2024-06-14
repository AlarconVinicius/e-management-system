using System.ComponentModel;

namespace EMS.Core.Responses.Companies;

public class CompanyResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id do Plano")]
    public Guid PlanId { get; set; }

    [DisplayName("Nome da Empresa")]
    public string Name { get; set; }

    [DisplayName("Logo da Empresa")]
    public string Brand { get; set; }

    [DisplayName("CPF/CNPJ")]
    public string Document { get; set; }

    [DisplayName("Ativo")]
    public bool IsActive { get; set; } = true;

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public CompanyResponse() { }

    public CompanyResponse(Guid id, Guid planId, string name, string brand, string document, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        PlanId = planId;
        Name = name;
        Brand = brand;
        Document = document;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
