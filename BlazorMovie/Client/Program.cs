using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MovieClient;
using MovieClient.Services;
using MudBlazor.Services;

/* Creating a WebAssemblyHostBuilder object. */
WebAssemblyHostBuilder? builder = WebAssemblyHostBuilder.CreateDefault(args);

/* Adding the App component to the DOM. */
builder.RootComponents.Add<App>("#app");
/* Adding the `HeadOutlet` component to the DOM. */
builder.RootComponents.Add<HeadOutlet>("head::after");

/* Adding a new HttpClient to the service collection. */
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
/* Adding the `IOptions` service to the service collection. */
builder.Services.AddOptions();
/* Adding the `AuthorizationStateProvider` service to the service collection. */
builder.Services.AddAuthorizationCore();
/* Adding the `AccountService` to the service collection. */
builder.Services.AddScoped<AccountService>();
/* Adding the `AuthenticationStateProvider` service to the service collection. */
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
/* Adding the MudBlazor services to the service collection. */
builder.Services.AddMudServices();
/* Adding the `ShowAlertService` to the service collection. */
builder.Services.AddScoped<ShowAlertService>();

/* Building the application and running it. */
await builder.Build().RunAsync();
