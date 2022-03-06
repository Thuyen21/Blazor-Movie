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
            Account user = (await _httpClient.GetFromJsonAsync<Account>("user/GetCurrentUser"))!;
            if (user.Email != null)
            {

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role!),
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
