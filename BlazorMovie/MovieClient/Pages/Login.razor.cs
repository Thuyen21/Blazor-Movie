using BlazorMovie.Shared.Account;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Login
{
    private readonly LoginModel login = new LoginModel();

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

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/Login", login);
        if (response.IsSuccessStatusCode)
        {
            alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            accountService.checkAuthentication();
            _navigationManager.NavigateTo("/");
        }
        else
        {
            alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
