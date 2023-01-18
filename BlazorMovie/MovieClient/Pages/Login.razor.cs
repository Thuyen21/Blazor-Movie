using BlazorMovie.Shared;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Login
{
    /* Creating a new instance of the LogInModel class. */
    private readonly LogInModel login = new();

    /// <summary>
    /// The function is called when the user clicks the submit button. It calls a javascript function
    /// that returns the user agent. It then sends the user agent and the user's credentials to the
    /// server. If the server returns a success status code, the user is redirected to the home page. If
    /// the server returns an error status code, the user is shown an error message
    /// </summary>
    /// <returns>
    /// The response from the server.
    /// </returns>
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
