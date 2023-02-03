namespace MovieClient.Pages;

public partial class Logout
{
    /// <summary>
    /// The function is called when the component is initialized. It calls the logout function on the
    /// server, then checks the authentication status of the user, and then redirects the user to the
    /// home page
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        _ = await _httpClient.GetAsync("user/Logout");
        _accountService.checkAuthentication();
        _navigationManager.NavigateTo("/");
    }
}
