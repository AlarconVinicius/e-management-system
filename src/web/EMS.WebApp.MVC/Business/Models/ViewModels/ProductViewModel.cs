using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class ProductViewModel
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

    [DisplayName("Imagem")]
    public string Image { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Ativo")]
    public bool IsActive { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }
}
