using EMS.Core.Requests.Clients;
using EMS.Core.Responses.Clients;
using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Mappings;

public static class ClientMappings
{
    public static ClientResponse MapClientToClientResponse(this Client client)
    {
        if (client == null)
        {
            return null;
        }

        return new ClientResponse
        {
            Id = client.Id,
            CompanyId = client.CompanyId,
            Name = client.Name,
            LastName = client.LastName,
            Email = client.Email.Address,
            PhoneNumber = client.PhoneNumber,
            Document = client.Document.Number,
            Role = client.Role.MapERoleToERoleCore(),
            IsActive = client.IsActive,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt
        };
    }

    public static Client MapClientResponseToClient(this ClientResponse clientResponse)
    {
        if (clientResponse == null)
        {
            return null;
        }

        return new Client(clientResponse.Id, clientResponse.CompanyId, clientResponse.Name, clientResponse.LastName, clientResponse.Email, clientResponse.PhoneNumber, clientResponse.Document, clientResponse.Role.MapERoleCoreToERole());
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