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
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Services.AddBlazorWebView();
        builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/") });
        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddMudServices();
        builder.Services.AddSingleton<ShowAlertService>();

        builder.Services.AddApiAuthorization();
        return builder.Build();
    }
}
