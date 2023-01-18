using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ResetPassword
{
    /* Creating a new instance of the ResetPasswordModel class. */
    private readonly ResetPasswordModel resetPasswordModel = new();

    /// <summary>
    /// It sends a POST request to the server with the resetPasswordModel as the body of the request
    /// </summary>
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/resetPassword", resetPasswordModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

}