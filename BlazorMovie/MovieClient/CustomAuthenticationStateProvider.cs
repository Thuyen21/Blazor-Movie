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
    public void Update()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

}
