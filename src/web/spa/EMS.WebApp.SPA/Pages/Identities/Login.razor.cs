using EMS.Core.Requests.Identities;
using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EMS.WebApp.SPA.Pages.Identities;

public partial class LoginPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public LoginUserRequest InputModel { get; set; } = new();

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
            var result = await Handler.LoginAsync(InputModel);
            if (result.Success)
            {
                Snackbar.Add("Logado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
            else
                Snackbar.Add("Falha ao logar no sistema.", Severity.Error);
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