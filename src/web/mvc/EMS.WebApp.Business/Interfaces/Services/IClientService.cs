using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Services;

public interface IClientService
{
    Task Add(Client client);
    Task Update(Client client);
    Task Delete(Guid id);
}
