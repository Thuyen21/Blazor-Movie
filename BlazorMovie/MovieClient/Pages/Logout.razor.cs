namespace MovieClient.Pages;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        await _httpClient.GetAsync("user/Logout");
        _accountService.checkAuthentication();
        _navigationManager.NavigateTo("/");
    }
}
