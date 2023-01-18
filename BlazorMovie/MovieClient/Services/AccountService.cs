using Microsoft.AspNetCore.Components.Authorization;

namespace MovieClient.Services;

public class AccountService
{
    private readonly AuthenticationStateProvider authenticationStateProvider;
    public AccountService(AuthenticationStateProvider authenticationStateProvider)
    {
        this.authenticationStateProvider = authenticationStateProvider;
    }
    /// <summary>
    /// It calls the Update() function of the CustomAuthenticationStateProvider class
    /// </summary>
    public void checkAuthentication()
    {
        (authenticationStateProvider as CustomAuthenticationStateProvider).Update();
    }
}


