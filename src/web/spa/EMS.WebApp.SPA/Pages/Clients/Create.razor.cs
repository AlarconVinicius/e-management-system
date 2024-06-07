using EMS.Core.Requests.Clients;
using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EMS.WebApp.SPA.Pages.Clients;

public partial class CreateClientPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateClientRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public IClientHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.CreateAsync(InputModel);
            if (result.Success)
            {
                Snackbar.Add("Cliente adicionado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/clientes");
            }
            else
                Snackbar.Add("Falha ao adicionar cliente.", Severity.Error);
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
}