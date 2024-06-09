using EMS.Core.Configuration;
using EMS.WebApp.SPA;
using EMS.WebApp.SPA.Configuration;
using EMS.WebApp.SPA.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddHttpClient(
                    WebConfigurationDefault.HttpClientName, 
                    options =>
                    {
                        options.BaseAddress = new Uri(ConfigurationDefault.ApiUrl);
                    }
                );

builder.Services.AddTransient<IClientHandler, ClientHandler>();

await builder.Build().RunAsync();
