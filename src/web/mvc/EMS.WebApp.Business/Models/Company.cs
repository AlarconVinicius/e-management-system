using EMS.Core.DomainObjects;

namespace EMS.WebApp.Business.Models;

public class Company : Entity
{
    public Guid PlanId { get; private set; }
    public string Name { get; private set; }
    public Cpf Document { get; private set; }
    public bool IsActive { get; private set; }

    public Plan Plan { get; private set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public Company() { }

    public Company(Guid planId, string name, string cpfOrCnpj, bool isActive)
    {
        PlanId = planId;
        Name = name;
        Document = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
    public Company(Guid id, Guid planId, string name, string cpfOrCnpj, bool isActive)
    {
        ChangeId(id);
        PlanId = planId;
        Name = name;
        Document = new Cpf(cpfOrCnpj);
        IsActive = isActive;
    }
}