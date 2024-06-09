using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.Core.Responses.Employees;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class ClientMappings
{
    public static ClientResponse MapClientToClientResponse(this Client client)
    {
        if (client == null)
        {
            return null;
        }

        return new ClientResponse(client.Id, client.CompanyId, client.Name, client.LastName, client.Email.Address, client.PhoneNumber, client.Document.Number, client.Role.MapERoleToERoleCore(), client.IsActive, client.CreatedAt, client.UpdatedAt);
    }

    public static Client MapClientResponseToClient(this ClientResponse clientResponse)
    {
        if (clientResponse == null)
        {
            return null;
        }

        return new Client(clientResponse.Id, clientResponse.CompanyId, clientResponse.Name, clientResponse.LastName, clientResponse.Email, clientResponse.PhoneNumber, clientResponse.Document, clientResponse.Role.MapERoleCoreToERole());
    }

    public static PagedResponse<ClientResponse> MapPagedClientsToPagedResponseClients(this PagedResult<Client> client)
    {
        if (client == null)
        {
            return null;
        }

        return new PagedResponse<ClientResponse>(client.List.Select(x => x.MapClientToClientResponse()).ToList(), client.TotalResults, client.PageIndex, client.PageSize);
    }

    public static Client MapCreateClientRequestToClient(this CreateClientRequest clientRequest)
    {
        if (clientRequest == null)
        {
            return null;
        }

        return new Client(clientRequest.CompanyId, clientRequest.Name, clientRequest.LastName, clientRequest.Email, clientRequest.PhoneNumber, clientRequest.Document, clientRequest.Role.MapERoleCoreToERole());
    }
}