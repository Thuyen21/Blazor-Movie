using BlazorApp3.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class ResetPassword
{
    private readonly ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
    private string content;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<ResetPasswordModel>("user/resetPassword", resetPasswordModel);
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