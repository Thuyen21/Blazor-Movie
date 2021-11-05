using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Profile
{
    private readonly ChangeEmailModel changeEmail = new ChangeEmailModel();
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task ChangeEmail()
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("User/ChangeEmail", changeEmail);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
            showAlert = true;
            _accountService.Login();
            _navigationManager.NavigateTo("/Profile");
        }
        else
        {
            severity = Severity.Error;
        }

        showAlert = true;
        HttpResponseMessage? r = await _httpClient.PostAsJsonAsync("User/ChangeEmail", changeEmail);
        if (r.IsSuccessStatusCode)
        {
            _accountService.Login();
            _navigationManager.NavigateTo("/Profile");
        }
        else
        {
            content = await r.Content.ReadAsStringAsync();
        }
    }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    private async Task ChangePass()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
        _navigationManager.NavigateTo("/ResetPassword");
    }

    private AccountManagementModel accountManagementModel = new AccountManagementModel();
    private string? content;
    protected override async Task OnInitializedAsync()
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile");
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/EditProfile", accountManagementModel);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
        }
        else
        {
            severity = Severity.Error;
        }

        showAlert = true;
    }

}