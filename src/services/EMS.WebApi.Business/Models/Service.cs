using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EMS.WebApi.Business.Models;
public class Service : Entity
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool IsActive { get; set; }

    public Company Company { get; set; }

    public ICollection<ServiceAppointment> ServiceAppointments { get; set; }

    public Service() { }

    public Service(Guid companyId, string name, decimal price, TimeSpan duration, Company company, bool isActive)
    {
        CompanyId = companyId;
        Name = name;
        Price = price;
        Duration = duration;
        Company = company;
        IsActive = isActive;
    }

    public Service(Guid id, Guid companyId, string name, decimal price, TimeSpan duration, Company company, bool isActive)
    {
        SetId(id);
        CompanyId = companyId;
        Name = name;
        Price = price;
        Duration = duration;
        Company = company;
        IsActive = isActive;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
    }

    public void SetDuration(TimeSpan duration)
    {
        Duration = duration;
    }
}