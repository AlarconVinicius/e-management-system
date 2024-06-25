using EMS.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.Companies;

public class UpdateCompanyRequest : Request
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome da Empresa")]
    public string Name { get; set; }

    [DisplayName("Logo da Empresa")]
    public string Brand { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; } = true;

    public UpdateCompanyRequest() { }

    public UpdateCompanyRequest(Guid id, string name, string brand, bool isActive)
    {
        Id = id;
        Name = name;
        Brand = brand;
        IsActive = isActive;
    }
}
