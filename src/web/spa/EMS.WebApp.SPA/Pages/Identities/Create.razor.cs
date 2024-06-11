using EMS.Core.Requests.Identities;
using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EMS.WebApp.SPA.Pages.Identities;

public partial class CreatePage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateUserRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public IIdentityHandler Handler { get; set; } = null!;

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
                Snackbar.Add("Usuário criado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
            else
                Snackbar.Add("Falha ao adicionar conta no sistema.", Severity.Error);
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