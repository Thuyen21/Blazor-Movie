using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ResetPassword
{
    private EmailModel email = new();
    private ResetPasswordConfirmationModel reset = new();
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/resetPassword", email);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
    private async Task HandleValidReset()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/ResetPasswordConfirmation", reset);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}