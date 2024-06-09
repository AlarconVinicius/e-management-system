using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;

namespace EMS.Core.Handlers;

public interface IClientHandler
{
    Task CreateAsync(CreateClientRequest request);
    Task UpdateAsync(UpdateClientRequest request);
    Task DeleteAsync(DeleteClientRequest request);
    Task<ClientResponse> GetByIdAsync(GetClientByIdRequest request);
    Task<PagedResponse<ClientResponse>> GetAllAsync(GetAllClientsRequest request);
}