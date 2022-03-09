using BlazorMovie.Shared;
using Microsoft.JSInterop;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Login
{
    private readonly LogInModel login = new LogInModel();
    
    private async Task HandleValidSubmit()
    {
        try
        {
            string? remoteUserAgent = await JS.InvokeAsync<string>("getUserAgent");
            login.UserAgent = remoteUserAgent;
        }
        catch
        {
            alertService.ShowAlert(Severity.Error, "Turn Off AdBlock");
            return;
        }

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/login", login);
        if (response.IsSuccessStatusCode)
        {
            alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            _accountService.checkAuthentication();
            _navigationManager.NavigateTo("/");
        }
        else
        {
            alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
