using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using MauiApp1.Data;
using Microsoft.AspNetCore.Components.Authorization;
using MauiApp1.Services;

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