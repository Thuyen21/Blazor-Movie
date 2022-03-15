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
            var respon = await _httpClient.PostAsync("api/Account/GetCurrentUser", null);

            var user = await respon.Content.ReadFromJsonAsync<UserModel>();


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                    //new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Id.Value.ToString())
                }, "serverAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);

        }
        catch (Exception ex)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

    }
    public void Update()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

}
