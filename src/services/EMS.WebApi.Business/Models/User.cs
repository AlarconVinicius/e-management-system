using EMS.Core.DomainObjects;

namespace EMS.WebApi.Business.Models;

public abstract class User : Entity
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public Cpf Document { get; private set; }
    public ERole Role { get; private set; }
    public Address Address { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }
    public ICollection<ServiceAppointment> ServiceAppointments { get; set; }

    public User() { }
    protected User(Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role)
    {
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = new Email(email);
        PhoneNumber = phoneNumber;
        Document = new Cpf(document);
        IsActive = true;
        Role = role;
    }

    protected User(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role)
    {
        SetId(id);
        CompanyId = companyId;
        Name = name;
        LastName = lastName;
        Email = new Email(email);
        PhoneNumber = phoneNumber;
        Document = new Cpf(document);
        IsActive = true;
        Role = role;
    }

    #region Setters
    public void SetName(string name)
    {
        Name = name;
    }
    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }
    public void SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
    public void SetEmail(string email)
    {
        Email = new Email(email);
    }
    public void SetRole(ERole role)
    {
        Role = role;
    }
    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
    public void AssignAddress(Address address)
    {
        Address = address;
    }
    #endregion
}
