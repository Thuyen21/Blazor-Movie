using BlazorApp3.Client;
using BlazorApp3.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)/*, DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher, DefaultRequestVersion = Version.Parse("3.0")*/ });
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();
