using MauiApp2.Data;
using MauiApp2.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System;
using System.Net.Http;

namespace MauiApp2
{
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

            builder.Services.AddBlazorWebView();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/"),/*DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,*/ /*DefaultRequestVersion = Version.Parse("3.0")*/ });


            return builder.Build();
        }
    }
}