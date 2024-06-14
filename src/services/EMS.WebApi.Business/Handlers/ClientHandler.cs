using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Clients;
using EMS.Core.Responses;
using EMS.Core.Responses.Clients;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;

namespace EMS.WebApi.Business.Handlers;

public class ClientHandler : BaseHandler, IClientHandler
{
    public readonly IClientRepository _clientRepository;
    public readonly ICompanyRepository _companyRepository;

    public ClientHandler(INotifier notifier, IAspNetUser appUser, IClientRepository clientRepository, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _clientRepository = clientRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ClientResponse> GetByIdAsync(GetClientByIdRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            var client = await _clientRepository.GetByIdAsync(request.Id, TenantId);

            if (client is null)
            {
                Notify("Cliente não encontrado.");
                return null;
            }
            return client.MapClientToClientResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o cliente.");
            return null;
        }
    }

    public async Task<PagedResponse<ClientResponse>> GetAllAsync(GetAllClientsRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            return (await _clientRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, TenantId, request.Query)).MapPagedClientsToPagedResponseClients();
        }
        catch
        {
            Notify("Não foi possível consultar os clientes.");
            return null;
        }
    }

    public async Task CreateAsync(CreateClientRequest request)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return;
        if (TenantIsEmpty()) return;
        if (IsCpfInUse(request.Document, TenantId)) return;
        if (!CompanyExists(TenantId)) return;

        request.CompanyId = TenantId;
        var clientMapped = request.MapCreateClientRequestToClient();
        try
        {
            await _clientRepository.AddAsync(clientMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o cliente.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateClientRequest request)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return;
        if (TenantIsEmpty()) return;
        if (!UserExists(request.Id, TenantId)) return;
        var clientDb = await _clientRepository.GetByIdAsync(request.Id, TenantId);

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

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o cliente.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteClientRequest request)
    {
        if (TenantIsEmpty()) return;
        try
        {
            if (!UserExists(request.Id, TenantId)) return;

            await _clientRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o cliente.");
            return;
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

    private bool UserExists(Guid id, Guid companyId)
    {
        if (_clientRepository.SearchAsync(f => f.Id == id && f.CompanyId == companyId).Result.Any())
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }

    private bool CompanyExists(Guid companyId)
    {
        if (_companyRepository.GetByIdAsync(companyId).Result is not null)
        {
            return true;
        };

        Notify("TenantId não encontrado.");
        return false;
    }
}
