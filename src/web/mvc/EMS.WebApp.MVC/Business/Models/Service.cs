namespace EMS.WebApp.MVC.Business.Models;

public class Service : Entity
{
    public Guid CompanyId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }
    public Tenant Tenant { get; private set; }

    public Service() { }

    public Service(Guid companyId, Guid tenantId, string title, string description, decimal value)
    {
        CompanyId = companyId;
        TenantId = tenantId;
        Title = title;
        Description = description;
        Value = value;
        IsActive = true;
    }
}
