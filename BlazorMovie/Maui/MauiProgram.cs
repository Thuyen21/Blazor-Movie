using Microsoft.AspNetCore.Components.Authorization;

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-android)'
Before:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using Microsoft.Extensions.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-windows10.0.19041)'
Before:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using Microsoft.Extensions.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-maccatalyst)'
Before:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using Microsoft.Extensions.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-android)'
Before:
using BlazorMovie.Shared;
using System.Net.Http;
After:
using Microsoft.Maui.Hosting;
using MovieClient;
*/

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-windows10.0.19041)'
Before:
using BlazorMovie.Shared;
using System.Net.Http;
After:
using Microsoft.Maui.Hosting;
using MovieClient;
*/

/* Unmerged change from project 'BlazorMovie.Maui (net6.0-maccatalyst)'
Before:
using BlazorMovie.Shared;
using System.Net.Http;
After:
using Microsoft.Maui.Hosting;
using MovieClient;
*/
using MovieClient;
using MovieClient.Services;
using MudBlazor.Services;
using System;
using System.Net.Http;

namespace Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddBlazorWebView();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/") });
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddMudServices();

            return builder.Build();
        }
    }
}