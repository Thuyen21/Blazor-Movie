using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MovieClient;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// It gets the current user from the server, and if the user is not null, it creates a new
    /// ClaimsIdentity with the user's email, role, and email, and then creates a new ClaimsPrincipal
    /// with the ClaimsIdentity, and returns a new AuthenticationState with the ClaimsPrincipal
    /// </summary>
    /// <returns>
    /// The AuthenticationState is being returned.
    /// </returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            AccountManagementModel user = (await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/GetCurrentUser"))!;
            if (user.Email != null)
            {

                ClaimsIdentity claimsIdentity = new(new[] {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role!),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "serverAuth");
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
        }
        catch
        {

        }
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    /// <summary>
    /// "If the user is authenticated, then we'll get the user's profile information and store it in the
    /// `UserProfile` property. If the user is not authenticated, then we'll clear the `UserProfile`
    /// property."
    /// 
    /// The `GetAuthenticationStateAsync` function is an `async` function, which means that it will
    /// return immediately and the rest of the function will be executed on a background thread. This is
    /// important because we don't want to block the UI thread while we're waiting for the
    /// authentication state to be retrieved
    /// </summary>
    public void Update()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

}
