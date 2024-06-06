using EMS.Core.Handlers;
using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Mappings;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Services;

namespace EMS.WebApp.Business.Handlers;
public class ClientHandler : MainService//, IClientHandler
{
    public readonly IClientRepository _clientRepository;

    public ClientHandler(INotifier notifier, IClientRepository clientRepository) : base(notifier)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Response<ClientResponse>> GetByIdAsync(GetClientByIdRequest request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(request.Id);

            return client is null
                ? new Response<ClientResponse>(null, 200, "Cliente não encontrado.")
                : new Response<ClientResponse>(client.MapClientToClientResponse());
        }
        catch
        {
            return new Response<ClientResponse>(null, 500, "Não foi possível recuperar o cliente.");
        }
    }

    //public async Task<PagedResponse<List<ClientResponse>>> GetAllAsync(GetAllClientsRequest request)
    //{
    //    try
    //    {
    //        var clients = await _clientRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, request.Query);

    //        return new PagedResponse<List<ClientResponse>>(
    //            clients.List.Select(x => x.MapClientToClientResponse()).ToList(),
    //            clients.TotalResults,
    //            clients.PageIndex,
    //            clients.PageSize);
    //    }
    //    catch
    //    {
    //        return new PagedResponse<List<ClientResponse>>(null, 500, "Não foi possível consultar os clientes.");
    //    }
    //}

    //public async Task<Response<ClientResponse>> CreateAsync(CreateClientRequest request)
    //{
    //    //if (!ExecuteValidation(new ClientValidation(), client)) return;
    //    if (IsCpfInUse(request.Cpf, request.CompanyId)) return null;
    //    var clientMapped = request.MapCreateClientRequestToClient();
    //    try
    //    {
    //        await _clientRepository.AddAsync(clientMapped);
    //        return new Response<ClientResponse>(clientMapped.MapClientToClientResponse(), 201, "Cliente criado com sucesso!");
    //    }
    //    catch
    //    {
    //        return new Response<ClientResponse>(null, 500, "Não foi possível criar o cliente.");
    //    }
    //}

    public async Task<Response<ClientResponse>> UpdateAsync(UpdateClientRequest request)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return;
        if (!await UserExists(request.Id)) return null;
        var clientDb = await _clientRepository.GetByIdAsync(request.Id);

        try
        {
            if (request.Email != clientDb.Email.Address)
            {
                clientDb.SetEmail(request.Email);
            }

            clientDb.SetName(request.Name);
            clientDb.SetLastName(request.LastName);
            clientDb.SetPhoneNumber(request.PhoneNumber);
            clientDb.SetIsActive(request.IsActive);

            await _clientRepository.UpdateAsync(clientDb);

            return new Response<ClientResponse>(null, 204, "Cliente atualizado com sucesso!");
        }
        catch
        {
            return new Response<ClientResponse>(null, 500, "Não foi possível atualizar o cliente.");
        }
    }

    public async Task<Response<ClientResponse>> DeleteAsync(DeleteClientRequest request)
    {
        try
        {
            if (!await UserExists(request.Id)) return null;

            await _clientRepository.DeleteAsync(request.Id);

            return new Response<ClientResponse>(null, 204, "Cliente deletado com sucesso!");
        }
        catch
        {
            return new Response<ClientResponse>(null, 500, "Não foi possível deletar o cliente.");
        }
    }

    private bool IsCpfInUse(string cpf, Guid companyId)
    {
        if (_clientRepository.SearchAsync(f => f.Document.Number == cpf && f.CompanyId == companyId).Result.Any())
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }

    private async Task<bool> UserExists(Guid id)
    {
        var userExist = await _clientRepository.GetByIdAsync(id);

        if (userExist != null)
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }
}
