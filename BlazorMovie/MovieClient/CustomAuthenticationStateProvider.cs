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

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {


#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            AccountManagementModel user = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/GetCurrentUser");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (user.Email != null)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "serverAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
        }
        catch
        {


        }
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    }
    public void Update()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

}
