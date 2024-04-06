using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface IClientRepository : IRepository<Client>
{
    void AddClient(Client client);
    void UpdateClient(Client client);
    Task<bool> DeleteClient(Client client);
    Task<PagedResult<Client>> GetAllClients(int pageSize, int pageIndex, string query = null);
    Task<Client> GetById(Guid id);
}
