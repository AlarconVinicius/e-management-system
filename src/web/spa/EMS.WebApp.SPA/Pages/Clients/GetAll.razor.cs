using EMS.Core.Requests.Clients;
using EMS.Core.Responses.Clients;
using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EMS.WebApp.SPA.Pages.Clients;

public partial class GetAllClientsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<ClientResponse> Clients { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IDialogService Dialog { get; set; } = null!;

    [Inject]
    public IClientHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllClientsRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.Success)
                Clients = result.Data!.List.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    public async void OnDeleteButtonClickedAsync(Guid id, string name)
    {
        var result = await Dialog.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o cliente {name} será removido. Deseja continuar?",
            yesText: "Excluir",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, name);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(Guid id, string name)
    {
        try
        {
            var request = new DeleteClientRequest(id);
            await Handler.DeleteAsync(request);
            Clients.RemoveAll(x => x.Id == id);
            Snackbar.Add($"Cliente {name} removido!", Severity.Info);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}