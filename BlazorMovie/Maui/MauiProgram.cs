using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using BlazorMovie.Shared;
using System.Net.Http;
using MovieClient.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MovieClient;
using System;

namespace Maui
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