using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Services;

public interface IClientService
{
    Task Add(Client client);
    Task Update(Client client);
    Task Delete(Guid id);
}
