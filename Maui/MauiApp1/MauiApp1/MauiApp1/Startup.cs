
/* Unmerged change from project 'MauiApp1.WinUI (net6.0-windows10.0.19041)'
Before:
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using MauiApp1.Data;
using MauiApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/

/* Unmerged change from project 'MauiApp1 (net6.0-android)'
Before:
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using MauiApp1.Data;
using MauiApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/

/* Unmerged change from project 'MauiApp1 (net6.0-maccatalyst)'
Before:
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
After:
using MauiApp1.Data;
using MauiApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
*/
using MauiApp1.Data;
using MauiApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using
/* Unmerged change from project 'MauiApp1.WinUI (net6.0-windows10.0.19041)'
Before:
using Microsoft.AspNetCore.Components.Authorization;
using MauiApp1.Services;
After:
using Microsoft.Maui.Hosting;
*/

/* Unmerged change from project 'MauiApp1 (net6.0-android)'
Before:
using Microsoft.AspNetCore.Components.Authorization;
using MauiApp1.Services;
After:
using Microsoft.Maui.Hosting;
*/

/* Unmerged change from project 'MauiApp1 (net6.0-maccatalyst)'
Before:
using Microsoft.AspNetCore.Components.Authorization;
using MauiApp1.Services;
After:
using Microsoft.Maui.Hosting;
*/
Microsoft.Maui.Hosting;
using System;
using System.Net.Http;
[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace MauiApp1
{
	public class Startup : IStartup
	{
		public void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder
				.RegisterBlazorMauiWebView()
				.UseMicrosoftExtensionsServiceProviderFactory()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				})
				.ConfigureServices(services =>
				{
					services.AddBlazorWebView();
					services.AddSingleton<WeatherForecastService>();
					services.AddOptions();
					services.AddAuthorizationCore();
					services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
					services.AddScoped(
				sp => new HttpClient { BaseAddress = new Uri("https://movie213.herokuapp.com/") });
					services.AddScoped<IAccountService, AccountService>();
				});
		}
	}
}