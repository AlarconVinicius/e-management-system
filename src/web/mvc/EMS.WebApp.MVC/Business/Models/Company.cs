using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Business.Models;

public class Company : Entity
{
    public Guid PlanId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public Cpf CpfOrCnpj { get; private set; }
    public bool IsActive { get; private set; }

    public Plan Plan { get; private set; }
    public Tenant Tenant { get; private set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public Company() { }

    public Company(Guid planId, Guid tenantId, string name, string cpfOrCnpj, bool isActive)
    {
        PlanId = planId;
        TenantId = tenantId;
        Name = name;
        CpfOrCnpj = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
    public Company(Guid id, Guid planId, Guid tenantId, string name, string cpfOrCnpj, bool isActive)
    {
        ChangeId(id);
        PlanId = planId;
        TenantId = tenantId;
        Name = name;
        CpfOrCnpj = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
}