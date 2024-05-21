using EMS.Core.DomainObjects;

namespace EMS.WebApp.Business.Models;

public class Company : Entity
{
    public Guid PlanId { get; private set; }
    public string Name { get; private set; }
    public string Brand { get; private set; }
    public Cpf Document { get; private set; }
    public bool IsActive { get; private set; }

    public Plan Plan { get; private set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public Company() { }

    public Company(Guid planId, string name, string brand, string cpfOrCnpj, bool isActive)
    {
        PlanId = planId;
        Name = name;
        Brand = brand;
        Document = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
    public Company(Guid id, Guid planId, string name, string brand, string cpfOrCnpj, bool isActive)
    {
        SetId(id);
        PlanId = planId;
        Name = name;
        Brand = brand;
        Document = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }

    public void BlockCompany(bool status)
    {
        IsActive = status;
    }
}