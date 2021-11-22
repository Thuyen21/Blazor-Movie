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

    private async Task ChangePass()
    {
        _navigationManager.NavigateTo("/ResetPassword");
    }

    private AccountManagementModel accountManagementModel = new AccountManagementModel();
    private string? content;
    protected override async Task OnInitializedAsync()
    {
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile");
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