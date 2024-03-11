using EMS.Core.DomainObjects;

namespace EMS.Users.API.Models;

public class Address : Entity
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public Guid UserId { get; private set; }

    // EF Relation
    public User User { get; protected set; }

    public Address(string street, string number, string complement, string neighborhood, string zipCode, string city, string state, Guid userId)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
        UserId = userId;
    }
}
