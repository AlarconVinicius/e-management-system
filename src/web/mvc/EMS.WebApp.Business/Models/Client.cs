namespace EMS.WebApp.Business.Models;
public class Client : User
{
    public Client()
    {
        
    }
    public Client(Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role = ERole.Client) : base(companyId, name, lastName, email, phoneNumber, document, role)
    {
    }
    public Client(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role = ERole.Client) : base(id, companyId, name, lastName, email, phoneNumber, document, role)
    {
    }
}
