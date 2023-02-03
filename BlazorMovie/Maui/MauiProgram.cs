using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Logging;
using MovieClient;
using MovieClient.Services;
using MudBlazor.Services;

namespace Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        /* Creating a new instance of the MauiAppBuilder class. */
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        /* Adding the MauiBlazorWebView to the service collection. */
        builder.Services.AddMauiBlazorWebView();
        /* Adding the BlazorWebViewDeveloperTools to the service collection. */
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        /* Clearing the logging providers. */
        builder.Logging.ClearProviders();
        /* Adding a console logger to the service collection. */
        builder.Logging.AddConsole();
        /* Adding the BlazorWebView to the service collection. */
        builder.Services.AddBlazorWebView();
        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/") });
        /* Adding a new HttpClient to the service collection. */
        builder.Services.AddHttpClient("Movie213.ServerAPI", client => client.BaseAddress = new Uri("https://movie213.herokuapp.com/"));
        /* Adding a new HttpClient to the service collection. */
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Movie213.ServerAPI"));
        /* Adding the Options pattern to the service collection. */
        builder.Services.AddOptions();
        /* Adding the AuthorizationCore to the service collection. */
        builder.Services.AddAuthorizationCore();
        /* Adding the AccountService to the service collection. */
        builder.Services.AddScoped<AccountService>();
        /* Adding the `CustomAuthenticationStateProvider` to the service collection. */
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        /* Adding the MudBlazor services to the service collection. */
        builder.Services.AddMudServices();
        /* Adding the `ShowAlertService` to the service collection. */
        builder.Services.AddScoped<ShowAlertService>();

        /* Returning the MauiAppBuilder class. */
        return builder.Build();
    }
}
