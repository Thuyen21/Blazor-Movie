using BlazorApp3.Client;
using BlazorApp3.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder? builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),/*DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,*/ DefaultRequestVersion = Version.Parse("3.0") });
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

await builder.Build().RunAsync();
