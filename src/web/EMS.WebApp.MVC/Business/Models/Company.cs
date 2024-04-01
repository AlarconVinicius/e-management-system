using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Business.Models;

public class Company : Entity
{
    public Guid PlanId { get; set; }
    public string Name { get; set; }
    public Cpf CpfOrCnpj { get; set; }
    public bool IsActive { get; set; }

    public Plan Plan { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public Company() { }

    public Company(Guid planId, string name, string cpfOrCnpj, bool isActive)
    {
        PlanId = planId;
        Name = name;
        CpfOrCnpj = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
    public Company(Guid id, Guid planId, string name, string cpfOrCnpj, bool isActive)
    {
        ChangeId(id);
        PlanId = planId;
        Name = name;
        CpfOrCnpj = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
}