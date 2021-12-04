using BlazorMovie.Shared;
using MovieClient.Services;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ResetPassword
{
    private readonly ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
    private readonly ShowAlertService alertService = new();
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/resetPassword", resetPasswordModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

}