namespace EMS.WebApp.Business.Models;
public class Client : User
{
    public Client()
    {
        
    }
    public Client(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, string role) : base(id, companyId, name, lastName, email, phoneNumber, document, role)
    {
    }
}
