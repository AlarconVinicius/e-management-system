namespace EMS.WebApp.MVC.Business.Models;

public class Address : Entity
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? ClientId { get; private set; }

    public User User { get; protected set; }
    public Client Client { get; protected set; }

    protected Address() { }

    public Address(string street, string number, string complement, string neighborhood, string zipCode, string city, string state, Guid userId, Guid clientId)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
        if (!UserIdAndClientIdAreValid(userId, clientId))
        {
            throw new ArgumentException("Exactly one of UserId or ClientId must be provided.");
        }
        UserId = userId != Guid.Empty ? userId : null;
        ClientId = clientId != Guid.Empty ? clientId : null;
    }

    private static bool UserIdAndClientIdAreValid(Guid userId, Guid clientId)
    {
        return (userId != Guid.Empty && clientId == Guid.Empty) || (userId == Guid.Empty && clientId != Guid.Empty);
    }
}