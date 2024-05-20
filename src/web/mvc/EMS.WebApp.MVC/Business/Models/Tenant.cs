namespace EMS.WebApp.MVC.Business.Models;

public class Tenant : Entity
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
    protected Tenant() { }
    public Tenant(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }
    public void Setname(string name)
    {
        Name = name;
    }
    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}
