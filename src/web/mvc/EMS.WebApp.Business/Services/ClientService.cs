using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;

namespace EMS.WebApp.Business.Services;

public class ClientService : MainService, IClientService
{
    public readonly IClientRepository _clientRepository;

    public ClientService(INotifier notifier, IClientRepository clientRepository) : base(notifier)
    {
        _clientRepository = clientRepository;
    }

    public async Task Add(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return;
        if (IsCpfInUse(client.Id, client.Document.Number)) return;
        await _clientRepository.AddAsync(client);
        return;
    }

    public async Task Update(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return;
        if (!await UserExists(client.Id)) return;

        var clientDb = await _clientRepository.GetByIdAsync(client.Id);

        if (client.Email.Address != clientDb.Email.Address)
        {
            clientDb.SetEmail(client.Email.Address);
        }

        clientDb.SetName(client.Name);
        clientDb.SetLastName(client.LastName);
        clientDb.SetPhoneNumber(client.PhoneNumber);
        clientDb.SetIsActive(client.IsActive);

        await _clientRepository.UpdateAsync(clientDb);

        return;
    }

    public async Task Delete(Guid id)
    {
        if (!await UserExists(id)) return;

        await _clientRepository.DeleteAsync(id);

        return;
    }

    private bool IsCpfInUse(Guid id, string document)
    {
        if (_clientRepository.SearchAsync(f => f.Document.Number == document && f.Id != id).Result.Any())
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
