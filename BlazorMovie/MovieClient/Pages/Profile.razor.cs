using BlazorMovie.Shared;
using MovieClient.Services;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Profile
{
    private readonly ChangeEmailModel changeEmail = new ChangeEmailModel();
    private readonly ShowAlertService alertService = new();
    private AccountManagementModel accountManagementModel = new AccountManagementModel();
    private async Task ChangeEmail()
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("User/ChangeEmail", changeEmail);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        _accountService.checkAuthentication();
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile");
    }

    private Task ChangePass()
    {
        _navigationManager.NavigateTo("/ResetPassword");
        return Task.CompletedTask;
    }


    protected override async Task OnInitializedAsync()
    {
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile");
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/EditProfile", accountManagementModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

}