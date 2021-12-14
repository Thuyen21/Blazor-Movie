﻿using Microsoft.AspNetCore.Components.WebView.Maui;
using MovieClient.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MovieClient;

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

		builder.Services.AddBlazorWebView();
		builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddOptions();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddSingleton<AccountService>();
		builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
		builder.Services.AddMudServices();

		return builder.Build();
	}
}
