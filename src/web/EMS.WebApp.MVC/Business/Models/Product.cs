namespace EMS.WebApp.MVC.Business.Models;

public class Product : Entity
{
    public Guid CompanyId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal UnitaryValue { get; private set; }
    public string Image { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }
    public Tenant Tenant { get; private set; }

    public Product() { }

    public Product(Guid companyId, Guid tenantId, string title, string description, decimal unitaryValue, string image, bool isActive)
    {
        CompanyId = companyId;
        TenantId = tenantId;
        Title = title;
        Description = description;
        UnitaryValue = unitaryValue;
        Image = image;
        IsActive = isActive;
    }

    #region Setters
    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetUnitaryValue(decimal unitaryValue)
    {
        UnitaryValue = unitaryValue;
    }

    public void SetImage(string image)
    {
        Image = image;
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
    #endregion
}