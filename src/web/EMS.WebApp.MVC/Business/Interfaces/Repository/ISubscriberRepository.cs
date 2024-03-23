using EMS.WebApp.MVC.Business.Models.Users;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface ISubscriberRepository : IRepository<Subscriber>
{
    void AddSubscriber(Subscriber subscriber);
    void UpdateSubscriber(Subscriber subscriber);
    Task<bool> DeleteSubscriber(Subscriber subscriber);
    Task<IEnumerable<Subscriber>> GetAllSubscribers();
    Task<Subscriber> GetById(Guid id);
    Task<Subscriber> GetByCpf(string cpf);
}
