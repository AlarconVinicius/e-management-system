using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Business.Models.Users;

public abstract class User : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public bool Deleted { get; private set; }
    public Address Address { get; private set; }

    // EF Relation
    protected User() { }

    public User(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        Deleted = false;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
    public void ChangeEmail(string email)
    {
        Email = new Email(email);
    }
    public void AssignAddress(Address address)
    {
        Address = address;
    }
}
