namespace EMS.WebApp.MVC.Business.Models.Users;

public class Subscriber : User
{
    public Subscriber()
    {
        
    }
    public Subscriber(Guid id, string name, string email, string cpf) : base(id, name, email, cpf)
    {

    }
}
