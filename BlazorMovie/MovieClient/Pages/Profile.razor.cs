using BlazorMovie.Shared.Account;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Profile
{
    private readonly ChangeEmailModel changeEmail = new ChangeEmailModel();

    private UserViewModel user = new UserViewModel();
    private async Task ChangeEmail()
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("api/Account/ChangeEmail", changeEmail);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            response = await _httpClient.PostAsync("api/Account/GetCurrentUser", null);
            user = await response.Content.ReadFromJsonAsync<UserViewModel>();
        }

        //_accountService.checkAuthentication
    }

    private Task ChangePass()
    {
        _navigationManager.NavigateTo("/ResetPassword");
        return Task.CompletedTask;
    }


    protected override async Task OnInitializedAsync()
    {
        var response = await _httpClient.PostAsync("api/Account/GetCurrentUser", null);

        user = await response.Content.ReadFromJsonAsync<UserViewModel>();
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/UpdateProfile", user);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            response = await _httpClient.PostAsync("api/Account/GetCurrentUser", null);
            user = await response.Content.ReadFromJsonAsync<UserViewModel>();
        }
    }

}