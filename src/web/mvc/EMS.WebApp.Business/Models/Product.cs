namespace EMS.WebApp.Business.Models;

public class Product : Entity
{
    public Guid CompanyId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal UnitaryValue { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }

    public Product() { }

    public Product(Guid companyId, string title, string description, decimal unitaryValue, bool isActive)
    {
        CompanyId = companyId;
        Title = title;
        Description = description;
        UnitaryValue = unitaryValue;
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

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
    #endregion
}