﻿using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using MauiApp1.Data;
using Microsoft.AspNetCore.Components.Authorization;

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
					services.AddAuthorizationCore();
					services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
					services.AddBlazorWebView();
					services.AddSingleton<WeatherForecastService>();
				});
		}
	}
}