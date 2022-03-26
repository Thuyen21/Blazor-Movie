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
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        _ = builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                _ = fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        _ = builder.Logging.ClearProviders();
        _ = builder.Logging.AddConsole();
        _ = builder.Services.AddBlazorWebView();
        _ = builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/") });
        _ = builder.Services.AddOptions();
        _ = builder.Services.AddAuthorizationCore();
        _ = builder.Services.AddSingleton<AccountService>();
        _ = builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        _ = builder.Services.AddMudServices();
        _ = builder.Services.AddSingleton<ShowAlertService>();

        return builder.Build();
    }
}
