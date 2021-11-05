using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ResetPassword
{
    private readonly ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/resetPassword", resetPasswordModel);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
        }
        else
        {
            severity = Severity.Error;
        }

        showAlert = true;
    }

}