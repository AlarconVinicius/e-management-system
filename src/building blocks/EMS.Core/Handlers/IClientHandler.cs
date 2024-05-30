using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;

namespace EMS.Core.Handlers;
public interface IClientHandler
{
    Task<Response<ClientResponse>> CreateAsync(CreateClientRequest request);
    Task<Response<ClientResponse>> UpdateAsync(UpdateClientRequest request);
    Task<Response<ClientResponse>> DeleteAsync(DeleteClientRequest request);
    Task<Response<ClientResponse>> GetByIdAsync(GetClientByIdRequest request);
    Task<PagedResponse<List<ClientResponse>>> GetAllAsync(GetAllClientsRequest request);
}