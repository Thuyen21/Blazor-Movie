namespace MovieClient.Pages;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        await _httpClient.PostAsync("api/Account/Logout", null);
        _accountService.checkAuthentication();
        _navigationManager.NavigateTo("/");
    }
}
