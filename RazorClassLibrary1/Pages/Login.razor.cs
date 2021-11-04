using BlazorApp3.Shared;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class Login
{
    private string divCSS = "display: none;";
    private void DivCSS(string divCSS)
    {
        this.divCSS = divCSS;
    }

    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private readonly LogInModel login = new LogInModel();
    private string? content;
    private async Task HandleValidSubmit()
    {
        try
        {
            string? remoteUserAgent = await JS.InvokeAsync<string>("getUserAgent");
            login.UserAgent = remoteUserAgent;
        }
        catch
        {
            content = "Turn Off AdBlock";
            severity = Severity.Error;
            showAlert = true;
            return;
        }

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LogInModel>("user/login", login);
        if (response.IsSuccessStatusCode)
        {
            content = await response.Content.ReadAsStringAsync();
            severity = Severity.Success;
            showAlert = true;
            _accountService.Login();
            _navigationManager.NavigateTo("/");
        }
        else
        {
            content = await response.Content.ReadAsStringAsync();
            severity = Severity.Error;
            showAlert = true;
        }
    }
}
