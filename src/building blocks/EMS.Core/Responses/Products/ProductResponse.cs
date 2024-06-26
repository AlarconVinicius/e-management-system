﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.Products;

public class ProductResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id da Empresa")]
    public Guid CompanyId { get; set; }

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
    [DisplayName("Status")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ProductResponse() { }

    public ProductResponse(Guid id, Guid companyId, string title, string description, decimal unitaryValue, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
        Title = title;
        Description = description;
        UnitaryValue = unitaryValue;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
